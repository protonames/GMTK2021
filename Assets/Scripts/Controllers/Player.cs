using System.Collections.Generic;
using GMTK.Characters;
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
		private CharacterData kingData;

		[SerializeField]
		private float moveSpeed = 6f;

		[SerializeField]
		private float rotationSpeed = 180f;

		private List<Character> activeCharacters;
		private Character main;
		private Rigidbody2D rb;

		private void Awake()
		{
			rb = GetComponent<Rigidbody2D>();
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
				character.OnDiedEvent += () => activeCharacters.Remove(character);
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
			Move();
		}

		private void Die()
		{
			Debug.Log("GAME OVER");
		}

		private void Move()
		{
			Transform myTransform = transform;
			Vector3 nextPosition = myTransform.position + (myTransform.right * (moveSpeed * Time.deltaTime));

			if (HelperExtras.IsInsideCameraViewport(nextPosition))
			{
				transform.position = nextPosition;
			}
		}

		private void AdjustUnitsPosition()
		{
			Vector3 mouseWorldPosition = Helper.GetMouseWorldPosition();
			float angle = Helper.GetAngleFromVector(transform.position.DirectionTo(mouseWorldPosition));
			angle = Mathf.MoveTowardsAngle(transform.eulerAngles.z, angle, rotationSpeed * Time.deltaTime);
			transform.eulerAngles = new Vector3(0, 0, angle);
		}
	}
}