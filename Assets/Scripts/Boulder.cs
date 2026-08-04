using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Boulder : MonoBehaviour
{
    public Rigidbody rb;
    public GameObject boulderHolder;
    private Vector3 boulderOffset = new Vector3(0f,3.25f,0f);
    [SerializeField] private float throwingForce = 10f;
    [SerializeField] private float lifeTime = 0.5f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(boulderHolder.GetComponent<FoeMovement>().IsBoulderHolder)
        {
            transform.position = boulderHolder.transform.position + boulderOffset;
        }
    }

    /// <summary>
    /// Find the difference between the target and current positions to find direction, then throw in that direction
    /// </summary>
    /// <param name="targetPosition"></param>
    public void BeThrownByHolder(Vector3 targetPosition)
    {
        Vector3 throwingDirection = (targetPosition - transform.position).normalized;
        rb.AddForce(throwingDirection * throwingForce, ForceMode.Impulse);
        StartCoroutine("DestroyIfMissed", lifeTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }

    IEnumerator DestroyIfMissed(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Destroy(gameObject);
    }
}
