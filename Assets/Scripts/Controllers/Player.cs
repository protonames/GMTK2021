using System.Collections;
using System.Collections.Generic;
using GMTK.Characters;
using GMTK.Enemies;
using GMTK.Info;
using GMTK.Utilities;
using PNLib.Utility;
using UnityEngine;
using CharacterInfo = GMTK.Info.CharacterInfo;

namespace GMTK.Controllers
{
	public class Player : MonoBehaviour
	{
		[SerializeField]
		private float centerDistance = 1.5f;

		[SerializeField]
		private Character characterPrefab;

		[SerializeField]
		private CharacterInfo kingInfo;

		[SerializeField]
		private float moveSpeed = 6f;

		[SerializeField]
		private float moveThreshold = .75f;

		[SerializeField]
		private PartyInfo partyInfo;

		[SerializeField]
		private float rotationSpeed = 180f;

		private readonly List<Character> activeCharacters = new List<Character>();
		private Character main;

		private void Awake()
		{
			main = GetComponent<Character>();
		}

		private void Start()
		{
			main.Set(kingInfo);
			float angleStep = 360f / partyInfo.Party.Count;

			for (int i = 0; i < partyInfo.Party.Count; i++)
			{
				CharacterInfo info = partyInfo.Party[i];

				if (info == null)
				{
					continue;
				}

				Vector3 direction = HelperExtras.GetNormalizedCircularPosition(angleStep * i);

				Character character = Instantiate(
					characterPrefab,
					transform.position + (direction * centerDistance),
					Quaternion.identity,
					transform
				);

				character.Set(info);
				activeCharacters.Add(character);

				character.OnDiedEvent += () =>
				{
					activeCharacters.Remove(character);
					TriggerDeathEffects();
				};
			}
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

		private void TriggerDeathEffects()
		{
			HelperExtras.Slow(.15f, .8f);
			HelperExtras.Shake(6, 60, 0.4f);
			HelperExtras.Flash(.2f);
		}

		private void GetUnitTargets()
		{
			for (int i = 0; i < activeCharacters.Count; i++)
			{
				Character character = activeCharacters[i];

				if (!character.Target
					&& Helper.GetClosestObjectInCircleRadius(
						character.transform.position,
						character.Info.AttackRadius,
						out Enemy hit
					))
				{
					character.Target = hit.transform;
				}
			}
		}

		private void Die()
		{
			StartCoroutine(GameOverRoutine());
		}

		private IEnumerator GameOverRoutine()
		{
			TriggerDeathEffects();
			yield return new WaitForSecondsRealtime(3f);

			GameSceneManager.Load(0);
		}

		private void Move()
		{
			Vector3 mouseWorldPosition = Helper.GetMouseWorldPosition();
			Vector3 position = transform.position;

			if (Vector2.Distance(position, mouseWorldPosition) < moveThreshold)
			{
				return;
			}

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
				{
					moveDirection = character.transform.position.DirectionTo(character.Target.position);
				}
				else
				{
					moveDirection = transform.position.DirectionTo(mouseWorldPosition);
				}

				scale = character.GraphicsContainer.transform.localScale;
				scale.x = moveDirection.x >= 0 ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
				character.GraphicsContainer.transform.localScale = scale;
			}
		}
	}
}