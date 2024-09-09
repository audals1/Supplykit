using System.Collections.Generic;
using System.Linq;
using Common;
using NaughtyAttributes;
using UnityEngine;

namespace Game.StageMap
{
    public class Cell
    {
        public MapObjectType Bottom;
        public MapObjectType Top;

        public MapObjectType this[int floor]
        {
            get => floor == 0 ? Bottom : Top;
            set
            {
                if (floor == 0) Bottom = value;
                else Top = value;
            }
        }
    }

    public class MapManager : SingletonMonoBehaviour<MapManager>
    {
        [SerializeField] private List<Map> _preSpawnedMaps;
        
        [SerializeField] private MapData _data;
        [SerializeField] private Map _mapPrefab;
        [SerializeField] private Transform _mapParent;
        [SerializeField] private List<MapObject> _mapObjectList;
        [SerializeField] private Transform _characterTransform;
        [SerializeField] private SupplySpawner _supplySpawner;
        [SerializeField] private Monster monster;

        private Dictionary<MapObjectType, MapObject> _mapObjects;
        private readonly List<Cell> _world = new(10000);
        private int _currentLength;

        private int _nextHole;
        private int _nextTallWall;
        private int _reservedHoleLength;

        private readonly List<Map> _maps = new();

        public bool GameStarted => _preSpawnedMaps.LastOrDefault() == null;

        protected override void Awake()
        {
            Application.targetFrameRate = 60;

            base.Awake();

            Initialize();

            if (_preSpawnedMaps.Count > 0)
            {
                foreach (var map in _preSpawnedMaps)
                {
                    CreateNextMap(map);
                }
            }
            else
            {
                _mapParent.DestroyAllChildren();
                CreateNextMap();
            }
        }

        public void Initialize()
        {
            _mapObjects = _mapObjectList.ToDictionary(mo => mo.Type, mo => mo);
        }

        [Button]
        public void TestCreateNextMap()
        {
            Initialize();

            _mapParent.DestroyAllChildrenImmediately();
            _maps.Clear();

            CreateNextMap();
        }

        private void Update()
        {
            for (int i = 0; i < _maps.Count; i++)
            {
                var map = _maps[i];
                if (_characterTransform != null && _characterTransform.position.x > map.DeleteMarkerPosition())
                {
                    Destroy(map.gameObject);
                    _maps.Remove(map);
                    i--;
                    continue;
                }

                if (i == _maps.Count - 1 && !map.NextMapCreated && map.NeedNextMap())
                {
                    map.NextMapCreated = true;
                    CreateNextMap();
                }
            }
        }

