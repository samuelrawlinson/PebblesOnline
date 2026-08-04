using UnityEditor.Callbacks;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    [SerializeField] private float rotateSpeed = 3f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0f, 90f * Time.deltaTime * rotateSpeed, 0f ) ;
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Boulder"))
        {
            Debug.Log("Collided with boulder");
            Destroy(collision.gameObject);
        }
    }
}
