using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using Random = UnityEngine.Random;

public enum ChunkType
{
    Shop,     // 상점
    Deal,     // 도박장
    Room,     // 방 (기본)
    Alter,    // 플레이어 회복 & 강화
    Monster,  // 몬스터 하우스
    Boss,     // 보스방
    Start,    // 시작지점
    Finish,   // 끝 청크
    Max
}

[Flags]
public enum ConnectDirection
{
    None = 0,
    Up = 1,
    Down = 2,
    Left = 4,
    Right = 8,
}

namespace MainStage.MapMaker
{
    public class ChunkGenerator : MapGenerator
    {

        protected static readonly System.Random Rand = new System.Random();
        protected Chunk[][] chunkMap;
        protected int ChunkHeight() => (MapY - 6) / ChunkSize;
        protected int ChunkLenght() => (ChunkHeight() + 1) * 2 - 1;
        protected override void InitializeMapData(int x = MapData.X, int y = MapData.Y)
        {
            base.InitializeMapData(x, y);

            var setup = Task.Run(SetupChunkMap);
            setup.Wait();
        }
        
        # region InitializeChunkMap
        private Chunk GetStartChunk()
        {
            var startY = (ChunkSize + 3) / 2;
            var startX = (MapX - 1) / 2 - 4;
            var endY = startY + ChunkSize - 1;
            var endX = startX + ChunkSize - 1;

            return new Chunk
            {
                Index = (0, 0),
                Start = (startX, startY),
                End = (endX, endY),
                IsCriticalChunk = true,
                Type = ChunkType.Start,
                IsVisited = false
            };
        }

        private void InitializeStartChunk()
        {
            chunkMap[0] = new Chunk[1];
            chunkMap[0][0] = GetStartChunk();
        }

        private void SetEndChunk(int height, int length)
        {
            var chunkIndex = Rand.Next(length);


            var endChunk = chunkMap[height - 1][chunkIndex];
            
            endChunk.Type = ChunkType.Finish;
            endChunk.IsCriticalChunk = true;
        }
        private void SetupChunkMap()
        {
            var height = ChunkHeight();
            chunkMap = new Chunk[height][];

            InitializeStartChunk();
            
            for (var y = 1; y < height; y++)
            {
                var length = (y + 1) * 2 - 1;
                chunkMap[y] = new Chunk[length];
                var chunk = chunkMap[y - 1][0];
                
                var start = (
                    x: chunk.Start.x - ChunkSize,
                    y: chunk.End.y + 1
                );
                var end = (
                    x: start.x + ChunkSize - 1,
                    y: start.y + ChunkSize - 1
                );
                
                for (var x = 0; x < length; x++)
                {
                    chunkMap[y][x] = new Chunk()
                    {
                        Index = (x, y),
                        Start = start,
                        End = end,
                        Type = ChunkType.Room,
                        IsVisited = false
                    };
                    start.x += ChunkSize;
                    end.x += ChunkSize;
                }
            }

            SetEndChunk(height, height * 2 - 1);
        }

        #endregion
    }
}