using UnityEditor;
using UnityEngine;

namespace Common.Editor
{
    public static class ScriptableEditorMenu
    {
        private const string SerializeOnBuildKey = "Editor_SerializeOnBuild";
        private const string SerializeOnBuildMenuName = "Scriptable/Serialize on Build";

        public static bool SerializeOnBuild
        {
            get => EditorPrefs.GetBool(SerializeOnBuildKey);
            private set => EditorPrefs.SetBool(SerializeOnBuildKey, value);
        }
        
        [MenuItem(SerializeOnBuildMenuName)]
        private static void ToggleAction()
        {
            SerializeOnBuild = !SerializeOnBuild;
        }
 
        [MenuItem(SerializeOnBuildMenuName, true)]
        private static bool ToggleActionValidate()
        {
            Menu.SetChecked(SerializeOnBuildMenuName, SerializeOnBuild);
            return true;
        }
        
        [MenuItem("Scriptable/Create All")]
        private static void CreateAllSingletonScriptables()
        {
            var baseType = typeof(ScriptableSingleton<>);
            var dataTypes = TypeCache.GetTypesDerivedFrom(baseType);

            object created = null;
            Resources.LoadAll(string.Empty);
            foreach (var singletonType in dataTypes)
            {
                var type = baseType.MakeGenericType(singletonType);
                var createMethod = type.GetMethod(nameof(ScriptableSingleton<ScriptableObject>.CreateIfNotExist));
                created = createMethod.Invoke(null, null);
            }

            if (created != null)
            {
                EditorUtility.FocusProjectWindow();
                Selection.activeObject = created as Object;
            }
        }
    }
}
