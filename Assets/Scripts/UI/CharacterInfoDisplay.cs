using TMPro;
using UnityEngine;
using UnityEngine.UI;
using CharacterInfo = GMTK.Info.CharacterInfo;

namespace GMTK.UI
{
	public class CharacterInfoDisplay : MonoBehaviour
	{
		[SerializeField]
		private Image characterImage;

		public CharacterInfo Info { get; set; }

		[SerializeField]
		private TMP_Text nameText;

		[SerializeField]
		private Image weaponImage;

		[SerializeField]
		public HoverDisplay hover;

		public void Display(CharacterInfo info)
		{
			Info = info;

			if (nameText)
			{
				nameText.SetText(info.name);
			}

			if (characterImage)
			{
				characterImage.sprite = info.BodySprite;
			}

			if (weaponImage)
			{
				weaponImage.sprite = info.WeaponSprite;
			}
		}

		public void PointerEnter()
		{
			hover.transform.position = transform.position;
			hover.DisplayCharacter(Info);
		}

		public void PointerExit()
		{
			hover.Hide();
		}
	}
}