using AgamaLibrary.Methods;
using AKH.Network;
using Assets._00.Work.AKH.Scripts.Packet;
using Assets._00.Work.CDH.Code.Events;
using Assets._00.Work.CDH.Code.Sound;
using Assets._00.Work.YHB.Scripts.Core;
using Assets._00.Work.YHB.Scripts.Entities;
using Assets._00.Work.YHB.Scripts.ExecuteBehaviour;
using Assets._00.Work.YHB.Scripts.ExecuteBehaviour.DataTypes;
using Assets._00.Work.YHB.Scripts.Players;
using DewmoLib.ObjectPool.RunTime;
using DewmoLib.Utiles;
using UnityEngine;

namespace Assets._00.Work.YHB.Scripts.SkillSystem
{
    public class NormalAttackSkill : CoolTimeSkill, IEntityResolver
    {
        [Header("Behaviour Setting")]
        [SerializeField] private InputSO inputSO;
        [SerializeField] private CompositeBehaviourSO hunterCameraRotateComposite;
        [SerializeField] private ScriptableBehaviourSO rotateBehaviour;
        [SerializeField] private ScriptableBehaviourSO shootBehaviourSO;

        [Header("Attack Setting")]
        [SerializeField] private Transform muzzle;
        [SerializeField] private PoolItemSO bulletPoolItem;

        [Header("Animation Setting")]
        [SerializeField] private string attackAnimParam = "attack";

        [Header("Sound Setting")]
        [SerializeField] private SoundPlayCompo attackSoundPlayer;

        private EntityAnimationTrigger _entityAnimationTrigger;
        private PlayerAnimator _playerAnimator;
        private Entity _entity;
        private EntityMovement _entityMovement;

        private ShootBulletData _shootBulletData;

        private int _attackAnimHash;
        private bool _isAttacking;

        public override void EarlyInitialize()
        {
            base.EarlyInitialize();

            _shootBulletData = new ShootBulletData();
            _shootBulletData.bulletItem = bulletPoolItem;
        }

        public void Initialize(EntityComponentRegistry registry)
        {
            _playerAnimator = registry.ResolveComponent<PlayerAnimator>();
            _entityAnimationTrigger = registry.ResolveComponent<EntityAnimationTrigger>();

            _entity = registry.ResolveComponent<Entity>();
            _entityMovement = registry.ResolveComponent<EntityMovement>();

            _attackAnimHash = Animator.StringToHash(attackAnimParam);
        }

        protected override void ActivateCore()
        {
            base.ActivateCore();

            _playerAnimator.OnAnimationChangeEvent += HandleAnimationChangeEvent;

            _entityAnimationTrigger.OnAttackTriggerEvent += HandleAttackTriggerEvent;
            _entityAnimationTrigger.OnAnimationEndEvent += HandleAnimationEnd;
            _entityAnimationTrigger.OnRotateStatusChangeEvent += HandleRotateStatusChangeEvent;
        }

        public void Release(EntityComponentRegistry registry) { }

        protected override void DeActivateCore()
        {
            base.DeActivateCore();
            HandleAnimationChangeEvent();
            _playerAnimator.OnAnimationChangeEvent -= HandleAnimationChangeEvent;

            _entityAnimationTrigger.OnAttackTriggerEvent -= HandleAttackTriggerEvent;
            _entityAnimationTrigger.OnAnimationEndEvent -= HandleAnimationEnd;
            _entityAnimationTrigger.OnRotateStatusChangeEvent -= HandleRotateStatusChangeEvent;

            hunterCameraRotateComposite.nextBehaviourList.Remove(rotateBehaviour);
        }

        public override void UseSkill()
        {
            base.UseSkill();

            // 처음 스킬 사용시 해당 방향을 보게 하기 위함.
            LookAtCameraDirection();
            _playerAnimator.ChangeAnimation(_attackAnimHash); // 애니메이션 변경 후
            _playerAnimator.SetAnimationLock(true); // 애니메이션 락
            _entityMovement.CanMovement = false; // 움직임 봉인

            // 모든 처리 종료 후 어택 상태로 바꿔야 애니메이션 변경시의 처리(HandleAnimationChangeEvent)가 가능해짐.
            _isAttacking = true;
        }

        private Vector3 GetTargetDirection(Vector3 origin)
        {
            Vector3 direction;
            if (inputSO.GetWorldPosition(out Vector3 worldPosition))
                direction = worldPosition - origin;
            else
                direction = Camera.main.transform.forward;

            return direction;
        }

        private void LookAtCameraDirection()
        {
            // 카메라 방향이 아닌 타격된 방향을 보게 하기위함.
            Vector3 direction = GetTargetDirection(_entity.transform.position);
            direction.y = 0; // 수평 방향만 남김

            if (direction != Vector3.zero)
                _entityMovement.SetRotationDirection(Quaternion.LookRotation(direction));
        }

        private void HandleAttackTriggerEvent()
        {
            if (!_isAttacking)
                return;

            Vector3 direction = GetTargetDirection(muzzle.position);

            _shootBulletData.startPosition = muzzle.position;
            _shootBulletData.direction = direction;
            bool success = shootBehaviourSO.Execute<ShootBulletData>(_shootBulletData);

            if (success)
            {
                C_Shoot shoot = new()
                {
                    startPos = muzzle.position.ToPacket(),
                    direction = direction.ToPacket()
                };
                NetworkManager.Instance.SendPacket(shoot);
                attackSoundPlayer.PlaySound();
            }
            // 공격 실패시 쿨타임 초기화
            else
            {
                skillInfo.SetCoolTime();
            }
        }

        private void HandleRotateStatusChangeEvent(bool canRotate)
        {   
            if (!_isAttacking)
                return;

            if (canRotate)
                hunterCameraRotateComposite.nextBehaviourList.TryAdd(rotateBehaviour);
            else
                hunterCameraRotateComposite.nextBehaviourList.Remove(rotateBehaviour);
        }

        private void HandleAnimationEnd()
        {
            if (!_isAttacking)
                return;

            _playerAnimator.SetAnimationLock(false);
            _playerAnimator.TurnBeforeAnim(false);
        }

        // 애니메이션이 도중에 끊겼을 때의 처리와 애니메이션이 끝났을 때의 처리를 하기위해 관련 처리를 여기서 작성
        private void HandleAnimationChangeEvent()
        {
            if (!_isAttacking)
                return;

            _isAttacking = false;
            _entityMovement.CanMovement = true;
            hunterCameraRotateComposite.nextBehaviourList.Remove(rotateBehaviour);
        }
    }
}
