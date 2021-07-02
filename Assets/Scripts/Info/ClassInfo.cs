using UnityEngine;
using GMTK.Weapons;
using GMTK.Characters;

namespace GMTK.Info
{
	[CreateAssetMenu(fileName = "New Character", menuName = "Info/Character")]
	public class ClassInfo : ScriptableObject
	{
		public int GoldCost = 3;
		public float AttackRadius = 1;
		public float AttackSpeed = 1;
		public Sprite BodySprite;
		public int Damage = 1;
		public string Description;

		[Header("[STATS]")]
		public int Health = 10;

		public Color MainColor;
		public SynergyInfo[] Sinergies;
		public Sprite WeaponSprite;

		[Header("[WEAPON]")]
		public Projectile projectile;
		public SpecialEffect specialEffect;

		public WeaponData weaponData;

		public void LaunchProjectile(Transform originPoint, Health target)
		{
			// Projectile projectileInstance = Instantiate(projectile, originPoint.position, Quaternion.identity, null);
			// projectileInstance.Launch(target, true, 5, Damage, AttackRadius);
		}
	}
}