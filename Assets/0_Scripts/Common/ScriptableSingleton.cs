using UnityEngine;

namespace Common
{
    public class ScriptableSingleton<T> : ScriptableObject where T : ScriptableObject
    {
        private static T _instance;
        
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    Resources.Load<T>(typeof(T).Name);
                    var instances = Resources.FindObjectsOfTypeAll<T>();
                    if (instances.Length > 0)
                    {
                        _instance = instances[0];
                    }
                }

                return _instance;
            }
        }

#if UNITY_EDITOR
        public static T CreateIfNotExist()
        {
            if (Instance == null)
            {
                _instance = CreateInstance<T>();

                if (!UnityEditor.AssetDatabase.IsValidFolder("Assets/Resources"))
                {
                    UnityEditor.AssetDatabase.CreateFolder("Assets", "Resources");
                }

                UnityEditor.AssetDatabase.CreateAsset(_instance, $"Assets/Resources/{typeof(T).Name}.asset");
                UnityEditor.AssetDatabase.SaveAssets();
            }

            return _instance;
        }
#endif
    }
}
