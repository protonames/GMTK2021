using System.Collections;
using UnityEngine;

namespace PNTemplate
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

		private void Start()
		{
			spawnCadenceAux = Time.time + spawnCadence;
		}

		private void Update()
		{
			if (spawnCadenceAux > Time.time)
			{
				return;
			}

			spawnCadenceAux = Time.time + spawnCadence;
			StartCoroutine(SpawnWaitRoutine());
		}

		private IEnumerator SpawnWaitRoutine()
		{
			GameObject spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
			Vector3 spawnPosition = spawnPoint.transform.position;
			GameObject effect = Instantiate(spawnEffect, spawnPosition, Quaternion.identity, null);
			yield return new WaitForSeconds(spawnEffectDelay);

			Destroy(effect.gameObject);
			Instantiate(enemyPrefab, spawnPosition, Quaternion.identity, null);
		}
	}
}