using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GMTK.Characters;

namespace GMTK.Weapons
{
	public class ProjectileController : AttackController
	{
		Projectile projectilePrefab;

		public override void SetUp(Character character, WeaponData weaponData)
		{
			base.SetUp(character, weaponData);
			projectilePrefab = weaponData.Projectile;
		}

		public override void Activate(Transform target)
		{
			if (!target) return;

			Character.AttackAnimation();
			Character.RotateFirePointTowardsTarget(target);

			Health targetHealth = target.GetComponent<Health>();

			Projectile projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity, null);
			projectile.Launch(targetHealth, true, WeaponData.Speed, Character.Info.Damage); // TODO Pegar o character.isPlayer para playerProjectile
		}
	}
}
