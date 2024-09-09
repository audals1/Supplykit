using System.Collections.Generic;
using Common;
using Game;
using UnityEngine;

namespace Actor
{
    public partial class Character
    {
        private Weapon _currentWeapon;
        private Dictionary<WeaponType, int> _ammos = new();

        [SerializeField] private Transform _hand;

        public void GainWeapon(WeaponType type)
        {
            if (!_ammos.TryGetValue(type, out int ammo))
            {
                ammo = 0;
                _ammos.Add(type, 0);
            }

            ammo += WeaponData.Instance.GetInitialAmmo(type);
            _ammos[type] = ammo;
            EquipWeapon(type);

            UpdateAmmoCount();
        }

        public void GainAmmo()
        {
            if (_currentWeapon == null) return;

            _ammos[_currentWeapon.Type] += WeaponData.Instance.GetInitialAmmo(_currentWeapon.Type);
            UpdateAmmoCount();
        }

        public void UpdateAmmoCount()
        {
            _ammos.TryGetValue(WeaponType.Default, out int d);
            _ammos.TryGetValue(WeaponType.Laser, out int l);
            _ammos.TryGetValue(WeaponType.Beam, out int b);

            StageManager.Instance.UpdateBulletCount(d, l, b);
        }

        private void EquipWeapon(WeaponType type)
        {
            _hand.DestroyAllChildren();
            var prefab = WeaponData.Instance.GetWeaponPrefab(type);
            _currentWeapon = Instantiate(prefab, _hand);
            _currentWeapon.Initialize();
        }

        public bool TryAttack(bool autoFire, bool doNotTrigger)
        {
            if (_currentWeapon == null) return false;
            if (autoFire && !_currentWeapon.IsAutoFire) return false;
            if (_ammos[_currentWeapon.Type] <= 0) return false;
            if (!_currentWeapon.CanFire()) return false;
            if (!_isGrounded && _currentWeapon.GroundedOnly) return false;

            if (!doNotTrigger)
            {
                _characterView.SetState(_isCrouching ? PlayerState.CrouchAttack : PlayerState.Attack);
            }

            _characterView.Animator.Update(0);
            _currentWeapon.Fire();
            _ammos[_currentWeapon.Type]--;

            UpdateAmmoCount();
            CheckCurrentWeaponAmmo();
            return true;
        }

        private void CheckCurrentWeaponAmmo()
        {
            if (_currentWeapon == null)
            {
                return;
            }

            if (_ammos[_currentWeapon.Type] != 0)
            {
                return;
            }

            foreach (var pair in _ammos)
            {
                if (pair.Key == _currentWeapon.Type) continue;
                if (pair.Value > 0)
                {
                    EquipWeapon(pair.Key);
                }
            }
        }
    }
}
