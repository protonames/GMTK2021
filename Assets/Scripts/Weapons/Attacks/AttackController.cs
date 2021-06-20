using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GMTK.Characters;

namespace GMTK.Weapons
{
	public abstract class AttackController : MonoBehaviour
	{
		public WeaponData weaponData;
		public Character character;

		public virtual void SetUp(Character character, WeaponData weaponData)
		{
			this.character = character;
			this.weaponData = weaponData;
		}

		public virtual void Activate(Transform target)
		{
			if (!target) return;
		}
	}
}
