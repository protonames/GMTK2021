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
		
		public bool PlayerProjectile = true;
		public int Damage;
		private Rigidbody2D rb;
		public event Action OnDiedEvent;
		public float AreaSize;

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
				}
			}
			else
			{
				Character character = other.GetComponent<Character>();

				if (character)
				{
					health.TakeDamage(Damage);
					Die();
				}
			}
		}

		public void Launch(bool playerProjectile, float speed, int damage, float areaSize = 0)
		{
			PlayerProjectile = playerProjectile;
			Damage = damage;
			AreaSize = areaSize;
			rb.velocity = transform.right * speed;
		}

		private void Die()
		{
			OnDiedEvent?.Invoke();
			Destroy(gameObject);
		}
	}
}