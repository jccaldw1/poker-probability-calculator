namespace PokerProbabilityCalculator.Model;

public class Deck
{
    public List<Card> cards;

    public Deck()
    {
        cards = new List<Card>();

        for (int i = 0; i < Enum.GetNames(typeof(Suit)).Length; i++)
        {
            for(int j = 0; j < Enum.GetNames(typeof(Value)).Length; j++)
            {
                cards.Add(new Card((Suit)i, (Value)j));
            }
        }

        Shuffle();
    }

    public bool RemoveCard(Card cardToRemove)
    {
        return cards.Remove(cardToRemove);
    }

    public bool RemoveRange(List<Card> cardsToRemove)
    {
        bool returnValue = true;

        cardsToRemove.ForEach(card =>
        {
            returnValue &= RemoveCard(card);
        });

        return returnValue;
    }

    public bool Contains(List<Card> cards)
    {
        foreach (Card card in cards)
        {
            if (!this.cards.Contains(card))
            {
                return false;
            }
        }

        return true;
    }

    private void Shuffle()
    {
        Random random = new();
        HashSet<Card> hashedCards = cards.ToHashSet();
        List<Card> shuffledCards = new List<Card>();

        while(hashedCards.Count > 0)
        {
            int cardToAddIndex = random.Next(hashedCards.Count);
            Card nextCard = hashedCards.ElementAt(cardToAddIndex);
            shuffledCards.Add(nextCard);
            hashedCards.Remove(nextCard);
        }

        cards = shuffledCards;
    }
}
