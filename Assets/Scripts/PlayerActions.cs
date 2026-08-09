using System;
using System.Xml.Schema;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerActions : MonoBehaviour
{
    [Header("Cards")]
    [SerializeField] private bool isCardSelected = false;
    [SerializeField] private int cardIndexSelected;
    public event Action OnCardsPlayed;


    [Header("References")]
    public GameManager.GameMode CurrentMode;
    [SerializeField] private DeckManager deck;
    [SerializeField] private FoeActions foe;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private PlayerMiniGameBehavior playerBoulderBehavior;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = GameManager.Instance;
        playerBoulderBehavior = GetComponent<PlayerMiniGameBehavior>();
        deck = GameObject.Find("CardManager").GetComponent<DeckManager>();
        foe = GameObject.Find("Foe").GetComponent<FoeActions>();
        CurrentMode = GameManager.GameMode.Playing;
    }

    void OnEnable()
    {
        OnCardsPlayed += RevealCards;
    }

    void OnDisable()
    {
        OnCardsPlayed -= RevealCards;
    }

    void Update()
    {
        // If the player hasn't chosen a card yet, then you can select one
        // DISCLAIMER: I genuinely wasn't sure if there's a better way to do these index checks and assignments without magic numbers
        if(isCardSelected == false && gameManager.IsGameOver != true)
        {
            if(Input.GetButtonDown("SelectCardOne"))
            {
                if(deck.BlankCards[0] != null)
                {
                    cardIndexSelected = 0;
                    SelectCard();
                }
                else
                {
                    Debug.Log("There is no card here: choose another one");
                }
            }

            else if(Input.GetButtonDown("SelectCardTwo"))
            {
                if(deck.BlankCards[1] != null)
                {
                    cardIndexSelected = 1;
                    SelectCard();
                }
                else
                {
                    Debug.Log("There is no card here: choose another one");
                }
            }

            else if(Input.GetButtonDown("SelectCardThree"))
            {
                if(deck.BlankCards[2] != null)
                {
                    cardIndexSelected = 2;
                    SelectCard();
                }
                else
                {
                    Debug.Log("There is no card here: choose another one");
                }
            }
        }

        // If there IS a card selected, you can deselect it
        else
        {
            if(Input.GetButtonDown("DeselectCard"))
            {
                DeselectCard(cardIndexSelected); 
            }
        }

        // If both players have a card selected, trigger the OnCardsPlayed event
        if(isCardSelected && foe.HasCardSelected)
        {
            if(Input.GetButtonDown("Ready"))
            {
                OnCardsPlayed?.Invoke();
            }
        }
    }


    public bool GetPlayerBoulderHoldingStatus()
    {
        return playerBoulderBehavior.IsPlayerBoulderHolder;
    }
    


    /// <summary>
    /// Pass the selected card to the DeckManager and set isCardSelected to true
    /// </summary>
    void SelectCard()
    {
        deck.SelectOrDeselectCard(cardIndexSelected, true); 
        isCardSelected = true;   
    }
    
    /// <summary>
    /// Pass the selected card to the DeckManager and set isCardSelected to fakse
    /// </summary>'
    void DeselectCard(int cardSlot)
    {
        deck.SelectOrDeselectCard(cardSlot, false);  
        isCardSelected = false; 
    }

    /// <summary>
    /// Pass both selected card indexes to the DeckManager and set both selections to false
    /// </summary>
    void RevealCards()
    {
        deck.RevealCards(cardIndexSelected, foe.CardIndexSelected);
        isCardSelected = false;
        foe.HasCardSelected = false;
        foe.StartCoroutine("ChooseCard");
    }
}
