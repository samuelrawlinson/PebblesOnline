using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;
    private AudioSource audioSource;

    void Awake()
    {
        if(Instance != null & Instance != this)
        {
            Destroy(gameObject);
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
