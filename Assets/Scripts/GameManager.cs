using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; private set;}
    public static int PlayerWins = 0;
    public static int FoeWins = 0;
    public static int Round = 1;

    [Header("Public Variables")]
    public bool IsPaused { get; private set; } = false;
    public bool IsGameOver { get; private set;} = false;
    public GameObject boulderPrefab;

    [Header("Managers")]
    [SerializeField] private GhostSpawner ghostSpawner;
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private FoeHealth foeHealth;
    [SerializeField] private DeckManager deckManager;
    [SerializeField] private HUDManager hudManager;

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
        if(ghostSpawner == null)
        {
            ghostSpawner = FindAnyObjectByType<GhostSpawner>();
        }
        if(audioManager == null)
        {
            audioManager = FindAnyObjectByType<AudioManager>();
        }
        if(playerHealth == null)
        {
            playerHealth = FindAnyObjectByType<PlayerHealth>();
        }
        if(foeHealth == null)
        {
            foeHealth = FindAnyObjectByType<FoeHealth>();
        }
        if(deckManager == null)
        {
            deckManager = FindAnyObjectByType<DeckManager>();
        }
        if(hudManager == null)
        {
            hudManager = FindAnyObjectByType<HUDManager>();
        }
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
        // Update overlay text
        hudManager.UpdateRoundStats(PlayerWins, FoeWins, Round);
        Round++;
        IsGameOver = true;
    }

    public void ManageWins()
    {
        if(playerHealth.CurrentHealth <= 0 && foeHealth.CurrentHealth <= 0)
        {
            Debug.Log("You tied round " + Round);
            FoeWins++;
            PlayerWins++;
            UpdateRound();
            hudManager.YouTied.SetActive(true);
        }
        else if(foeHealth.CurrentHealth <= 0)
        {
            Debug.Log("You win round " + Round + "!");
            PlayerWins++;
            UpdateRound();
            hudManager.YouWin.SetActive(true);
        }
        else if(playerHealth.CurrentHealth <= 0)
        {
            Debug.Log("You lose round " + Round);
            FoeWins++;
            UpdateRound();
            hudManager.YouLose.SetActive(true);
        }


        // Declare Pebbler
        if(PlayerWins >= 3)
        {
            IsGameOver = true;
            Debug.Log("Player is the new Pebbler!");
        }
        else if(FoeWins >= 3)
        {
            IsGameOver = true;
            Debug.Log("Foe is the new Pebbler!");
        }
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
