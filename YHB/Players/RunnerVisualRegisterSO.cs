using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets._00.Work.YHB.Scripts.Players
{
	[CreateAssetMenu(fileName = "RunnerVisualRegisterSO", menuName = "SO/Role/Runner/Visual/Register", order = 0)]
	public class RunnerVisualRegisterSO : ScriptableObject
	{
		private List<RunnerVisualObject> _visualObjects;
		private List<int> _poolValues;

		public int Count => _visualObjects.Count;

		private void OnEnable()
		{
			_visualObjects = new List<RunnerVisualObject>();
			_poolValues = new List<int>();
		}

		public void AddVisualObjects(IEnumerable<RunnerVisualObject> enumerable)
		{
			_visualObjects.Clear();
			_visualObjects.AddRange(enumerable);
			_visualObjects = _visualObjects.Distinct().ToList();
		}

		public RunnerVisualObject GetAt(int index)
		{
			if (0 > index || _visualObjects.Count <= index)
				return null;

			//RemovePoolIndex(index); //왜지움????????????????? // 서버에서 처리해야하는 걸 간과해서?

			return _visualObjects[index];
		}
		public void ResetPool()
		{
			_poolValues.Clear();
			for (int i = 0; i < _visualObjects.Count; i++)
				_poolValues.Add(i);
		}

		public RunnerVisualObject GetRandomValue()
		{
			int randomPoolIndex = UnityEngine.Random.Range(0, _poolValues.Count);
			int index = _poolValues[randomPoolIndex];
			//RemovePoolIndex(index);

			return _visualObjects[index];
		}

		private void RemovePoolIndex(int index)
		{
			if (_poolValues.Count <= 0)
				ResetPool();
			_poolValues.Remove(index);
		}
	}
}
