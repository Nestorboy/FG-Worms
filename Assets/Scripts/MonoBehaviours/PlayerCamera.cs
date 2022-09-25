using Input;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private float cameraMovementSpeed = 1f;
    [SerializeField] private float cameraRotationSpeed = 1f;
    [SerializeField] private Vector3 cameraOffset;
    [SerializeField] private Vector3 cameraPivot;

    private float _initialCameraDistance;
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
        Vector3 worldPivot = PlayerManager.ActivePlayer.transform.position + cameraPivot;

        Quaternion newRot = _initialRotation * rotationOffset;
        Vector3 newPos;

        if (Physics.Raycast(worldPivot, newRot * Vector3.back, out RaycastHit hit, _initialCameraDistance))
            newPos = hit.point;
        else
            newPos = worldPivot + newRot * _initialPosition;

        transform.SetPositionAndRotation(newPos, newRot);
    }

    public void UpdateInitialValues()
    {
        _initialRotation = PlayerManager.ActivePlayer.transform.rotation;
        _rotationOffsets = new Vector2(0f, Vector3.SignedAngle(Vector3.forward, cameraPivot - cameraOffset, Vector3.right));
        Vector3 flatCameraVector = cameraOffset - cameraPivot;
        flatCameraVector.y = 0;
        flatCameraVector.Normalize();

        _initialCameraDistance = (cameraOffset - cameraPivot).magnitude;

        _initialPosition = flatCameraVector * _initialCameraDistance;
    }
}
