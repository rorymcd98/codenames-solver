namespace codenames_solver
{
    public class CardInfo
    {
        public string Text { get; set; }
        public CardColor Color { get; set; }
        public CardInfo()
        {
            Text = string.Empty;
            Color = CardColor.Neutral;
        }
    }
}
