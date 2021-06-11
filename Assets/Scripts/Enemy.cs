using PNLib.Utility;
using PNTemplate._Temp;
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

		private Servant target;
		private Rigidbody2D rb;

		private void Awake()
		{
			rb = GetComponent<Rigidbody2D>();
		}

		private void Start()
		{
			LookForTarget();
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

		private void Move()
		{
			Transform myTransform = transform;
			Vector3 newPosition = myTransform.position + (myTransform.right * (moveSpeed * Time.deltaTime));

			if (HelperExtras.IsInsideCameraViewport(newPosition))
			{
				rb.velocity = transform.right * moveSpeed;
			}
		}

		private void RotateTowardsTarget()
		{
			//TODO: Lerp rotation here
			float angle = Helper.GetAngleFromVector(transform.position.DirectionTo(target.transform.position));
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