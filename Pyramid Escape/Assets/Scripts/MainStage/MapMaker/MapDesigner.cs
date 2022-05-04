using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace MainStage.MapMaker
{
    public class MapDesigner : ChunkConnectionGenerator
    {
        protected override void InitializeMapData(int x = MapData.X, int y = MapData.Y)
        {
            base.InitializeMapData(x, y);

            var designChunk = Task.Run(MapDesign);
            designChunk.Wait();
        }
        private void MapDesign()
        {
            foreach (var chunk in criticalChunks)
            {
                FillChunk(chunk);
            }

            SetChunkType();
        }
        private void FillChunk(Chunk chunk)
        {
            var xStart = chunk.Start.x;
            var yStart = chunk.Start.y;
            var xEnd = chunk.End.x;
            var yEnd = chunk.End.y;
            
            for (var y = yStart; y <= yEnd; y++)
            {
                for (var x = xStart; x <= xEnd; x++)
                {
                    var connect = chunk.Connections;
                    
                    Map[y, x] = TileCode.Wall;
                    
                    if (y == yStart && (connect & ConnectDirection.Up) == 0)
                    {
                        Map[y, x] = TileCode.Borderless;
                    }

                    if (y == yEnd && (connect & ConnectDirection.Down) == 0)
                    {
                        Map[y, x] = TileCode.Borderless;
                    }

                    if (x == xStart && (connect & ConnectDirection.Left) == 0)
                    {
                        Map[y, x] = TileCode.Borderless;
                    }

                    if (x == xEnd && (connect & ConnectDirection.Right) == 0)
                    {
                        Map[y, x] = TileCode.Borderless;
                    }
                }
            }
        }
        
        private static void SetChunkType()
        {
            // 다시 고민해 볼 것
        }
    }
}
