using System.Collections.Generic;
using UnityEngine;

namespace GMTK.Info
{
	[CreateAssetMenu(fileName = "New Party Info", menuName = "Info/Party", order = 0)]
	public class PartyInfo : ScriptableObject
	{
		public List<ClassInfo> Party = new List<ClassInfo>();

		private void OnEnable()
		{
			hideFlags = HideFlags.DontUnloadUnusedAsset; // TODO isso é armengue, melhor criar um objeto que salva os dados todos entre as cenas, já vamos precisar para o mapa e o dinheiro msm
		}
	}
}