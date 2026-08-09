using UnityEditor.Callbacks;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    [SerializeField] private float decendSpeed = 3f;
    [SerializeField] private float decendLimit = -1;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * decendSpeed * Time.deltaTime);
        if(transform.position.y <= decendLimit)
        {
            Destroy(gameObject);
        }
    }
}
