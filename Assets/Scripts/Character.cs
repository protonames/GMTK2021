using System;
using UnityEngine;

namespace PNTemplate
{
	[RequireComponent(typeof(Health))]
	public class Character : MonoBehaviour
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

		private void Die()
		{
			throw new NotImplementedException();
		}
	}
}