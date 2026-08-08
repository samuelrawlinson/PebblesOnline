using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance {get; private set;}

    [Header("Music")]
    public AudioSource MusicSource;
    [SerializeField] private AudioClip menuMusic;
    [SerializeField] private AudioClip gameMusic;

    [Header("SFX")]
    public AudioSource SFXSource;
    [SerializeField] private AudioClip buttonClick;
    [SerializeField] private AudioClip drawCard;
    [SerializeField] private AudioClip boulderThrow;
    [SerializeField] private AudioClip impactCrunch;
    [SerializeField] private AudioClip ghostCry;
    [SerializeField] private AudioClip cardSelect;

    void Awake()
    {
        if(Instance != null & Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        
    }

    void Start()
    {
        UpdateMusic(false);
        MusicSource.volume = 0.5f;
    }


    /// <summary>
    /// If the scene is the main menu, play the main menu music. If it's not, play the game music
    /// </summary>
    public void UpdateMusic(bool hasGameStarted)
    {
        if(MusicSource != null && hasGameStarted == false)
        {
            MusicSource.clip = menuMusic;
            MusicSource.loop = true;
            MusicSource.Play();
        }
        else if(MusicSource != null && hasGameStarted == true)
        {
            MusicSource.Stop();
            MusicSource.clip = gameMusic;
            MusicSource.loop = true;
            MusicSource.Play();
        }
    }
}
