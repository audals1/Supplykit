using UnityEngine;

namespace Common
{
    public static class Vector2IntExtensions
    {
        public static int Min(this Vector2Int minMax) => minMax.x;
        public static int Max(this Vector2Int minMax) => minMax.y;

        public static int GetRandom(this Vector2Int minMax) => Random.Range(minMax.Min(), minMax.Max());
    }
}
