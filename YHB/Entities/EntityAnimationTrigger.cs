using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets._00.Work.YHB.Scripts.Entities
{
	public class EntityAnimationTrigger : EntityComponent
	{
		public event Action OnAnimationEndEvent;
		public event Action OnAttackTriggerEvent;

		public event Action<bool> OnRotateStatusChangeEvent;

		public void OnAnimationEnd() => OnAnimationEndEvent?.Invoke();
		public void OnAttackTrigger() => OnAttackTriggerEvent?.Invoke();

		public void OnRotate() => OnRotateStatusChangeEvent?.Invoke(true);
		public void OnRotateStop() => OnRotateStatusChangeEvent?.Invoke(false);
	}
}
