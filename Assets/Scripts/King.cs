using System.Collections.Generic;
using PNLib.Utility;
using UnityEngine;

namespace PNTemplate
{
	public class King : MonoBehaviour
	{
		[SerializeField]
		private float distanceFromKing;

		[SerializeField]
		private float moveSpeed = 6f;

		[SerializeField]
		private float rotationSpeed = 180f;

		[SerializeField]
		private Transform servantContainer;

		[SerializeField]
		private List<Servant> servants;

		private Rigidbody2D rb;

		private void Awake()
		{
			rb = GetComponent<Rigidbody2D>();
		}

		private void Update()
		{
			RotateServantsTowardsMouse();
			Move();
		}

		public void AddServant(Servant servant)
		{
			servants.Add(servant);
			servant.transform.SetParent(servantContainer);
			OrganizeServants();
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

		private void RotateServantsTowardsMouse()
		{
			Vector3 mouseWorldPosition = Helper.GetMouseWorldPosition();
			float angle = Helper.GetAngleFromVector(servantContainer.position.DirectionTo(mouseWorldPosition));
			angle = Mathf.MoveTowardsAngle(transform.eulerAngles.z, angle, rotationSpeed * Time.deltaTime);
			transform.eulerAngles = new Vector3(0, 0, angle);
		}

		private void OrganizeServants()
		{
			int degreeStep = 360 / servants.Count;

			for (int i = 0; i < servants.Count; i++)
			{
				Servant servant = servants[i];
				Vector3 position = HelperExtras.GetNormalizedCircularPosition(i * degreeStep);
				servant.transform.position = transform.position + (position * distanceFromKing);
				servant.GetComponent<Rigidbody2D>().isKinematic = true;
			}
		}
	}
}