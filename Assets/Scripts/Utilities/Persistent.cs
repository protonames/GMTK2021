using UnityEngine;

namespace GMTK.Utilities
{
	public class Persistent : MonoBehaviour
	{
		private void Awake()
		{
			DontDestroyOnLoad(gameObject);
		}
	}
}