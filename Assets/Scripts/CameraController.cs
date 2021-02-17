using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] ForestBehaviour _forestBehaviour;
    [SerializeField] float _cameraSpeed;
    [SerializeField] private Vector3 _cameraPreset; // Camera distance from the tree

    private bool _isCameraReadyToMove;

    private Vector3 _cameraNewPosition;
    private Vector3 _cameraNewRotation;

    private void Start()
    {
        _cameraNewPosition = transform.position;
    }
    private void Update()
    {
        if (_isCameraReadyToMove)
            ChangeCameraPosition();
    }

    public void ChangeCameraPosition()
    {
        transform.position = Vector3.Lerp(transform.position, _cameraNewPosition, _cameraSpeed);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(_cameraNewRotation - transform.position), _cameraSpeed * 2);
    }

    public void SetNewCameraDestanation(Vector3 newPosition) // Changes the rotation and position to which camera should lerp.
    {
        if (!_isCameraReadyToMove) // Checking for first changing of positions.
            _isCameraReadyToMove = true;
        _cameraNewPosition = newPosition;
        _cameraNewPosition += _cameraPreset;
        _cameraNewRotation = newPosition;
    }
}
