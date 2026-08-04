using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    [SerializeField] private GameObject[] cardTypes;
    [SerializeField] private List<GameObject> deck = new List<GameObject>();
    [SerializeField] private int numberOfBoulderCards = 3;
    [SerializeField] private int numberOfOtherCards = 10;
    [SerializeField] private int cardsDealtEachRound = 3;
    [SerializeField] private Vector3[] cardSlots = {new Vector3(-1, 1.05f, 0), new Vector3(0, 1.05f, 0), new Vector3(1, 1.05f, 0)};
    private GameManager gameManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = GameManager.Instance;

        CreateDeck();
        DealCards();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CreateDeck()
    {
        for(int cardsInDeck = 0; cardsInDeck < numberOfOtherCards; cardsInDeck++)
        {
            deck.Add(cardTypes[0]);
            deck.Add(cardTypes[1]);
        }
        for(int bouldersInDeck = 0; bouldersInDeck < numberOfBoulderCards; bouldersInDeck++)
        {
            deck.Add(cardTypes[2]);
        }
    }

    /// <summary>
    /// Deal blank cards equal to cardsDealtEachRound
    /// </summary>
    void DealCards()
    {
        for(int cardsDealt = 0; cardsDealt < cardsDealtEachRound; cardsDealt++)
        {
            GameObject blankCard = Instantiate(cardTypes[3], cardSlots[cardsDealt], transform.rotation);
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
