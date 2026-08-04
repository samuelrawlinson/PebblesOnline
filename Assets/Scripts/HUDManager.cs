using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] public Image healthBar;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(healthBar == null)
        {
            healthBar = GameObject.Find("HealthBar").GetComponent<Image>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateHealthBar(float healthPercentage)
    {
        healthBar.fillAmount = healthPercentage;
    }
}
