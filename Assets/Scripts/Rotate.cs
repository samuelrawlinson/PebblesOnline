using UnityEngine;

public class Rotate : MonoBehaviour
{
    [SerializeField] private float rotateSpeed = 5f;
    [SerializeField] public GameObject target;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // transform.Rotate(0f, 90f * Time.deltaTime * rotateSpeed, 0f ) ;
        transform.LookAt(target.transform);
    }
}
