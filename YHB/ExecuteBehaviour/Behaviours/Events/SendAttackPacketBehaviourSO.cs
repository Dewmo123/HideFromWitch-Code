using AKH.Network;
using Assets._00.Work.YHB.Scripts.ExecuteBehaviour.DataTypes;
using UnityEngine;

namespace Assets._00.Work.YHB.Scripts.ExecuteBehaviour.Behaviours.Events
{
	[CreateAssetMenu(fileName = "SendAttackPacketBehaviour", menuName = "SO/ScriptableBehaviour/Behaviour/Event/SendAttackPacket", order = 0)]
	public class SendAttackPacketBehaviourSO : ScriptableBehaviourSO
	{
		protected override bool LogicExecute<T>(T data)
		{
			if (data is not ShootBulletData bulletData)
				return false;

			// bulletData 써서 필요한 데이터 보내주세요.~^^
			NetworkManager.Instance.SendPacket(new C_Attack());

			return true;
		}
	}
}
