using GMTK.Characters;
using GMTK.Utilities;
using PNLib.Utility;
using UnityEngine;

namespace GMTK.Enemies
{
	public class Enemy : MonoBehaviour
	{
		[SerializeField]
		private Transform graphicsContainer;

		[SerializeField]
		private float moveSpeed = 4f;

		[SerializeField]
		private float rotationSpeed = 180f;

		[SerializeField]
		private float sightRadius = 18f;

		private Health health;
		private Rigidbody2D rb;
		private Character target;

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
			Vector3 moveDirection = myTransform.position.DirectionTo(nextPosition);
			Vector3 scale = graphicsContainer.localScale;
			scale.x = moveDirection.x >= 0 ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
			graphicsContainer.localScale = scale;

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
			if (Helper.GetClosestObjectInCircleRadius(transform.position, sightRadius, out Character hit))
			{
				target = hit;
			}
		}
	}
}