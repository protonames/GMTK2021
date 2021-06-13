using System.Collections;
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

		[SerializeField]
		private HoverDisplay hoverDisplay;

		[SerializeField]
		private List<SynergyDisplay> synergyDisplays;

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
			
			foreach (SynergyInfo synergy in display.Info.Sinergies)
			{
				SynergyManager.Instance.ActiveSynergies.Remove(synergy.Type);
			}
			
			displayedCharacters.First(x => x.Info == display.Info).GetComponent<Button>().interactable = true;
			display.Display(emptyCharacter);
			UpdateSynergyDisplays();
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
				display.hover = hoverDisplay;
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

				foreach (SynergyInfo synergy in info.Sinergies)
				{
					SynergyManager.Instance.ActiveSynergies.Add(synergy.Type);
				}

				UpdateSynergyDisplays();
				break;
			}
		}

		private void UpdateSynergyDisplays()
		{
			foreach (var display in synergyDisplays)
			{
				display.Refresh();
			}
		}
	}
}