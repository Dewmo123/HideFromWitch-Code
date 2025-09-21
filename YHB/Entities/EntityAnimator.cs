using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets._00.Work.YHB.Scripts.Entities
{
	public class EntityAnimator : EntityComponent
	{
        [SerializeField] private Animator animator;

		public void SetParam(int hash, float value) => animator.SetFloat(hash, value);
		public void SetParam(int hash, int value) => animator.SetInteger(hash, value);
		public void SetParam(int hash, bool value) => animator.SetBool(hash, value);
		public void SetParam(int hash) => animator.SetTrigger(hash);
	}
}
