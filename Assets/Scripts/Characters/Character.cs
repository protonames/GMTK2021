using System;
using DG.Tweening;
using GMTK.Controllers;
using GMTK.Utilities;
using GMTK.Weapons;
using PNLib.Utility;
using UnityEngine;
using ClassInfo = GMTK.Info.ClassInfo;

namespace GMTK.Characters
{
	[RequireComponent(typeof(Health))]
	public class Character : MonoBehaviour
	{
		[SerializeField]
		private ParticleSystem deathParticles;

		[SerializeField]
		private Transform firePoint;

		public ClassInfo Info { get; private set; }

		[SerializeField]
		private Muzzle muzzlePrefab;

		public Transform Target { get; set; }
		public event Action OnDiedEvent;
		public GraphicsContainer GraphicsContainer;
		private Health health;
		private AttackController attackController;

		public Player Party;

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

		public void Set(ClassInfo info)
		{
			Info = info;
			GraphicsContainer.BodySpriteRenderer.sprite = info.BodySprite;
			GraphicsContainer.WeaponSpriteRenderer.sprite = info.WeaponSprite;
			health.SetHealth(info.Health);

			// CancelInvoke(nameof(TriggerAttack));

			if (info.weaponData != null)
			{
				if (attackController != null) Destroy(attackController.gameObject);
				attackController = info.weaponData.ReturnController(this);
			}

			// InvokeRepeating(nameof(TriggerAttack), 1f / info.AttackSpeed, 1f / info.AttackSpeed);
		}

		private void Update()
		{
			LookUpdate();
		}

		public void LookUpdate()
		{
			if (Party == null) return;

			Vector3 moveDirection = Party.PartyLookPoint;
			if (attackController.Target != null)
			{
				moveDirection = this.transform.position.DirectionTo(attackController.Target.transform.position);
			}

			Vector3 scale = this.GraphicsContainer.transform.localScale;
			scale.x = moveDirection.x >= 0 ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
			this.GraphicsContainer.transform.localScale = scale;
		}


		public void RotateFirePointTowardsTarget(Transform targetTransform)
		{
			if (!targetTransform)
			{
				return;
			}

			float angle = Helper.GetAngleFromVector(firePoint.position.DirectionTo(targetTransform.transform.position));
			firePoint.eulerAngles = new Vector3(0, 0, angle);
		}

		// private void TriggerAttack()
		// {
		// 	if (attackController != null)
		// 		attackController.Activate(Target);

		// 	if (!Target)
		// 	{
		// 		return;
		// 	}

		// 	float distanceToTarget = Vector2.Distance(Target.position, transform.position);

		// 	if (distanceToTarget > Info.AttackRadius)
		// 	{
		// 		Target = null;
		// 		return;
		// 	}

		// 	GraphicsContainer.BodySpriteRenderer.transform.DOScale(Vector3.one, .3f)
		// 		.From(Vector3.one * 1.25f)
		// 		.SetEase(Ease.OutBounce);

		// 	RotateFirePointTowardsTarget();

		// 	// Attack();
		// }

		// private void Attack()
		// {
		// 	Transform weaponTransform;

		// 	(weaponTransform = GraphicsContainer.WeaponSpriteRenderer.transform).DOScale(Vector3.one, .3f)
		// 		.From(Vector3.one * 1.25f)
		// 		.SetEase(Ease.OutBounce);

		// 	Health targetHealth = Target.GetComponent<Health>();

		// 	if (Info.projectile != null)
		// 	{
		// 		Instantiate(muzzlePrefab, weaponTransform.position, Quaternion.identity);
		// 		Info.LaunchProjectile(firePoint, targetHealth);
		// 	}
		// 	else
		// 	{
		// 		targetHealth.TakeDamage(Info.Damage);
		// 		if (Info.specialEffect != null) Info.specialEffect.Activate();
		// 	}
		// }

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