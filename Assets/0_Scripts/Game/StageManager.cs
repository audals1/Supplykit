using Actor;
using Common;
using Game.UI;
using TMPro;
using UnityEngine;

namespace Game
{
    public class StageManager : SingletonMonoBehaviour<StageManager>
    {
        [SerializeField] private Character _character;
        [SerializeField] private bool _giveWeapon;
        [SerializeField] private WeaponType _defaultWeaponType = WeaponType.Default;
        [SerializeField] private int _refillAmmoCount;

        [SerializeField] private BulletUI _defaultBullet;
        [SerializeField] private BulletUI _laserBullet;
        [SerializeField] private BulletUI _beamBullet;
        [SerializeField] private LifeUI _lifeUI;
        
        private void Start()
        {
            if (_giveWeapon)
            {
                _character.GainWeapon(_defaultWeaponType);
                for (int i = 0; i < _refillAmmoCount; i++)
                {
                    _character.GainAmmo();
                }
            }
        }

        public void UpdateBulletCount(int defaultBullet, int laserBullet, int beamBullet)
        {
            _defaultBullet.SetValue(defaultBullet);
            _laserBullet.SetValue(laserBullet);
            _beamBullet.SetValue(beamBullet);
        }

        public void UpdateHP(int hp)
        {
            _lifeUI.UpdateHP(hp);
        }
    }
}
