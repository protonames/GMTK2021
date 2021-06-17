using System.Collections;
using GMTK.Enemies;
using GMTK.Weapons;
using PNLib.Utility;
using UnityEngine;

namespace PNTemplate._Temp
{
	public class Projectile : MonoBehaviour
	{
		[SerializeField]
		private float homingRadius = 18f;

		[SerializeField]
		private float rotateSpeed = 2f;

		private float moveSpeed;
		private Enemy target;
		private ProjectilePattern type;

		private void Update()
		{
			MoveForward();

			if (type == ProjectilePattern.Homing)
			{
				if (!target)
				{
					FindTarget();
					StartCoroutine(RotateTowardsTargetEnumerator());
				}
			}
		}

		private void OnTriggerEnter2D(Collider2D other)
		{
			if (other.GetComponent<Projectile>())
			{
				return;
			}

			Destroy(gameObject);
		}

		public void Fire(float speed, ProjectilePattern type)
		{
			moveSpeed = speed;
			this.type = type;
		}

		private void FindTarget()
		{
			if (Helper.GetClosestObjectInCircleRadius(transform.position, homingRadius, out Enemy hit))
			{
				target = hit;
			}
		}

		private IEnumerator RotateTowardsTargetEnumerator()
		{
			if (!target)
			{
				yield break;
			}

			float t = 0f;

			while (t < 1)
			{
				t += Time.deltaTime / rotateSpeed;
				Vector3 direction = transform.position.DirectionTo(target.transform.position);
				float angle = Helper.GetAngleFromVector(direction);
				angle = Mathf.LerpAngle(transform.eulerAngles.z, angle, t);
				transform.eulerAngles = new Vector3(0, 0, angle);
				yield return null;
			}
		}

		private void MoveForward()
		{
			Transform myTransform = transform;
			myTransform.position += myTransform.right * (moveSpeed * Time.deltaTime);
		}
	}
}