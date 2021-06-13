using UnityEngine;

namespace GMTK.Info
{
	[CreateAssetMenu(fileName = "New Synergy", menuName = "Info/Synergy")]
	public class SynergyInfo : ScriptableObject
	{
		public Color Color;
		public string Description;
		public SynergyType Type;
	}
}