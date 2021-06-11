using UnityEngine;

namespace PNTemplate
{
	public class Debug : MonoBehaviour
	{
		[SerializeField]
		private Servant servantPrefab;

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.K))
			{
				FindObjectOfType<King>().AddServant(Instantiate(servantPrefab));
			}
		}
	}
}