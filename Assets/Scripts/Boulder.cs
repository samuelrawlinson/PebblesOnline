using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Boulder : MonoBehaviour
{
    [Header("Boulder")]
    public GameObject boulderHolder;
    public int Strikes = 0;
    public int StrikesTilOut = 3;
    [SerializeField] private int boulderDamage = -3;
    [SerializeField] private FoeMiniGameBehavior foeBehavior;
    [SerializeField] private PlayerMiniGameBehavior playerBehavior;

    [Header("References")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private DeckManager deckManager;
    [SerializeField] private Vector3 boulderOffset = new Vector3(0f,3.25f,0f);
    [SerializeField] private float throwingForce = 20f;


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
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player") && gameManager.PlayerActions.CurrentMode != GameManager.GameMode.Throwing)
        {
            // Update the player's health with boulder's damage
            EndMinigame(true);
        }
        if(collision.gameObject.CompareTag("Foe") && gameManager.PlayerActions.CurrentMode != GameManager.GameMode.Dodging)
        {
            // Update the foe's health with boulder's damage
            EndMinigame(false);
           
        }
        if(collision.gameObject.CompareTag("Wall"))
        {
            Debug.Log("Boulder hit the wall");
            if(gameManager.PlayerActions.CurrentMode == GameManager.GameMode.Throwing)
            {
                Debug.Log("Boulder returned to player");
                playerBehavior.IsPlayerBoulderHolder = true;
            }
            else if(gameManager.PlayerActions.CurrentMode == GameManager.GameMode.Dodging)
            {
                Debug.Log("Boulder returned to foe");
                foeBehavior.IsFoeBoulderHolder = true;
            }

            Strikes++;

            if(Strikes >= StrikesTilOut)
            {
                Debug.Log("struck out, back to game");
                gameManager.OnMiniGameEnd?.Invoke();
                Destroy(gameObject);
            }
        }
    }

    private void EndMinigame(bool damagePlayer)
    {
        gameManager.OnMiniGameEnd?.Invoke();
        gameManager.UpdateHealth(damagePlayer, boulderDamage);
        Destroy(gameObject);
    }
}
