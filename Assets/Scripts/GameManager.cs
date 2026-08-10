using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; private set;}
    public static int PlayerWins;
    public static int FoeWins;

    [Header("Publics")]
    public bool IsPaused { get; private set; } = false;
    public bool IsGameOver { get; private set;} = false;
    public UnityEvent OnMiniGameStart = new UnityEvent();
    public UnityEvent OnMiniGameEnd = new UnityEvent();
    public UnityEvent OnGameOver = new UnityEvent();
    public GameObject boulderPrefab;

    [Header("Managers")]
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private DeckManager deckManager;
    [SerializeField] private HUDManager hudManager;

    [Header("Players")]
    [SerializeField] private int winsNeededToBecomePebbler = 3;
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private FoeHealth foeHealth;
    [SerializeField] private FoeActions foeActions;
    public PlayerActions PlayerActions;


    public enum GameMode
    {
        Playing,
        Dodging,
        Throwing,
    }


    void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        if(audioManager == null)
        {
            audioManager = AudioManager.Instance;
        }
        if(deckManager == null)
        {
            deckManager = FindAnyObjectByType<DeckManager>();
        }
        if(hudManager == null)
        {
            hudManager = FindAnyObjectByType<HUDManager>();
        }
        if(playerHealth == null)
        {
            playerHealth = FindAnyObjectByType<PlayerHealth>();
        }
        if(foeHealth == null)
        {
            foeHealth = FindAnyObjectByType<FoeHealth>();
        }
        if(PlayerActions == null)
        {
            PlayerActions = FindAnyObjectByType<PlayerActions>();
        }
        if(foeActions == null)
        {
            foeActions = FindAnyObjectByType<FoeActions>();
        }

        hudManager.UpdateRoundStats(PlayerWins, FoeWins);
        audioManager.UpdateMusic(true);
    }

    public void ManageGameModes(GameMode player, GameMode foe)
    {
        PlayerActions.CurrentMode = player;
        foeActions.CurrentMode = foe;
        Debug.Log(player);
        Debug.Log(foe);
    }

    public void UpdateHealth(bool isPlayer, int amount)
    {
        if(isPlayer)
        {
            playerHealth.UpdateHealth(amount);
        }
        else
        {
            foeHealth.UpdateHealth(amount);
        }
    }

    private void UpdateRound()
    {
        // Update player's overlay text
        hudManager.UpdateRoundStats(PlayerWins, FoeWins);
        OnGameOver?.Invoke();
        IsGameOver = true;
    }

    public void ManageWins()
    {
        if(playerHealth.CurrentHealth <= 0 && foeHealth.CurrentHealth <= 0)
        {
            Debug.Log("You tied this round");
            FoeWins++;
            PlayerWins++;
            UpdateRound();
            hudManager.YouTied.SetActive(true);
        }
        else if(foeHealth.CurrentHealth <= 0)
        {
            Debug.Log("You win this round!");
            PlayerWins++;
            UpdateRound();
            hudManager.YouWin.SetActive(true);
        }
        else if(playerHealth.CurrentHealth <= 0)
        {
            Debug.Log("You lose this round");
            audioManager.PlaySoundEffect(AudioManager.SoundEffect.WompWomp);
            FoeWins++;
            UpdateRound();
            hudManager.YouLose.SetActive(true);
        }


        // Declare Pebbler
        if(PlayerWins >= winsNeededToBecomePebbler && FoeWins >= winsNeededToBecomePebbler)
        {
            ManagePebblerOutcomes();
            hudManager.BothBecomePebbler.SetActive(true);
            Debug.Log("Both players are the new Pebbler!");
        }
        else if(PlayerWins >= winsNeededToBecomePebbler)
        {
            ManagePebblerOutcomes();
            hudManager.YouBecomePebbler.SetActive(true);
            Debug.Log("Player is the new Pebbler!");
        }
        else if(FoeWins >= winsNeededToBecomePebbler)
        {
            ManagePebblerOutcomes();
            hudManager.FoeBecomesPebbler.SetActive(true);
            Debug.Log("Foe is the new Pebbler!");
        }
    }

    private void ManagePebblerOutcomes()
    {
        IsGameOver = true;
        PlayerWins = 0;
        FoeWins = 0;
    }

    public void Pause()
    {
        IsPaused = true;
        Time.timeScale = 0;
    }

    public void Resume()
    {
        IsPaused = false;
        Time.timeScale = 1;
    }
}
