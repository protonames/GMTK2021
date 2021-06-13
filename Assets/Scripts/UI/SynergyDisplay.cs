using PNLib.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GMTK
{
	public class SynergyDisplay : MonoBehaviour
	{
		[SerializeField]
		public SynergyInfo synergy;

		[SerializeField]
		private TMP_Text synergyName;
		
		[SerializeField]
		private TMP_Text synergyCount;

		[SerializeField]
		private Image backgroundImage;
		
		[SerializeField]
		HoverDisplay hover;

		void Start()
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