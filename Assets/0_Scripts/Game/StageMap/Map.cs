using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

namespace Game.StageMap
{
    [Serializable]
    public class Decoration
    {
        public TileBase TargetTile;
        public float Probability;
        public TileBase Tile;
    }

    public class Map : MonoBehaviour
    {
        [SerializeField] private Tilemap _baseMap;
        [SerializeField] private Tilemap _platformMap;
        [SerializeField] private Tilemap _obstacleMap;
        [SerializeField] private PlatformEffector2D _platformEffector;
        [SerializeField] private TilemapCollider2D _platformCollider;
        [SerializeField] private Transform _rightMost;
        [SerializeField] private Transform _deleteMarker;
        [SerializeField] private List<Decoration> _decorations;

        public bool NextMapCreated { get; set; }
        
        public int Start { get; private set; }
        public int End => Start + Length - 1;
        public int Length => 30;
        
        private static readonly Vector3 _tileBounds = Vector3.one;
        private static readonly Dictionary<TileBase, List<Decoration>> _decorationMap = new(); 

        public void Initialize(int start)
        {
            Start = start;
            foreach (var decoration in _decorations)
            {
                if (!_decorationMap.TryGetValue(decoration.TargetTile, out var list))
                {
                    list = new();
                    _decorationMap.Add(decoration.TargetTile, list);
                }

                list.Add(decoration);
            }
        }

        public int ToLocal(int position)
        {
            return position - Start;
        }

        public void SetTile(Vector3Int worldPosition, TileBase tile, MapObjectType type)
        {
            worldPosition.x = ToLocal(worldPosition.x);

            switch (type)
            {
                case MapObjectType.Cover:
                case MapObjectType.ShortWall:
                case MapObjectType.TallWall:
                {
                    _obstacleMap.SetTile(worldPosition, tile);
                    break;
                }
                case MapObjectType.Hole:
                {
                    _platformMap.SetTile(worldPosition, tile);
                    break;
                }
            }
        }

        public float DeleteMarkerPosition()
        {
            return _deleteMarker.position.x;
        }
        
        public bool NeedNextMap()
        {
            return CameraController.Instance.IsVisible(_rightMost.position, _tileBounds);
        }

        public void Decorate()
        {
            int totalHeight = 2 * MapData.Instance.DefaultHeight + MapData.Instance.FloorHeight;
            for (int y = 0; y <= totalHeight * 2; y++) // :p
            {
                for (int x = 0; x < Length; x++)
                {
                    var targetMap = _baseMap;
                    var targetTile = _baseMap.GetTile(new(x, y, 0));
                    
                    if (targetTile == null)
                    {
                        targetMap = _platformMap;
                        targetTile = _platformMap.GetTile(new Vector3Int(x, y, 0));
                    }
                    
                    if (targetTile == null)
                    {
                        targetMap = _obstacleMap;
                        targetTile = _obstacleMap.GetTile(new Vector3Int(x, y, 0));
                    }

                    if (targetTile != null)
                    {
                        foreach (var decoration in _decorationMap[targetTile])
                        {
                            if (Random.value < decoration.Probability)
                            {
                                targetMap.SetTile(new(x, y, 0), decoration.Tile);
                                break;
                            }
                        }
                    }
                }                
            }
        }

        public void SetEnablePlatformCollider(bool enable)
        {
            _platformEffector.enabled = enable;
            _platformCollider.enabled = enable;
        }

        public Vector3 GetTilePosition(Vector2Int coord, bool isDrop)
        {
            coord.x = ToLocal(coord.x);
            int p = coord.y;
            coord.y = MapData.Instance.FloorHeight + (MapData.Instance.FloorHeight + 2) * coord.y;
            if (isDrop && p == 1)
            {
                coord.y += 7;
            }

            return _baseMap.CellToWorld(new(coord.x, coord.y, 0));
        }
    }
}
