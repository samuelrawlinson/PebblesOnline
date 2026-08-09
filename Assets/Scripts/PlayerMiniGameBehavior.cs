using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerMiniGameBehavior : MonoBehaviour
{
    [Header("Game Modes")]
    public bool IsPlayerBoulderHolder = false;
    [SerializeField] private Vector3 dodgingPosition = new Vector3(0, 0, -6);
    [SerializeField] private Vector3 playingPosition = new Vector3(0, 0, -4);

    [Header("Boulder")]
    [SerializeField] private Boulder boulder;
    [SerializeField] private GameObject boulderObject;
    [SerializeField] private Vector3 boulderOffset = new Vector3(0f,3.25f,0f);

    [Header("References")]
    [SerializeField] private Transform target;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private Animator animator;
    [SerializeField] private DeckManager deck;
    [SerializeField] private PlayerActions player;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        player = GetComponent<PlayerActions>();
        deck = FindAnyObjectByType<DeckManager>();
        gameManager = GameManager.Instance;

        // Add subscribers
        gameManager.OnMiniGameStart.AddListener(SetGameMode);
        gameManager.OnMiniGameEnd.AddListener(ReturnToPlayState);
    }

    void OnDisable()
    {
        gameManager.OnMiniGameStart.RemoveListener(SetGameMode);
        gameManager.OnMiniGameEnd.RemoveListener(ReturnToPlayState);
    }

    void Update()
    {
        if(Input.GetButtonDown("ShootBoulder") && player.CurrentMode == GameManager.GameMode.Throwing)
        {
            ThrowBoulder();
        }
    }

    private void ReturnToPlayState()
    {
        player.CurrentMode = GameManager.GameMode.Playing;
        IsPlayerBoulderHolder = false;
        deck.RoundlyDamage();

        if(gameManager.IsGameOver != true)
        {
            gameManager.ManageWins();
        }

        SetGameMode();
    }

    private void SetGameMode()
    {
        if(player.CurrentMode == GameManager.GameMode.Dodging)
        {
            transform.position = dodgingPosition;
            Debug.Log("Player in dodging position");
        }
        else if(player.CurrentMode == GameManager.GameMode.Playing && transform.position != playingPosition && gameManager.IsGameOver != true)
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
            boulderObject = Instantiate(gameManager.boulderPrefab, transform.position + boulderOffset, transform.rotation);
            boulder = boulderObject.GetComponent<Boulder>();
        }
    }

    /// <summary>
    /// No longer the Boulder Holder, so change the animation and throw the boulder
    /// </summary>
    private void ThrowBoulder()
    {
        IsPlayerBoulderHolder = false;
        animator.SetBool("IsHoldingBoulder", false);
        Vector3 throwDirection = Camera.main.transform.forward;
        boulder.BeThrownByHolder(throwDirection);
    }
}
