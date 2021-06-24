using System;
using DG.Tweening;
using GMTK.Utilities;
using GMTK.Weapons;
using PNLib.Utility;
using UnityEngine;
using CharacterInfo = GMTK.Info.CharacterInfo;

namespace GMTK.Characters
{
	[RequireComponent(typeof(Health))]
	public class Character : MonoBehaviour
	{
		[SerializeField]
		private ParticleSystem deathParticles;

		[SerializeField]
		private Transform firePoint;

		public CharacterInfo Info { get; private set; }

		[SerializeField]
		private Muzzle muzzlePrefab;

		public Transform Target { get; set; }
		public event Action OnDiedEvent;
		public GraphicsContainer GraphicsContainer;
		private Health health;
		private AttackController attackController;

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

		public void Set(CharacterInfo info)
		{
			Info = info;
			GraphicsContainer.BodySpriteRenderer.sprite = info.BodySprite;
			GraphicsContainer.WeaponSpriteRenderer.sprite = info.WeaponSprite;
			health.SetHealth(info.Health);

			CancelInvoke(nameof(TriggerAttack));

			if (info.weaponData != null)
			{
				if (attackController != null) Destroy(attackController.gameObject);
				attackController = info.weaponData.ReturnController(this);
			}

			// InvokeRepeating(nameof(TriggerAttack), 1f / info.AttackSpeed, 1f / info.AttackSpeed);
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
			if (attackController != null)
				attackController.Activate(Target);

			if (!Target)
			{
				return;
			}

			float distanceToTarget = Vector2.Distance(Target.position, transform.position);

			if (distanceToTarget > Info.AttackRadius)
			{
				Target = null;
				return;
			}

			GraphicsContainer.BodySpriteRenderer.transform.DOScale(Vector3.one, .3f)
				.From(Vector3.one * 1.25f)
				.SetEase(Ease.OutBounce);

			RotateFirePointTowardsTarget();

			// Attack();
		}

		private void Attack()
		{
			Transform weaponTransform;

			(weaponTransform = GraphicsContainer.WeaponSpriteRenderer.transform).DOScale(Vector3.one, .3f)
				.From(Vector3.one * 1.25f)
				.SetEase(Ease.OutBounce);

			Health targetHealth = Target.GetComponent<Health>();

			if (Info.projectile != null)
			{
				Instantiate(muzzlePrefab, weaponTransform.position, Quaternion.identity);
				Info.LaunchProjectile(firePoint, targetHealth);
			}
			else
			{
				targetHealth.TakeDamage(Info.Damage);
				if (Info.specialEffect != null) Info.specialEffect.Activate();
			}
		}

		public void AttackAnimation()
		{
			Transform weaponTransform;

			(weaponTransform = GraphicsContainer.WeaponSpriteRenderer.transform).DOScale(Vector3.one, .3f)
				.From(Vector3.one * 1.25f)
				.SetEase(Ease.OutBounce);
		}

		private void Die()
		{
			OnDiedEvent?.Invoke();
			ParticleSystem particles = Instantiate(deathParticles, transform.position, Quaternion.identity);
			Destroy(particles.gameObject, particles.main.duration);
			Destroy(gameObject);
		}
	}
}