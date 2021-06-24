using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using GMTK.Enemies;

namespace GMTK.Levels
{
	public class MapProgress : MonoBehaviour
	{
		public LevelHolder LevelHolder;

		public MapNodeData[] mapStructure;

		bool initialized;

		public static MapProgress Instance;

		public List<Enemy> EnemiesToSpawn;

		public int CurrentGold = 50;

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

		// Update is called once per frame
		void Update()
		{
			if (Input.GetKeyDown(KeyCode.B)) ReturnToMap();
			// if (Input.GetKeyDown(KeyCode.L)) LoadMap();

			// if (initialized) return;
			// LoadMap();
			// initialized = true;
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

		public void PrepareCombatData(NodeTypes types, int layer = 1)
		{
			LevelData room;
			switch (types)
			{
				case NodeTypes.Combat:
					print("Layer: " + layer);
					if (layer < 3)
						room = LevelHolder.EasyLevels[Random.Range(0, LevelHolder.EasyLevels.Length)];
					else if (layer < 5)
						room = LevelHolder.MediumLevels[Random.Range(0, LevelHolder.MediumLevels.Length)];
					else
						room = LevelHolder.HardLevels[Random.Range(0, LevelHolder.HardLevels.Length)];

					EnemiesToSpawn = room.GetRoll();
					break;
				case NodeTypes.Elite:
					room = LevelHolder.EliteLevels[Random.Range(0, LevelHolder.EliteLevels.Length)];
					EnemiesToSpawn = room.GetRoll();
					break;
				case NodeTypes.Boss:
					room = LevelHolder.BossLevels[Random.Range(0, LevelHolder.BossLevels.Length)];
					EnemiesToSpawn = room.GetRoll();
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
