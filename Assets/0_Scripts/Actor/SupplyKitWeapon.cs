using UnityEngine;

namespace Actor
{
    public class SupplyKitWeapon : SupplyKit
    {
        [SerializeField] private WeaponType _type;

        protected override void OnInteractInternal(Character character)
        {
            character.GainWeapon(_type);
        }
    }
}
