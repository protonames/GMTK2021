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
			InvokeRepeating(nameof(TriggerAttack), 1f / info.AttackSpeed, 1f / info.AttackSpeed);
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

			if (distanceToTarget > Info.AttackRadius)
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
			Attack();
		}

		private void Attack()
		{
			Target.GetComponent<Health>().TakeDamage(Info.Damage);
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