using PNLib.Utility;
using UnityEngine;

namespace PNTemplate
{
	public class Enemy : MonoBehaviour
	{
		[SerializeField]
		private float moveSpeed = 4f;

		[SerializeField]
		private float rotationSpeed = 180f;

		[SerializeField]
		private float sightRadius = 18f;

		private Health health;
		private Rigidbody2D rb;
		private Servant target;

		private void Awake()
		{
			rb = GetComponent<Rigidbody2D>();
			health = GetComponent<Health>();
		}

		private void Start()
		{
			LookForTarget();
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
			if (!target)
			{
				LookForTarget();
			}
			else
			{
				RotateTowardsTarget();
				Move();
			}
		}

		private void Die()
		{
			Destroy(gameObject);
		}

		private void Move()
		{
			Transform myTransform = transform;
			Vector3 nextPosition = myTransform.position + (myTransform.right * (moveSpeed * Time.deltaTime));

			if (HelperExtras.IsInsideCameraViewport(nextPosition))
			{
				rb.velocity = transform.right * moveSpeed;
			}
			else
			{
				rb.velocity = Vector2.zero;
			}
		}

		private void RotateTowardsTarget()
		{
			float angle = Helper.GetAngleFromVector(transform.position.DirectionTo(target.transform.position));
			angle = Mathf.MoveTowardsAngle(transform.eulerAngles.z, angle, rotationSpeed * Time.deltaTime);
			transform.eulerAngles = new Vector3(0, 0, angle);
		}

		private void LookForTarget()
		{
			if (Helper.GetClosestObjectInCircleRadius(transform.position, sightRadius, out Servant hit))
			{
				target = hit;
			}
		}
	}
}