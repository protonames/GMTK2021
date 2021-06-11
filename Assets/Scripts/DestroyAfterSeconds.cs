using UnityEngine;

namespace PNTemplate
{
	public class DestroyAfterSeconds : MonoBehaviour
	{
		[SerializeField]
		private float seconds = 1f;

		public float Seconds
		{
			get => seconds;
			set => seconds = value;
		}

		private void Start()
		{
			Destroy(gameObject, Seconds);
		}
	}
}