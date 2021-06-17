using System.Collections;
using DG.Tweening;
using GMTK.Utilities;
using UnityEngine;

namespace PNTemplate._Temp
{
	public class ShipProjectile : MonoBehaviour
	{
		[SerializeField]
		private Sprite squareSprite;

		private float speed;
		private SpriteRenderer spriteRenderer;

		private void Awake()
		{
			spriteRenderer = GetComponentInChildren<SpriteRenderer>();
		}

		private void Update()
		{
			Transform myTransform = transform;
			Vector3 nextPosition = myTransform.position + (myTransform.right * (speed * Time.deltaTime));

			if (!HelperExtras.IsInsideCameraViewport(myTransform.position))
			{
				Die();
				return;
			}

			myTransform.position = nextPosition;
		}

		public void AccelerateOverTime(float startSpeed, float endSpeed, float duration)
		{
			StartCoroutine(AccelerateEnumerator(startSpeed, endSpeed, duration));
		}

		public void SetSpeed(float fireSpeed)
		{
			speed = fireSpeed;
		}

		private void Die()
		{
			StopAllCoroutines();
			speed = 0;
			transform.rotation = Quaternion.identity;
			spriteRenderer.sprite = squareSprite;
			spriteRenderer.DOColor(Color.red, .2f).OnComplete(() => Destroy(gameObject));
		}

		private IEnumerator AccelerateEnumerator(float startSpeed, float endSpeed, float duration)
		{
			float t = 0f;

			while (t < 1)
			{
				t += Time.deltaTime / duration;
				speed = Mathf.Lerp(startSpeed, endSpeed, t);
				yield return null;
			}
		}
	}
}