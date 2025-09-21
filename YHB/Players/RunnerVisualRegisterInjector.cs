using Alchemy.Inspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets._00.Work.YHB.Scripts.Players
{
    [DefaultExecutionOrder(-10)]
	public class RunnerVisualRegisterInjector : MonoBehaviour
	{
		[SerializeField] public List<RunnerVisualObject> visualObjects;
		[SerializeField] private RunnerVisualRegisterSO runnerVisualRegister;

		private void Awake()
		{
			foreach (RunnerVisualObject visualObject in visualObjects)
				visualObject.gameObject.SetActive(false);
			runnerVisualRegister.AddVisualObjects(visualObjects);
			runnerVisualRegister.ResetPool();
		}

#if UNITY_EDITOR
		[Button]
		private void SetVisualObjects()
		{
			visualObjects = GetComponentsInChildren<RunnerVisualObject>(true)
				.ToList();
		}

		[Button]
		private void AllVisualObjectSetFilterAndCollider()
		{
			foreach (RunnerVisualObject visualObject in visualObjects)
			{
				visualObject.SetFilterAndCollider();
			}
		}

		private void OnValidate()
		{
			SetVisualObjects();
		}
#endif
	}
}
