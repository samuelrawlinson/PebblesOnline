using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; private set;}
    public static float gamesWon = 0;

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
        if(ghostSpawner != null)
        {
            ghostSpawner = FindAnyObjectByType<GhostSpawner>();
        }
        if(audioManager != null)
        {
            audioManager = FindAnyObjectByType<AudioManager>();
        }
        if(playerHealth != null)
        {
            playerHealth = FindAnyObjectByType<PlayerHealth>();
        }
        if(foeHealth != null)
        {
            foeHealth = FindAnyObjectByType<FoeHealth>();
        }
        if(deckManager != null)
        {
            deckManager = FindAnyObjectByType<DeckManager>();
        }
        if(hudManager != null)
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

    public void PlayerLose()
    {
        IsGameOver = true;
        Debug.Log("You lose!");
    }

    public void PlayerWin()
    {
        IsGameOver = true;
        Debug.Log("You win!");
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
