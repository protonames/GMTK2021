using GMTK.Characters;
using GMTK.Enemies;
using GMTK.Utilities;
using UnityEngine;

namespace GMTK.Weapons
{
	public class Projectile : MonoBehaviour
	{
		public bool PlayerProjectile = true;
		private int damage;
		private Rigidbody2D rb;

		private void Awake()
		{
			rb = GetComponent<Rigidbody2D>();
		}

		private void Update()
		{
			if (!HelperExtras.IsInsideCameraViewport(transform.position))
			{
				Destroy(gameObject);
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
					health.TakeDamage(damage);
					Die();
				}
			}
			else
			{
				Character character = other.GetComponent<Character>();

				if (character)
				{
					health.TakeDamage(damage);
					Die();
				}
			}
		}

		public void Launch(bool playerProjectile, float speed, int damage)
		{
			PlayerProjectile = playerProjectile;
			this.damage = damage;
			rb.velocity = transform.right * speed;
		}

		private void Die()
		{
			Destroy(gameObject);
		}
	}
}