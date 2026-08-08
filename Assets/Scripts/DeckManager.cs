using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    [SerializeField] private GameObject[] cardTypes;
    [SerializeField] private List<GameObject> deck = new List<GameObject>();
    [SerializeField] private int numberOfBoulderCards = 7;
    [SerializeField] private int numberOfPebbleCards = 3;
    [SerializeField] private int numberOfNormalCards = 18;
    [SerializeField] private int cardsDealtEachRound = 3;
    [SerializeField] private Vector3[] cardSlots;
    private GameManager gameManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = GameManager.Instance;

        CreateDeck();
        DealCards();

    }


    void CreateDeck()
    {
        for(int cardsInDeck = 0; cardsInDeck < numberOfNormalCards; cardsInDeck++)
        {
            deck.Add(cardTypes[0]);
            deck.Add(cardTypes[1]);
        }
        for(int bouldersInDeck = 0; bouldersInDeck < numberOfBoulderCards; bouldersInDeck++)
        {
            deck.Add(cardTypes[2]);
        }
        for(int pebblesInDeck = 0; pebblesInDeck < numberOfPebbleCards; pebblesInDeck++)
        {
            deck.Add(cardTypes[3]);
        }
    }

    /// <summary>
    /// Deal blank cards equal to cardsDealtEachRound
    /// </summary>
    void DealCards()
    {
        for(int cardsDealt = 0; cardsDealt < cardsDealtEachRound; cardsDealt++)
        {
            GameObject blankCard = Instantiate(cardTypes[4], cardSlots[cardsDealt], transform.rotation);
            deck.Add(blankCard);
        }
    }

    /// <summary>
    /// Remove blank cards, and reveal cards equal to cardsDealtEachRound, then remove those from the deck
    /// </summary>
    public void RevealCards()
    {
        for(int cardsDealt = 0; cardsDealt < cardsDealtEachRound; cardsDealt++)
        {
            RemoveCard(deck.Count - 1, true);

            int randomCard = Random.Range(0, deck.Count);
            Instantiate(deck[randomCard], cardSlots[cardsDealt], transform.rotation);
            RemoveCard(randomCard, false);
        }
    }

    void RemoveCard(int cardIndex, bool beDestroyed)
    {
        if(beDestroyed == true)
        {
            Destroy(deck[cardIndex]);
        }
        deck.Remove(deck[cardIndex]);
    }
}
