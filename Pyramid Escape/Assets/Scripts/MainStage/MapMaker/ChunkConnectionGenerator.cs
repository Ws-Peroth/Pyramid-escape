using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace MainStage.MapMaker
{
    public class ChunkConnectionGenerator : CriticalRouteGenerator
    {
        protected override void InitializeMapData(int x = MapData.X, int y = MapData.Y)
        {
            base.InitializeMapData(x, y);
            var setConnections = Task.Run(SetChunkConnections);
            setConnections.Wait();
        }

        private void SetChunkConnections()
        {
            var fixedChunkConnections = new List<ConnectDirection>();
            fixedChunkConnections.Add(criticalChunks[0].Connections);
            
            for (var i = 1; i < criticalChunks.Count; i++)
            {
                fixedChunkConnections.Add(criticalChunks[i].Connections);
                var fixChunk = fixedChunkConnections[i];
                fixChunk |= GetReverseDirection(criticalChunks[i - 1].Connections);
                fixedChunkConnections[i] = fixChunk;
            }

            for (var i = 0; i < criticalChunks.Count; i++)
            {
                criticalChunks[i].Connections = fixedChunkConnections[i];
            }
        }
        
        private ConnectDirection GetReverseDirection(ConnectDirection direction)
        {
            var reverseDirection = ConnectDirection.None;

            if ((direction & ConnectDirection.Up) != 0) reverseDirection |= ConnectDirection.Down;
            if ((direction & ConnectDirection.Down) != 0) reverseDirection |= ConnectDirection.Up;
            if ((direction & ConnectDirection.Right) != 0) reverseDirection |= ConnectDirection.Left;
            if ((direction & ConnectDirection.Left) != 0) reverseDirection |= ConnectDirection.Right;
            
            return reverseDirection;
        }
    }
}
