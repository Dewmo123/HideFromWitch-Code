using AgamaLibrary.Unity.Methods;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets._00.Work.YHB.Scripts.Entities
{
	[RequireComponent(typeof(Initializer), typeof(EntityComponentRegistry))]
	public class EntityInjector : MonoBehaviour, IEarlyInitialize
	{
		private EntityComponentRegistry _registry;
		private List<IEntityResolver> _resolvers;

		private Initializer _initializer;

		public void EarlyInitialize()
		{
			_initializer = transform.GetComponent<Initializer>();
			_initializer.OnInitializeEnd += Initialize;
		}

		private void Initialize()
		{
			_initializer.OnInitializeEnd -= Initialize;

			_registry = transform.ForceGetComponent<EntityComponentRegistry>();

			_registry.ResetRegistry();

			_resolvers = GetComponentsInChildren<IEntityResolver>().ToList();
			foreach (IEntityResolver resolver in _resolvers)
				resolver.Initialize(_registry);
		}

		public void EarlyRelease()
		{
			foreach (IEntityResolver resolver in _resolvers)
				resolver.Release(_registry);
		}
	}
}
