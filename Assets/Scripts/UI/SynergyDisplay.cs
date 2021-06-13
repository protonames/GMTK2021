using GMTK.Info;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GMTK.UI
{
	public class SynergyDisplay : MonoBehaviour
	{
		[SerializeField]
		private Image backgroundImage;

		[SerializeField]
		private HoverDisplay hover;

		[SerializeField]
		private TMP_Text synergyCount;

		[SerializeField]
		private TMP_Text synergyName;

		[SerializeField]
		public SynergyInfo synergy;

		private void Start()
		{
			Display(synergy);
		}

		public void Display(SynergyInfo synergyInfo)
		{
			backgroundImage.color = synergyInfo.Color;
			synergyName.text = synergyInfo.name;
			synergyCount.text = "";

			for (int i = 0; i < 4; i++)
			{
				if (i < SynergyManager.Instance.GetCount(synergyInfo.Type))
				{
					synergyCount.text += "I";
				}
				else
				{
					synergyCount.text += "<color=black>I</color>";
				}
			}
		}

		public void Refresh()
		{
			Display(synergy);
		}

		public void PointEnter()
		{
			hover.transform.position = transform.position;
			hover.DisplaySynergy(synergy);
		}

		public void PointExit()
		{
			hover.Hide();
		}
	}
}