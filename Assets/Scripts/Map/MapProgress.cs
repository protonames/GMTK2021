using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GMTK
{
	public class MapProgress : MonoBehaviour
	{
		MapNodeData[] mapStructure;

		bool initialized = false;

		// Start is called before the first frame update
		void Start()
		{
			DontDestroyOnLoad(gameObject);
		}

		// Update is called once per frame
		void Update()
		{
			if (Input.GetKeyDown(KeyCode.L)) LoadMap();

			if (initialized) return;

			LoadMap();
			initialized = true;
		}

		void LoadMap()
		{
			MapGenerator generator = FindObjectOfType<MapGenerator>();
			if (generator == null) return;


			if (mapStructure == null)
			{
				mapStructure = generator.GenerateMap();
			}
			else
			{
				generator.GenerateMap(mapStructure);
			}
		}
	}

	public class MapNodeData
	{
		public int id;
		public Vector3 pos;
		public int[] connections;
		public NodeTypes type;
		public bool isStarter;
	}
}
