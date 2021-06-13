using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Random = UnityEngine.Random;
using TMPro;
using UnityEngine.SceneManagement;

namespace GMTK
{
	public class MapNode : MonoBehaviour
	{
		[SerializeField] private List<MapNode> connectedNodes;
		[SerializeField] private RectTransform linePrefab;
		[SerializeField] private Button buttonPrefab;

		int maxConnections = 4;
		public bool isStarter = false;
		public bool isConnected = false;
		public NodeTypes type = NodeTypes.Combat;
		public Vector3 pos;
		public int id;

		// Update is called once per frame
		void Update()
		{

		}

		public void SetConnections(List<MapNode> upperNodes, GameObject parentAux)
		{
			int connections = maxConnections = Random.Range(1, Mathf.Min(maxConnections, upperNodes.Count));
			connectedNodes = new List<MapNode>();

			// Preguica mental lvl 60
			float thisX = transform.position.x;
			for (int i = 0; i < connections; i++)
			{
				MapNode menorNode = null;
				float dist = 99999999;
				foreach (MapNode node in upperNodes)
				{
					if (connectedNodes.IndexOf(node) != -1)
						continue;

					float curDist = Mathf.Abs(thisX - node.transform.position.x);
					if (curDist < dist)
						dist = curDist;
					menorNode = node;
				}

				connectedNodes.Add(menorNode);
			}


			foreach (MapNode mapNode in connectedNodes)
			{
				DrawConnection(mapNode, parentAux);
			}

		}

		public void SetConnections(MapNode upperNode, GameObject parentAux)
		{
			connectedNodes.Add(upperNode);
			DrawConnection(upperNode, parentAux);
		}

		void DrawConnection(MapNode mapNode, GameObject parentAux)
		{
			mapNode.isConnected = true;
			if (isStarter)
				isConnected = true;

			float lineWidth = 10f;
			Vector3 differenceVector = mapNode.pos - pos;

			var line = Instantiate(linePrefab, pos, Quaternion.identity, parentAux.transform);

			line.sizeDelta = new Vector2(differenceVector.magnitude, lineWidth);
			line.pivot = new Vector2(0, 0.5f);
			float angle = Mathf.Atan2(differenceVector.y, differenceVector.x) * Mathf.Rad2Deg;
			line.rotation = Quaternion.Euler(0, 0, angle);
			line.SetAsFirstSibling();
		}

		public void SetUp(bool rollForMe)
		{
			if (rollForMe && Random.Range(0, 3) == 0)
			{
				List<NodeTypes> prepareToRoll = new List<NodeTypes>();
				var aux = Enum.GetValues(typeof(NodeTypes));
				foreach (NodeTypes item in aux)
				{
					if (item == NodeTypes.Combat || item == NodeTypes.Boss)
						continue;
					prepareToRoll.Add(item);
				}
				type = prepareToRoll[Random.Range(0, prepareToRoll.Count)];
			}

			Button buttonAux = Instantiate(buttonPrefab, transform);
			buttonAux.onClick.AddListener(Click);
			var debugText = buttonAux.GetComponentInChildren<TMP_Text>();
			debugText.text = type.ToString();
		}

		public MapNodeData GetSaveData()
		{
			MapNodeData resp = new MapNodeData();
			resp.pos = pos;
			resp.id = id;
			resp.type = type;
			resp.isStarter = isStarter;

			resp.connections = new int[connectedNodes.Count];
			for (int i = 0; i < connectedNodes.Count; i++)
			{
				resp.connections[i] = connectedNodes[i].id;
			}

			return resp;
		}

		public void Click()
		{
			switch (type)
			{
				case NodeTypes.Combat:
					SceneManager.LoadScene("Main");
					break;
				case NodeTypes.Elite:
					print("TODO: Click on Elite.");
					break;
				case NodeTypes.Shop:
					SceneManager.LoadScene("CharacterSelector");
					break;
				case NodeTypes.Boss:
					print("TODO: Click on Boss.");
					break;
			}
		}
	}


	public enum NodeTypes
	{
		Combat,
		Elite,
		Shop,
		Boss
	}
}
