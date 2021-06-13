using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace GMTK
{
	public class MapGenerator : MonoBehaviour
	{
		[SerializeField] private MapNode nodePrefab;

		int camadas = 5;
		int minNodesPorCamada = 2;
		int maxNodesPorCamada = 6;

		float yRange = 200f;
		float xSpacing = 100f;

		// Start is called before the first frame update
		void Start()
		{
			// GenerateMap();
		}

		// Update is called once per frame
		void Update()
		{

		}

		public MapNodeData[] GenerateMap()
		{
			float midX = 600;
			float ceilingY = 600;

			float incrementX = 200;
			float incrementY = 200;

			int id = 0;

			Vector3 pos = new Vector3(midX, ceilingY, 0);
			MapNode bossNodeAux = Instantiate(nodePrefab, pos, Quaternion.identity, transform);
			bossNodeAux.type = NodeTypes.Boss;
			bossNodeAux.pos = pos;
			bossNodeAux.id = id;
			bossNodeAux.SetUp(false);
			id++;

			List<List<MapNode>> matrix = new List<List<MapNode>>();
			int totalLevel = 7;
			for (int level = 1; level < totalLevel; level++)
			{
				int totalCamada = Random.Range(minNodesPorCamada, maxNodesPorCamada);

				if (level == totalLevel - 1)
					totalCamada++;

				float maxDist = (totalCamada - 1) * incrementX;
				float midCamada = maxDist / 2;
				float offset = midX - midCamada;
				List<MapNode> nodesCamadas = new List<MapNode>();
				for (int camada = 0; camada < totalCamada; camada++)
				{
					var posX = incrementX * camada + offset + Random.Range(-50, 50);
					var posY = ceilingY - level * incrementY + Random.Range(-50, 0);
					MapNode nodeAux = CreateNode(posX, posY);
					if (level == totalLevel - 1)
						nodeAux.isStarter = true;
					nodeAux.id = id;
					id++;
					nodesCamadas.Add(nodeAux);
				}
				matrix.Add(nodesCamadas);
			}

			for (int i = 0; i < matrix[0].Count; i++)
			{
				matrix[0][i].SetConnections(bossNodeAux, gameObject);
			}

			int[] prob = new int[] { 0, 0, 0, 0, 1 };
			for (int i = 1; i < matrix.Count; i++)
			{
				var currentLayer = matrix[i];
				var previousLayer = matrix[i - 1];

				for (int j = 0; j < currentLayer.Count; j++)
				{
					List<MapNode> orderedList = OrderNodes(currentLayer[j], previousLayer);
					int randomPick = prob[Random.Range(0, prob.Length)];
					randomPick = Mathf.Min(randomPick, orderedList.Count - 1);

					currentLayer[j].SetConnections(orderedList[randomPick], gameObject);
				}

				for (int j = 0; j < previousLayer.Count; j++)
				{
					if (!previousLayer[j].isConnected)
					{
						List<MapNode> orderedList = OrderNodes(previousLayer[j], currentLayer);
						int randomPick = prob[Random.Range(0, prob.Length)];
						randomPick = Mathf.Min(randomPick, orderedList.Count - 1);

						orderedList[randomPick].SetConnections(previousLayer[j], gameObject);
					}
				}
			}

			prob = new int[] { 0, 0, 0, 1 };
			for (int i = 0; i < matrix.Count - 1; i++)
			{
				var currentLayer = matrix[i];
				for (int j = 0; j < currentLayer.Count; j++)
				{
					currentLayer[j].SetUp(true);
				}
			}

			for (int j = 0; j < matrix[matrix.Count - 1].Count; j++)
			{
				matrix[matrix.Count - 1][j].SetUp(false);
			}

			MapNodeData[] saverAux = new MapNodeData[id];
			saverAux[0] = bossNodeAux.GetSaveData();
			int indexAux = 1;

			for (int i = 0; i < matrix.Count; i++)
			{
				var currentLayer = matrix[i];
				for (int j = 0; j < currentLayer.Count; j++)
				{
					saverAux[indexAux] = currentLayer[j].GetSaveData();
					indexAux++;
				}
			}

			return saverAux;
		}

		public void GenerateMap(MapNodeData[] savedStructure)
		{
			MapNode[] nodes = new MapNode[savedStructure.Length];
			for (int i = 0; i < savedStructure.Length; i++)
			{
				Vector3 position = savedStructure[i].pos;
				MapNode nodeAux = CreateNode(position.x, position.y);
				nodeAux.type = savedStructure[i].type;
				nodeAux.isStarter = savedStructure[i].isStarter;
				nodes[i] = nodeAux;
			}

			for (int i = 0; i < savedStructure.Length; i++)
			{
				int[] connections = savedStructure[i].connections;
				for (int j = 0; j < connections.Length; j++)
				{
					nodes[i].SetConnections(nodes[connections[j]], gameObject);
				}
			}

			foreach (var node in nodes)
			{
				node.SetUp(false);
			}
		}

		MapNode CreateNode(float posX, float posY)
		{
			Vector3 pos = new Vector3(posX, posY, 0);
			MapNode nodeAux = Instantiate<MapNode>(nodePrefab, pos, Quaternion.identity, transform);
			nodeAux.pos = pos;
			return nodeAux;
		}

		List<MapNode> OrderNodes(MapNode mainNode, List<MapNode> candidates)
		{
			List<MapNode> ordered = new List<MapNode>();

			for (int i = 0; i < candidates.Count; i++)
			{
				int index = 0;
				float distCur = Vector3.Distance(mainNode.pos, candidates[i].pos);
				for (; index < ordered.Count; index++)
				{
					if (Vector3.Distance(mainNode.pos, ordered[index].pos) > distCur)
						break;
				}
				ordered.Insert(index, candidates[i]);
			}

			return ordered;
		}
	}
}
