using UnityEngine;

namespace Common
{
    public class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<T>();
                    if (_instance == null)
                    {
                        var prefab = Resources.Load<T>(typeof(T).Name);
                        if (prefab != null)
                        {
                            _instance = Instantiate(prefab);
                        }

                        if (_instance == null)
                        {
                            _instance = new GameObject($"Singleton{typeof(T)}").AddComponent<T>();
                        }
                    }
                }

                return _instance;
            }
        }

        protected virtual void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
        }

        private void OnDestroy()
        {
            if (Instance == null)
            {
                _instance = null;
            }
        }
    }
}
