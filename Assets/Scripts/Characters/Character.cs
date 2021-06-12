using System;
using System.Collections.Generic;
using GMTK.Enemies;
using GMTK.Weapons;
using PNLib.Utility;
using UnityEngine;

namespace GMTK.Characters
{
	[RequireComponent(typeof(Health))]
	public class Character : MonoBehaviour
	{
		[SerializeField]
		private Transform firePoint;

		public event Action OnDiedEvent;
		private CharacterData data;
		private Health health;
		private Enemy target;

		private void Awake()
		{
			health = GetComponent<Health>();
		}

		private void Start()
		{
			InvokeRepeating(nameof(TriggerAttack), data.AttackSpeed, data.AttackSpeed);
		}

		private void OnEnable()
		{
			health.OnDiedEvent += Die;
		}

		private void OnDisable()
		{
			health.OnDiedEvent -= Die;
		}

		private void Update()
		{
			if (!target
				&& Helper.GetClosestObjectInCircleRadius(transform.position, data.AttackRange, out Enemy hit))
			{
				target = hit;
			}
		}

		public void SetData(CharacterData characterData)
		{
			data = characterData;
			health.SetHealth(data.Health);
		}

		private void RotateFirePointTowardsTarget()
		{
			if (!target)
			{
				return;
			}

			float angle = Helper.GetAngleFromVector(firePoint.position.DirectionTo(target.transform.position));
			firePoint.eulerAngles = new Vector3(0, 0, angle);
		}

		private void TriggerAttack()
		{
			if (!target)
			{
				return;
			}

			RotateFirePointTowardsTarget();

			switch (data.Weapon.AttackType)
			{
				case AttackType.Projectile:
					Fire();
					break;
				case AttackType.Instantaneous:
					Attack(target.transform.position);
					break;
			}
		}

		private void Fire()
		{
			Vector3 spawnAngle = firePoint.eulerAngles;
			Vector3 spawnPoint = firePoint.position;
			Projectile projectile = Instantiate(data.Weapon.Projectile, spawnPoint, Quaternion.Euler(spawnAngle));
			projectile.Launch(true, data.Weapon.Speed, data.Damage);
		}

		private void Attack(Vector3 point)
		{
			if (Helper.GetAllObjectsInCircleRadius(point, data.Weapon.AreaSize, out List<Enemy> hits))
			{
				foreach (Enemy enemy in hits)
				{
					enemy.GetComponent<Health>().TakeDamage(data.Damage);
				}
			}
		}

		private void Die()
		{
			OnDiedEvent?.Invoke();
			Destroy(gameObject);
		}
	}
}