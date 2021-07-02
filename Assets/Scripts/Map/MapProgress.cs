using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using GMTK.Enemies;
using GMTK.Info;

namespace GMTK.Levels
{
	public class MapProgress : MonoBehaviour
	{
		public LevelHolder LevelHolder;
		public MapNodeData[] mapStructure;
		public static MapProgress Instance;
		public List<Enemy> EnemiesToSpawn;
		public List<ClassInfo> ClassesToShop;
		public NodeTypes CurrentRoomType;
		public int CurrentGold = 50;
		public PartyInfo partyInfo;

		// Start is called before the first frame update
		void Start()
		{
			if (Instance == null)
			{
				Instance = this;
				DontDestroyOnLoad(gameObject);
			}
			else
			{
				Destroy(gameObject);
			}
		}

		public void LoadMap()
		{
			MapGenerator generator = FindObjectOfType<MapGenerator>();
			if (generator == null) return;


			if (mapStructure == null)
			{
				mapStructure = generator.GenerateMap(this);
			}
			else
			{
				generator.GenerateMap(this, mapStructure);
			}
		}

		public void UpdateMovement(MapNode mapNode)
		{
			foreach (var node in mapStructure)
			{
				node.canEnter = false;
			}

			foreach (var node in mapNode.connectedNodes)
			{
				foreach (var node2 in mapStructure)
				{
					if (node.id == node2.id)
					{
						node2.canEnter = true;
						break;
					}
				}
			}
		}

		public void PrepareRoomData(MapNode node, NodeTypes type, int layer)
		{
			CurrentRoomType = type;
			UpdateMovement(node);

			LevelData room;
			switch (type)
			{
				case NodeTypes.Combat:
					if (layer < 3)
						room = LevelHolder.EasyLevels[Random.Range(0, LevelHolder.EasyLevels.Length)];
					else if (layer < 5)
						room = LevelHolder.MediumLevels[Random.Range(0, LevelHolder.MediumLevels.Length)];
					else
						room = LevelHolder.HardLevels[Random.Range(0, LevelHolder.HardLevels.Length)];

					EnemiesToSpawn = room.GetRoll();
					SceneManager.LoadScene("Game");
					break;
				case NodeTypes.Elite:
					room = LevelHolder.EliteLevels[Random.Range(0, LevelHolder.EliteLevels.Length)];
					EnemiesToSpawn = room.GetRoll();
					SceneManager.LoadScene("Game");
					break;
				case NodeTypes.Boss:
					room = LevelHolder.BossLevels[Random.Range(0, LevelHolder.BossLevels.Length)];
					EnemiesToSpawn = room.GetRoll();
					SceneManager.LoadScene("Game");
					break;
				case NodeTypes.Shop:
					var shop = LevelHolder.ShopLevels[Random.Range(0, LevelHolder.ShopLevels.Length)];
					ClassesToShop = shop.GetRoll(partyInfo.Party);
					SceneManager.LoadScene("Shop");
					break;
			}
		}

		public void ReturnToMap()
		{
			SceneManager.LoadScene("Map");
		}
	}

	public class MapNodeData
	{
		public int id;
		public Vector3 pos;
		public int[] connections;
		public NodeTypes type;
		public bool canEnter;
		public int layer;
	}
}
