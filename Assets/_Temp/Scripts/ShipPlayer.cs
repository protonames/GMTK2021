using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using GMTK.Utilities;
using GMTK.Weapons;
using TMPro;
using UnityEngine;

namespace PNTemplate._Temp
{
	public class ShipPlayer : MonoBehaviour
	{
		[SerializeField]
		private ParticleSystem deathEffectPrefab;

		[SerializeField]
		private Transform firePoint;

		[SerializeField]
		private GameObject graphics;

		[SerializeField]
		private float rotationSpeed = 180f;

		[SerializeField]
		private List<ShipData> ships;

		private ShipData ship;
		private TrailRenderer trail;

		private void Awake()
		{
			trail = GetComponentInChildren<TrailRenderer>();
		}

		private void Start()
		{
			SetShip(0);
		}

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.Space))
			{
				NextShip();
			}

			Rotate();
			Move();

			if (!HelperExtras.IsInsideCameraViewport(transform.position))
			{
				Die();
			}

			if (Input.GetKeyDown(KeyCode.F4))
			{
				Die();
			}
		}

		private void NextShip()
		{
			int shipIndex = ships.IndexOf(ship);
			shipIndex = (shipIndex + 1) % ships.Count;
			SetShip(shipIndex);
		}

		private void SetShip(int shipIndex)
		{
			ship = ships[shipIndex];
			FindObjectOfType<TMP_Text>().SetText(ship.name);

			GameObject newVisual = Instantiate(
				ship.Graphics,
				graphics.transform.position,
				graphics.transform.rotation,
				graphics.transform.parent
			);

			Destroy(graphics);
			graphics = newVisual;
			CancelInvoke(nameof(Fire));
			InvokeRepeating(nameof(Fire), ship.FireRate, ship.FireRate);
		}

		private void Move()
		{
			float boost = 1f;
			trail.startColor = trail.endColor = Color.yellow;

			if (Input.GetKey(KeyCode.UpArrow))
			{
				boost = 1 + ship.BoostRate;
				trail.startColor = trail.endColor = Color.cyan;
			}
			else if (Input.GetKey(KeyCode.DownArrow))
			{
				boost = 1 - ship.BoostRate;
				trail.startColor = trail.endColor = Color.cyan;
			}

			Transform myTransform = transform;
			float speed = ship.MoveSpeed * boost;
			myTransform.position += myTransform.right * (speed * Time.deltaTime);
		}

		private void Rotate()
		{
			if (Input.GetKey(KeyCode.LeftArrow))
			{
				transform.eulerAngles += Vector3.forward * (Time.deltaTime * rotationSpeed);
			}
			else if (Input.GetKey(KeyCode.RightArrow))
			{
				transform.eulerAngles -= Vector3.forward * (Time.deltaTime * rotationSpeed);
			}
		}

		private void Fire()
		{
			switch (ship.ProjectilePattern)
			{
				case ProjectilePattern.Straight:
					StartCoroutine(LinearFire(ship.ProjectileAmount, ship.ProjectileAccelerate));
					break;
				case ProjectilePattern.Angle:
					StartCoroutine(AngleFire(ship.ProjectileAmount, ship.ProjectileAccelerate));
					break;
				case ProjectilePattern.Radial:
					StartCoroutine(RadialFire(ship.ProjectileAmount, ship.ProjectileAccelerate));
					break;
				case ProjectilePattern.Burst:
					StartCoroutine(AlternateFire(ship.ProjectileAmount, ship.ProjectileAccelerate));
					break;
			}
		}

		private IEnumerator AlternateFire(int amount, bool accelerate)
		{
			int offsetDirection = -1;

			for (int i = 0; i < amount; i++)
			{
				float currentOffset = (ship.LinearShotSpread / 2f) * offsetDirection;
				offsetDirection *= -1;
				Vector3 spawnAngle = firePoint.eulerAngles;
				Vector3 spawnPoint = firePoint.position;

				ShipProjectile shipProjectile = Instantiate(
					ship.ShipProjectilePrefab,
					spawnPoint + (transform.up * currentOffset),
					Quaternion.Euler(spawnAngle)
				);

				MuzzleFlash(shipProjectile.transform.position);
				shipProjectile.SetSpeed(ship.ProjectileSpeed);

				if (accelerate)
				{
					AccelerateProjectile(shipProjectile);
				}

				yield return new WaitForSeconds(ship.BurstInterval);
			}
		}

		private IEnumerator AngleFire(int amount, bool accelerate)
		{
			Vector3 spawnAngle = firePoint.eulerAngles;
			Vector3 angleStep = new Vector3(0, 0, ship.MultiShotSpread);
			int projectilesPerSide = amount / 2;
			Vector3 currentAngle = spawnAngle - (angleStep * projectilesPerSide);

			if (amount != 0 && (amount % 2) == 0)
			{
				currentAngle -= angleStep / 2f;
			}

			for (int i = 0; i < amount; i++)
			{
				Vector3 spawnPoint = firePoint.position;

				ShipProjectile shipProjectile = Instantiate(
					ship.ShipProjectilePrefab,
					spawnPoint,
					Quaternion.Euler(currentAngle)
				);

				MuzzleFlash(shipProjectile.transform.position);
				shipProjectile.SetSpeed(ship.ProjectileSpeed);

				if (accelerate)
				{
					AccelerateProjectile(shipProjectile);
				}

				currentAngle += angleStep;
				yield return new WaitForSeconds(ship.BurstInterval);
			}
		}

		private IEnumerator RadialFire(int amount, bool accelerate)
		{
			Vector3 centerPoint = transform.position;
			int angleStep = 360 / amount;
			float currentAngle = firePoint.eulerAngles.z;
			float distanceFromCenter = Vector2.Distance(firePoint.position, centerPoint);

			for (int i = 0; i < amount; i++)
			{
				centerPoint = transform.position;
				Vector3 position = HelperExtras.GetNormalizedCircularPosition(currentAngle);

				ShipProjectile shipProjectile = Instantiate(
					ship.ShipProjectilePrefab,
					centerPoint + (position * distanceFromCenter),
					Quaternion.Euler(Vector3.forward * currentAngle)
				);

				MuzzleFlash(shipProjectile.transform.position);
				shipProjectile.SetSpeed(ship.ProjectileSpeed);

				if (accelerate)
				{
					AccelerateProjectile(shipProjectile);
				}

				currentAngle += angleStep;
				yield return new WaitForSeconds(ship.BurstInterval);
			}
		}

		private void AccelerateProjectile(ShipProjectile shipProjectile)
		{
			shipProjectile.AccelerateOverTime(
				ship.ProjectileSpeed * (1f - ship.AccelerationScale),
				ship.ProjectileSpeed * (1f + ship.AccelerationScale),
				ship.AccelerationTime
			);
		}

		private IEnumerator LinearFire(int amount, bool accelerate)
		{
			float offsetStep = ship.LinearShotSpread;
			int projectilesPerSide = amount / 2;
			float currentOffset = -(offsetStep * projectilesPerSide);

			if (amount != 0 && (amount % 2) == 0)
			{
				currentOffset += offsetStep / 2f;
			}

			for (int i = 0; i < amount; i++)
			{
				Vector3 spawnAngle = firePoint.eulerAngles;
				Vector3 spawnPoint = firePoint.position;

				ShipProjectile shipProjectile = Instantiate(
					ship.ShipProjectilePrefab,
					spawnPoint + (transform.up * currentOffset),
					Quaternion.Euler(spawnAngle)
				);

				MuzzleFlash(shipProjectile.transform.position);
				shipProjectile.SetSpeed(ship.ProjectileSpeed);

				if (accelerate)
				{
					AccelerateProjectile(shipProjectile);
				}

				currentOffset += offsetStep;
				yield return new WaitForSeconds(ship.BurstInterval);
			}
		}

		private void MuzzleFlash(Vector3 point)
		{
			Instantiate(ship.Muzzle, point, transform.rotation);
			transform.DOScale(Vector3.one, .3f).From(Vector3.one * 1.4f).SetEase(Ease.OutBounce);
		}

		private void Die()
		{
			ParticleSystem particles = Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);
			Destroy(particles.gameObject, particles.main.duration);
			HelperExtras.Slow(.15f, .8f);
			HelperExtras.Shake(6, 60, 0.4f);
			HelperExtras.Flash(.2f);
			Destroy(gameObject);
		}
	}
}