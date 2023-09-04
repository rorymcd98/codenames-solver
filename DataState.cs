namespace codenames_solver
{
    public class DataState
    {
        public List<SimilarityItem> CurrentSimilarityItems { get; private set; }
        public bool IsLoadingSimilarity { get; private set; }

        public DataState()
        {
            CurrentSimilarityItems = new List<SimilarityItem>();
            IsLoadingSimilarity = false;
        }
        public void UpdateSimilarityItems(List<SimilarityItem> NewSimilarityItems)
        {
            this.CurrentSimilarityItems = NewSimilarityItems;
            NotifyStateChanged("SimilarityItems");
        }
        public void UpdateIsLoadingSimilarity(bool isLoading)
        {
            this.IsLoadingSimilarity = isLoading;
            NotifyStateChanged("LoadingState");
        }

        public event Action<string> StateChanged;
        private void NotifyStateChanged(string Property)
            => StateChanged?.Invoke(Property);
    }
}
