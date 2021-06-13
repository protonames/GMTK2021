using UnityEngine;

namespace GMTK.Info
{
	[CreateAssetMenu(fileName = "New Character", menuName = "Info/Character")]
	public class CharacterInfo : ScriptableObject
	{
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
	}
}