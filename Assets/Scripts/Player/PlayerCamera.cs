using System;
using Input;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private float cameraMovementSpeed = 1f;
    [SerializeField] private float cameraRotationSpeed = 1f;
    [SerializeField] private Vector3 cameraOffset;
    [SerializeField] private Vector3 cameraPivot;

    public new Camera camera;
    public Vector3 targetPosition;
    public Vector3 targetLook;
    
    private float _initialCameraDistance;
    private Quaternion _initialRotation = Quaternion.identity;
    private Vector3 _initialPosition;
    private Vector2 _rotationOffsets;

    public enum MotionType
    {
        Normal = 0,
        SmoothMovement = 1 << 0,
        SmoothRotation = 1 << 1,
        Smooth = SmoothMovement | SmoothRotation
    }

    public MotionType motionType;

    private void Awake()
    {
        camera = GetComponent<Camera>();
    }

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
        Vector3 directionBack = newRot * Vector3.back;

        if (Physics.SphereCast(worldPivot, ComputeCameraMinRadius(camera), directionBack, out RaycastHit hit, _initialCameraDistance))
        {
            newPos = worldPivot + Vector3.Project(hit.point - worldPivot, directionBack);
        }
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
    
    /// <summary>
    /// Computes the minimum radius of a sphere which perfectly encapsulates the cameras near clipping plane.
    /// </summary>
    /// <param name="cam"></param>
    public static float ComputeCameraMinRadius(Camera cam)
    {
        Matrix4x4 invMat = cam.projectionMatrix.inverse;
        Vector4 cameraCorner = new Vector4(-1, -1, -1, 1);
        Vector4 pLocal = invMat * cameraCorner;
        pLocal /= pLocal.w;

        //Vector3 pWorld = cam.cameraToWorldMatrix * pLocal;
        return Vector3.Magnitude(new Vector3(pLocal.x, pLocal.y, 0));
    }
}
