using UnityEngine;

namespace Assets._00.Work.YHB.Scripts.Entities.GroundCheckers
{
	public class BoxGroundChecker : GroundChecker
	{
		[SerializeField] private Vector3 defaultBoxSize;
		private Vector3 boxSize;

		private void Awake()
		{
			SetCheckerSize(defaultBoxSize);
		}

		public void SetCheckerSize(Vector3 size)
		{
			if (size == Vector3.zero)
			{
				boxSize = defaultBoxSize;
				return;
			}

			boxSize = size;
		}

		public override bool CheckGround()
		{
			return 0 < Physics.OverlapBox(transform.position, boxSize / 2, Quaternion.identity, groundLayer).Length;
		}

		protected override void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.red;
			Gizmos.DrawWireCube(transform.position, boxSize == Vector3.zero ? default : boxSize);
		}
	}
}
