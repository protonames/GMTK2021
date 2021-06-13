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

		public void Display(CharacterInfo info)
		{
			hoverPanel.SetActive(true);
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

		public void Hide()
		{
			hoverPanel.SetActive(false);
		}
	}
}