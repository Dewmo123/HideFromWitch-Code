using AKH.Network;
using Assets._00.Work.CDH.Code.Sound;
using Assets._00.Work.YHB.Scripts.Entities;
using Assets._00.Work.YHB.Scripts.Players;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

namespace Assets._00.Work.YHB.Scripts.Others
{
	public class SphereDamageCaster : DamageCaster
	{
		[SerializeField] private float castRadius;
		[SerializeField] private int maxColliderCount = 1;

		[Header("Sound Setting")]
		[SerializeField] private SoundPlayCompo hitSoundPlayer;

		private Collider[] _colliders;
		private HashSet<Transform> _hitObjects;

		public override void InitCaster()
		{
			_colliders = new Collider[maxColliderCount];
            _hitObjects = new HashSet<Transform>(maxColliderCount);
		}

		public override bool CastDamage()
		{
            int count = Physics.OverlapSphereNonAlloc(transform.position, castRadius, _colliders, whatIsTarget);

			for (int i = 0; i < count; i++)
			{
				Transform target = _colliders[i].transform;

				if (_hitObjects.Contains(target.root))
					continue;

				if (_hitObjects.Count >= maxColliderCount)
					continue;
				_hitObjects.Add(target.root);

				if (target.TryGetComponent<Player>(out Player player))
				{
					hitSoundPlayer.PlaySound(player.transform);	

                    NetworkManager.Instance.SendPacket(new C_Attack()
					{
						hitIndex = player.Index
					});
					Debug.Log($"hitIndex: {player.Index}");
				}
			}

			return count > 0;
		}

		private void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(transform.position, castRadius);
		}
	}
}
