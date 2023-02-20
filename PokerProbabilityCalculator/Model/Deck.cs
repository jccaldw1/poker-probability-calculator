using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    public void Shuffle()
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
