using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GMTK.Enemies;

namespace GMTK.Levels
{
	[CreateAssetMenu(fileName = "New Level", menuName = "Data/Level", order = 2)]
	public class LevelData : ScriptableObject
	{
		public Enemy[] ObligatorySpawns;
		public Enemy[] OpcionalSpawns;
		public int BuyPoints = 10;

		public List<Enemy> GetRoll()
		{
			List<Enemy> resp = new List<Enemy>();
			int remainingPoints = BuyPoints;

			foreach (var enemy in ObligatorySpawns)
			{
				resp.Add(enemy);
				remainingPoints -= enemy.EnemyCost;
			}

			while (remainingPoints > 0 && OpcionalSpawns.Length > 0)
			{
				Enemy enemy = OpcionalSpawns[Random.Range(0, OpcionalSpawns.Length)];
				resp.Add(enemy);
				remainingPoints -= enemy.EnemyCost;
			}

			int n = resp.Count;
			while (n > 1)
			{
				n--;
				int k = Random.Range(0, n + 1);
				Enemy value = resp[k];
				resp[k] = resp[n];
				resp[n] = value;
			}

			return resp;
		}
	}
}
