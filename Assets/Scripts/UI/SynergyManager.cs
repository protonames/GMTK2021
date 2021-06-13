using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GMTK
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