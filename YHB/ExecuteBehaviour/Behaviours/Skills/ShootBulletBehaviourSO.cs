using AKH.Network;
using Assets._00.Work.AKH.Scripts.Packet;
using Assets._00.Work.CIW.Code;
using Assets._00.Work.YHB.Scripts.ExecuteBehaviour.DataTypes;
using DewmoLib.ObjectPool.RunTime;
using UnityEngine;

namespace Assets._00.Work.YHB.Scripts.ExecuteBehaviour.Behaviours.Skills
{
	[CreateAssetMenu(fileName = "ShootBulletBehaviour", menuName = "SO/ScriptableBehaviour/Behaviour/Skill/ShootBullet", order = 0)]
	public class ShootBulletBehaviourSO : ScriptableBehaviourSO
	{
		[SerializeField] private PoolManagerSO poolManagerSO;

		protected override bool LogicExecute<T>(T data)
		{
			if (data is not ShootBulletData bulletData)
				return false;

			Bullet bullet = poolManagerSO.Pop(bulletData.bulletItem) as Bullet;
			bullet.InitBullet(bulletData.startPosition, bulletData.direction);

			return true;
		}
	}
}
