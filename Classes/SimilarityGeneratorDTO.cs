//I wasn't sure how to abstract the Similarity endpoints properly
namespace codenames_solver
{
    public class SimilarityGeneratorPostRequestDTO
    {
        public List<CardInfo> Cards { get; set; } = new List<CardInfo>();
        public int CurrentNumberOfWords { get; set; }
        public Team CurrentTeam { get; set; }
    }
    public class SimilarityGeneratorPostResponseDTO
    {
        public List<SimilarityGeneratorItem> SimilarCodewords { get; set; } = new List<SimilarityGeneratorItem>();
    }

    public class SimilarityGeneratorDTOBuilder
    {
        private readonly CardsState _cardsState;
        private readonly ControlState _controlState;

        public SimilarityGeneratorDTOBuilder(CardsState cardsState, ControlState controlState)
        {
            _cardsState = cardsState;
            _controlState = controlState;
        }

        public SimilarityGeneratorPostRequestDTO BuildSimilarityPostDTO()
        {
            return new SimilarityGeneratorPostRequestDTO
            {
                Cards = _cardsState.Cards,
                CurrentNumberOfWords = _controlState.CurrentNumberOfWords,
                CurrentTeam = _controlState.CurrentTeam
            };
        }
    }

    public class SimilarityGeneratorItem
    {
        public List<Tuple<string, double>> CodeWordScores { get; set; } = new List<Tuple<string, double>>();
        public List<string> RelatedWords { get; set; }
        public SimilarityGeneratorItem(List<string> relatedWords)
        {
            RelatedWords = relatedWords;
        }
    }
}
