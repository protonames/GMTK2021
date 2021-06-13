using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GMTK
{
    public class CharacterInfoDisplay : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text nameText;

        [SerializeField]
        private Image characterImage;
        
        [SerializeField]
        private Image weaponImage;

        public CharacterInfo Info { get; set; } 

        public void Display(CharacterInfo info)
        {
            Info = info; 
            
            if (nameText)
                nameText.SetText(info.name);

            if (characterImage)
                characterImage.sprite = info.BodySprite;

            if (weaponImage)
                weaponImage.sprite = info.WeaponSprite;
        }
    }
}