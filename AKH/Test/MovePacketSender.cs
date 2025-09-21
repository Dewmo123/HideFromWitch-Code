using AKH.Network;
using Assets._00.Work.AKH.Scripts.Packet;
using Assets._00.Work.YHB.Scripts.Entities;
using Assets._00.Work.YHB.Scripts.Events;
using DewmoLib.Utiles;
using System.Collections;
using UnityEngine;

namespace AKH.Scripts.Test
{
    [DefaultExecutionOrder(-10)]
    public class MovePacketSender : MonoBehaviour
    {
        [SerializeField] private EventChannelSO gameChannel;
        private float _speed;
        private Vector3 _currentDirection;
        private EntityMovement _movement;
        private static Vector3 _defaultVel = new Vector3(0, -0.03f, 0);
        [SerializeField] private float sendDelay = 0.02f;
        private WaitForSeconds _delayWait;
        private void Awake()
        {
            _delayWait = new(sendDelay);
            gameChannel.AddListener<RotateEvent>(HandleRotate);
            gameChannel.AddListener<MoveSpeedChangeEvent>(HandleSpeedChange);
            gameChannel.AddListener<MoveDirectionChangeEvent>(HandleDirectionChange);
            StartCoroutine(SendLoop());
        }
        private void OnDestroy()
        {
            gameChannel.RemoveListener<RotateEvent>(HandleRotate);
            gameChannel.RemoveListener<MoveSpeedChangeEvent>(HandleSpeedChange);
            gameChannel.RemoveListener<MoveDirectionChangeEvent>(HandleDirectionChange);
        }

        private IEnumerator SendLoop()
        {
            while (true)
            {
                if (_movement != null&&_movement.Velocity != _currentDirection)
                {
                    _currentDirection = _movement.Velocity;
                    SendMovePacket();
                }
                yield return _delayWait;
            }
        }
        private void HandleDirectionChange(MoveDirectionChangeEvent @event)
        {
            //_currentDirection = @event.entityMovement.Velocity;
            _movement = @event.entityMovement;
            //SendMovePacket();
        }

        private void HandleSpeedChange(MoveSpeedChangeEvent @event)
        {
            //_speed = @event.newMoveSpeed;
            _movement = @event.entityMovement;
            //SendMovePacket();
        }
        private void SendMovePacket()
        {
            C_Move move = new()
            {
                direction = _movement.Velocity.ToPacket(),
                position = _movement.transform.position.ToPacket(),
                speed = _speed
            };
            NetworkManager.Instance.SendPacket(move);

        }
        private void HandleRotate(RotateEvent @event)
        {
            C_Rotate rotate = new()
            {
                rotation = @event.rotateValue.ToPacket()
            };
            NetworkManager.Instance.SendPacket(rotate);
            SendMovePacket();
        }
    }
}
