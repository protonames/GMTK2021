﻿using System;
using UnityEngine;

namespace PNTemplate
{
	public class Health : MonoBehaviour
	{
		[SerializeField]
		private int max = 3;

		public event Action OnChangedEvent;
		public event Action OnDiedEvent;
		private int current;

		private void Awake()
		{
			current = max;
		}

		public void TakeDamage(int amount)
		{
			current -= amount;

			if (current <= 0)
			{
				current = 0;
				OnDiedEvent?.Invoke();
			}
			else
			{
				OnChangedEvent?.Invoke();
			}
		}

		public void AddHealth(int amount)
		{
			current += amount;

			if (current > max)
			{
				current = max;
			}

			OnChangedEvent?.Invoke();
		}
	}
}