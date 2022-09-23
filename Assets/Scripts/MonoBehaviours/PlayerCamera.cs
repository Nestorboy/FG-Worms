using Input;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

[RequireComponent(typeof(Camera))]
public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private float cameraMovementSpeed = 1f;
    [SerializeField] private float cameraRotationSpeed = 1f;
    [SerializeField] private Vector3 cameraOffset;
    [SerializeField] private Vector3 cameraPivot;

    private Quaternion _initialRotation = Quaternion.identity;
    private Vector3 _initialPosition;
    private Vector2 _rotationOffsets;

    public Vector3 targetPosition;
    public Vector3 targetLook;

    public enum MotionType
    {
        Normal = 0,
        SmoothMovement = 1 << 0,
        SmoothRotation = 1 << 1,
        Smooth = SmoothMovement | SmoothRotation
    }

    public MotionType motionType;
    
    private void LateUpdate()
    {
        Vector2 lookDirection = InputManager.GetInstance().inputLookDirection;
        lookDirection.y = -lookDirection.y;
        
        _rotationOffsets += lookDirection * cameraRotationSpeed;
        _rotationOffsets.y = Mathf.Clamp(_rotationOffsets.y, -90f, 90f);
        
        Quaternion rotationOffset = Quaternion.Euler(_rotationOffsets.y, _rotationOffsets.x, 0);
        transform.rotation = _initialRotation * rotationOffset;
        transform.position = PlayerManager.ActivePlayer.transform.position + cameraPivot + transform.rotation * _initialPosition;

        //print(Quaternion.LookRotation(cameraPivot - cameraOffset, PlayerManager.ActivePlayer.transform.up).eulerAngles);
        
        //Vector3 targetPosition = PlayerManager.ActivePlayer.transform.TransformPoint(cameraOffset);
        //transform.position = Vector3.Lerp(transform.position, targetPosition, 1f - cameraMovementSpeed * Time.deltaTime);
    }

    public void UpdateInitialValues()
    {
        //_initialRotation = PlayerManager.ActivePlayer.transform.rotation * Quaternion.LookRotation(cameraPivot - cameraOffset, PlayerManager.ActivePlayer.transform.up);
        _initialRotation = PlayerManager.ActivePlayer.transform.rotation;
        _rotationOffsets = new Vector2(0f, Vector3.SignedAngle(Vector3.forward, cameraPivot - cameraOffset, Vector3.right));
        Vector3 flatCameraVector = cameraOffset - cameraPivot;
        flatCameraVector.y = 0;
        flatCameraVector.Normalize();
        
        _initialPosition = flatCameraVector * (cameraOffset - cameraPivot).magnitude;
    }
}
