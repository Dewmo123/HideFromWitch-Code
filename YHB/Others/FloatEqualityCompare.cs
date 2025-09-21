using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets._00.Work.YHB.Scripts.Others
{
	public class FloatEqualityCompare : IEqualityComparer<float>
	{
		public bool Equals(float x, float y)
		{
			return Mathf.Approximately(x, y);
		}

		public int GetHashCode(float obj)
		{
			throw new NotImplementedException();
		}
	}
}
