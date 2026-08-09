using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    [Header("Setup")]
    public bool CardsInPlay = false;
    public List<GameObject> BlankCards = new List<GameObject>();
    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameObject deckProp; 
    [SerializeField] private GameObject[] cardPrefabs;
    [SerializeField] private Vector3[] cardSlots;
    [SerializeField] private float selectionHeightModifier = 2f;
    private List<Card> deck = new List<Card>();
    private List<Card> cards = new List<Card>();


    public enum CardType
    {
        Hill,
        Valley,
        Boulder,
        Pebble,
        Blank
    }


    public struct Card
    {
        public CardType cardType;
        public GameObject cardPrefab;
        public int damage;
    }


    [Header("Discard")]
    private List<Card> discardPile = new List<Card>();
    [SerializeField] private Vector3 discardLocation = new Vector3(2, 1.05f, 0);
    [SerializeField] private float cardChangeTime = 2f;
    [SerializeField] private float cardThickness = 0.1f;


    [Header("Card Effects")]
    [SerializeField] private int foeMinIndex = 3;
    [SerializeField] private int foeMaxIndex = 6; // exclusive
    [SerializeField] private int playerMinIndex = 0;
    [SerializeField] private int playerMaxIndex = 3; // exclusive


    [Header("Deck Composition")]
    [SerializeField] private int numberOfBoulderCards = 3;
    [SerializeField] private int numberOfPebbleCards = 1;
    [SerializeField] private int numberOfNormalCards = 7;
    [SerializeField] private int cardsDealtEachRound = 6;
    private Card hillCard;
    private Card valleyCard;
    private Card boulderCard;
    private Card pebbleCard;
    private Card blankCard;

    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = GameManager.Instance;
        deckProp = GameObject.Find("Deck");

        cards.Add(hillCard = new Card() {cardType = CardType.Hill, cardPrefab = cardPrefabs[0], damage = 1});
        cards.Add(valleyCard = new Card() {cardType = CardType.Valley, cardPrefab = cardPrefabs[1], damage = -1});
        cards.Add(boulderCard = new Card() {cardType = CardType.Boulder, cardPrefab = cardPrefabs[2], damage = -3});
        cards.Add(pebbleCard = new Card() {cardType = CardType.Pebble, cardPrefab = cardPrefabs[3], damage = -3});
        cards.Add(blankCard = new Card() {cardType = CardType.Blank, cardPrefab = cardPrefabs[4]});

        CreateDeck();
        DealCards();
    }


    /// <summary>
    /// Add numbers of each card to the deck according to their numberOf variable listed in Deck Composition
    /// </summary>
    void CreateDeck()
    {
        for(int cardsInDeck = 0; cardsInDeck < numberOfNormalCards; cardsInDeck++)
        {
            deck.Add(cards[(int)CardType.Hill]);
            deck.Add(cards[(int)CardType.Valley]);
        }
        for(int bouldersInDeck = 0; bouldersInDeck < numberOfBoulderCards; bouldersInDeck++)
        {
            deck.Add(cards[(int)CardType.Boulder]);
        }
        for(int pebblesInDeck = 0; pebblesInDeck < numberOfPebbleCards; pebblesInDeck++)
        {
            deck.Add(cards[(int)CardType.Pebble]);
        }
    }


    /// <summary>
    /// Deal blank cards equal to cardsDealtEachRound after cardChangeTime has elapsd, and set CardsInPlay to true
    /// </summary>
    void DealCards()
    {
        for(int cardsDealt = 0; cardsDealt < cardsDealtEachRound; cardsDealt++)
        {
            GameObject blankCard = Instantiate(cardPrefabs[(int)CardType.Blank], cardSlots[cardsDealt], transform.rotation);
            BlankCards.Add(blankCard);
        }

        // Move the deck object down the equivilant thickness of cards dealt
        deckProp.transform.Translate(new Vector3(0, cardThickness * -cardsDealtEachRound, 0));
        CardsInPlay = true;
    }


    /// <summary>
    /// Find the card that the user has chosen, and translate it up if it is selected, and back down if it is deselected
    /// </summary>
    /// <param name="cardIndex"></param>
    /// <param name="selecting"></param>
    public void SelectOrDeselectCard(int cardIndex, bool selecting)
    {
        if(selecting)
        {
            BlankCards[cardIndex].transform.Translate(Vector3.up / selectionHeightModifier);
        }
        else
        {
            BlankCards[cardIndex].transform.Translate(Vector3.down / selectionHeightModifier);
        }
    }


    /// <summary>
    /// Reveal both selected cards by creating actual cards, then discard those and delete the blank cards
    /// </summary>
    public void RevealCards(int playerCardIndex, int foeCardIndex)
    {
        // Get random cards from the deck to reveal
        int randomFoeCard = Random.Range(0, deck.Count);
        int randomPlayerCard = Random.Range(0, deck.Count);

        // Create those random cards in the same place as the blanks
        Card newFoeCard = deck[randomFoeCard];
        Card newPlayerCard = deck[randomPlayerCard];
        newFoeCard.cardPrefab = Instantiate(newFoeCard.cardPrefab, BlankCards[foeCardIndex].transform.position, transform.rotation);
        newPlayerCard.cardPrefab = Instantiate(newPlayerCard.cardPrefab, BlankCards[playerCardIndex].transform.position, transform.rotation);
        
        // Remove the revealed cards from the deck, and clear the blankCards list
        StartCoroutine(DiscardCard(newFoeCard));
        StartCoroutine(DiscardCard(newPlayerCard));
        StartCoroutine(ManageCardOutcomes(playerCardIndex, foeCardIndex, newFoeCard, newPlayerCard));
    }


    /// <summary>
    /// After cardChangeTime has elapsed, move the revealed card to the discard pile
    /// </summary>
    /// <param name="cardIndex"></param>
    /// <param name="newCard"></param>
    /// <returns></returns>
    IEnumerator DiscardCard(Card newCard)
    {
        yield return new WaitForSeconds(cardChangeTime);

        newCard.cardPrefab.transform.position = discardLocation;
        newCard.cardPrefab.transform.Translate(new Vector3(0, cardThickness * discardPile.Count, 0));
        discardPile.Add(newCard);
    }


    /// <summary>
    /// After cardChangeTime has elapsed, destroy revealed cards, and manage the consequences of each card
    /// </summary>
    /// <returns></returns>
    IEnumerator ManageCardOutcomes(int firstIndex, int secondIndex, Card firstCard, Card secondCard)
    {
        yield return new WaitForSeconds(cardChangeTime);

        // Delete the revealed cards
        Destroy(BlankCards[firstIndex]);
        Destroy(BlankCards[secondIndex]);

        // If it's a Valley, the other player discards a card
        if(firstCard.cardType == CardType.Valley)
        {
            // Update the foe's health with valley's damage
            gameManager.UpdateHealth(false, cards[firstIndex].damage);


            // Delete the first card that hasn't been destroyed already
            for(int foeCards = foeMinIndex; foeCards < foeMaxIndex; foeCards++)
            {
                if(BlankCards[foeCards] != null)
                {
                    Destroy(BlankCards[foeCards]);
                    break;
                }
            }
        }
        if(secondCard.cardType == CardType.Valley)
        {
            // Update the player's health with valley's damage
            gameManager.UpdateHealth(true, cards[secondIndex].damage);

            // Delete the first card that hasn't been destroyed already
            for(int playerCards = playerMinIndex; playerCards < playerMaxIndex; playerCards++)
            {
                if(BlankCards[playerCards] != null)
                {
                    Destroy(BlankCards[playerCards]);
                    break;
                }
            }
        }

        

        // If it's a Hill, replace the card with a new one
        if(firstCard.cardType == CardType.Hill)
        {
            // Create a new blank card where the revealed card used to be
            BlankCards[firstIndex] = Instantiate(cards[(int)CardType.Blank].cardPrefab, cardSlots[firstIndex], transform.rotation);
            
            // Update the player's health with hill's healing
            gameManager.UpdateHealth(true, cards[firstIndex].damage);

            // Move the deck object down one card thickness
            deckProp.transform.Translate(new Vector3(0, cardThickness * -(cardsDealtEachRound / cardsDealtEachRound), 0));
        }
        if(secondCard.cardType == CardType.Hill)
        {
            // Create a new blank card where the revealed card used to be
            BlankCards[secondIndex] = Instantiate(cards[(int)CardType.Blank].cardPrefab, cardSlots[secondIndex], transform.rotation);
            
            // Update the foe's health with hill's healing
            gameManager.UpdateHealth(false, cards[secondIndex].damage);

            // Move the deck object down one card thickness
            deckProp.transform.Translate(new Vector3(0, cardThickness * -(cardsDealtEachRound / cardsDealtEachRound), 0));
        }


        
    }   
}

