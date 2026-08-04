using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private HUDManager hud;
    [SerializeField] private int maxHealth = 5;
    [SerializeField] private int currentHealth;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;

        if(hud == null)
        {
            hud = GameManager.Instance.HUDManager;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateHealth(int amount)
    {
        if(currentHealth < maxHealth)
        {
            currentHealth += amount;
        }
        else if(currentHealth <= 0)
        {
            GameManager.Instance.PlayerLose();
        }
        
        hud.UpdateHealthBar((float) currentHealth / (float) maxHealth);
    }
}
