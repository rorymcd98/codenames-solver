using Microsoft.AspNetCore.Components;

namespace codenames_solver
{
    public class CardsState
    {
        private const int NUMBER_OF_CARDS = 25;
        public List<CardInfo> cards { get; private set; }
        private ValidWords validWords;
        private Random random = new Random();
        public CardsState(ValidWords ValidWords)
        {
            cards = new List<CardInfo>(NUMBER_OF_CARDS);
            for (int i = 0; i < NUMBER_OF_CARDS; i++)
            {
                cards.Add(new CardInfo());
            }
            validWords = ValidWords;
        }

        private void GenerateRandomCardWords()
        {
            foreach (var card in cards)
            {
                card.Text = validWords.GetRandomWord();
            }
        }

        public void GenerateRandomCardColors()
        {
            var colorIndices = CardColors.GenerateIndeces();
            int index = 0;
            foreach (var card in cards)
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
        public void GenerateRandomBoard(ComponentBase Source)
        {
            GenerateRandomCardColors();
            GenerateRandomCardWords();
            NotifyStateChanged(Source, "Board");
        }

        public List<string> ListInvalidWords()
        {
            List<string> result = new List<string>();
            foreach (var card in cards)
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
            return cards[CardIndex].Picked;
        }

        public void UpdateCardPicked(ComponentBase Source, bool NewCardPicked, int CardIndex)
        {
            this.cards[CardIndex].Picked = NewCardPicked;
            NotifyStateChanged(Source, "CardPicked");
        }

        public CardColor GetCardColor(int CardIndex)
        {
            return cards[CardIndex].Color;
        }

        public void UpdateCardColor(ComponentBase Source, CardColor NewCardColor, int CardIndex)
        {
            this.cards[CardIndex].Color = NewCardColor;
            NotifyStateChanged(Source, "CardColor");
        }

        public string GetCardText(int CardIndex)
        {
            return cards[CardIndex].Text;
        }

        public void UpdateCardText(ComponentBase Source, string NewCardText, int CardIndex)
        {
            this.cards[CardIndex].Text = NewCardText;
            NotifyStateChanged(Source, "CardText");
        }

        public event Action<ComponentBase, string> StateChanged;

        private void NotifyStateChanged(ComponentBase Source, string Property)
            => StateChanged?.Invoke(Source, Property);
    }
}
