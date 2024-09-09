using System;
using Common;
using UnityEngine;

namespace Game.StageMap
{
    [Serializable]
    public class TallWallData
    {
        public Vector2Int Interval;
        public int MinDistanceFromHole;
    }
    
    [Serializable]
    public class ShortWallData
    {
        public float Probability;
        public int MinDistanceFromOtherObstacle;
    }
    
    [Serializable]
    public class CoverData
    {
        public float Probability;
        public int MinDistanceFromOtherObstacle;
    }
    
    [Serializable]
    public class HoleData
    {
        public Vector2Int Length;
        public Vector2Int Interval;
    }
    
    public class MapData : ScriptableSingleton<MapData>
    {
        public int FloorHeight = 3;
        public int DefaultHeight = 1;

        public LayerMask WallLayers;

        public bool IsWallLayer(Collider2D collider)
        {
            return ((1 << collider.gameObject.layer) & WallLayers) != 0;
        }
        
        [Header("높은 장애물")]
        public TallWallData TallWallData;

        [Header("낮은 장애물")]
        public ShortWallData ShortWallData;

        [Header("엄폐물")]
        public CoverData CoverData;

        [Header("낙하 틈새")]
        public HoleData HoleData;
    }
}
