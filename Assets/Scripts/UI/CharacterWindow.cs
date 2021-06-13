using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace GMTK
{
	public class CharacterWindow : MonoBehaviour
	{
		[SerializeField]
		private CharacterInfoDisplay characterInfoDisplayPrefab;

		[SerializeField]
		private CharacterInfo[] charactersInfo;

		[SerializeField]
		private CharacterInfoDisplay[] partyDisplay;

		[SerializeField]
		private Transform characterOptionsContainer;

		private List<CharacterInfoDisplay> displayedCharacters;

		private void Start()
		{
			CreateAllCharacters(charactersInfo);
		}

		private void CreateAllCharacters(CharacterInfo[] characters)
		{
			foreach (Transform child in characterOptionsContainer)
			{
				Destroy(child.gameObject);
			}

			foreach (CharacterInfo info in characters)
			{
				CharacterInfoDisplay display = Instantiate(characterInfoDisplayPrefab, characterOptionsContainer);
				displayedCharacters.Add(display);
				display.Display(info);
				display.GetComponent<Button>().onClick.AddListener(() => TryAddToParty(display.Info));
			}
		}

		private void TryAddToParty(CharacterInfo info)
		{
			foreach (var partySlot in partyDisplay)
			{
				if (partySlot.Info)
					continue;
				
				partySlot.Display(info);
				displayedCharacters.First(x => x.Info == info).GetComponent<Button>().interactable = false;
				break;
			}
		}
	}
}