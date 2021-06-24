using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GMTK.Characters;
using PNLib.Utility;
using GMTK.Enemies;

namespace GMTK.Weapons
{
	public abstract class AttackController : MonoBehaviour
	{
		public WeaponData weaponData;
		public Character character;

		private float cooldown;
		private float attackRange;
		private LayerMask targetLayer;

		public virtual void SetUp(Character character, WeaponData weaponData)
		{
			this.character = character;
			this.weaponData = weaponData;
			SetTargetLayer();
			LoadData();
			StartCoroutine(CooldownCoroutine());
		}

		protected virtual void LoadData()
		{
			this.cooldown = this.weaponData.Cooldown;
			this.attackRange = this.weaponData.AttackRange;
		}

		protected virtual IEnumerator CooldownCoroutine()
		{
			while (true)
			{
				yield return StartCoroutine(CanAttackCoroutine());
				yield return new WaitForSeconds(this.cooldown);
			}
		}

		protected virtual IEnumerator CanAttackCoroutine()
		{
			for (; ; )
			{
				if (FindTargetByLayer(out Character target))
				{
					Activate(target.gameObject.transform);
					break;
				}
				yield return new WaitForSeconds(0.1f);
			}
		}

		private bool FindTargetByLayer(out Character closestTarget)
		{
			closestTarget = null;
			float dist = float.MaxValue;
			if (!Helper.GetAllObjectsInCircleRadius(transform.position, this.attackRange, out List<Character> characters))
				return false;

			foreach (Character cha in characters)
			{
				if (cha.gameObject.layer == targetLayer)
				{
					float testDist = Vector3.Distance(cha.transform.position, transform.position);
					if (testDist < dist)
					{
						dist = testDist;
						closestTarget = cha;
					}
				}
			}

			return closestTarget != null;
		}

		protected void SetTargetLayer()
		{
			bool isEnemy = GetComponentInParent<Enemy>() != null;

			if (isEnemy)
			{
				targetLayer = LayerMask.NameToLayer("Player");
			}
			else
			{
				targetLayer = LayerMask.NameToLayer("Enemy");
			}
		}

		public virtual void Activate(Transform target)
		{
			if (!target) return;
		}
	}
}
