using UnityEngine;

public class FoeHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 3;
    public int CurrentHealth;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CurrentHealth = maxHealth;
    }

    public void UpdateHealth(int amount)
    {
        if(CurrentHealth < maxHealth || amount < 0)
        {
            Debug.Log("FoeHealth changing by " + amount);
            CurrentHealth += amount;
            Debug.Log("FoeHealth: " + CurrentHealth);
        }
    }
}
