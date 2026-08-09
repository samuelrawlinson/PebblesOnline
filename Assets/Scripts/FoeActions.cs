using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class FoeActions : MonoBehaviour
{
    // Declaration of Variables
    [Header("Cards")]
    [SerializeField] public bool HasCardSelected;
    [SerializeField] public int CardIndexSelected;
    [SerializeField] private int lowestCardIndex = 3;
    [SerializeField] private int highestCardIndex = 6; // exclusive

    [SerializeField] private float  selectionThinkTime;
    [SerializeField] private float fastestThinkTime = 3;
    [SerializeField] private float slowestThinkTime = 5;
    
    
    [Header("Boulder")]
    [SerializeField] private float maxThrowDistance = 3f;
    [SerializeField] private GameObject personalBoulder;
    [SerializeField] public bool IsBoulderHolder = false;

    [Header("References")]
    [SerializeField] private GameManager gameManager;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform target;
    [SerializeField] private DeckManager deck;

    [Header("AI")]
    [SerializeField] GameManager.GameMode currentMode = GameManager.GameMode.Playing;


    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        agent.speed = 9f;
        agent.stoppingDistance = 5f;
        
    }

    void Start()
    {
        gameManager = GameManager.Instance;
        deck = GameObject.Find("CardManager").GetComponent<DeckManager>();
        selectionThinkTime = Random.Range(fastestThinkTime, slowestThinkTime);
        StartCoroutine("ChooseCard");
    }


    void FixedUpdate()
    {
        // float distanceToPlayer = Vector3.Distance(transform.position, target.position);

        // if(distanceToPlayer <= maxThrowDistance && personalBoulder != null)
        // {
        //     ThrowBoulder();
        // }
    }

    /// <summary>
    /// Select a card at random, and set HasCardSelected to true
    /// </summary>
    /// <returns></returns>
    IEnumerator ChooseCard()
    {
        yield return new WaitForSeconds(selectionThinkTime);

        while(HasCardSelected != true && gameManager.IsGameOver != true)
        { 
            CardIndexSelected = Random.Range(lowestCardIndex, highestCardIndex);

            // Avoid empty card slots until you find a live one
            if(deck.BlankCards[CardIndexSelected] != null)
            {
                deck.SelectOrDeselectCard(CardIndexSelected, true);
                HasCardSelected = true;
            }
        }
    

        
    }

    /// <summary>
    /// If the foe is the Boulder Holder, instantiate a new boulder for them to throw
    /// </summary>
    private void GetNewBoulder()
    {
        if(IsBoulderHolder)
        {
            animator.SetBool("IsHoldingBoulder", true);
            personalBoulder = Instantiate(gameManager.boulderPrefab, transform.position, transform.rotation);
            personalBoulder.GetComponent<Boulder>().boulderHolder = gameObject;
        }
    }

    /// <summary>
    /// No longer the Boulder Holder, so change the animation and throw the boulder
    /// </summary>
    private void ThrowBoulder()
    {
        IsBoulderHolder = false;
        animator.SetBool("IsHoldingBoulder", false);
        personalBoulder.GetComponent<Boulder>().BeThrownByHolder(target.position);
    }
}
