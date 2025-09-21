using UnityEngine;

namespace Assets._00.Work.YHB.Scripts.Entities
{
	public interface IMover
	{
		public float MoveSpeed { get; set; }
		public void SetMovementDirection(Vector2 direction);
		public void SetMovementDirection(Vector2 direction, Quaternion rotation);
		public void SetMovement(Vector3 direction);
	}
}
