using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; private set;}
    public static float gamesWon = 0;
    public bool IsPaused = false;

    [Header("Managers")]
    [SerializeField] private GhostSpawner ghostSpawner;
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private DeckManager deckManager;
    public HUDManager HUDManager {get; private set;}

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
        if(deckManager != null)
        {
            deckManager = FindAnyObjectByType<DeckManager>();
        }
        if(HUDManager != null)
        {
            HUDManager = FindAnyObjectByType<HUDManager>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RevealAllCards()
    {
        deckManager.RevealCards();
    }

    public void PlayerLose()
    {
        return;
    }

    public void Pause()
    {
        IsPaused = true;
        Time.timeScale = 0f;
    }

    public void Unpause()
    {
        IsPaused = false;
        Time.timeScale = 1f;
    }
}
