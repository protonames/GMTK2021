using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PNTemplate
{
	public class EnemySpawner : MonoBehaviour
	{
		[SerializeField] private GameObject enemyPrefab;
		[SerializeField] private GameObject spawnEffect;
		[SerializeField] private float spawnEffectDelay = 2f;
		[SerializeField] private GameObject[] spawnPoints;
		[SerializeField] private float spawnCadence = 5f;
		private float spawnCadenceAux;

		// Start is called before the first frame update
		void Start()
		{
			spawnCadenceAux = Time.time + spawnCadence;
		}

		// Update is called once per frame
		void Update()
		{
			if (spawnCadenceAux > Time.time) return;
			spawnCadenceAux = Time.time + spawnCadence;
			StartCoroutine(waiter());
		}

		IEnumerator waiter()
		{
			var spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

			var effect = Instantiate(spawnEffect, spawnPoint.transform.position, Quaternion.identity, null);
			yield return new WaitForSeconds(spawnEffectDelay);

			Destroy(effect.gameObject);
			Instantiate(enemyPrefab, spawnPoint.transform.position, Quaternion.identity, null);
		}
	}


	// public class Wave {
	// 	WaveGroup[] groups;
	// }

	// public class WaveGroup {
	// 	GameObject enemyType;
	// 	int enemyAmount;
	// }
}
