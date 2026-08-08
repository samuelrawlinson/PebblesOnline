using System.Xml.Schema;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private GameManager gameManager;

    void Awake()
    {
        
    }

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
