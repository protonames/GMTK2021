using UnityEngine;

namespace GMTK
{
    [CreateAssetMenu(fileName = "New Character", menuName = "Info/Character")]
    public class CharacterInfo : ScriptableObject
    {
        public Sprite BodySprite;
        public Sprite WeaponSprite;
        public Color MainColor;
        public SynergyInfo[] Sinergies;
    }
}