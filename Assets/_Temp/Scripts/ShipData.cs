using GMTK.Weapons;
using UnityEngine;

namespace PNTemplate._Temp
{
	[CreateAssetMenu(menuName = "Create Ship", fileName = "Ship", order = 0)]
	public class ShipData : ScriptableObject
	{
		public ShipProjectile ShipProjectilePrefab => Projectiles[Random.Range(0, Projectiles.Length)];
		public float AccelerationScale = .5f;
		public float AccelerationTime = .5f;
		public float BoostRate = .5f;
		public float BurstInterval;
		public float FireRate = .24f;
		public ProjectilePattern ProjectilePattern = ProjectilePattern.Straight;
		public GameObject Graphics;
		public float LinearShotSpread = .35f;
		public float MoveSpeed = 6f;
		public float MultiShotSpread = 15f;
		public Muzzle Muzzle;
		public bool ProjectileAccelerate;
		public int ProjectileAmount = 1;
		public ShipProjectile[] Projectiles;
		public float ProjectileSpeed = 12f;
	}
}