using System.Collections.Generic;
using GMTK.Characters;
using GMTK.Enemies;
using GMTK.Utilities;
using PNLib.Utility;
using UnityEngine;

namespace GMTK.Controllers
{
	public class Player : MonoBehaviour
	{
		[SerializeField]
		private float centerDistance = 1.5f;

		[SerializeField]
		private List<CharacterData> charactersInfo;

		[SerializeField]
		private float moveThreshold = .75f;
		
		[SerializeField]
		private CharacterData kingData;

		[SerializeField]
		private float moveSpeed = 6f;

		[SerializeField]
		private float rotationSpeed = 180f;

		private List<Character> activeCharacters;
		private Character main;

		private void Awake()
		{
			main = GetComponent<Character>();
		}

		private void Start()
		{
			main.SetData(kingData);
			float angleStep = 360f / charactersInfo.Count;

			for (int i = 0; i < charactersInfo.Count; i++)
			{
				CharacterData data = charactersInfo[i];

				if (data == null)
				{
					continue;
				}

				Vector3 direction = HelperExtras.GetNormalizedCircularPosition(angleStep * i);

				Character character = Instantiate(
					data.CharacterPrefab,
					transform.position + (direction * centerDistance),
					Quaternion.identity,
					transform
				);

				character.SetData(data);
				activeCharacters.Add(character);
				character.OnDiedEvent += () =>
				{
					activeCharacters.Remove(character);
					TriggerDeathEffects();
				};
			}
		}

		private void TriggerDeathEffects()
		{
			HelperExtras.Slow(.15f, .8f);
			HelperExtras.Shake(6, 60, 0.4f);
			HelperExtras.Flash(.2f);
		}

		private void OnEnable()
		{
			main.OnDiedEvent += Die;
		}

		private void OnDisable()
		{
			main.OnDiedEvent -= Die;
		}

		private void Update()
		{
			AdjustUnitsPosition();
			GetUnitTargets();
			Move();
		}

		private void GetUnitTargets()
		{
			for (int i = 0; i < activeCharacters.Count; i++)
			{
				Character character = activeCharacters[i];

				if (!character.Target
					&& Helper.GetClosestObjectInCircleRadius(
						character.transform.position,
						character.data.AttackRange,
						out Enemy hit
					))
				{
					character.Target = hit.transform;
				}
			}
		}

		private void Die()
		{
			TriggerDeathEffects();
			Debug.Log("GAME OVER");
		}

		private void Move()
		{
			Vector3 mouseWorldPosition = Helper.GetMouseWorldPosition();
			var position = transform.position;

			if (Vector2.Distance(position, mouseWorldPosition) < moveThreshold)
				return;
			
			Vector3 moveDirection = position.DirectionTo(mouseWorldPosition);
			Vector3 nextPosition = position + (moveDirection * (moveSpeed * Time.deltaTime));

			if (HelperExtras.IsInsideCameraViewport(nextPosition))
			{
				transform.position = nextPosition;
			}
		}

		private void AdjustUnitsPosition()
		{
			Vector3 mouseWorldPosition = Helper.GetMouseWorldPosition();
			Vector3 moveDirection = transform.position.DirectionTo(mouseWorldPosition);
			float angle = Helper.GetAngleFromVector(moveDirection);
			angle = Mathf.MoveTowardsAngle(transform.eulerAngles.z, angle, rotationSpeed * Time.deltaTime);
			transform.eulerAngles = new Vector3(0, 0, angle);
			
			Vector3 scale = main.GraphicsContainer.transform.localScale;
			scale.x = moveDirection.x >= 0 ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
			main.GraphicsContainer.transform.localScale = scale;
			
			foreach (Character character in activeCharacters)
			{
				if (character.Target)
					moveDirection = character.transform.position.DirectionTo(character.Target.position);
				else
					moveDirection = transform.position.DirectionTo(mouseWorldPosition);
				
				scale = character.GraphicsContainer.transform.localScale;
				scale.x = moveDirection.x >= 0 ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
				character.GraphicsContainer.transform.localScale = scale;
			}
		}
	}
}