using Common;
using UnityEngine;

namespace Game
{
    public class Tutorial : MonoBehaviour
    {
        [SerializeField] private GameObject _content;
        [SerializeField] private Transform _visiblePoint;
        [SerializeField] private float _delay = 1f;
        
        private bool _isActive;

        private void Awake()
        {
            _content.SetActive(false);
        }

        private async void Update()
        {
            if (!_isActive)
            {
                if (CameraController.Instance.IsVisible(_visiblePoint.position, Vector3.one))
                {
                    _isActive = true;
                    //await UniTaskHelper.DelaySeconds(_delay);
                    _content.SetActive(true);
                }
            }
        }
    }
}
