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
            var level = 1;
            foreach (var chunk in criticalChunks)
            {
                var startChunk = chunk.Start;
                var indexY = (int) Calc.Avg(startChunk.y, startChunk.y + ChunkSize);
                var x0 = FindStartIndex(startChunk.x, indexY);
                var xn = FindEndIndex(startChunk.x, indexY);
                
                if (x0 == -1 || xn == -1) break;
                if ((chunk.Connections & ConnectDirection.Down) != 0) continue;
                if (!(chunk.Type > ChunkType.Room || chunk.Type < ChunkType.Start)) continue;
                
                switch (chunk.Type)
                {
                    case ChunkType.Finish: CreatFinishChunk(x0, xn, indexY); break;
                    case ChunkType.Alter: CreatAlterChunk(x0, xn, indexY); break;
                    case ChunkType.Monster: level += CreatMonsterChunk(x0, xn, indexY, level); break;
                }
            }
        }

        private int CreatMonsterChunk(int x0, int xn, int indexY, int level)
        {
            var spawnMonsterCount = Random.Range(2, 3);
            var d = (xn - x0) / spawnMonsterCount;
            
            for (var i = 0; i < spawnMonsterCount; i++)
            {
                var indexX = i * d + x0;
                var tile = tileMapObjects[indexY, indexX];
                var position = tile.transform.position;
                var enemy = SpawnCreature(position, PoolCode.Mummy, level++);
                var enemyScript = enemy.GetComponent<Enemy>();
                var tileOptimizer = tile.GetComponent<SpriteOptimizer>();
                enemyScript.optimizeObject = tileOptimizer;
                tile.GetComponent<SpriteOptimizer>().spawnMonster.Add(enemy);
            }

            return level + 1;
        }
        private void CreatAlterChunk(int x0, int xn, int indexY)
        {
            var d = (xn - x0) / 2;
            var indexX = d + x0;
            var tile = tileMapObjects[indexY, indexX];
            var position = tile.transform.position;
                    
            var creature = PoolManager.instance.CreatPrefab(PoolCode.Alter);
            creature.transform.position = new Vector3(position.x + 1, position.y);
            creature.SetActive(true);
        }
        private void CreatFinishChunk(int x0, int xn, int indexY)
        {
            var d = (xn - x0) / 2;
            var indexX = d + x0;
            var tile = tileMapObjects[indexY, indexX];
            var position = tile.transform.position;

            var creature = PoolManager.instance.CreatPrefab(PoolCode.EndDoor);
            creature.transform.position = new Vector3(position.x + 1, position.y);
            creature.SetActive(true);
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
