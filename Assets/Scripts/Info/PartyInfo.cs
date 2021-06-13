using System.Collections.Generic;
using UnityEngine;

namespace GMTK.Info
{
	[CreateAssetMenu(fileName = "New Party Info", menuName = "Info/Party", order = 0)]
	public class PartyInfo : ScriptableObject
	{
		public List<CharacterInfo> Party = new List<CharacterInfo>();
	}
}