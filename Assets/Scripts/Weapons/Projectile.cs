using System;
using GMTK.Characters;
using GMTK.Enemies;
using GMTK.Utilities;
using UnityEngine;

namespace GMTK.Weapons
{
	public class Projectile : MonoBehaviour
	{
		[SerializeField]
		private GameObject destroyParticles;

		public event Action OnDiedEvent;
		public float AreaSize;
		public int Damage;
		public bool PlayerProjectile = true;
		private Rigidbody2D rb;
		private Health target;
		private SpecialEffect specialEffect;

		private void Awake()
		{
			rb = GetComponent<Rigidbody2D>();
		}

		private void Update()
		{
			if (!HelperExtras.IsInsideCameraViewport(transform.position))
			{
				Die();
			}
		}

		private void OnTriggerEnter2D(Collider2D other)
		{
			Health health = other.GetComponent<Health>();

			if (!health)
			{
				return;
			}

			if (PlayerProjectile)
			{
				Enemy enemy = other.GetComponent<Enemy>();

				if (enemy)
				{
					health.TakeDamage(Damage);
					Die();
					if (specialEffect != null) specialEffect.Activate();
				}
			}
			else
			{
				Character character = other.GetComponent<Character>();

				if (character)
				{
					health.TakeDamage(Damage);
					Die();
					if (specialEffect != null) specialEffect.Activate();
				}
			}
		}

		public void Launch(Health target, bool playerProjectile, float speed, int damage, float areaSize = 0)
		{
			this.target = target;
			PlayerProjectile = playerProjectile;
			Damage = damage;
			AreaSize = areaSize;

			transform.right = target.gameObject.transform.position - transform.position;
			rb.velocity = transform.right * speed;
		}

		protected virtual void Die()
		{
			OnDiedEvent?.Invoke();
			Destroy(gameObject);
		}
	}
}