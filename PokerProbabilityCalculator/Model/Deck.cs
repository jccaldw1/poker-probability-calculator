using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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

    /// <summary>
    /// Remove some cards from the deck.
    /// </summary>
    //public bool RemoveCards(List<Card> cardsToRemove)
    //{
    //    bool cardsCouldBeRemoved = true;

    //    foreach(Card card in cardsToRemove)
    //    {
    //        cardsCouldBeRemoved &= cards.Remove(card);
    //    }

    //    return cardsCouldBeRemoved;
    //}

    //public bool RemoveCards(Card cardToRemove)
    //{
    //    return cards.Remove(cardToRemove);
    //}

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
