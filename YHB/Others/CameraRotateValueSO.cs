using Alchemy.Inspector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets._00.Work.YHB.Scripts.Others
{
	[CreateAssetMenu(fileName = "carema rotate value", menuName = "SO/Camera/Rotate value", order = 0)]
	public class CameraRotateValueSO : ScriptableObject
	{
		public float rotationSensitivity = 1f;
		public float rotationXMin = -80;
		public float rotationXMax = 80;
	}
}
