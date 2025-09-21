using Assets._00.Work.CSH._01_Scripts.Hunter;
using Assets._00.Work.YHB.Scripts.Entities;
using DewmoLib.Dependencies;
using DewmoLib.ObjectPool.RunTime;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets._00.Work.CSH._01_Scripts.Component
{
    public class AttackCompo : EntityComponent, IEntityResolver,INeedInject
    {
        [Inject] private PoolManagerMono poolManager;
        [SerializeField] private PoolItemSO bulletPrefab;
        [SerializeField] private Transform firePos;
        [SerializeField] private float attackSpeed = 1.0f;
        [SerializeField] private float attackRange = 2.0f;
        [SerializeField] private int attackDamage = 10;
        [SerializeField] private float attackDelay = 0.5f;

        private float _timer = 0f;
        private bool _canAttack = true;

        public bool Injected { get; set; } = false;
        private AwaitableCompletionSource _waitInject = new();
        public AwaitableCompletionSource WaitInject => _waitInject;
        public override async void Initialize(EntityComponentRegistry registry)
        {
            base.Initialize(registry);
            await ((INeedInject)this).Init();
        }
        private void Update()
        {
            _timer += Time.deltaTime;
            if (_timer >= attackDelay)
            {
                _timer = 0f;
                _canAttack = true;
            }
        }

        public bool Fire()
        {

            if(!_canAttack)
            {
                return false;
            }
            var projectile = poolManager.Pop<Projectile>(bulletPrefab);
            projectile.transform.position = firePos.position;

            Vector3 targetPoint = Camera.main.transform.position + Camera.main.transform.forward * Camera.main.farClipPlane;

            Vector3 direction = (targetPoint - firePos.position).normalized;
            projectile.Fire(direction, attackSpeed);

            _canAttack = false;
            return true;
        }


        
    }
}
