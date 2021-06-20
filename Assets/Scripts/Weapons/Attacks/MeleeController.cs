using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GMTK.Characters;
using GMTK.Enemies;

namespace GMTK.Weapons
{
	public class MeleeController : AttackController
	{
		[SerializeField] private CleaveArea cleavePrefab;

		public override void SetUp(Character character, WeaponData weaponData)
		{
			base.SetUp(character, weaponData);
		}

		public override void Activate(Transform target)
		{
			if (!target) return;

			character.AttackAnimation();

			Vector3 dir = target.transform.position - transform.position;
			dir = Vector3.Normalize(dir);
			// Debug.DrawRay(transform.position, dir, Color.blue, 5f);
			CleaveArea cleave = Instantiate(cleavePrefab, transform.position, Quaternion.identity, transform);
			List<Enemy> enemies = cleave.GetInArea(dir);

			foreach (var enemy in enemies)
			{
				Health targetHealth = enemy.GetComponent<Health>();
				targetHealth.TakeDamage(character.Info.Damage);
			}
		}
	}
}
