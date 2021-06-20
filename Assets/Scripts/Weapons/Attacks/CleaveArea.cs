using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using PNLib.Utility;
using GMTK.Enemies;

namespace GMTK.Weapons
{
	public class CleaveArea : MonoBehaviour
	{
		[SerializeField] private Vector3 center;
		[SerializeField] private float radius;
		[SerializeField] private GameObject pivot;

		void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.green;
			Gizmos.DrawWireSphere(pivot.transform.position + center, radius);
		}

		void Start()
		{
		}

		public List<Enemy> GetInArea(Vector3 direction)
		{
			transform.right = direction;

			Helper.GetAllObjectsInCircleRadius(pivot.transform.position + center, radius, out List<Enemy> hits);
			Destroy(gameObject, 0.2f);

			return hits;
		}
	}
}
