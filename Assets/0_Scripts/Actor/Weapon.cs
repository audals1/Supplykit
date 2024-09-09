using Common;
using UnityEngine;

namespace Actor
{
    public enum WeaponType
    {
        Default,
        Laser,
        Beam,
    }
    
    public class Weapon : MonoBehaviour
    {
        [SerializeField] private WeaponType _type;
        [SerializeField] private bool _isAutoFire;
        [SerializeField] private float _delay;
        [SerializeField] private Bullet _bulletPrefab;
        [SerializeField] private int _damage;
        [SerializeField] private int _initialAmmoCount;
        
        [SerializeField] private float _bulletSpeed;
        [SerializeField] private bool _groundedOnly;
        
        [SerializeField] private Transform _firePoint;
        [SerializeField] private ClipType _sound;
        

        private float _nextShootableTime;

        public WeaponType Type => _type;
        public bool IsAutoFire => _isAutoFire;
        public bool GroundedOnly => _groundedOnly;
        public int InitialAmmoCount => _initialAmmoCount;

        public void Initialize()
        {
        }

        public bool CanFire()
        {
            return Time.time > _nextShootableTime;
        }

        public void Fire()
        {
            if (!CanFire())
            {
                return;
            }

            _nextShootableTime = Time.time + _delay;
            var bullet = Instantiate(_bulletPrefab, _firePoint.transform);
            bullet.transform.parent = null;
            bullet.Initialize(_bulletSpeed, _damage, true);
            SoundManager.PlaySfx(_sound);
        }
    }
}
