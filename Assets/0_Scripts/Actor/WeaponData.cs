using System.Collections.Generic;
using Common;

namespace Actor
{
    public class WeaponData : ScriptableSingleton<WeaponData>
    {
        public float AmmoRefillRate = 0.25f;
        public int HPRecover = 3;
        public List<Weapon> WeaponPrefabs;

        public Weapon GetWeaponPrefab(WeaponType type)
        {
            return WeaponPrefabs.Find(w => w.Type == type);
        }

        public int GetInitialAmmo(WeaponType type)
        {
            var prefab = GetWeaponPrefab(type);
            return prefab.InitialAmmoCount;
        }
    }
}
