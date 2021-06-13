using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using GMTK.Info;
using GMTK.Utilities;
using PNLib.Utility;
using UnityEngine;
using UnityEngine.UI;
using CharacterInfo = GMTK.Info.CharacterInfo;

namespace GMTK.UI
{
	public class CharacterWindow : MonoBehaviour
	{
		[SerializeField]
		private CharacterInfoDisplay characterInfoDisplayPrefab;

		[SerializeField]
		private Transform characterOptionsContainer;

		[SerializeField]
		private CharacterInfo[] charactersInfo;

		[SerializeField]
		private AudioClip clickSFX;

		[SerializeField]
		private AudioClip confirmClickSFX;

		[SerializeField]
		private CharacterInfo emptyCharacter;

		[SerializeField]
		private HoverDisplay hoverDisplay;

		[SerializeField]
		private CharacterInfoDisplay[] partyDisplay;

		[SerializeField]
		private PartyInfo partyInfo;

		[SerializeField]
		private Button playButton;

		[SerializeField]
		private List<SynergyDisplay> synergyDisplays;

		private List<CharacterInfoDisplay> displayedCharacters;

		private void Start()
		{
			foreach (CharacterInfoDisplay display in partyDisplay)
			{
				display.GetComponent<Button>().onClick.AddListener(() => TryRemoveFromParty(display));
			}

			DOTween.To(() => MusicManagerExtras.Volume, x => MusicManagerExtras.Volume = x, 0.0625f, 1f);
			CreateAllCharacters(charactersInfo);

			playButton.onClick.AddListener(
				() =>
				{
					StartCoroutine(LoadGameRoutine());
				}
			);
		}

		private IEnumerator LoadGameRoutine()
		{
			SoundManagerExtras.Play(confirmClickSFX);
			yield return new WaitForSeconds(confirmClickSFX.length);

			DOTween.To(() => MusicManagerExtras.Volume, x => MusicManagerExtras.Volume = x, 0.5f, 1f);
			partyInfo.Party.Clear();

			foreach (CharacterInfoDisplay item in partyDisplay)
			{
				partyInfo.Party.Add(item.Info);
			}

			GameSceneManager.LoadNext();
		}

		private void TryRemoveFromParty(CharacterInfoDisplay display)
		{
			if (!display.Info)
			{
				return;
			}

			foreach (SynergyInfo synergy in display.Info.Sinergies)
			{
				SynergyManager.Instance.ActiveSynergies.Remove(synergy.Type);
			}

			displayedCharacters.First(x => x.Info == display.Info).GetComponent<Button>().interactable = true;
			display.Display(emptyCharacter);
			SoundManagerExtras.Play(clickSFX);
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
			foreach (CharacterInfoDisplay partySlot in partyDisplay)
			{
				if (partySlot.Info)
				{
					continue;
				}

				partySlot.Display(info);
				displayedCharacters.First(x => x.Info == info).GetComponent<Button>().interactable = false;

				foreach (SynergyInfo synergy in info.Sinergies)
				{
					SynergyManager.Instance.ActiveSynergies.Add(synergy.Type);
				}

				SoundManagerExtras.Play(clickSFX);
				UpdateSynergyDisplays();
				break;
			}
		}

		private void UpdateSynergyDisplays()
		{
			foreach (SynergyDisplay display in synergyDisplays)
			{
				display.Refresh();
			}
		}
	}
}