using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance {get; private set;}

    [Header("Music")]
    public AudioSource MusicSource;
    public static float MusicVolume { get; private set;}
    [SerializeField] private AudioClip menuMusic;
    [SerializeField] private AudioClip gameMusic;

    [Header("SFX")]
    public AudioSource SFXSource;
    public static float SFXVolume { get; private set;}
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
        MusicVolume = MusicSource.volume = 0.5f;
        SFXVolume = SFXSource.volume = 0.5f;
        
    }

    void Start()
    {
        UpdateMusic(false);

        // Preserve volume levels
        MusicSource.volume = MusicVolume;
        SFXSource.volume = SFXVolume;
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
            // Stop the menu song and play the gameplay music
            MusicSource.Stop();
            MusicSource.clip = gameMusic;
            MusicSource.loop = true;
            MusicSource.Play();
        }
    }

    /// <summary>
    /// Set the static music volume variable equal to the new present volume set by the slider
    /// </summary>
    /// <param name="newVolume"></param>
    public void UpdateMusicVolume(float newVolume)
    {
        MusicVolume = newVolume;
    }


    /// <summary>
    /// Set the static SFX volume variable equal to the new present volume set by the slider
    /// </summary>
    /// <param name="newVolume"></param>
    public void UpdateSFXVolume(float newVolume)
    {
        SFXVolume = newVolume;
    }
}
