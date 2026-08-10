using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    [Header("Rounds")]
    [SerializeField] private TextMeshProUGUI rounds;
    [SerializeField] private TextMeshProUGUI roundsWon;
    public GameObject YouWin;
    public GameObject YouLose;
    public GameObject YouTied;
    public GameObject YouBecomePebbler;
    public GameObject FoeBecomesPebbler;
    public GameObject BothBecomePebbler;

    [Header("Sound")]
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(musicSlider == null)
        {
            musicSlider = GameObject.Find("MusicPauseSlider").GetComponent<Slider>();
        }

        if(sfxSlider == null)
        {
            sfxSlider = GameObject.Find("SFXPauseSlider").GetComponent<Slider>();
        }
    }


    public void UpdateRoundStats(int playerWins, int foeWins)
    {
        roundsWon.text = "Player Wins: " + playerWins + " / 3 \n" 
                        + "Foe Wins: " + foeWins + " / 3";
    }

}
