using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.XR;

public class FoeActions : MonoBehaviour
{
    // Declaration of Variables
    [Header("Cards")]
    [SerializeField] public bool HasCardSelected;
    [SerializeField] public int CardIndexSelected;
    [SerializeField] private int lowestCardIndex = 3;
    [SerializeField] private int highestCardIndex = 6; // exclusive
    [SerializeField] private float selectionThinkTime = 3;
    

    [Header("References")]
    public Animator Animator;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private DeckManager deck;

    [Header("AI")]
    public GameManager.GameMode CurrentMode = GameManager.GameMode.Playing;


    void Start()
    {
        Animator = GetComponent<Animator>();
        gameManager = GameManager.Instance;
        deck = GameObject.Find("CardManager").GetComponent<DeckManager>();
        StartCoroutine("ChooseCard");
    }

    /// <summary>
    /// Select a card at random, and set HasCardSelected to true
    /// </summary>
    /// <returns></returns>
    public IEnumerator ChooseCard()
    {
        yield return new WaitForSeconds(selectionThinkTime);

        while(HasCardSelected != true && gameManager.IsGameOver != true && CurrentMode == GameManager.GameMode.Playing)
        { 
            CardIndexSelected = Random.Range(lowestCardIndex, highestCardIndex);

            // Avoid empty card slots until you find a live one
            if(deck.BlankCards[CardIndexSelected] != null)
            {
                deck.SelectOrDeselectCard(CardIndexSelected, true);
                HasCardSelected = true;
            }
            if(gameManager.IsGameOver)
            {
                break;
            }
        }
    }
}
