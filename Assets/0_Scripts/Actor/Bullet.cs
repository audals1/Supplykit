using System;
using Cysharp.Threading.Tasks;
using Game.StageMap;
using UnityEngine;

namespace Actor
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private GameObject _hitEffect;
        
        private float _speed;
        private int _damage;
        private bool _isFromAlly;
        
        public void Initialize(float speed, int damage, bool isFromAlly)
        {
            _speed = speed;
            _damage = damage;
            _isFromAlly = isFromAlly;
        }

        private void Update()
        {
            transform.position += transform.right * (_speed * Time.deltaTime);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_isFromAlly)
            {
                var monster = other.GetComponent<Monster>();
                if (monster != null)
                {
                    monster.HP -= _damage;
                    Instantiate(_hitEffect, transform.position, Quaternion.identity);
                    Destroy(gameObject);
                    return;
                }
            }
            
            if (!_isFromAlly)
            {
                var character = other.GetComponent<Character>();
                if (character != null)
                {
                    character.HP -= _damage;
                    Instantiate(_hitEffect, transform.position, Quaternion.identity);
                    Destroy(gameObject);
                    return;
                }
            }

            if (MapData.Instance.IsWallLayer(other))
            {
                Instantiate(_hitEffect, transform.position, Quaternion.identity);
                Destroy(gameObject);
                return;
            }
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            if (_isFromAlly)
            {
                var monster = col.gameObject.GetComponent<Monster>();
                if (monster != null)
                {
                    monster.HP -= _damage;
                    Instantiate(_hitEffect, transform.position, Quaternion.identity);
                    Destroy(gameObject);
                    return;
                }
            }
            
            if (!_isFromAlly)
            {
                var character = col.gameObject.GetComponent<Character>();
                if (character != null)
                {
                    character.HP -= _damage;
                    Instantiate(_hitEffect, transform.position, Quaternion.identity);
                    Destroy(gameObject);
                    return;
                }
            }
        }

        private async void OnBecameInvisible()
        {
            await UniTask.Delay(1000);
            if (this != null)
            {
                Destroy(gameObject);
            }
        }
    }
}
