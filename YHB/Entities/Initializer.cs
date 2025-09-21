using System;
using UnityEngine;

namespace Assets._00.Work.YHB.Scripts.Entities
{
	[DefaultExecutionOrder(-1)]
	public class Initializer : MonoBehaviour
	{
		public event Action OnInitializeEnd;

		private IEarlyInitialize[] _earlyInitializes;
		private IEarlyInitialize[] EarlyInitializes
		{
			get
			{
				if (_earlyInitializes == null)
					ResearchInitializes();
				return _earlyInitializes;
			}
		}

		private void Awake()
		{
			Initialize();
		}

		private void OnDestroy()
		{
			Release();
		}

		public void ResearchInitializes()
		{
			_earlyInitializes = GetComponentsInChildren<IEarlyInitialize>(true);
		}

		public void Initialize()
		{
			foreach (IEarlyInitialize earlyInitializes in EarlyInitializes)
				earlyInitializes.EarlyInitialize();

			OnInitializeEnd?.Invoke();
		}

		public void Release()
		{
			foreach (IEarlyInitialize earlyInitializes in EarlyInitializes)
				earlyInitializes.EarlyRelease();
		}

	}
}
