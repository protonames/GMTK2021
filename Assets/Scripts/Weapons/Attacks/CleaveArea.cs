using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using PNLib.Utility;
using GMTK.Enemies;
using GMTK.Characters;

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

		public List<Character> GetInArea(Vector3 direction)
		{
			transform.right = direction;

			Helper.GetAllObjectsInCircleRadius(pivot.transform.position + center, radius, out List<Character> hits);
			Destroy(gameObject, 0.2f);

			return hits;
		}
	}
}
