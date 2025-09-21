using Alchemy.Inspector;
using Assets._00.Work.YHB.Scripts.Entities;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets._00.Work.YHB.Scripts.Players
{
	public interface IRoleActiver
	{
		void ChangeRole(Role role);
	}
	public class RoleActiver : MonoBehaviour, IEarlyInitialize, IRoleActiver
	{
#if UNITY_EDITOR
		[SerializeField] private bool debugFlag = false;
#endif

		[SerializeField] private Role activeRole;

		private List<IActiver> _activers;

		public void ChangeRole(Role role)
		{
			bool enable = (activeRole & role) != 0;

#if UNITY_EDITOR
			if (debugFlag)
				Debug.Log(gameObject.name + " : " + enable + _activers.Count);
#endif

			if (enable)
			{
				gameObject.SetActive(true);
				foreach (IActiver activer in _activers)
					activer.Active();
			}
			else
			{
				foreach (IActiver activer in _activers)
					activer.DeActive();
				gameObject.SetActive(false);
			}
		}
        private void OnDestroy()
        {
            foreach (IActiver activer in _activers)
                activer.DeActive();
        }
        public void EarlyInitialize()
		{
			_activers = GetComponents<IActiver>().ToList();
		}

		public void EarlyRelease() { }

#if UNITY_EDITOR
		[Button]
		private void TestEarlyInitialize()
		{
			EarlyInitialize();
		}
		[Button]
		private void TestChangeRole(Role role)
		{
			ChangeRole(role);
		}
#endif
	}
}
