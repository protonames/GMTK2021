using System.Collections.Generic;
using System.Linq;
using GMTK.Info;
using UnityEngine;

namespace GMTK.UI
{
	public class SynergyManager : MonoBehaviour
	{
		public static SynergyManager Instance { get; set; }
		public List<SynergyType> ActiveSynergies = new List<SynergyType>();

		private void Awake()
		{
			Instance = this;
		}

		public int GetCount(SynergyType type)
		{
			return ActiveSynergies.Count(x => x == type);
		}
	}
}