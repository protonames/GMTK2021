using System.Collections;
using System.Collections.Generic;
using GMTK.Characters;
using UnityEngine;

namespace GMTK.Weapons
{
	public class CasterController : AttackController
	{
		[SerializeField] private int activeSummonCap = 2;
		[SerializeField] private int activeSummons = 0;
		[SerializeField] private Character summonPrefab;

		public override void Activate(Transform target)
		{
			if (activeSummons >= activeSummonCap)
				return;

			activeSummons++;
			print("Summon!");
		}
	}
}
