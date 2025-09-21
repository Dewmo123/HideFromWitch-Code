using Assets._00.Work.YHB.Scripts.Core;
using Assets._00.Work.YHB.Scripts.Entities;
using Assets._00.Work.YHB.Scripts.ExecuteBehaviour;
using Assets._00.Work.YHB.Scripts.ExecuteBehaviour.DataTypes;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Assets._00.Work.YHB.Scripts.Executors
{
	public class EntityCameraInputExecutor : CameraInputExecutor, IEntityResolver
	{
		public void Initialize(EntityComponentRegistry registry)
		{
			_cameraData.mover = registry.ResolveComponent<EntityMovement>();
		}

		public void Release(EntityComponentRegistry registry) { }
	}
}
