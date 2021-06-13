using GMTK.Characters;
using GMTK.Utilities;
using PNLib.Utility;
using UnityEngine;
using CharacterInfo = GMTK.Info.CharacterInfo;

namespace GMTK.Enemies
{
	public class Enemy : MonoBehaviour
	{
		[SerializeField]
		private CharacterInfo enemyInfo;

		[SerializeField]
		private float moveSpeed = 4f;

		[SerializeField]
		private LayerMask playerLayer;

		[SerializeField]
		private float rotationSpeed = 180f;

		private Character character;
		private Health health;
		private Rigidbody2D rb;

		private void Awake()
		{
			rb = GetComponent<Rigidbody2D>();
			health = GetComponent<Health>();
			character = GetComponent<Character>();
		}

		private void Start()
		{
			character.Set(enemyInfo);
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
			if (!character.Target)
			{
				LookForTarget();
			}

			RotateTowardsTarget();
			Move();
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
			Vector3 scale = character.GraphicsContainer.transform.localScale;
			scale.x = moveDirection.x >= 0 ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
			character.GraphicsContainer.transform.localScale = scale;

			if (character.Target)
			{
				float distanceToTarget = Vector2.Distance(character.Target.position, transform.position);

				if (distanceToTarget < enemyInfo.AttackRadius)
				{
					rb.velocity = Vector2.zero;
					return;
				}
			}

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
			float angle = Helper.GetAngleFromVector(transform.position.DirectionTo(character.Target.position));
			angle = Mathf.MoveTowardsAngle(transform.eulerAngles.z, angle, rotationSpeed * Time.deltaTime);
			transform.eulerAngles = new Vector3(0, 0, angle);
		}

		private void LookForTarget()
		{
			if (!character.Target
				&& HelperExtras.GetClosestObjectInCircleRadius(
					character.transform.position,
					999,
					out PlayerUnit hit,
					playerLayer
				))
			{
				character.Target = hit.transform;
			}
		}
	}
}