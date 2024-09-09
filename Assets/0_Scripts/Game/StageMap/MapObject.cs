using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Game.StageMap
{
    public enum MapObjectType
    {
        None,
        Cover,
        ShortWall,
        TallWall,
        Hole,
    }
    
    [Serializable]
    public class TilePair
    {
        public int Y;
        public TileBase Tile;
    }
    
    public class MapObject : MonoBehaviour
    {
        [SerializeField] private MapObjectType _type;
        [SerializeField] private List<TilePair> _tiles;

        public MapObjectType Type => _type;
        public List<TilePair> Tiles => _tiles;
    }
}
