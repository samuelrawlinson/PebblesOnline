using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerMiniGameBehavior : MonoBehaviour
{
    [Header("Game Mode")]
    public bool IsPlayerBoulderHolder = false;
    [SerializeField] private float maxThrowDistance = 6;
    [SerializeField] private Vector3 dodgingPosition = new Vector3(0, 0, -6);
    [SerializeField] private Vector3 playingPosition = new Vector3(0, 0, -4);

    [Header("References")]
    [SerializeField] private Transform target;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerActions player;
    [SerializeField] private GameObject personalBoulder;
    [SerializeField] private Vector3 boulderOffset = new Vector3(0f,3.25f,0f);


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        player = GetComponent<PlayerActions>();
        gameManager = GameManager.Instance;
    }

    void OnEnable()
    {
        gameManager.OnMiniGameStart.AddListener(SetGameMode);
    }

    void OnDisable()
    {
        gameManager.OnMiniGameStart.RemoveListener(SetGameMode);
    }

    void Update()
    {
        if(Input.GetButtonDown("Shoot"))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // ThrowBoulder()
        }
    }

    private void SetGameMode()
    {
        if(player.CurrentMode == GameManager.GameMode.Dodging)
        {
            transform.position = dodgingPosition;
            Debug.Log("Player in dodging position");
        }
        else if(player.CurrentMode == GameManager.GameMode.Playing)
        {
            transform.position = playingPosition;
            Debug.Log("Player in playing position");
        }
        else if(player.CurrentMode == GameManager.GameMode.Throwing)
        {
            IsPlayerBoulderHolder = true;
            GetNewBoulder();
            Debug.Log("Player in throwing position");
        }
    }

    /// <summary>
    /// If the player is the Boulder Holder, instantiate a new boulder for them to throw
    /// </summary>
    private void GetNewBoulder()
    {
        if(IsPlayerBoulderHolder)
        {
            animator.SetBool("IsHoldingBoulder", true);
            personalBoulder = Instantiate(gameManager.boulderPrefab, transform.position + boulderOffset, transform.rotation);
        }
    }

    /// <summary>
    /// No longer the Boulder Holder, so change the animation and throw the boulder
    /// </summary>
    private void ThrowBoulder(Vector3 target)
    {
        IsPlayerBoulderHolder = false;
        animator.SetBool("IsHoldingBoulder", false);
        // personalBoulder.GetComponent<Boulder>().BeThrownByHolder(Physics.Raycast(transform.position, target, maxThrowDistance));
    }
}
