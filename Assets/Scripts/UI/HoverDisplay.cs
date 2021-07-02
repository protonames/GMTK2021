using GMTK.Info;
using GMTK.Utilities;
using TMPro;
using UnityEngine;
using ClassInfo = GMTK.Info.ClassInfo;

namespace GMTK.UI
{
	public class HoverDisplay : MonoBehaviour
	{
		[SerializeField]
		private AudioSource audioSource;

		[SerializeField]
		private TMP_Text characterName;

		[SerializeField]
		private TMP_Text description;

		[SerializeField]
		private GameObject hoverPanel;

		public void DisplayCharacter(ClassInfo info)
		{
			if (!info)
			{
				return;
			}

			hoverPanel.SetActive(true);
			audioSource.Play();

			if (characterName)
			{
				characterName.SetText($"{info.name}");
			}

			description.text = "";

			for (int i = 0; i < info.Sinergies.Length; i++)
			{
				if (i != 0)
				{
					description.text += ", ";
				}

				SynergyInfo synergy = info.Sinergies[i];
				description.text += $"<color=#{synergy.Color.ToHexString()}>{synergy.name}</color>";
			}
		}

		public void DisplaySynergy(SynergyInfo info)
		{
			hoverPanel.SetActive(true);
			audioSource.Play();

			if (characterName)
			{
				characterName.SetText($"{info.name}");
			}

			description.text = info.Description;
		}

		public void Hide()
		{
			hoverPanel.SetActive(false);
		}
	}
}