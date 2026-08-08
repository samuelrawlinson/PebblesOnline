using System.Xml.Schema;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerActions : MonoBehaviour
{
    [Header("References")]
    private GameManager gameManager;
    public GameManager.GameMode currentMode = GameManager.GameMode.Playing;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = GameManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnChooseCardOne(InputValue value)
    {   
        if(value.isPressed)
        {
            gameManager.RevealAllCards();
        }
    }
}
