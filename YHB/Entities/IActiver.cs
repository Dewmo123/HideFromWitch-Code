using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets._00.Work.YHB.Scripts.Entities
{
	public interface IActiver
	{
		public bool IsActived { get; }

		public void Active();
		public void DeActive();
	}
}
