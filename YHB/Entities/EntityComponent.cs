using Assets._00.Work.YHB.Scripts.Executors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets._00.Work.YHB.Scripts.Entities
{
	public abstract class EntityComponent : ActiverMono, IEntityResolver
	{
		public virtual void Initialize(EntityComponentRegistry registry)
		{
		}

		protected override void ActivateCore()
		{
		}

		public virtual void Release(EntityComponentRegistry registry)
		{
		}

		protected override void DeActivateCore()
		{
		}
	}
}
