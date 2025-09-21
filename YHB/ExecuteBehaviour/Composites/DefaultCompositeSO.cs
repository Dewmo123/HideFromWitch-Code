using UnityEngine;

namespace Assets._00.Work.YHB.Scripts.ExecuteBehaviour.Composites
{
	[CreateAssetMenu(fileName = "Composite_Default", menuName = "SO/ScriptableBehaviour/Composite/Default", order = 0)]
	public class DefaultCompositeSO : CompositeBehaviourSO
	{
		[SerializeField] private bool useNextBehaviourResult = true;

		public override bool CanExecuteNext<T>(T data)
		{
			return true;
		}

		protected override bool LogicExecute<T>(T data)
		{
			ExecuteBeforeBehaviour(data);
			bool resultValue = TryExecuteNext(data);
			// 다음 실행 Behaviour를 사용하지 않는다면 false
			return !useNextBehaviourResult || resultValue;
		}
	}
}
