using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using GMTK.Info;
using GMTK.Levels;
using GMTK.UI;
using GMTK.Utilities;
using PNLib.Utility;
using UnityEngine;
using UnityEngine.UI;
using CharacterInfo = GMTK.Info.CharacterInfo;

namespace GMTK.Shop
{
	public class ShopWindow : MonoBehaviour
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

		private List<CharacterInfoDisplay> displayedCharacters = new List<CharacterInfoDisplay>();

		[SerializeField]
		private TMPro.TMP_Text goldText;
		private string goldBaseText = "Gold:";


		private MapProgress mapProgress;

		private void Start()
		{
			mapProgress = MapProgress.Instance;


			foreach (CharacterInfoDisplay display in partyDisplay)
			{
				display.GetComponent<Button>().onClick.AddListener(() => TryRemoveFromParty(display));
			}

			DOTween.To(() => MusicManagerExtras.Volume, x => MusicManagerExtras.Volume = x, 0.0625f, 1f);

			goldText.text = goldBaseText + mapProgress.CurrentGold;
			InitiateParty();
			CreateAllCharacters(charactersInfo);

			playButton.onClick.AddListener(
				() =>
				{
					partyInfo.Party.Clear();

					foreach (CharacterInfoDisplay item in partyDisplay)
					{
						partyInfo.Party.Add(item.Info);
					}
					mapProgress.ReturnToMap();
					// StartCoroutine(LoadGameRoutine());
				}
			);
		}

		// private IEnumerator LoadGameRoutine()
		// {
		// 	SoundManagerExtras.Play(confirmClickSFX);
		// 	yield return new WaitForSeconds(confirmClickSFX.length);

		// 	DOTween.To(() => MusicManagerExtras.Volume, x => MusicManagerExtras.Volume = x, 0.5f, 1f);
		// 	partyInfo.Party.Clear();

		// 	foreach (CharacterInfoDisplay item in partyDisplay)
		// 	{
		// 		partyInfo.Party.Add(item.Info);
		// 	}

		// 	GameSceneManager.LoadNext();
		// }

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

			mapProgress.CurrentGold += display.Info.GoldCost;
			goldText.text = goldBaseText + mapProgress.CurrentGold;

			int charIndex = displayedCharacters.FindIndex(x => x.Info == display.Info);
			if (charIndex != -1)
			{
				displayedCharacters[charIndex].GetComponent<Button>().interactable = true;
			}

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
				Button buttonAux = display.GetComponent<Button>();
				buttonAux.onClick.AddListener(() => TryAddToParty(display.Info));

				if (partyInfo.Party.FindIndex(x => x == info) > -1)
				{
					buttonAux.interactable = false;
				}
			}
		}

		private void InitiateParty()
		{
			for (int i = 0; i < partyInfo.Party.Count; i++)
			{
				var info = partyInfo.Party[i];
				if (info == null)
					continue;

				partyDisplay[i].Display(info);
				foreach (SynergyInfo synergy in info.Sinergies)
				{
					SynergyManager.Instance.ActiveSynergies.Add(synergy.Type);
				}
			}
			UpdateSynergyDisplays();
		}

		private void TryAddToParty(CharacterInfo info)
		{
			if (mapProgress.CurrentGold < info.GoldCost)
				return;

			foreach (CharacterInfoDisplay partySlot in partyDisplay)
			{
				if (partySlot.Info)
				{
					continue;
				}

				mapProgress.CurrentGold -= info.GoldCost;
				goldText.text = goldBaseText + mapProgress.CurrentGold;

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