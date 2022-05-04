using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MainStage.MapMaker
{
    public class CriticalRouteGenerator : ChunkGenerator
    {
        private ConnectDirection _connectDirection;
        protected readonly List<Chunk> criticalChunks = new List<Chunk>(); 
        
        protected override void InitializeMapData(int x = MapData.X, int y = MapData.Y)
        {
            base.InitializeMapData(x, y);
            print("Generate Critical Route Generator Start");
            var generateRoute = Task.Run(GenerateChunk);
            generateRoute.Wait();
        }
        
        #region GenerateChunkMap
        private void GenerateChunk()
        {
            const int filledPercent = 25;
            int cnt = 0;
            Stack<Chunk> chunkStack;
            
            while (true)
            {
                print($"loop count: {++cnt}");
                chunkStack = new Stack<Chunk>();
                ResetVisitChunk();
                ChunkTraveler(chunkStack);

                if (chunkStack.Count >= ChunkHeight() * ChunkLenght() / 100 * filledPercent) break;
            }

            foreach (var chunk in chunkStack)
            {
                criticalChunks.Add(chunk);
            }
            criticalChunks.Reverse();
        }
        private void ChunkTraveler(Stack<Chunk> chunkStack)
        {
            _connectDirection = ConnectDirection.None;
            var currentChunk = chunkMap[0][0]; // Start Chunk
            
            while (true)
            {
//                print($"travel :{++cnt}, connect = {_connectDirection}");
                var (direction, chunk) = GetNextChunk(currentChunk);
                currentChunk.IsVisited = true;

                if (chunk == null)
                {
                    currentChunk = chunkStack.Pop();
                    continue;
                }

                currentChunk.Connections = direction;
                chunkStack.Push(currentChunk);
                currentChunk = chunk;
                _connectDirection = direction;

                if (chunk.Type == ChunkType.Finish)
                {
                    chunkStack.Push(currentChunk);
                    break;
                }

            }
        }
        private (ConnectDirection direction, Chunk chunk) GetNextChunk(Chunk currentChunk)
        {
            var chunkList = new List<(ConnectDirection, Chunk)>();

            var upChunk = GetUpChunk(currentChunk);
            var downChunk = GetDownChunk(currentChunk);
            var leftChunk = GetLeftChunk(currentChunk);
            var rightChunk = GetRightChunk(currentChunk);
            
            if (downChunk is {IsVisited: false} &&  _connectDirection != ConnectDirection.Down)
            {
                chunkList.Add((ConnectDirection.Down, downChunk));
            }
            if (upChunk is {IsVisited: false} && _connectDirection != ConnectDirection.Up)
            {
                chunkList.Add((ConnectDirection.Up, upChunk));
            }
            if (leftChunk is {IsVisited: false})
            {
                chunkList.Add((ConnectDirection.Left, leftChunk));
            }
            if (rightChunk is {IsVisited: false})
            {
                chunkList.Add((ConnectDirection.Right, rightChunk));
            }

            var index = Rand.Next(0, chunkList.Count);
//            print(chunkList.Count);
            return chunkList.Count == 0 ? (ConnectDirection.None, null) : chunkList[index];
        }
        private void ResetVisitChunk()
        {
            foreach (var chunks in chunkMap)
            {
                foreach (var chunk in chunks)
                {
                    chunk.IsVisited = false;
                }
            }
        }

        #endregion
        
        #region GetNearChunk
        private Chunk GetUpChunk(Chunk currentChunk)
        {
            var x = currentChunk.Index.x;
            var y = currentChunk.Index.y;

            if (0 < x && 0 < y && x < chunkMap[y - 1].Length)
            {
                return chunkMap[y - 1][x - 1];
            }

            return null;
        }

        private Chunk GetDownChunk(Chunk chunk)
        {
            var x = chunk.Index.x;
            var y = chunk.Index.y;
            
            if (y < chunkMap.Length - 1 && x < chunkMap[y + 1].Length)
            {
                return chunkMap[y + 1][x + 1];
            }
            return null;
        }

        private Chunk GetLeftChunk(Chunk chunk)
        {
            var x = chunk.Index.x;
            var y = chunk.Index.y;
            return x > 0 ? chunkMap[y][x - 1] : null;
        }

        private Chunk GetRightChunk(Chunk chunk)
        {
            var x = chunk.Index.x;
            var y = chunk.Index.y;
            return x < chunkMap[y].Length - 1 ? chunkMap[y][x + 1] : null;
        }
        
        #endregion
    }
}
