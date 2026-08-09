using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private HUDManager hud;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private int maxHealth = 3;
    public int CurrentHealth;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CurrentHealth = maxHealth;
        gameManager = GameManager.Instance;

        if(hud == null)
        {
            hud = GetComponent<HUDManager>();
        }
    }

    public void UpdateHealth(int amount)
    {
        if(CurrentHealth < maxHealth || amount < 0)
        {
            Debug.Log("PlayerHealth changing by " + amount);
            CurrentHealth += amount;
            Debug.Log("PlayerHealth: " + CurrentHealth);
        }
    }
}
