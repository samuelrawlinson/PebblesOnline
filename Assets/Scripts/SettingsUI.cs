using UnityEngine;

public class SettingsUI : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void SetMusicVolume(float amount)
    {
        AudioManager.Instance.MusicSource.volume = amount;
        AudioManager.Instance.UpdateMusicVolume(amount);
    }

    public void SetSFXVolume(float amount)
    {
        AudioManager.Instance.SFXSource.volume = amount;
        AudioManager.Instance.UpdateSFXVolume(amount);
    }
}
