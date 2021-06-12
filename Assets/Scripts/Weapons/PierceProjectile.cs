using GMTK.Weapons;
using UnityEngine;

namespace GMTK
{
    public class PierceProjectile : Projectile
    {
        [SerializeField]
        private int pierceCount = 3;
        protected override void Die()
        {
            pierceCount--;

            if (pierceCount <= 0)
            {
                base.Die();
            }
        }
    }
}