using UnityEngine;
using UnityEngine.AI;

public class FoeMovement : MonoBehaviour
{
    // Declaration of Variables
    [Header("Boulder")]
    [SerializeField] private GameObject newBoulder;
    [SerializeField] private float maxThrowDistance = 3f;
    [SerializeField] private GameObject personalBoulder;
    [SerializeField] public bool IsBoulderHolder = false;

    [Header("References")]
    [SerializeField] private Transform target;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Animator animator;
    
    

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        agent.speed = 9f;
        agent.stoppingDistance = 5f;
        personalBoulder = Instantiate(newBoulder, transform.position, transform.rotation);
        personalBoulder.GetComponent<Boulder>().boulderHolder = gameObject;
    }

    void Start()
    {
        gameManager = GameManager.Instance;

        if(IsBoulderHolder)
        {
            animator.SetBool("IsHoldingBoulder", true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // if (target != null)
        // {
        //     agent.SetDestination(target.transform.position);
        // }
    }

    void FixedUpdate()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, target.position);

        if(distanceToPlayer <= maxThrowDistance && personalBoulder != null)
        {
            ThrowBoulder();
        }
    }

    /// <summary>
    /// No longer the Boulder Holder, so change the animation and throw the boulder
    /// </summary>
    void ThrowBoulder()
    {
        IsBoulderHolder = false;
        animator.SetBool("IsHoldingBoulder", false);
        personalBoulder.GetComponent<Boulder>().BeThrownByHolder(target.position);
    }
}
