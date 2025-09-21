using Assets._00.Work.YHB.Scripts.Core;
using UnityEngine;

public class CameraRotater : MonoBehaviour
{
    [SerializeField] private InputSO _input;
    private Transform _camera;

    [Header("회전 세팅")]
    [SerializeField] private float sensitivity = 0.1f;   // 마우스 감도
    [SerializeField] private float maxYaw = 30f;         // 좌우 최대 각도
    [SerializeField] private float maxPitch = 15f;       // 상하 최대 각도
    [SerializeField] private float smoothSpeed = 5f;     // 부드럽게 보간 속도

    private float _targetYaw;
    private float _targetPitch;
    private float _currentYaw;
    private float _currentPitch;
    private Quaternion _originRotation;

    private void Start()
    {
        _camera = Camera.main.transform;
        _originRotation = _camera.localRotation;

        _input.OnLookChangedEvent += HandleMouseMove;
    }
    private void OnDestroy()
    {
        _input.OnLookChangedEvent -= HandleMouseMove;
    }
    private void HandleMouseMove(Vector2 moveDir)
    {
        _targetYaw += moveDir.x * sensitivity;
        _targetPitch -= moveDir.y * sensitivity;

        _targetYaw = Mathf.Clamp(_targetYaw, -maxYaw, maxYaw);
        _targetPitch = Mathf.Clamp(_targetPitch, -maxPitch, maxPitch);
    }

    private void Update()
    {
        _currentYaw = Mathf.Lerp(_currentYaw, _targetYaw, Time.deltaTime * smoothSpeed);
        _currentPitch = Mathf.Lerp(_currentPitch, _targetPitch, Time.deltaTime * smoothSpeed);

        Quaternion yawRotation = Quaternion.Euler(0f, _currentYaw, 0f);
        Quaternion pitchRotation = Quaternion.Euler(_currentPitch, 0f, 0f);

        _camera.localRotation = _originRotation * yawRotation * pitchRotation;
    }
}
