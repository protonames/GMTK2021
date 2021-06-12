using GMTK.Weapons;
using UnityEngine;

namespace GMTK.Characters
{
	[CreateAssetMenu(fileName = "New Character", menuName = "Data/Character", order = 0)]
	public class CharacterData : ScriptableObject
	{
		public float AttackRange = 2f;
		public float AttackSpeed = .3f;
		public int Health = 5;
		public int Damage = 5;
		public WeaponData Weapon;
		public Character CharacterPrefab;
	}
}