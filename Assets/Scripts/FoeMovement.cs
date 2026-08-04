using UnityEngine;
using UnityEngine.AI;

public class FoeMovement : MonoBehaviour
{
    // Declaration of Variables
    private NavMeshAgent agent;
    [SerializeField] private Transform target;
    [SerializeField] private GameObject newBoulder;
    [SerializeField] private float maxThrowDistance = 3f;
    private GameObject personalBoulder;
    private Animator animator;
    public bool isBoulderHolder = true;
    

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
        if(isBoulderHolder)
        {
            animator.SetBool("IsHoldingBoulder", true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            agent.SetDestination(target.transform.position);
        }
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
        isBoulderHolder = false;
        animator.SetBool("IsHoldingBoulder", false);
        personalBoulder.GetComponent<Boulder>().BeThrownByHolder(target.position);
    }
}
