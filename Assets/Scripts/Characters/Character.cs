using System;
using System.Collections.Generic;
using DG.Tweening;
using GMTK.Enemies;
using GMTK.Utilities;
using GMTK.Weapons;
using PNLib.Utility;
using UnityEngine;

namespace GMTK.Characters
{
	[RequireComponent(typeof(Health))]
	public class Character : MonoBehaviour
	{
		[SerializeField]
		private ParticleSystem deathParticles;

		[SerializeField]
		private Transform firePoint;

		[SerializeField]
		private Muzzle muzzlePrefab;

		public event Action OnDiedEvent;
		public CharacterData data;
		public GraphicsContainer GraphicsContainer;
		public Transform Target;
		private Health health;
		[SerializeField]
		private LayerMask targetLayer;

		private void Awake()
		{
			health = GetComponent<Health>();
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
			CancelInvoke(nameof(TriggerAttack));
			InvokeRepeating(nameof(TriggerAttack), 1f/data.AttackSpeed, 1f/data.AttackSpeed);
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

			float distanceToTarget = Vector2.Distance(Target.position, transform.position);

			if (distanceToTarget > data.AttackRange)
			{
				Target = null;
				return;
			}
			
			GraphicsContainer.BodySpriteRenderer.transform.DOScale(Vector3.one, .3f)
				.From(Vector3.one * 1.25f)
				.SetEase(Ease.OutBounce);

			Transform weaponTransform;

			(weaponTransform = GraphicsContainer.WeaponSpriteRenderer.transform).DOScale(Vector3.one, .3f)
				.From(Vector3.one * 1.25f)
				.SetEase(Ease.OutBounce);

			Instantiate(muzzlePrefab, weaponTransform.position, Quaternion.identity);
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
			if (data.Weapon.AreaSize <= 0.1f)
			{
				Target.GetComponent<Health>().TakeDamage(data.Damage);
			}
			else if (HelperExtras.GetAllObjectsInCircleRadius(point, data.Weapon.AreaSize, out List<Health> hits, targetLayer))
			{
				foreach (Health hit in hits)
				{
					hit.TakeDamage(data.Damage);
				}
			}
		}

		private void Die()
		{
			OnDiedEvent?.Invoke();
			ParticleSystem particles = Instantiate(deathParticles, transform.position, Quaternion.identity);
			Destroy(particles.gameObject, particles.main.duration);
			Destroy(gameObject);
		}

		private void OnDrawGizmosSelected()
		{
			if (data)
				Gizmos.DrawWireSphere(transform.position, data.AttackRange);
		}
	}
}