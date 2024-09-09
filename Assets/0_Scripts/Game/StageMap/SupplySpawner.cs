using System;
using System.Collections.Generic;
using System.Linq;
using Actor;
using Common;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.StageMap
{
    [Serializable]
    public class SpawnData
    {
        public SupplyKit Prefab;
        public List<SupplyKit> WeaponList;
        public int MinDistance = 90;
        public int Interval;
        public float Probability;
        public float DropProbability;
        public Vector2Int CountPerMap;
        public bool IsOneTime;

        [NonSerialized] public int NextSpawnPosition;
        [NonSerialized] public bool IsSpawned;
    }

    public class SupplySpawner : SingletonMonoBehaviour<SupplySpawner>
    {
        [SerializeField] private List<Monster> _monsterPrefabs;
        [SerializeField] private List<SpawnData> _spawnDataList;

        public const int Top = 1;
        public const int Bottom = 0;

        private int _lastSpawnedWeaponIndex;

        private void Start()
        {
            foreach (var data in _spawnDataList)
            {
                data.NextSpawnPosition = data.MinDistance;
            }
        }

        public void SpawnThings(List<Cell> world, int start, int end, Map map)
        {
            var spawnableCells = new List<Vector2Int>();
            for (int i = start; i < end; i++)
            {
                var cell = world[i];
                if (cell.Top == MapObjectType.Hole)
                {
                    spawnableCells.Add(new(i, Top));
                    continue;
                }

                if (cell.Top != MapObjectType.TallWall)
                {
                    spawnableCells.Add(new(i, Top));
                }

                if (cell.Bottom == MapObjectType.None || cell.Bottom == MapObjectType.Cover)
                {
                    spawnableCells.Add(new(i, Bottom));
                }
            }

            spawnableCells = spawnableCells.OrderBy(e => Random.value).ToList();
            int spCursor = 0;

            foreach (var spawnData in _spawnDataList)
            {
                if (spawnData.NextSpawnPosition > start) continue;
                if (Random.value > spawnData.Probability) continue;
                if (spawnData.IsOneTime && spawnData.IsSpawned) continue;

                if (spawnData.Prefab != null)
                {
                    int count = spawnData.CountPerMap.GetRandom();
                    for (int i = 0; i < count; i++)
                    {
                        if (!TryGetSpawnablePosition(out var coord))
                        {
                            return;
                        }

                        bool isDrop = Random.value < spawnData.DropProbability;

                        var position = map.GetTilePosition(coord, isDrop);
                        var supply = Instantiate(spawnData.Prefab, position, Quaternion.identity);
                        supply.transform.parent = map.transform;
                        supply.Initialize(isDrop);
                    }

                    spawnData.IsSpawned = true;
                    spawnData.NextSpawnPosition += spawnData.Interval;
                }
                else
                {
                    if (!TryGetSpawnablePosition(out var coord))
                    {
                        return;
                    }

                    int nextIndex = _lastSpawnedWeaponIndex + Random.Range(1, spawnData.WeaponList.Count - 1);
                    nextIndex %= spawnData.WeaponList.Count;

                    _lastSpawnedWeaponIndex = nextIndex;
                    var prefab = spawnData.WeaponList[nextIndex];
                    
                    var position = map.GetTilePosition(coord, false);
                    var supply = Instantiate(prefab, position, Quaternion.identity);
                    supply.transform.parent = map.transform;
                    supply.Initialize(false);
                    
                    spawnData.IsSpawned = true;
                    spawnData.NextSpawnPosition += spawnData.Interval;
                }
            }

            int foo = end / 150;
            int c = Random.Range(3, 6) + foo;
            for (int i = 0; i < c; i++)
            {
                if (!TryGetSpawnablePosition(out var coord))
                {
                    return;
                }

                var position = map.GetTilePosition(coord, false);
                var pre = _monsterPrefabs[Random.Range(0, _monsterPrefabs.Count)];
                var monster = Instantiate(pre, position, Quaternion.identity);
                monster.Initiate(monster._data);
            }


            bool TryGetSpawnablePosition(out Vector2Int coord)
            {
                coord = default;
                if (spCursor >= spawnableCells.Count) return false;
                coord = spawnableCells[spCursor++];
                return true;
            }
        }
    }
}
