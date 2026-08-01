using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Declaration of Variables
    [SerializeField] private Transform target;
    [SerializeField] private float distance = 3f;
    [SerializeField] private float sensitivity = 5f;
    [SerializeField] private float maxPitch = 10f;
    [SerializeField] private float minPitch = -60f;
    private float yaw;
    private float pitch;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        yaw += Input.GetAxis("Mouse X") * sensitivity;
        pitch -= Input.GetAxis("Mouse Y") * sensitivity;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);
        Vector3 offset = rotation * new Vector3(0f, distance, -distance);
        transform.position = target.position + offset;
        transform.LookAt(target);

    }
}
