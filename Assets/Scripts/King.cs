using System.Collections;
using System.Collections.Generic;
using PNLib.Utility;
using PNTemplate._Temp;
using UnityEngine;

namespace PNTemplate
{
	public class King : MonoBehaviour
	{
		[SerializeField]
		private float mouseDistanceFollowThreshold = .5f;

		[SerializeField]
		private float moveSpeed = 6f;

		[SerializeField]
		private List<Servant> servants;

		[SerializeField]
		private float distanceFromKing;
		
		private Rigidbody2D rb;

		private void Awake()
		{
			rb = GetComponent<Rigidbody2D>();
		}

		private void Update()
		{
			RotateTowardsMouse();

			if (Input.GetMouseButton(0))
			{
				Attack();
			}
			else
			{
				Move();
			}
		}

		private void Attack() { }

		private void Move()
		{
			Transform myTransform = transform;
			var newPosition = myTransform.position + myTransform.right * (moveSpeed * Time.deltaTime);

			if (HelperExtras.IsInsideCameraViewport(newPosition))
			{
				rb.velocity = transform.right * moveSpeed;
			}
		}

		private void RotateTowardsMouse()
		{
			//TODO: Lerp rotation so it doesn't rotate instantly -- Prevents crazy mouse spinning around character
			Vector3 mouseWorldPosition = Helper.GetMouseWorldPosition();

			if (Vector2.Distance(transform.position, mouseWorldPosition) < mouseDistanceFollowThreshold)
			{
				return;
			}

			float angle = Helper.GetAngleFromVector(transform.position.DirectionTo(mouseWorldPosition));
			transform.eulerAngles = new Vector3(0, 0, angle);
		}

		public void AddServant(Servant servant)
		{
			servants.Add(servant);
			StartCoroutine(OrganizeServants());
		}

		private IEnumerator OrganizeServants()
		{
			int degreeStep = 360 / servants.Count;
			for (int i = 0; i < servants.Count; i++)
			{
				Servant servant = servants[i];
				Vector3 position = HelperExtras.GetNormalizedCircularPosition(i * degreeStep);
				servant.DisconnectAll();
				servant.transform.position = transform.position + (position * distanceFromKing);
			}
			
			yield return null;
			
			foreach (Servant servant in servants)
			{
				servant.Connect(this, servants);
			}
		}
	}
}