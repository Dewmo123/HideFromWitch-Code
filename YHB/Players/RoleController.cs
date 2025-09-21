using Alchemy.Inspector;
using Assets._00.Work.YHB.Scripts.Entities;
using Assets._00.Work.YHB.Scripts.Events;
using DewmoLib.Utiles;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets._00.Work.YHB.Scripts.Players
{
	[Flags]
	public enum Role
	{
		None = 1,
		Hunter = 2,
		Runner = 4,
		Observer = 8,
		Dead = 16
	}

	public class RoleController : MonoBehaviour, IEarlyInitialize
	{
		[SerializeField] private GameObject activerParent;
		private List<IRoleActiver> _roleActivers;

		public void EarlyInitialize()
		{
			_roleActivers = activerParent.GetComponentsInChildren<IRoleActiver>(true).ToList();
		}

		public void EarlyRelease()
		{
			ChangeRole(Role.None);
		}
		public void ChangeRole(Role role)
		{
			foreach (IRoleActiver roleActiver in _roleActivers)
			{
				roleActiver.ChangeRole(role);
			}
		}

#if UNITY_EDITOR
		[Header("Editor")]
		[SerializeField] private Role testRole;

		[Button]
		private void ChangeRoleTest()
		{
			PlayerChangeRoleEvent playerChangeRoleEvent = new PlayerChangeRoleEvent();
			playerChangeRoleEvent.role = testRole;
			ChangeRole(testRole);
		}
#endif
	}
}
