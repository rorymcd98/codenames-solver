namespace codenames_solver
{
    public class SimilarityPostDTO
    {
        public List<CardInfo> Cards { get; set; }
        public string CurrentCodeWord { get; set; }
        public int CurrentNumberOfWords { get; set; }
        public Team CurrentTeam { get; set; }
    }

    public class SimilarityItem
    {
        public List<Tuple<string, double>> CodeWordScores { get; set; } = new List<Tuple<string, double>>();
        public List<string> RelatedWords { get; set; }
        public SimilarityItem(List<string> relatedWords)
        {
            this.RelatedWords = relatedWords;
        }
    }

    public class SimilarityPostResponseDTO
    {
        public List<SimilarityItem> SimilarCodewords { get; set; } = new List<SimilarityItem>();
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
            return new SimilarityPostDTO
            {
                Cards = _cardsState.Cards,
                CurrentCodeWord = _controlState.CurrentCodeWord,
                CurrentNumberOfWords = _controlState.CurrentNumberOfWords,
                CurrentTeam = _controlState.CurrentTeam
            };
        }
    }
}