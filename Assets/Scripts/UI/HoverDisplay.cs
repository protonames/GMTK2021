using GMTK.Utilities;
using TMPro;
using UnityEngine;

namespace GMTK
{
	public class HoverDisplay : MonoBehaviour
	{
		[SerializeField]
		private GameObject hoverPanel;

		[SerializeField]
		private TMP_Text characterName;
		
		[SerializeField]
		private TMP_Text description;

		[SerializeField]
		private AudioSource audioSource;

		public void DisplayCharacter(CharacterInfo info)
		{
			if (!info)
				return;
			
			hoverPanel.SetActive(true);
			audioSource.Play();
			
			if (characterName)
				characterName.SetText($"{info.name}");
			
			description.text = "";

			for (int i = 0; i < info.Sinergies.Length; i++)
			{
				if (i != 0)
					description.text += ", ";

				var synergy = info.Sinergies[i];
				description.text += $"<color=#{synergy.Color.ToHexString()}>{synergy.name}</color>";
			}
		}

		public void DisplaySynergy(SynergyInfo info)
		{
			hoverPanel.SetActive(true);
			audioSource.Play();
			
			if (characterName)
				characterName.SetText($"{info.name}");
			
			description.text = info.Description;
		}

		public void Hide()
		{
			hoverPanel.SetActive(false);
		}
	}
}