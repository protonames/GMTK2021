using PNTemplate._Temp;
using UnityEngine;

namespace PNTemplate
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
				Servant servant = other.GetComponent<Servant>();

				if (servant)
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