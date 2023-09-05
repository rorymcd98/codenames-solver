namespace codenames_solver
{
    public enum Team
    {
        Red,
        Blue
    }
    public class ControlState
    {
        public string CurrentCodeWord { get; private set; }
        public Team CurrentTeam { get; private set; }
        public int CurrentNumberOfWords { get; private set; }

        public ControlState()
        {
            CurrentCodeWord = string.Empty;
            CurrentTeam = Team.Blue;
            CurrentNumberOfWords = 2;
        }
        public void UpdateCodeWord(string NewCodeWord)
        {
            CurrentCodeWord = NewCodeWord;
            NotifyStateChanged("CodeWord");
        }
        public void UpdateTeam(Team NewTeam)
        {
            CurrentTeam = NewTeam;
            NotifyStateChanged("Team");
        }
        public void UpdateNumberOfWords(int NewNumberOfWords)
        {
            CurrentNumberOfWords = NewNumberOfWords;
            NotifyStateChanged("NumberOfWords");
        }

        public event Action<string>? StateChanged;

        private void NotifyStateChanged(string Property)
            => StateChanged?.Invoke(Property);
    }
}
