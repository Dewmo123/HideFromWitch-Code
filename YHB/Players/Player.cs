using Assets._00.Work.YHB.Scripts.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets._00.Work.YHB.Scripts.Players
{
	public class Player : Entity
	{
		public int Index { get; set; }
		public string Name { get; set; }
		public Role Role { get; set; }
		public void InitPlayer(int index,string name, Role role)
		{
			Index = index;
			Name = name;
			Role = role;
		}
	}
}
