using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class DeckManager : MonoBehaviour
{
    public static DeckManager Inst { get; private set; }
    private void Awake()
    {
        if (Inst != null && Inst != this)
        {
            Destroy(gameObject);
            return;
        }

        Inst = this;

        DontDestroyOnLoad(gameObject);
    }

    DeckSO selectedStartDeck = null;
    List<CardContent> deck = null;

    public void SetStartDeck(DeckSO selectedDeck)
    {
        selectedStartDeck = selectedDeck;
    }

    public void InitDeck()
    {
        deck = new List<CardContent>();
        foreach(CardDataSO cardData in selectedStartDeck.startingDeck)
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