        public void CreateNextMap(Map preSpawned = null)
        {
            var map = preSpawned;
            if (map == null)
            {
                map = Instantiate(_mapPrefab, _mapParent);
            }

            var lastMap = _maps.LastOrDefault();
            if (lastMap != null)
            {
                map.transform.position = lastMap.transform.position + new Vector3(lastMap.Length, 0, 0);
            }

            map.Initialize(_currentLength);
            _world.AddRange(Enumerable.Range(0, map.Length).Select(_ => new Cell()));

            int start = _currentLength;
            int end = _currentLength + map.Length;

            if (preSpawned != null)
            {
                _nextHole = end;
                _nextTallWall = end;
            }
            else
            {
                // Hole
                if (_reservedHoleLength != 0)
                {
                    for (int x = 0; x < _reservedHoleLength; x++)
                    {
                        SetMapObject(map, start + x, 1, MapObjectType.Hole);
                    }

                    _nextHole = start + _reservedHoleLength - 1 + _data.HoleData.Interval.GetRandom();
                    _reservedHoleLength = 0;
                }

                while (_nextHole < end)
                {
                    int length = _data.HoleData.Length.GetRandom();
                    for (; length > 0 && _nextHole < end; length--, _nextHole++)
                    {
                        SetMapObject(map, _nextHole, 1, MapObjectType.Hole);
                    }

                    _reservedHoleLength = length;
                    _nextHole += _data.HoleData.Interval.GetRandom();
                }

                // TallWall
                while (_nextTallWall < end)
                {
                    for (; _nextTallWall < end && _world[_nextTallWall][1] == MapObjectType.Hole; _nextTallWall++) ;
                    if (_nextTallWall >= end) break;

                    // Backward distance
                    for (int d = 1; d <= _data.TallWallData.MinDistanceFromHole; d++)
                    {
                        int point = _nextTallWall - d;
                        if (point < 0) break;

                        if (_world[point][1] == MapObjectType.Hole)
                        {
                            _nextTallWall = point + _data.TallWallData.MinDistanceFromHole;
                            break;
                        }
                    }

                    if (_nextTallWall >= end) break;

                    // Forward distance
                    int emptyDistance = 0;
                    for (int d = 1; d <= _data.TallWallData.MinDistanceFromHole; d++)
                    {
                        int searchPoint = _nextTallWall + d;
                        if (searchPoint >= end)
                        {
                            emptyDistance = _data.TallWallData.MinDistanceFromHole;
                            break;
                        }

                        if (_world[searchPoint][1] == MapObjectType.Hole)
                        {
                            _nextTallWall = searchPoint + 1;
                            break;
                        }

                        emptyDistance++;
                    }

                    if (emptyDistance == _data.TallWallData.MinDistanceFromHole)
                    {
                        int floor = Random.Range(0, 2);
                        //
                        floor = 1;
                        
                        SetMapObject(map, _nextTallWall, floor, MapObjectType.TallWall);
                        _nextTallWall += _data.TallWallData.Interval.GetRandom();
                    }
                }

                // ShortWall
                int shortWallMinDistance = _data.ShortWallData.MinDistanceFromOtherObstacle;
                for (int floor = 0; floor < 2; floor++)
                {
                    for (int position = start + shortWallMinDistance; position < end - shortWallMinDistance; position++)
                    {
                        bool shouldContinue = false;
                        for (int d = -shortWallMinDistance; d <= shortWallMinDistance; d++)
                        {
                            if (_world[position + d][floor] != MapObjectType.None)
                            {
                                shouldContinue = true;
                                break;
                            }
                        }

                        if (shouldContinue) continue;
                        if (Random.value < _data.ShortWallData.Probability)
                        {
                            SetMapObject(map, position, floor, MapObjectType.ShortWall);
                            _nextTallWall = Mathf.Max(_nextTallWall, position + shortWallMinDistance);
                        }
                    }
                }

                // Cover
                int coverMinDistance = _data.CoverData.MinDistanceFromOtherObstacle;
                for (int floor = 0; floor < 2; floor++)
                {
                    for (int position = start + coverMinDistance; position < end - coverMinDistance; position++)
                    {
                        bool shouldContinue = false;
                        for (int d = -coverMinDistance; d <= coverMinDistance; d++)
                        {
                            if (_world[position + d][floor] != MapObjectType.None)
                            {
                                shouldContinue = true;
                                break;
                            }
                        }

                        if (shouldContinue) continue;
                        if (Random.value < _data.CoverData.Probability)
                        {
                            SetMapObject(map, position, floor, MapObjectType.Cover);
                            _nextTallWall = Mathf.Max(_nextTallWall, position + coverMinDistance);
                        }
                    }
                }

                if (_supplySpawner != null)
                {
                    _supplySpawner.SpawnThings(_world, start, end, map);
                }

                map.Decorate();
            }

            _currentLength = end;
            _maps.Add(map);
        }

        public void SetMapObject(Map map, int position, int floor, MapObjectType type)
        {
            _world[position][floor] = type;
            var mapObject = _mapObjects[type];
            foreach (var pair in mapObject.Tiles)
            {
                map.SetTile(new Vector3Int(position, floor * _data.FloorHeight + _data.DefaultHeight + pair.Y, 0), pair.Tile, type);
            }
        }

        public void SetEnabledPlatformColliders(bool enable)
        {
            foreach (var map in _maps)
            {
                map.SetEnablePlatformCollider(enable);
            }
        }
    }
}
