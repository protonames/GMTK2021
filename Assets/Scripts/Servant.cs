using PNLib.Utility;
using UnityEngine;

namespace PNTemplate
{
	[RequireComponent(typeof(Health))]
	public class Servant : MonoBehaviour
	{
		[SerializeField]
		private Transform firePoint;

		[SerializeField]
		private ServantData servantData;

		private Health health;
		private Enemy target;

		private void Awake()
		{
			health = GetComponent<Health>();
		}

		private void Start()
		{
			//TODO: Assign servant data dynamically 
			InvokeRepeating(nameof(Fire), servantData.FireRate, servantData.FireRate);
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
				&& Helper.GetClosestObjectInCircleRadius(transform.position, servantData.SightRadius, out Enemy hit))
			{
				target = hit;
			}
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

		private void Fire()
		{
			if (!target)
			{
				return;
			}

			RotateFirePointTowardsTarget();
			Vector3 spawnAngle = firePoint.eulerAngles;
			Vector3 spawnPoint = firePoint.position;
			Projectile projectile = Instantiate(servantData.ProjectilePrefab, spawnPoint, Quaternion.Euler(spawnAngle));
			projectile.Launch(true, servantData.ProjectileSpeed, servantData.ProjectileDamage);
		}

		private void Die()
		{
			Destroy(gameObject);
		}
	}
}