using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance {get; private set;}

    [Header("Music")]
    public AudioSource MusicSource;
    public static float MusicVolume { get; private set;}
    [SerializeField] private AudioClip menuMusic;
    [SerializeField] private AudioClip gameMusic;

    public enum SoundEffect
    {
        Swoosh,
        Crunch,
        Crash,
        WompWomp,
        Spooky
    }

    [Header("SFX")]
    public AudioSource SFXSource;
    public static float SFXVolume { get; private set;}
    [SerializeField] private AudioClip buttonClick;
    [SerializeField] private AudioClip boulderThrow;
    [SerializeField] private AudioClip boulderCrunch;
    [SerializeField] private AudioClip wallCrash;
    [SerializeField] private AudioClip playerLost;
    [SerializeField] private AudioClip ghostDenial;

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
        AudioSource[] sources = GetComponents<AudioSource>();
        MusicSource = sources[0];
        SFXSource = sources[1];
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
    /// Play a sound effec that matches the provided SoundEffect enum
    /// </summary>
    /// <param name="soundEffect"></param>
    public void PlaySoundEffect(SoundEffect soundEffect)
    {
        switch (soundEffect)
        {
            case SoundEffect.Swoosh:
                SFXSource.PlayOneShot(boulderThrow);
                break;
            case SoundEffect.Crunch:
                SFXSource.PlayOneShot(boulderCrunch);
                break;
            case SoundEffect.Crash:
                SFXSource.PlayOneShot(wallCrash);
                break;
            case SoundEffect.WompWomp:
                SFXSource.PlayOneShot(playerLost);
                break;
            case SoundEffect.Spooky:
                SFXSource.PlayOneShot(ghostDenial);
                break;
        }
    }


    /// <summary>
    /// Quick reference for UI buttons to call related sound
    /// </summary>
    public void PlayButtonPress()
    {
        SFXSource.PlayOneShot(buttonClick);
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
