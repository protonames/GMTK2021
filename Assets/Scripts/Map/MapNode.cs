using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Random = UnityEngine.Random;
using TMPro;
using UnityEngine.SceneManagement;

namespace GMTK.Levels
{
	public class MapNode : MonoBehaviour
	{
		public List<MapNode> connectedNodes;
		[SerializeField] private RectTransform linePrefab;
		[SerializeField] private Button buttonPrefab;

		int maxConnections = 4;
		public bool canEnter = false;
		public bool isConnected = false;
		public NodeTypes type = NodeTypes.Combat;
		public Vector3 pos;
		public int id;
		public int layer;

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
			if (canEnter)
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

		public void SetUp(bool rollForMe = false)
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

		public void SetUp(NodeTypes forcedType)
		{
			type = forcedType;
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
			resp.canEnter = canEnter;
			resp.layer = layer;

			resp.connections = new int[connectedNodes.Count];
			for (int i = 0; i < connectedNodes.Count; i++)
			{
				resp.connections[i] = connectedNodes[i].id;
			}

			return resp;
		}

		public void Click()
		{
			if (!canEnter) return;
			MapProgress.Instance.UpdateMovement(this);

			switch (type)
			{
				case NodeTypes.Combat:
					MapProgress.Instance.PrepareCombatData(NodeTypes.Combat, layer);
					SceneManager.LoadScene("Game");
					break;
				case NodeTypes.Elite:
					MapProgress.Instance.PrepareCombatData(NodeTypes.Elite);
					SceneManager.LoadScene("Game");
					break;
				case NodeTypes.Shop:
					SceneManager.LoadScene("Shop");
					break;
				case NodeTypes.Boss:
					MapProgress.Instance.PrepareCombatData(NodeTypes.Boss);
					SceneManager.LoadScene("Game");
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
