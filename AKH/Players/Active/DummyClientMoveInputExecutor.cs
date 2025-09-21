using Assets._00.Work.YHB.Scripts.Entities;
using Assets._00.Work.YHB.Scripts.ExecuteBehaviour;
using Assets._00.Work.YHB.Scripts.ExecuteBehaviour.DataTypes;
using Assets._00.Work.YHB.Scripts.Players;
using UnityEngine;

namespace AKH.Scripts.Players.Active
{
    public class DummyClientMoveInputExecutor : MonoBehaviour
    {
        [Header("Value")]
        [SerializeField] private ScriptableBehaviourSO moveBehaviour;
        [SerializeField] private ScriptableBehaviourSO rotateBehaviour;

        [Header("Value")]
        [SerializeField] private DummyClient dummyClient;
        [SerializeField] private EntityMovement entityMovement;
        [SerializeField] private PlayerAnimator animator;

        private MovementData _moveData;
        private RotateValueData _rotateData;
        private Quaternion _rotation;
        private Vector3 _moveDirection;
        private int _idleHash = Animator.StringToHash("idle");
        private int _moveHash = Animator.StringToHash("move");
        private void Awake()
        {
            _rotateData = new();
            _moveData = new MovementData();
            _rotateData.entityMovement = entityMovement;
            _moveData.movement = entityMovement;
            _moveData.moveRotation = Quaternion.identity;
            _moveData.moveDirection = Vector2.zero;

            dummyClient.OnMoveEvent += MoveHandler;
            dummyClient.OnRotationEvent += RotationHandler;
        }
        private void OnDestroy()
        {
            dummyClient.OnMoveEvent -= MoveHandler;
            dummyClient.OnRotationEvent -= RotationHandler;
        }
        private void RotationHandler(Quaternion rotation)
        {
            _rotation = rotation;
            _rotateData.rotateValue = _rotation;
            rotateBehaviour.Execute(_rotateData);
        }

        private void MoveHandler(Vector3 moveDirection)
        {
            _moveDirection = moveDirection;
            _moveData.moveDirection = _moveDirection;
            if (moveDirection.sqrMagnitude > 0)
                animator.ChangeAnimation(_moveHash);
            else
                animator.ChangeAnimation(_idleHash);
            moveBehaviour.Execute<MovementData>(_moveData);
        }

        // 점프는 나중에 연결
    }
}
