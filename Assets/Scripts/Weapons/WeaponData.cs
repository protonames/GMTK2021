using UnityEngine;

namespace GMTK.Weapons
{
	[CreateAssetMenu(fileName = "New Weapon", menuName = "Data/Weapon", order = 0)]
	public class WeaponData : ScriptableObject
	{
		public AttackType AttackType;
		
		[Header("[AREA OF EFFECT]")]
		public int AreaSize = 5;
		
		[Header("[PROJECTILE]")]
		public int Amount;
		public Projectile[] Projectiles;
		public Projectile Projectile => Projectiles[Random.Range(0, Projectiles.Length)];
		public int Speed;
		public ProjectilePattern Pattern;
		public int LinearSpread;
		public int AngleSpread;
	}
}