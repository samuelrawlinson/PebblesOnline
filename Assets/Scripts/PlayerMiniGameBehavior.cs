using UnityEngine;

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
        gameManager.OnGameOver.AddListener(EndCoroutines);
    }

    void OnDisable()
    {
        gameManager.OnMiniGameStart.RemoveListener(SetGameMode);
        gameManager.OnMiniGameEnd.RemoveListener(ReturnToPlayState);
        gameManager.OnGameOver.RemoveListener(EndCoroutines);
    }

    void Update()
    {
        // If the game isn't over, the player is supposed to be holding the boulder, 
        // and the player actually is holding the boulder, then it can be thrown
        if(Input.GetButtonDown("ShootBoulder") && 
            gameManager.IsGameOver != true &&
            player.CurrentMode == GameManager.GameMode.Throwing && 
            IsPlayerBoulderHolder == true)
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
        else if(player.CurrentMode == GameManager.GameMode.Playing && gameManager.IsGameOver != true)
        {
            // Un-squish player model if squished
            UpdateAnimations("IsCrushed", false);
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

    public void UpdateAnimations(string boolName, bool active)
    {
        animator.SetBool(boolName, active);
    }

    private void EndCoroutines()
    {
        StopAllCoroutines();
    }
}
