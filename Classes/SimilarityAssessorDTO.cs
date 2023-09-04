namespace codenames_solver
{
    public class SimilarityAssessorPostRequestDTO : SimilarityGeneratorPostRequestDTO
    {
        public string CurrentCodeword { get; set; } = string.Empty;
    }
    public class SimilarityAssessorPostResponseDTO
    {
        public List<SimilarityAssessorPostResponseDTO> SimilarCodewords { get; set; } = new List<SimilarityAssessorPostResponseDTO>();
    }

    public class SimilarityAssessorDTOBuilder
    {
        private readonly CardsState _cardsState;
        private readonly ControlState _controlState;

        public SimilarityAssessorDTOBuilder(CardsState cardsState, ControlState controlState)
        {
            _cardsState = cardsState;
            _controlState = controlState;
        }

        public SimilarityAssessorPostRequestDTO BuildSimilarityPostDTO()
        {
            return new SimilarityAssessorPostRequestDTO
            {
                Cards = _cardsState.Cards,
                CurrentNumberOfWords = _controlState.CurrentNumberOfWords,
                CurrentTeam = _controlState.CurrentTeam
            };
        }
    }

    public class SimilarityAssessorItem
    {
        public List<Tuple<string, double>> CodeWordScores { get; set; } = new List<Tuple<string, double>>();
        public List<string> RelatedWords { get; set; }
        public SimilarityAssessorItem(List<string> relatedWords)
        {
            this.RelatedWords = relatedWords;
        }
    }
}
