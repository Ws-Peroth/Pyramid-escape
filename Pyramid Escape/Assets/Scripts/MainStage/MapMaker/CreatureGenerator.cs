using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MainStage.MapMaker
{
    public class CreatureGenerator : StructureGenerator
    {
        // Action nextAction;
        /*
        * MapDataInitializer ->        (초기화)
        * MapGenerator ->              (기초 맵 생성)
        * ChunkGenerator ->            (청크 생성)
        * CriticalRouteGenerator ->    (핵심 경로 생성)
        * ChunkConnectionGenerator ->  (전체 경로 생성)
        * ChunkDesigner ->             (청크 디자인)
        * TileObjectGenerator ->       (타일 생성)
        * MapDesigner                  (맵 수정)
        */

        protected override void Start()
        {
            base.Start();
            StartCreatureGenerate();
        }

        protected void StartCreatureGenerate(/*Action nextLogic*/)
        {
            // nextAction = nextLogic;
            StartStructureGenerate(CreatureGenerate);
            
        }

        private void CreatureGenerate()
        {
            // Todo: Generate Map Structures
            var level = 1;
            foreach (var chunk in criticalChunks)
            {
                if(chunk.Type != ChunkType.Monster) continue;

                var startChunk = chunk.Start;
                var spawnMonsterCount = Random.Range(2, 3);
                var indexY = (int) Calc.Avg(startChunk.y, startChunk.y + ChunkSize);
                var x0 = FindStartIndex(startChunk.x, indexY);
                var xn = FindEndIndex(startChunk.x, indexY);
                var d = (xn - x0) / spawnMonsterCount;

                if (x0 == -1 || xn == -1)
                {
                    break;
                }
                for (var i = 0; i < spawnMonsterCount; i++)
                {
                    if ((chunk.Connections & ConnectDirection.Down) != 0) continue;
                    var indexX = i * d + x0;
                    var tile = tileMapObjects[indexY, indexX];
                    var position = tile.transform.position;
                    var enemy =SpawnCreature(position, PoolCode.Mummy, level++);
                    tile.GetComponent<SpriteOptimizer>().spawnMonster.Add(enemy);
                }
                level++;
            }
            // End of Logic
            // nextAction();
        }

        private int FindStartIndex(int x, int y)
        {
            for (var i = x; i < x + ChunkSize; i++)
            {
                if (Map[y, i] == TileCode.Wall) return i;
            }

            return -1;
        }
        private int FindEndIndex(int x, int y)
        {
            for (var i = x + ChunkSize - 1; i >= x; i--)
            {
                if (Map[y, i] == TileCode.Wall) return i;
            }

            return -1;
        }
        
        private static GameObject SpawnCreature(Vector3 position, PoolCode creatureType, int level)
        {
            var creature = PoolManager.instance.CreatPrefab(creatureType);
            creature.transform.position = position;
            creature.SetActive(true);
            var enemy = creature.GetComponent<Enemy>();
            enemy.Level = level;
            enemy.Initialize();
            creature.SetActive(false);
            return creature;
        }
    }
}
