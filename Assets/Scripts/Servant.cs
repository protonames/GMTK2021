using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PNTemplate
{
	[RequireComponent(typeof(Health))]
	public class Servant : MonoBehaviour
	{
		private Health health;

		private void Awake()
		{
			health = GetComponent<Health>();
		}

		private void OnEnable()
		{
			health.OnDiedEvent += Die;
		}

		private void OnDisable()
		{
			health.OnDiedEvent -= Die;
		}

		public void Connect(King king, List<Servant> servants)
		{
			gameObject.AddComponent<DistanceJoint2D>().connectedBody = king.GetComponent<Rigidbody2D>();

			foreach (Servant servant in servants.Where(x => x != this))
			{
				gameObject.AddComponent<DistanceJoint2D>().connectedBody = servant.GetComponent<Rigidbody2D>();
			}
		}

		public void DisconnectAll()
		{
			//TODO: Replace for SpringJoints
			DistanceJoint2D[] joints = GetComponents<DistanceJoint2D>();

			foreach (DistanceJoint2D joint in joints)
			{
				Destroy(joint);
			}
		}

		private void Die() { }
	}
}