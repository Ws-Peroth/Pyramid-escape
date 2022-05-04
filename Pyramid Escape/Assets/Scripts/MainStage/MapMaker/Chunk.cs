using UnityEngine;

namespace MainStage.MapMaker
{
    public class Chunk
    {
        public (int x, int y) Start { get; set; }
        public (int x, int y) End { get; set; }
        public (int x, int y) Index { get; set; }
        public bool IsVisited { get; set; }
        public bool IsCriticalChunk { get; set; }
        public ConnectDirection Connections { get; set; }
        public ChunkType Type { get; set; }

        public int Level { get; set; }
    }
}