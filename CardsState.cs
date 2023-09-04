using Microsoft.AspNetCore.Components;

namespace codenames_solver
{
    public class CardsState
    {
        private const int NUMBER_OF_CARDS = 25;
        public List<CardInfo> Cards { get; private set; }
        private ValidWords validWords;
        private Random random = new Random();
        public CardsState(ValidWords ValidWords)
        {
            Cards = new List<CardInfo>(NUMBER_OF_CARDS);
            for (int i = 0; i < NUMBER_OF_CARDS; i++)
            {
                Cards.Add(new CardInfo());
            }
            validWords = ValidWords;
        }

        private void GenerateRandomCardWords()
        {
            foreach (var card in Cards)
            {
                card.Text = validWords.GetRandomWord();
            }
        }

        public void GenerateRandomCardColors()
        {
            var colorIndices = CardColors.GenerateIndeces();
            int index = 0;
            foreach (var card in Cards)
            {
                if (colorIndices.Blue.Contains(index))
                    card.Color = CardColor.Blue;
                else if (colorIndices.Red.Contains(index))
                    card.Color = CardColor.Red;
                else if (colorIndices.Black == index)
                    card.Color = CardColor.Black;
                else
                    card.Color = CardColor.Neutral;

                index++;
            }
        }

        private void ResetPicked()
        {
            foreach (var card in Cards)
            {
                card.Picked = false;
            }
        }
        public void GenerateRandomBoard(ComponentBase Source)
        {
            GenerateRandomCardColors();
            GenerateRandomCardWords();
            ResetPicked();
            NotifyStateChanged(Source, "Board");
        }

        public List<string> ListInvalidWords()
        {
            List<string> result = new List<string>();
            foreach (var card in Cards)
            {
                if (!validWords.IsValidWord(card.Text))
                {
                    result.Add(card.Text);
                }
            }
            return result;
        }

        public bool GetCardPicked(int CardIndex)
        {
            return Cards[CardIndex].Picked;
        }

        public void UpdateCardPicked(ComponentBase Source, bool NewCardPicked, int CardIndex)
        {
            this.Cards[CardIndex].Picked = NewCardPicked;
            NotifyStateChanged(Source, "CardPicked");
        }

        public CardColor GetCardColor(int CardIndex)
        {
            return Cards[CardIndex].Color;
        }

        public void UpdateCardColor(ComponentBase Source, CardColor NewCardColor, int CardIndex)
        {
            this.Cards[CardIndex].Color = NewCardColor;
            NotifyStateChanged(Source, "CardColor");
        }

        public string GetCardText(int CardIndex)
        {
            return Cards[CardIndex].Text;
        }

        public void UpdateCardText(ComponentBase Source, string NewCardText, int CardIndex)
        {
            this.Cards[CardIndex].Text = NewCardText;
            NotifyStateChanged(Source, "CardText");
        }

        public event Action<ComponentBase, string> StateChanged;

        private void NotifyStateChanged(ComponentBase Source, string Property)
            => StateChanged?.Invoke(Source, Property);
    }
}
