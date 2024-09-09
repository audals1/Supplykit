using System.Collections.Generic;
using Game.StageMap;
using UnityEngine;

namespace Actor
{
    public class GroundChecker : MonoBehaviour
    {
        [SerializeField] private Collider2D _collider;

        private List<Collider2D> _collidersCache = new();
        private Character _character;
        private int _groundCount;

        public void Initialize(Character character)
        {
            _character = character;
        }

        public void ForceGroundCheck()
        {
            _collider.OverlapCollider(default, _collidersCache);
            bool prevWasGrounded = _groundCount != 0;
            _groundCount = 0;
            foreach (var targetCollider in _collidersCache)
            {
                if (MapData.Instance.IsWallLayer(targetCollider))
                {
                    _groundCount++;
                }
            }

            bool isGround = _groundCount != 0;
            if (prevWasGrounded != isGround)
            {
                if (isGround) _character.OnEnterGround();
                else _character.OnExitGround();
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (MapData.Instance.IsWallLayer(other))
            {
                if (_groundCount++ == 0)
                {
                    _character.OnEnterGround();
                }
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (MapData.Instance.IsWallLayer(other))
            {
                if (--_groundCount == 0)
                {
                    _character.OnExitGround();
                }
            }
        }
    }
}
