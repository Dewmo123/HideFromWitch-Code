using Assets._00.Work.CSH._01_Scripts.Pools;
using Assets._00.Work.YHB.Scripts.Entities;
using Assets._00.Work.YHB.Scripts.Others;
using DewmoLib.ObjectPool.RunTime;
using DewmoLib.Utiles;
using UnityEngine;

namespace Assets._00.Work.CIW.Code
{
	// 총알에 수명 필요함.
	public class Bullet : MonoBehaviour, IPoolable
	{
		[Header("Bullet Setting")]
		[SerializeField] private DamageCaster damageCaster;
		[SerializeField] private LayerMask whatIsHitable;
		[SerializeField] private int damage = 1;
		[SerializeField] private float defaultBulletSpeed = 2f;
		[SerializeField] private float lifeTime = 10f;

		[Header("Object Pooling and event channel")]
		[SerializeField] private PoolItemSO bulletVfx;
		[SerializeField] private PoolItemSO bulletEffect;
		[SerializeField] private EventChannelSO shakeChannel;
		[SerializeField] private BulletTrail bulletTrail;
		[SerializeField] private bool isOwner;

		[Header("HitVFXPool")]
		[SerializeField] private PoolItemSO hitVFXPool;

		private float _lifeTimer;
		[SerializeField] private PoolManagerSO _poolManager;
		private Pool _pool;
		private Rigidbody _ridComp;
		private DamageData _damageData;

		[field: SerializeField] public PoolItemSO PoolItem { get; private set; }
		public GameObject GameObject => gameObject;
		public async void InitBullet(Vector3 position, Vector3 direction, float bulletSpeedMultiplier = 1f)
		{
			transform.position = position;
			transform.rotation = Quaternion.LookRotation(direction);
			_ridComp.linearVelocity = direction.normalized
				* (bulletSpeedMultiplier * defaultBulletSpeed);
			await Awaitable.NextFrameAsync();
			bulletTrail.PlayTrail();
		}


		private void Update()
		{
			_lifeTimer -= Time.deltaTime;
			if (_lifeTimer <= 0)
				_pool.Push(this);
		}

		public void ResetItem()
		{
			_lifeTimer = lifeTime;
			_ridComp.linearVelocity = Vector3.zero;
		}

		public void SetUpPool(Pool pool)
		{
			_pool = pool;

			_ridComp = transform.GetComponent<Rigidbody>(); // Awake에 빼는게 나으려나
			_damageData = new DamageData();
			_damageData.damage = damage;
			_damageData.bulletTrm = transform;

			damageCaster.InitCaster();
		}

		private void OnTriggerEnter(Collider other)
		{
			if (((1 << other.gameObject.layer) & whatIsHitable) != 0)
			{
				//            // Layer 비교해서 whatIsHitable에 있는 Dummy라면 true를 아니라면 false를 넘긴다
				//            int dummyLayerIdx = LayerMask.NameToLayer("Dummy");
				//int otherObjLayerIdx = other.gameObject.layer;
				//bool isDummy = (dummyLayerIdx == otherObjLayerIdx);
				DeadBullet(damageCaster.CastDamage());
				return;
			}
		}

		private void DeadBullet(bool isDummy)
		{
			if (isDummy)
				PlayHitVFX();
			else
				PlayBulletVFX();

			bulletTrail.StopTrail();
			_pool.Push(this);
		}

		private void PlayBulletVFX()
		{
			BulletVFX vfx = _poolManager.Pop(bulletVfx) as BulletVFX;
			BulletBoomEffect effect = _poolManager.Pop(bulletEffect) as BulletBoomEffect;
			vfx.PlayVFX();
			effect.PlayParticle(transform.position);
		}

		private void PlayHitVFX()
		{
			HitVFX vfx = _poolManager.Pop(hitVFXPool) as HitVFX;
			vfx.transform.position = transform.position;
			vfx.PlayVFX();

		}
	}
}
