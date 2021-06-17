using PNLib.Utility;
using UnityEngine;

namespace PNTemplate._Temp
{
	public class Player : MonoBehaviour
	{
		[SerializeField]
		private float fireInterval = .5f;

		[SerializeField]
		private float fireSpeed = 15f;

		[SerializeField]
		private float knockBackDuration = .1f;

		[SerializeField]
		private Transform[] missileFirePoints;

		[SerializeField]
		private float mouseDistanceFollowThreshold = .5f;

		[SerializeField]
		private float moveSpeed = 3f;

		[SerializeField]
		private ShipProjectile shipProjectilePrefab;

		[SerializeField]
		private Transform simpleFirePoint;

		private float nextFireTime = -1;
		private float nextKnockBack = -1;

		private void Update()
		{
			RotateTowardsMouse();

			if (Input.GetMouseButton(0))
			{
				HandleFire();
			}
			else if (Input.GetMouseButton(1))
			{
				HandleMissiles();
			}
			else
			{
				HandleMovement();
			}

			if (Time.time < nextKnockBack)
			{
				transform.position -= transform.right * (fireSpeed * Time.deltaTime);
			}
		}

		private void HandleMissiles()
		{
			if (Time.time < nextFireTime)
			{
				return;
			}

			SimpleMissiles();
		}

		private void SimpleMissiles()
		{
			nextFireTime = Time.time + fireInterval;

			foreach (Transform point in missileFirePoints)
			{
				ShipProjectile shipProjectile = Instantiate(shipProjectilePrefab, point.position, point.rotation);

				//shipProjectile.Fire(fireSpeed, FireType.Homing);
			}
		}

		private void HandleFire()
		{
			if (Time.time < nextFireTime)
			{
				return;
			}

			SimpleFire();
		}

		private void SimpleFire()
		{
			nextFireTime = Time.time + fireInterval;
			nextKnockBack = Time.time + knockBackDuration;

			ShipProjectile shipProjectile = Instantiate(
				shipProjectilePrefab,
				simpleFirePoint.position,
				simpleFirePoint.rotation
			);

			//shipProjectile.Fire(fireSpeed, FireType.Directional);
		}

		private void HandleMovement()
		{
			transform.position += transform.right * (moveSpeed * Time.deltaTime);
		}

		private void RotateTowardsMouse()
		{
			Vector3 mouseWorldPosition = Helper.GetMouseWorldPosition();

			if (Vector2.Distance(transform.position, mouseWorldPosition) < mouseDistanceFollowThreshold)
			{
				return;
			}

			float angle = Helper.GetAngleFromVector(transform.position.DirectionTo(mouseWorldPosition));
			transform.eulerAngles = new Vector3(0, 0, angle);
		}

		private void SwapFireMode()
		{
			nextFireTime = 0;
		}
	}
}