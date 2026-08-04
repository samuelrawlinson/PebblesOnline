using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance {get; private set;}

    [Header("Music")]
    public AudioSource MusicSource;
    [SerializeField] private AudioClip menuMusic;

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
        MusicSource.volume = 0.5f;
    }

    void Start()
    {
        if(MusicSource != null)
        {
            MusicSource.clip = menuMusic;
            MusicSource.loop = true;
            MusicSource.Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
