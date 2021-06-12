using System;
using System.Collections.Generic;
using DG.Tweening;
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

		[SerializeField]
		private Muzzle muzzlePrefab;
		
		public event Action OnDiedEvent;
		public CharacterData data;
		private Health health;
		public Transform Target;

		public GraphicsContainer GraphicsContainer;

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
		
		public void SetData(CharacterData characterData)
		{
			data = characterData;
			health.SetHealth(data.Health);
		}

		private void RotateFirePointTowardsTarget()
		{
			if (!Target)
			{
				return;
			}

			float angle = Helper.GetAngleFromVector(firePoint.position.DirectionTo(Target.transform.position));
			firePoint.eulerAngles = new Vector3(0, 0, angle);
		}

		private void TriggerAttack()
		{
			if (!Target)
			{
				return;
			}

			GraphicsContainer.BodySpriteRenderer.transform.DOScale(Vector3.one, .3f)
				.From(Vector3.one * 1.25f)
				.SetEase(Ease.OutBounce);
			
			GraphicsContainer.WeaponSpriteRenderer.transform.DOScale(Vector3.one, .3f)
				.From(Vector3.one * 1.25f)
				.SetEase(Ease.OutBounce);

			Instantiate(muzzlePrefab, firePoint.position, Quaternion.identity);
			RotateFirePointTowardsTarget();

			switch (data.Weapon.AttackType)
			{
				case AttackType.Projectile:
					Fire();
					break;
				case AttackType.Instantaneous:
					Attack(Target.transform.position);
					break;
			}
		}

		private void Fire()
		{
			Vector3 spawnAngle = firePoint.eulerAngles;
			Vector3 spawnPoint = firePoint.position;
			Projectile projectile = Instantiate(data.Weapon.Projectile, spawnPoint, Quaternion.Euler(spawnAngle));
			projectile.Launch(true, data.Weapon.Speed, data.Damage, data.Weapon.AreaSize);
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