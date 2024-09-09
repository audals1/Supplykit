using Common;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Game;
using UnityEngine;

namespace Actor
{
    public abstract class SupplyKit : MonoBehaviour
    {
        [SerializeField] private float _tween = 0.75f;
        [SerializeField] private Rigidbody2D _rb2d;
        
        private bool _isActive = true;
        private bool _isWaitingForDrop;

        public void Initialize(bool isDrop)
        {
            if (isDrop)
            {
                _rb2d.bodyType = RigidbodyType2D.Static;
                _isWaitingForDrop = true;
            }
        }
        
        protected abstract void OnInteractInternal(Character character);

        public async void Gain(Character character)
        {
            if (!_isActive) return;

            _isActive = false;
            OnInteractInternal(character);
            GetComponentInChildren<Collider2D>().enabled = false;
            _rb2d.bodyType = RigidbodyType2D.Static;

            SoundManager.PlaySfx(ClipType.GetItem);
            await transform.DOScale(Vector3.zero, _tween).SetEase(Ease.InBack);
            Destroy(gameObject);
        }

        private void Update()
        {
            if (!_isWaitingForDrop)
            {
                return;
            }
            
            var position = transform.position;
            position.y = 0;
            if (CameraController.Instance.IsVisible(position, Vector3.one))
            {
                _isWaitingForDrop = false;
                _rb2d.bodyType = RigidbodyType2D.Dynamic;
            }
        }
    }
}
