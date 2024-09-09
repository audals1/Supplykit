using Febucci.UI;
using TMPro;
using UnityEngine;

namespace Game.UI
{
    public class BulletUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _bullet;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private TextAnimatorPlayer _textAnimator;

        private int _lastValue = -1;

        public void SetValue(int leftBulletCount)
        {
            if (leftBulletCount != _lastValue)
            {
                _bullet.text = leftBulletCount.ToString();
                _lastValue = leftBulletCount;
            }

            _canvasGroup.alpha = leftBulletCount > 0 ? 1f : 0.5f;
        }
    }
}
