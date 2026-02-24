using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class DeckManager : MonoBehaviour
{
    List<CardContent> deck = null;

    public void InitDeck(List<CardDataSO> newDeck)
    {
        deck = new List<CardContent>();
        foreach(CardDataSO cardData in newDeck)
        {
            deck.Add(new CardContent(cardData.card));
        }
    }

    public List<CardContent> GetDeckdata()
    {
        return deck;
    }

    public void AddCardToDeck(CardContent newCard)
    {
        deck.Add(newCard);
    }

    public void RemoveCardToDeck(CardContent targetCard)
    {
        deck.Remove(targetCard);
    }
}
