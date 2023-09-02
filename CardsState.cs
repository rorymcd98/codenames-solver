using codenames_solver.Shared;
using Microsoft.AspNetCore.Components;
using System;

namespace codenames_solver
{
    public class CardsState
    {
        private const int NUMBER_OF_CARDS = 25;

        private List<CardInfo> cards;

        public CardsState()
        {
            cards = new List<CardInfo>(NUMBER_OF_CARDS);
            for (int i = 0; i < NUMBER_OF_CARDS; i++)
            {
                cards.Add(new CardInfo());
            }
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
