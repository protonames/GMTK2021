using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GMTK.Levels
{
	[CreateAssetMenu(fileName = "New Level Holder", menuName = "Data/Level Holder", order = 3)]
	public class LevelHolder : ScriptableObject
	{
		public LevelData[] EasyLevels;
		public LevelData[] MediumLevels;
		public LevelData[] HardLevels;
		public LevelData[] EliteLevels;
		public LevelData[] BossLevels;
		public LevelShopData[] ShopLevels;

		private void OnEnable()
		{
			hideFlags = HideFlags.DontUnloadUnusedAsset; // TODO isso é armengue, melhor criar um objeto que salva os dados todos entre as cenas, já vamos precisar para o mapa e o dinheiro msm
		}
	}
}
