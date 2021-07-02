using System.Collections;
using System.Collections.Generic;
using GMTK.Characters;
using GMTK.Levels;
using UnityEngine;
using CharacterInfo = GMTK.Info.ClassInfo;

namespace GMTK.Enemies
{
	public class EnemySpawner : MonoBehaviour
	{
		[SerializeField]
		private GameObject enemyPrefab;

		[SerializeField]
		private float spawnCadence = 5f;

		[SerializeField]
		private GameObject spawnEffect;

		[SerializeField]
		private float spawnEffectDelay = 2f;

		[SerializeField]
		private GameObject[] spawnPoints;

		private float spawnCadenceAux;

		private List<Enemy> spawnedEnemies = new List<Enemy>();
		private List<Enemy> toSpawnEnemies;
		private int completeCount;


		private void Start()
		{
			spawnCadenceAux = Time.time + spawnCadence;
			toSpawnEnemies = MapProgress.Instance.EnemiesToSpawn;
			completeCount = toSpawnEnemies.Count;
		}

		private void Update()
		{
			if (spawnCadenceAux > Time.time)
			{
				return;
			}

			spawnCadenceAux = Time.time + spawnCadence;

			if (toSpawnEnemies.Count == 0)
			{
				CheckEnd();
			}
			else
			{
				StartCoroutine(SpawnWaitRoutine());
			}
		}

		private IEnumerator SpawnWaitRoutine()
		{
			if (toSpawnEnemies.Count == 0)
				yield break;

			var enemyToSpawn = toSpawnEnemies[0];
			toSpawnEnemies.RemoveAt(0);

			GameObject spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
			Vector3 spawnPosition = spawnPoint.transform.position;
			GameObject effect = Instantiate(spawnEffect, spawnPosition, Quaternion.identity, null);
			yield return new WaitForSeconds(spawnEffectDelay);

			Destroy(effect.gameObject);
			Enemy enemy = Instantiate(enemyToSpawn, spawnPosition, Quaternion.identity, null);
			spawnedEnemies.Add(enemy);
		}

		private void CheckEnd()
		{
			if (completeCount != spawnedEnemies.Count) return;

			foreach (var enemy in spawnedEnemies)
			{
				if (enemy != null)
				{
					return;
				}
			}

			MapProgress.Instance.ReturnToMap();
		}
	}
}