using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Boulder : MonoBehaviour
{
    [Header("Boulder")]
    public GameObject boulderHolder;
    [SerializeField] private FoeMiniGameBehavior foeBehavior;
    [SerializeField] private PlayerMiniGameBehavior playerBehavior;

    [Header("References")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private DeckManager deckManager;
    [SerializeField] private Vector3 boulderOffset = new Vector3(0f,3.25f,0f);
    [SerializeField] private float throwingForce = 10f;
    [SerializeField] private float lifeTime = 0.5f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        gameManager = GameManager.Instance;
        deckManager = GameObject.Find("CardManager").GetComponent<DeckManager>();
        

        // If the player is the boulder holder, find it, if not, find the foe
        if(gameManager.PlayerActions.GetPlayerBoulderHoldingStatus() == true)
        {
            boulderHolder = GameObject.Find("Player");
            playerBehavior = boulderHolder.GetComponent<PlayerMiniGameBehavior>();
            foeBehavior = GameObject.Find("Foe").GetComponent<FoeMiniGameBehavior>();
        }
        else
        {
            boulderHolder = GameObject.Find("Foe");
            foeBehavior = boulderHolder.GetComponent<FoeMiniGameBehavior>();
            playerBehavior = GameObject.Find("Player").GetComponent<PlayerMiniGameBehavior>();
        }
    }


    // Update is called once per frame
    void Update()
    {
        if(foeBehavior.IsFoeBoulderHolder || playerBehavior.IsPlayerBoulderHolder)
        {
            transform.position = boulderHolder.transform.position + boulderOffset;
        }
    }


    /// <summary>
    /// Find the difference between the target and current positions to find direction, then throw in that direction
    /// </summary>
    /// <param name="targetPosition"></param>
    public void BeThrownByHolder(Vector3 targetPosition)
    {
        Vector3 throwingDirection = (targetPosition - transform.position).normalized;
        rb.AddForce(throwingDirection * throwingForce, ForceMode.Impulse);
        StartCoroutine("DestroyIfMissed", lifeTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player") && gameManager.PlayerActions.CurrentMode != GameManager.GameMode.Throwing)
        {
            // Update the player's health with boulder's damage
            gameManager.UpdateHealth(true, deckManager.Cards[(int)DeckManager.CardType.Boulder].damage);
            gameManager.ManageWins();
            Destroy(gameObject);
        }
        if(collision.gameObject.CompareTag("Foe") && gameManager.PlayerActions.CurrentMode != GameManager.GameMode.Dodging)
        {
            // Update the foe's health with boulder's damage
            gameManager.UpdateHealth(false, deckManager.Cards[(int)DeckManager.CardType.Boulder].damage);
            gameManager.ManageWins();
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }

    IEnumerator DestroyIfMissed(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Destroy(gameObject);
    }
}
