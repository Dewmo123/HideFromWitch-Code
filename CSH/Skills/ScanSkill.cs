using AKH.Network;
using Assets._00.Work.CDH.Code.Sound;
using Assets._00.Work.CSH._01_Scripts.Pools;
using Assets._00.Work.YHB.Scripts.Entities;
using Assets._00.Work.YHB.Scripts.Players;
using Assets._00.Work.YHB.Scripts.SkillSystem;
using DewmoLib.Dependencies;
using DewmoLib.ObjectPool.RunTime;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets._00.Work.CSH._01_Scripts.SkillSystem
{
    public class ScanSkill : CoolTimeSkill
    {
        [SerializeField] private float scanDuration;
        [SerializeField] private float scanRange;
        [SerializeField] private Material outLineMat;
        [SerializeField] private LayerMask whatIsPlayer;
        [SerializeField] private PoolItemSO scanVFX;
        [SerializeField] private SoundPlayCompo soundPlayer;


        [SerializeField] private PoolManagerSO poolManager;
        private Collider[] colliders;
        private Coroutine coroutine;


        public override void UseSkill()
        {

            if (CanUseSkill())
            {
                base.UseSkill();
                coroutine = StartCoroutine(Scan());
            }
            
        }

        private IEnumerator Scan()
        {
            if (coroutine == null)
            {
                var e = poolManager.Pop(scanVFX).GameObject;

                e.transform.position = transform.position;
                soundPlayer.PlaySound();
                colliders = Physics.OverlapSphere(transform.position, scanRange, whatIsPlayer);
                Debug.Log(colliders.Length);
                var temp = colliders.ToList();
                
                colliders = temp.ToArray();
                C_Scanned scanned = new();
                scanned.scanned = new List<ScannedInfo>();
                for (int i = 0; i < colliders.Length; i++)
                {
                    var player = colliders[i].GetComponentInChildren<Player>();
                    if (player == null)
                        continue;
                    scanned.scanned.Add(new ScannedInfo() { index = player.Index });
                    var visual = player.Registry.ResolveComponent<RunnerVisualController>();
                    visual.gameObject.layer = 8;
                }
                NetworkManager.Instance.SendPacket(scanned);
                yield return new WaitForSeconds(scanDuration);
                ClearScan();
            }
            yield return null;
        }


        private void ClearScan()
        {
            if (colliders == null) return;
            foreach (var obj in colliders)
            {
                var visual = obj.gameObject.GetComponentInChildren<MeshRenderer>();
                visual.gameObject.layer = LayerMask.NameToLayer("Dummy");
            }
            colliders = null;
            coroutine = null;
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, scanRange);

        }

#endif
    }
}
