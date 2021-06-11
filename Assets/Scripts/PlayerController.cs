using PNLib.Utility;
using PNTemplate._Temp;
using UnityEngine;

namespace PNTemplate
{
	public class PlayerController : MonoBehaviour
	{
		[SerializeField]
		private float mouseDistanceFollowThreshold = .5f;

		[SerializeField]
		private float moveSpeed = 6f;

		[SerializeField]
		private Character[] characters;

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
				myTransform.position = newPosition;
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
	}
}