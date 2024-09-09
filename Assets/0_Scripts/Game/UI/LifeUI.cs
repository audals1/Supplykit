using System.Collections.Generic;
using Common;
using UnityEngine;

namespace Game.UI
{
    public class LifeUI : MonoBehaviour
    {
        [SerializeField] private GameObject _heartPrefab;
        [SerializeField] private Transform _heartParent;
        
        public List<GameObject> Hearts;

        public void UpdateHP(int hp)
        {
            _heartParent.DestroyAllChildren(1);
            for (int i = 0; i < hp; i++)
            {
                var heart = Instantiate(_heartPrefab, _heartParent);
                heart.SetActive(true);
            }
        }
    }
}
