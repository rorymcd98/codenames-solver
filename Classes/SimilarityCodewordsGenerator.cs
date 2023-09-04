using Word2vec.Tools;

namespace codenames_solver
{
    public class SimilarityCodewordsGenerator
    {
        private readonly List<string> _currentTeamWords;
        private readonly List<string> _opposingTeamWords;
        private readonly List<string> _neutralWords;
        private readonly List<string> _assassinWords;

        private readonly List<List<string>> _wordPerms;

        private readonly Vocabulary _vocabulary;
        private readonly ValidWords _validWords;

        private const int CURRENT_BONUS = 500;

        private const double PENALTY_COEFFICIENT = 0.5;

        private const double ASSASSIN_PENALTY = -200 * PENALTY_COEFFICIENT;
        private const double OPPOSING_PENALTY = -100 * PENALTY_COEFFICIENT;
        private const double NEUTRAL_PENALTY = -50 * PENALTY_COEFFICIENT;

        private const int DISTANCES_COUNT = 100;
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
            object locker = new();

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
            List<List<string>> result = new();
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
            if (wordPerm == null || wordPerm.Count == 0)
                throw new ArgumentException("wordPerm cannot be null or empty");

            Representation addRepresentation = _vocabulary[wordPerm[0]];

            for (int i = 1; i < wordPerm.Count; i++)  // Start the loop from the 2nd item
            {
                addRepresentation = addRepresentation.Add(_vocabulary[wordPerm[i]]);
            }

            return addRepresentation;
        }


        private double ScoreDistances(DistanceTo additionDistance)
        {
            double score = 0;
            score += additionDistance.DistanceValue * CURRENT_BONUS;
            var word = additionDistance.Representation.WordOrNull;
            foreach (var opWord in _opposingTeamWords)
            {
                score += CosineSimilarity(opWord, word) * OPPOSING_PENALTY;
            }
            foreach (var asWord in _assassinWords)
            {
                score += CosineSimilarity(asWord, word) * ASSASSIN_PENALTY;
            }
            foreach (var neWord in _neutralWords)
            {
                score += CosineSimilarity(neWord, word) * NEUTRAL_PENALTY;
            }
            return score;
        }

        private SimilarityItem GenerateSimilarityItem(List<string> wordPerm)
        {
            SimilarityItem similarityItem = new(wordPerm);

            var additionRepresentation = GenerateAdditionRepresentation(wordPerm);
            var closestAdditions = _vocabulary.Distance(additionRepresentation, DISTANCES_COUNT);

            var codeWordScores = new List<Tuple<string, double>>();

            foreach (DistanceTo additionDistance in closestAdditions)
            {
                var codeWord = additionDistance.Representation.WordOrNull.ToString();
                if (wordPerm.Contains(codeWord))
                {
                    continue;
                }
                var score = ScoreDistances(additionDistance);
                var codeWordScore = new Tuple<string, double>(codeWord, score);

                codeWordScores.Add(codeWordScore);
            }

            codeWordScores.Sort((x, y) => y.Item2.CompareTo(x.Item2));
            if (codeWordScores.Count > CODEWORDS_COUNT)
            {
                codeWordScores = codeWordScores.Take(CODEWORDS_COUNT).ToList();
            }
            similarityItem.CodeWordScores = codeWordScores;

            return similarityItem;
        }

        public List<SimilarityItem> GenerateSimilarityItems()
        {
            var similarityItems = new List<SimilarityItem>();

            Console.WriteLine(_wordPerms.Count);

            foreach (List<string> wordPerm in _wordPerms)
            {
                var similarityItem = GenerateSimilarityItem(wordPerm);
                similarityItems.Add(similarityItem);
            }

            similarityItems.Sort((x, y) => y.CodeWordScores[0].Item2.CompareTo(x.CodeWordScores[0].Item2));

            if (similarityItems.Count > PERMUTATIONS_COUNT)
            {
                similarityItems = similarityItems.Take(PERMUTATIONS_COUNT).ToList();
            }
            return similarityItems;
        }

        public SimilarityCodewordsGenerator(SimilarityPostDTO similarityPostBody, Vocabulary vocabulary, ValidWords validWords)
        {
            _vocabulary = vocabulary;
            _validWords = validWords;

            var Cards = similarityPostBody.Cards;
            _currentTeamWords = new List<string>();
            _opposingTeamWords = new List<string>();
            _neutralWords = new List<string>();
            _assassinWords = new List<string>();


            CardColor CurrentTeamCardColor = similarityPostBody.CurrentTeam == Team.Red ? CardColor.Red : CardColor.Blue;


            foreach (var card in Cards)
            {
                if (card.Picked) continue;
                var originalWord = _validWords.GetOriginalWord(card.Text);
                if (card.Color == CardColor.Black)
                {
                    _assassinWords.Add(originalWord);
                }
                else if (card.Color == CardColor.Neutral)
                {
                    _neutralWords.Add(originalWord);
                }
                else if (card.Color == CurrentTeamCardColor)
                {
                    _currentTeamWords.Add(originalWord);
                }
                else
                {
                    _opposingTeamWords.Add(originalWord);
                }
            }

            _wordPerms = GenerateWordPermsChooseN(_currentTeamWords, similarityPostBody.CurrentNumberOfWords);
        }
    }
}
