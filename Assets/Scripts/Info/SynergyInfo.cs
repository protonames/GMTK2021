using UnityEngine;

namespace GMTK
{
	[CreateAssetMenu(fileName = "New Synergy", menuName = "Info/Synergy")]
	public class SynergyInfo : ScriptableObject
	{
		public SynergyType Type;
		public Color Color;
		public string Description;
	}
}