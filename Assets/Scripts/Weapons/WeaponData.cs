using UnityEngine;
using GMTK.Characters;
namespace GMTK.Weapons
{
	[CreateAssetMenu(fileName = "New Weapon", menuName = "Data/Weapon", order = 0)]
	public class WeaponData : ScriptableObject
	{
		public Projectile Projectile => Projectiles[Random.Range(0, Projectiles.Length)];

		[Header("[PROJECTILE]")]
		public int Amount;

		public int AngleSpread;

		[Header("[AREA OF EFFECT]")]
		public int AreaSize = 5;

		public AttackType AttackType;
		public int LinearSpread;
		public ProjectilePattern Pattern;
		public Projectile[] Projectiles;
		public int Speed;

		[SerializeField] private AttackController attackController;

		public AttackController ReturnController(Character character)
		{
			AttackController attack = Instantiate(attackController, character.transform);
			attack.SetUp(character, this);
			return attack;
		}
	}
}