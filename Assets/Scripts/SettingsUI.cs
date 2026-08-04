using UnityEngine;

public class SettingsUI : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void SetMusicVolume(float amount)
    {
        AudioManager.Instance.MusicSource.volume = amount;
    }

    public void SetSFXVolume(float amount)
    {
        AudioManager.Instance.SFXSource.volume = amount;
    }
}
