using codenames_solver;

namespace codenames_solver
{
    public class SimilarityPostDTO
    {
        public readonly List<CardInfo> Cards;
        public readonly string CurrentCodeWord;
        public readonly int CurrentNumberOfWords;
        public readonly Team CurrentTeam;
        public SimilarityPostDTO(List<CardInfo> Cards, string currentCodeWord,
                                 int currentNumberOfWords, Team currentTeam)
        {
            this.Cards = Cards;
            this.CurrentCodeWord = currentCodeWord;
            this.CurrentNumberOfWords = currentNumberOfWords;
            this.CurrentTeam = currentTeam;
        }
    }

    public class SimilarityItem
    {
        public List<Tuple<string, double>> CodeWordScores;
        public readonly List<string> RelatedWords;
        public double TopScore;
        
        public SimilarityItem(List<string> relatedWords)
        {
            this.CodeWordScores = new List<Tuple<string, double>>();
            this.RelatedWords = relatedWords;
            this.TopScore = 0;
        }
    }

    public class SimilarityPostResponseDTO
    {
        public readonly List<SimilarityItem> SimilarCodewords;

        public SimilarityPostResponseDTO(List<SimilarityItem> similarityItems)
        {
            SimilarCodewords = similarityItems;
        }
    }

    public class DTOBuilder
    {
        private readonly CardsState _cardsState;
        private readonly ControlState _controlState;

        public DTOBuilder(CardsState cardsState, ControlState controlState)
        {
            _cardsState = cardsState;
            _controlState = controlState;
        }

        public SimilarityPostDTO BuildSimilarityPostDTO()
        {
            return new SimilarityPostDTO(_cardsState.Cards, _controlState.CurrentCodeWord,
                                         _controlState.CurrentNumberOfWords, _controlState.CurrentTeam);
        }
    }
}