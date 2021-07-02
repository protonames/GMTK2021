using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GMTK.Levels
{
	public class LevelDecoration : MonoBehaviour
	{
		[SerializeField] private GameObject[] combatDecoration;
		[SerializeField] private GameObject[] eliteDecoration;
		[SerializeField] private GameObject[] bossDecoration;

		// Start is called before the first frame update
		void Start()
		{
			switch (MapProgress.Instance.CurrentRoomType)
			{
				case NodeTypes.Combat:
					combatDecoration[Random.Range(0, combatDecoration.Length)].SetActive(true);
					break;
				case NodeTypes.Elite:
					eliteDecoration[Random.Range(0, eliteDecoration.Length)].SetActive(true);
					break;
				case NodeTypes.Boss:
					bossDecoration[Random.Range(0, bossDecoration.Length)].SetActive(true);
					break;
			}
		}
	}
}
