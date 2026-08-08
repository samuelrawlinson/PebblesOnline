using UnityEngine;
using UnityEngine.AI;

public class FoeActions : MonoBehaviour
{
    // Declaration of Variables
    [Header("Boulder")]
    [SerializeField] private float maxThrowDistance = 3f;
    [SerializeField] private GameObject personalBoulder;
    [SerializeField] public bool IsBoulderHolder = false;

    [Header("References")]
    [SerializeField] private GameManager gameManager;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform target;

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
