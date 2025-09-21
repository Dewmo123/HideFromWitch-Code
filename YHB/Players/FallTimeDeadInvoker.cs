using AKH.Network;
using Assets._00.Work.YHB.Scripts.Entities;
using Assets._00.Work.YHB.Scripts.Executors;
using UnityEngine;
using UnityEngine.Events;

namespace Assets._00.Work.YHB.Scripts.Players
{
	public class FallTimeDeadInvoker : ActiverMono
	{
		[SerializeField] private EntityMovement entityMovement;
		[SerializeField] private float deadTime;

		public UnityEvent OnFallDead;

		public bool FallDead { get; set; }

		protected override void RequestActive()
		{
			base.RequestActive();

			FallDead = false;
		}

		protected override void ActivateCore()
		{
			entityMovement.FallTime.OnValueChanged += HandleFallTimeChange;
		}

		protected override void DeActivateCore()
		{
			entityMovement.FallTime.OnValueChanged -= HandleFallTimeChange;
		}

		private void HandleFallTimeChange(float previousValue, float nextValue)
		{
			if (nextValue < deadTime)
				return;

			if (!FallDead)
			{
				OnFallDead?.Invoke();
				FallDead = true;
                NetworkManager.Instance.SendPacket(new C_Fall());
			}
		}
	}
}
