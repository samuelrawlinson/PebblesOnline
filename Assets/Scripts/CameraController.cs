using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Declaration of Variables
    [SerializeField] private Transform target;
    [SerializeField] private float offsetZValue = 2f;
    [SerializeField] private float offsetYValue = 2f;
    [SerializeField] private float offsetModifier = 2f;    
    [SerializeField] private float sensitivity = 5f;
    [SerializeField] private float maxPitch = 10f;
    [SerializeField] private float minPitch = -60f;
    private float yaw;
    private float pitch;

    void OnEnable()
    {
        GameManager.Instance.OnMiniGameStart.AddListener(UpdateCameraOffsetForThrowing);
        GameManager.Instance.OnMiniGameEnd.AddListener(UpdateCameraOffsetForPlaying);
    }    

    void OnDisable()
    {
        GameManager.Instance.OnMiniGameStart.RemoveListener(UpdateCameraOffsetForThrowing);
        GameManager.Instance.OnMiniGameEnd.RemoveListener(UpdateCameraOffsetForPlaying);
    }    
    
    void LateUpdate()
    {
        if(GameManager.Instance.IsPaused == false && GameManager.Instance.IsGameOver == false)
        {
            yaw += Input.GetAxis("Mouse X") * sensitivity;
            pitch -= Input.GetAxis("Mouse Y") * sensitivity;
            pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

            Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);
            Vector3 offset = rotation * new Vector3(0f, offsetYValue, -offsetZValue);
            if(target != null)
            {
                transform.position = target.position + offset;
                transform.LookAt(target);
            }
        }
    }

    private void UpdateCameraOffsetForThrowing()
    {
        if(GameManager.Instance.PlayerActions.CurrentMode == GameManager.GameMode.Throwing)
        {
            offsetYValue += offsetModifier;
            offsetZValue += offsetModifier;
        }
    }

    private void UpdateCameraOffsetForPlaying()
    {
        offsetYValue = offsetModifier;
        offsetZValue = offsetModifier;
    }
    

}
