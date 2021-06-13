using UnityEngine;

namespace GMTK
{
	public class CharacterWindow : MonoBehaviour
	{
		[SerializeField]
		private CharacterInfoDisplay characterInfoDisplayPrefab;

		[SerializeField]
		private CharacterInfo[] charactersInfo;

		private void Start()
		{
			Display(charactersInfo);
		}

		private void Display(CharacterInfo[] characterInfos)
		{
			foreach (Transform child in transform)
			{
				Destroy(child.gameObject);
			}

			foreach (CharacterInfo info in characterInfos)
			{
				CharacterInfoDisplay display = Instantiate(characterInfoDisplayPrefab, transform);
				display.Display(info);
			}
		}
	}
}