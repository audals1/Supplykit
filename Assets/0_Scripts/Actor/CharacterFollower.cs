using Game.StageMap;
using UnityEngine;

namespace Actor
{
    public class CharacterFollower : MonoBehaviour
    {
        [SerializeField] private Transform _character;
        [SerializeField] private float _heightOffsetFactor;

        private float _heightThreshold;
        private float _initialHeight;

        private void Awake()
        {
            _heightThreshold = MapData.Instance.DefaultHeight + MapData.Instance.FloorHeight + 2;
            _initialHeight = transform.position.y;
        }

        private void Update()
        {
            var prevPosition = transform.position;
            var position = _character.position;
            position.x = Mathf.Max(prevPosition.x, position.x);

            if (position.y > _heightThreshold)
            {
                position.y = _initialHeight + (position.y - _heightThreshold) * _heightOffsetFactor;
            }
            else
            {
                position.y = _initialHeight;
            }

            transform.position = position;
        }
    }
}
