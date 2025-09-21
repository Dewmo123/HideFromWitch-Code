using Assets._00.Work.YHB.Scripts.Entities;
using Assets._00.Work.YHB.Scripts.Players;
using AYellowpaper.SerializedCollections;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace AKH.Scripts.Players
{
    public enum AnimType
    {
        None,
        Attack
    }
    public class DummyAnimationEvent : EntityComponent
    {
        private EntityComponentRegistry _registry;
        private EntityAnimationTrigger _trigger;
        private PlayerAnimator _animator;
        [SerializeField] private SerializedDictionary<AnimType, string> animNames;
        private Dictionary<AnimType, int> _animHashes;
        public override void Initialize(EntityComponentRegistry registry)
        {
            base.Initialize(registry);
            _animHashes = new();
            _registry = registry;
            _trigger = _registry.ResolveComponent<EntityAnimationTrigger>();
            _animator = _registry.ResolveComponent<PlayerAnimator>();
            _trigger.OnAnimationEndEvent += HandleAnimationEnd;
            foreach (var item in animNames)
            {
                _animHashes.Add(item.Key, Animator.StringToHash(item.Value));
            }
        }

        private void HandleAnimationEnd()
        {
            _animator.SetAnimationLock(false);
            _animator.TurnBeforeAnim(false);
        }

        public void PlayAnimation(AnimType animType)
        {
            _animator.ChangeAnimation(_animHashes[animType]);
            _animator.SetAnimationLock(true);
        }
    }
}
