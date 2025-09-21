using Assets._00.Work.YHB.Scripts.Entities;
using UnityEngine;

namespace Assets._00.Work.YHB.Scripts.Executors
{
	public abstract class ActiverMono : MonoBehaviour, IActiver, IEarlyInitialize
	{
		public bool IsActived { get; protected set; }
		public bool IsInitialize { get; protected set; }

		public virtual void EarlyInitialize()
		{
			IsInitialize = true;
		}

		public void Active()
		{
			RequestActive();

			if (IsActived)
				return;

			IsActived = true;
			ActivateCore();
		}

		protected virtual void RequestActive() { }
		protected abstract void ActivateCore();

		public virtual void EarlyRelease()
		{
			IsInitialize = false;
		}

		public void DeActive()
		{
			RequestDeActive();

			if (!IsActived)
				return;
			
			IsActived = false;
			DeActivateCore();
		}

		protected virtual void RequestDeActive() { }
		protected abstract void DeActivateCore();

	}
}
