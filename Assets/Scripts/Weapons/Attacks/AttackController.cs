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
		public WeaponData WeaponData;
		public Character Character;
		public Character Target;

		private float cooldown;
		private float attackRange;
		private LayerMask targetLayer;

		public virtual void SetUp(Character character, WeaponData weaponData)
		{
			this.Character = character;
			this.WeaponData = weaponData;
			SetTargetLayer();
			LoadData();
			StartCoroutine(FindTargetCoroutine());
			StartCoroutine(CooldownCoroutine());
		}

		protected virtual void LoadData()
		{
			this.cooldown = this.WeaponData.Cooldown;
			this.attackRange = this.WeaponData.AttackRange;
		}

		protected virtual IEnumerator CooldownCoroutine()
		{
			for (; ; )
			{
				yield return StartCoroutine(CanAttackCoroutine());
				yield return new WaitForSeconds(this.cooldown);
			}
		}

		protected virtual IEnumerator CanAttackCoroutine()
		{
			for (; ; )
			{
				if (Target != null)
				{
					Activate(Target.gameObject.transform);
					break;
				}
				yield return new WaitForSeconds(0.1f);
			}
		}

		protected virtual IEnumerator FindTargetCoroutine()
		{
			for (; ; )
			{
				if (!FindTargetByLayer(out Target)) Target = null;
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
