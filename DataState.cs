namespace codenames_solver
{
    public class DataState
    {
        public List<SimilarityGeneratorItem> CurrentSimilarityItems { get; private set; }
        public bool IsLoadingSimilarity { get; private set; }

        public DataState()
        {
            CurrentSimilarityItems = new List<SimilarityGeneratorItem>();
            IsLoadingSimilarity = false;
        }
        public void UpdateSimilarityItems(List<SimilarityGeneratorItem> NewSimilarityItems)
        {
            CurrentSimilarityItems = NewSimilarityItems;
            NotifyStateChanged("SimilarityItems");
        }
        public void UpdateIsLoadingSimilarity(bool isLoading)
        {
            IsLoadingSimilarity = isLoading;
            NotifyStateChanged("LoadingState");
        }

        public event Action<string>? StateChanged;
        private void NotifyStateChanged(string Property)
            => StateChanged?.Invoke(Property);
    }
}
