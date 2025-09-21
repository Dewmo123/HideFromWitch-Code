using AKH.Network;
using Assets._00.Work.YHB.Scripts.Entities;
using Assets._00.Work.YHB.Scripts.Events;
using Assets._00.Work.YHB.Scripts.Players;
using DewmoLib.Utiles;
using UnityEngine;

namespace Assets._00.Work.YHB.Scripts.SkillSystem
{
    public class MoveLockSkill : CoolTimeSkill, IEntityResolver
    {
        [SerializeField] private EventChannelSO roleChangeEventChannel;
        [SerializeField] private LayerMask groundLayer;
        private EntityMovement _entityMovement;
        private RoleController _roleController;
        private bool _isMoveLock = false;

        public void Initialize(EntityComponentRegistry registry)
        {
            _entityMovement = registry.ResolveComponent<EntityMovement>();
            _roleController = registry.ResolveComponent<RoleController>();
        }

        protected override void ActivateCore()
        {
            SetMoveLock(false, false);
        }

        public void Release(EntityComponentRegistry registry)
        {
        }

        protected override void DeActivateCore()
        {
            SetMoveLock(false, false);
        }

        public override void UseSkill()
        {
            if (CheckUnderfoot())
                SetMoveLock(!_isMoveLock, true);
        }
        private bool CheckUnderfoot()
        {
            return Physics.Raycast(transform.position, Vector3.down, 123, groundLayer);
        }
        private void SetMoveLock(bool value, bool roleChange = false)
        {
            _entityMovement.SetUseGravity(!value);
            _entityMovement.CanMovement = !value;
            _entityMovement.StopImmediately();
            _entityMovement.SetKinematic(value);
            _isMoveLock = value;
            NetworkManager.Instance.SendPacket(new C_MoveLock() { value = value });
            if (roleChange)
            {
                Role role = _isMoveLock ? Role.Observer : Role.Runner;
                _roleController.ChangeRole(role);
            }
        }
    }
}
