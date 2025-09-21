using DewmoLib.ObjectPool.RunTime;
using UnityEngine;

namespace Assets._00.Work.YHB.Scripts.ExecuteBehaviour.DataTypes
{
	public class ShootBulletData
	{
		public PoolItemSO bulletItem;
		public Vector3 startPosition;
		public Vector3 direction;

		public ShootBulletData Initialize(PoolItemSO item, Vector3 start, Vector3 dir)
		{
			this.bulletItem = item;
			this.startPosition = start;
			this.direction = dir;

			return this;
		}
	}
}
