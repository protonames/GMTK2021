using System.Collections.Generic;
using GMTK.Characters;
using GMTK.Enemies;
using GMTK.Utilities;
using GMTK.Weapons;
using PNLib.Utility;
using UnityEngine;

namespace GMTK.ProjectileEffects
{
	public class RadiusDamageOnHit : MonoBehaviour
	{
		[SerializeField]
		private Muzzle muzzlePrefab;

		private void OnEnable()
		{
			Projectile projectile = GetComponent<Projectile>();
			projectile.OnDiedEvent += () => TriggerAoE(projectile.Damage, projectile.AreaSize);
		}

		private void TriggerAoE(int damage, float radius)
		{
			Muzzle muzzle = Instantiate(muzzlePrefab, transform.position, Quaternion.identity);
			muzzle.transform.localScale = Vector3.one * (radius * 2f);
			HelperExtras.Shake(1, 90, 0.2f);

			if (Helper.GetAllObjectsInCircleRadius(transform.position, radius, out List<Enemy> enemiesHit))
			{
				foreach (Enemy enemy in enemiesHit)
				{
					enemy.GetComponent<Health>().TakeDamage(damage);
				}
			}
		}
	}
}