using System;
using System.Collections.Generic;
using System.Linq;
using PNLib.Utility;
using UnityEngine;

namespace PNTemplate
{
	[RequireComponent(typeof(Health))]
	public class Servant : MonoBehaviour
	{
		[SerializeField]
		private Transform firePoint;

		[SerializeField]
		private ServantData servantData;

		private Health health;

		private Enemy target;

		private void Awake()
		{
			health = GetComponent<Health>();
		}

		private void Start()
		{
			//TODO: Assign servant data dynamically 
			InvokeRepeating(nameof(Fire), servantData.FireRate, servantData.FireRate);
		}

		private void OnEnable()
		{
			health.OnDiedEvent += Die;
		}

		private void OnDisable()
		{
			health.OnDiedEvent -= Die;
		}

		private void Update()
		{
			if (!target && Helper.GetClosestObjectInCircleRadius(transform.position, servantData.SightRadius, out Enemy hit))
			{
				target = hit;
			}
			else
			{
				RotateTowardsTarget();
			}
		}

		
		private void RotateTowardsTarget()
		{
			//TODO: Lerp rotation here
			float angle = Helper.GetAngleFromVector(transform.position.DirectionTo(target.transform.position));
			transform.eulerAngles = new Vector3(0, 0, angle);
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

		private void Fire()
		{
			if (!target)
				return;
			
			Vector3 spawnAngle = firePoint.eulerAngles;
			Vector3 spawnPoint = firePoint.position;
			Projectile projectile = Instantiate(servantData.ProjectilePrefab, spawnPoint, Quaternion.Euler(spawnAngle));
			projectile.Launch(true, servantData.ProjectileSpeed, servantData.ProjectileDamage);
		}

		private void Die()
		{
			Destroy(gameObject);
		}
	}
}

