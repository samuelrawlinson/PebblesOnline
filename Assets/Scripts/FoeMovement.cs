using UnityEngine;
using UnityEngine.AI;

public class FoeMovement : MonoBehaviour
{
    // Declaration of Variables
    private NavMeshAgent agent;
    [SerializeField] private Transform target;


    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = 9f;
        agent.stoppingDistance = 5f;
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            agent.SetDestination(target.transform.position + new  Vector3(1f, 0f, 1f));
        }
    }
}
