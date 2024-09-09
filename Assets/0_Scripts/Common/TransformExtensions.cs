using UnityEngine;

namespace Common
{
    public static class TransformExtensions
    {
        public static void DestroyAllChildren(this Transform transform, int n = 0)
        {
            for (int i = transform.childCount - 1; i >= n; i--)
            {
                var child = transform.GetChild(i);
                Object.Destroy(child.gameObject);
            }
        }
        
        public static void DestroyAllChildrenImmediately(this Transform transform)
        {
            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                var child = transform.GetChild(i);
                Object.DestroyImmediate(child.gameObject);
            }
        }
    }
}
