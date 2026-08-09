using UnityEngine;

public class FoeHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private int currentHealth;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
    }

    public void UpdateHealth(int amount)
    {
        if(currentHealth < maxHealth)
        {
            Debug.Log("FoeHealth changing by " + amount);
            currentHealth += amount;
            Debug.Log("FoeHealth: " + currentHealth);
        }
        else if(currentHealth <= 0)
        {
            GameManager.Instance.PlayerWin();
        }
    }
}
