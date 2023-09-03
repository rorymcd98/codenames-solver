using Word2vec.Tools;

namespace codenames_solver
{
    class SortByScoreDescending : IComparer<Tuple<string, double>>
    {
        public int Compare(Tuple<string, double> x, Tuple<string, double> y)
        {
            return y.Item2.CompareTo(x.Item2);
        }
    }
    class SortByTopScoreDescending : IComparer<SimilarityItem>
    {
        public int Compare(SimilarityItem x, SimilarityItem y)
        {
            return y.TopScore.CompareTo(x.TopScore);
        }
    }

    public class SimilarityCodewordsGenerator
    {
        private List<string> currentTeamWords;
        private List<string> opposingTeamWords;
        private List<string> neutralWords;
        private List<string> assassinWords;

        private List<List<string>> wordPerms;

        private Vocabulary _vocabulary;
        private ValidWords _validWords;

        private const int CURRENT_BONUS = 500;
        private const int ASSASSIN_PENALTY = -200;
        private const int OPPOSING_PENALTY = -100;
        private const int NEUTRAL_PENALTY = -50;

        private const int DISTANCES_LIMIT = 100;
        private const int CODEWORDS_COUNT = 10;
        private const int PERMUTATIONS_COUNT = 10;

        private double CosineSimilarity(string word1, string word2)
        {
            if (!_vocabulary.ContainsWord(word1) || !_vocabulary.ContainsWord(word2))
            {
                throw new Exception("One or both words not found in vocabulary");
            }

            var vector1 = _vocabulary.GetRepresentationFor(word1).NumericVector;
            var vector2 = _vocabulary.GetRepresentationFor(word2).NumericVector;

            double dotProduct = 0.0;
            double norm1 = 0.0;
            double norm2 = 0.0;
            object locker = new object();

            Parallel.For(0, _vocabulary.VectorDimensionsCount, i =>
            {
                double tmpDot = vector1[i] * vector2[i];
                double tmpNorm1 = Math.Pow(vector1[i], 2);
                double tmpNorm2 = Math.Pow(vector2[i], 2);

                lock (locker)
                {
                    dotProduct += tmpDot;
                    norm1 += tmpNorm1;
                    norm2 += tmpNorm2;
                }
            });

            return dotProduct / (Math.Sqrt(norm1) * Math.Sqrt(norm2));
        }


        // Time to leetcode it up
        private List<List<string>> GenerateWordPermsChooseN(List<string> words, int chooseN)
        {
            List<List<string>> result = new List<List<string>>();
            GenerateWordPermsHelper(words, chooseN, new List<string>(), result);
            return result;
        }


        private void GenerateWordPermsHelper(List<string> words, int chooseN, List<string> current, List<List<string>> result)
        {
            if (chooseN == 0)
            {
                result.Add(new List<string>(current));
                return;
            }

            for (int i = 0; i <= words.Count - chooseN; ++i)
            {
                current.Add(words[i]);
                GenerateWordPermsHelper(words.GetRange(i + 1, words.Count - i - 1), chooseN - 1, current, result);
                current.RemoveAt(current.Count - 1);
            }
        }

        private Representation GenerateAdditionRepresentation(List<string> wordPerm)
        {
            var additionRepresentation = _vocabulary[wordPerm[0]];
            if (wordPerm.Count == 1)
            {
                return additionRepresentation;
            }
            for (int i = 1; i < wordPerm.Count; i++)
            {
                additionRepresentation.Add(_vocabulary[wordPerm[i]]);
            }
            return additionRepresentation;
        }

        private double ScoreDistances(DistanceTo additionDistance)
        {
            double score = 0;
            score += additionDistance.DistanceValue*CURRENT_BONUS;
            var word = additionDistance.Representation.WordOrNull;
            foreach (var opWord in opposingTeamWords)
            {
                score += CosineSimilarity(opWord, word)*OPPOSING_PENALTY;
            }
            foreach (var asWord in assassinWords)
            {
                score += CosineSimilarity(asWord, word)*ASSASSIN_PENALTY;
            }
            foreach (var neWord in neutralWords)
            {
                score += CosineSimilarity(neWord, word)*NEUTRAL_PENALTY;
            }
            return score;
        }

        private SimilarityItem GenerateSimilarityItem(List<string> wordPerm)
        {
            SimilarityItem similarityItem = new SimilarityItem(wordPerm);

            var additionRepresentation = GenerateAdditionRepresentation(wordPerm);
            var closestAdditions = _vocabulary.Distance(additionRepresentation, DISTANCES_LIMIT);

            var CodeWordScores = new SortedSet<Tuple<string, double>>(new SortByScoreDescending());

            foreach (DistanceTo additionDistance in closestAdditions)
            {
                var CodeWord = additionDistance.Representation.WordOrNull.ToString();
                var score = ScoreDistances(additionDistance);
                var CodeWordScore = Tuple.Create<string, double>(CodeWord, score);
                similarityItem.TopScore = Math.Max(score, similarityItem.TopScore);

                // Add item to the SortedSet
                CodeWordScores.Add(CodeWordScore);
                if (CodeWordScores.Count > CODEWORDS_COUNT)
                {
                    // Remove the lowest element if count exceeds CODEWORDS_COUNT
                    CodeWordScores.Remove(CodeWordScores.Min);
                }
            }
            similarityItem.CodeWordScores = CodeWordScores.ToList();
            return similarityItem;
        }

        public List<SimilarityItem> GenerateSimilarityItems()
        {
            var topSimilarityItems = new SortedSet<SimilarityItem>(new SortByTopScoreDescending());

            foreach (List<string> perm in wordPerms)
            {
                // Generating the similarity item for this permutation
                var similarityItem = GenerateSimilarityItem(perm);

                // Add item to the SortedSet
                topSimilarityItems.Add(similarityItem);
                if (topSimilarityItems.Count > PERMUTATIONS_COUNT)
                {
                    // Remove the lowest element if count exceeds PERMUTATIONS_COUNT
                    topSimilarityItems.Remove(topSimilarityItems.Min);
                }
            }
            return topSimilarityItems.ToList();
        }


        public SimilarityCodewordsGenerator(SimilarityPostDTO similarityPostBody, Vocabulary vocabulary, ValidWords validWords)
        {
            _vocabulary = vocabulary;
            _validWords = validWords;

            var Cards = similarityPostBody.Cards;
            currentTeamWords = new List<string>();
            opposingTeamWords = new List<string>();
            neutralWords = new List<string>();
            assassinWords = new List<string>();


            CardColor CurrentTeamCardColor = similarityPostBody.CurrentTeam == Team.Red ? CardColor.Red : CardColor.Blue;


            foreach (var card in Cards)
            {
                if (card.Picked) continue;
                var originalWord = _validWords.GetOriginalWord(card.Text);
                if (card.Color == CardColor.Black)
                {
                    assassinWords.Add(originalWord);
                }
                else if (card.Color == CardColor.Neutral)
                {
                    neutralWords.Add(originalWord);
                }
                else if (card.Color == CurrentTeamCardColor)
                {
                    currentTeamWords.Add(originalWord);
                }
                else
                {
                    opposingTeamWords.Add(originalWord);
                }
            }

            wordPerms = GenerateWordPermsChooseN(currentTeamWords, similarityPostBody.CurrentNumberOfWords);
        }
    }
}
