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
		[SerializeField]
		private CharacterInfo emptyCharacter;

		private void Start()
		{
			foreach (CharacterInfoDisplay display in partyDisplay)
			{
				display.GetComponent<Button>().onClick.AddListener(() => TryRemoveFromParty(display));
			}
			
			CreateAllCharacters(charactersInfo);
		}

		private void TryRemoveFromParty(CharacterInfoDisplay display)
		{
			if (!display.Info)
				return;
			
			displayedCharacters.First(x => x.Info == display.Info).GetComponent<Button>().interactable = true;
			display.Display(emptyCharacter);
			display.Info = null;
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