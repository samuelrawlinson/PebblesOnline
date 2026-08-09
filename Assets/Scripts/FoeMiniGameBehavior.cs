using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class FoeMiniGameBehavior : MonoBehaviour
{
    [Header("Throwing")]
    [SerializeField] private Boulder boulder;
    [SerializeField] private GameObject boulderObject;
    [SerializeField] public bool IsFoeBoulderHolder = false;
    [SerializeField] private Vector3 playingPosition = new Vector3(0, 0, 4);
    [SerializeField] private Vector3 boulderOffset = new Vector3(0f,3.25f,0f);

    [Header("Dodging")]
    [SerializeField] private float agentSpeed = 10f;
    [SerializeField] private float minXValue = -7f;
    [SerializeField] private float maxXValue = 7f;
    [SerializeField] private float decisionTime = 2f;
    [SerializeField] private Vector3 randomDestination = new Vector3(0, 0, 6);

    [Header("References")]
    [SerializeField] private GameManager gameManager;
    [SerializeField] private Transform target;
    [SerializeField] private FoeActions foe;
    [SerializeField] private UnityEngine.AI.NavMeshAgent agent;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        foe = GetComponent<FoeActions>();
        gameManager = GameManager.Instance;

        gameManager.OnMiniGameStart.AddListener(SetGameMode);
        gameManager.OnMiniGameEnd.AddListener(ReturnToPlayState);
        agent.speed = agentSpeed;
    }

    void OnDisable()
    {
        gameManager.OnMiniGameStart.RemoveListener(SetGameMode);
        gameManager.OnMiniGameEnd.RemoveListener(ReturnToPlayState);
    }

    private void ReturnToPlayState()
    {
        foe.CurrentMode = GameManager.GameMode.Playing;
        IsFoeBoulderHolder = false;
        SetGameMode();
    }


    private void SetGameMode()
    {
        if(foe.CurrentMode == GameManager.GameMode.Dodging)
        {
            StartCoroutine("PickRandomDestination");
            Debug.Log("Foe in dodging position");
        }
        else if(foe.CurrentMode == GameManager.GameMode.Throwing)
        {
            IsFoeBoulderHolder = true;
            GetNewBoulder();
            StartCoroutine("ThrowBoulder");
            Debug.Log("Foe in throwing position");
        }
        else if(foe.CurrentMode == GameManager.GameMode.Playing)
        {
            agent.SetDestination(playingPosition);
            foe.StartCoroutine("ChooseCard");
            Debug.Log("Foe in playing position");
        }
    }

    public IEnumerator PickRandomDestination()
    {
        yield return new WaitForSeconds(decisionTime / decisionTime);

        if(foe.CurrentMode == GameManager.GameMode.Dodging)
        {
            float randomXValue = Random.Range(minXValue, maxXValue);
            randomDestination.x = randomXValue;
            agent.SetDestination(randomDestination);

            StartCoroutine("PickRandomDestination");
        }
    }


    /// <summary>
    /// If the foe is the Boulder Holder, instantiate a new boulder for them to throw
    /// </summary>
    private void GetNewBoulder()
    {
        if(IsFoeBoulderHolder)
        {
            foe.Animator.SetBool("IsHoldingBoulder", true);
            boulderObject = Instantiate(gameManager.boulderPrefab, transform.position + boulderOffset, transform.rotation);
            boulder = boulderObject.GetComponent<Boulder>();    
        }
    }

    /// <summary>
    /// No longer the Boulder Holder, so change the animation and throw the boulder
    /// </summary>
    IEnumerator ThrowBoulder()
    {
        if(boulder.Strikes < boulder.StrikesTilOut)
        {
            yield return new WaitForSeconds(decisionTime);

            IsFoeBoulderHolder = false;
            foe.Animator.SetBool("IsHoldingBoulder", false);


            boulder.BeThrownByHolder(target.position + new Vector3(0, 2, 0));
            Debug.Log("Strikes: " + boulder.Strikes);
            // TODO make it the length of the throw animation
            yield return new WaitForSeconds(decisionTime + decisionTime);

            foe.Animator.SetBool("IsHoldingBoulder", true);
            StartCoroutine("ThrowBoulder");
        }
        else
        {
            IsFoeBoulderHolder = false;
            foe.Animator.SetBool("IsHoldingBoulder", false);
        }
    }


}
