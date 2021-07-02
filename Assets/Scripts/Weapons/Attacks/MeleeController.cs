using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GMTK.Characters;
using GMTK.Enemies;

namespace GMTK.Weapons
{
	public class MeleeController : AttackController
	{

		[SerializeField] private bool isCleave = true;
		[SerializeField] private CleaveArea cleavePrefab;


		public override void SetUp(Character character, WeaponData weaponData)
		{
			base.SetUp(character, weaponData);
		}

		public override void Activate(Transform target)
		{
			// TODO Mudar essa aquisição de alvo
			if (!target) return;

			float range = this.WeaponData.AttackRange;

			float distanceToTarget = Vector2.Distance(target.position, transform.position);

			if (distanceToTarget > range)
			{
				return;
			}

			Character.AttackAnimation();
			Character.RotateFirePointTowardsTarget(target);

			if (!isCleave)
			{
				Health targetHealth = target.GetComponent<Health>();
				targetHealth.TakeDamage(Character.Info.Damage);
				return;
			}

			Vector3 dir = target.transform.position - transform.position;
			dir = Vector3.Normalize(dir);

			CleaveArea cleave = Instantiate(cleavePrefab, transform.position, Quaternion.identity, transform);
			List<Character> targets = cleave.GetInArea(dir);

			bool isEnemy = GetComponentInParent<Enemy>() != null;
			foreach (var possibleTarget in targets)
			{
				bool isTargetEnemy = possibleTarget.GetComponent<Enemy>() != null;
				if ((isEnemy && isTargetEnemy) || (!isEnemy && !isTargetEnemy)) continue;

				Health targetHealth = possibleTarget.GetComponent<Health>();
				targetHealth.TakeDamage(Character.Info.Damage);
			}
		}
	}
}
