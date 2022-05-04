using System;
using System.Threading.Tasks;

[Flags]
public enum TileCode
{
    Borderless    = 0,
    Up            = 1,
    Down          = 2,
    Right         = 4,
    Left          = 8,
    DotUpRight    = 16,
    DotUpLeft     = 32,
    DotDownLeft   = 64,
    DotDownRight  = 128,
    VoidTile      = 129,
    Wall          = 130,  
    Max

}

namespace MainStage.MapMaker
{
    public class MapGenerator : MapDataInitializer
    {
        protected override void InitializeMapData(int x = MapData.X, int y = MapData.Y)
        {

            print($"Total Position : {MapX}, {MapY}");
            base.InitializeMapData(x, y);

            var t = Task.Run(SetTiles);
        
            t.Wait();
        }
        
        private void SetTiles()
        {
            var start = MapX / 2;
            var end = start;
            for (var i = 0; i < MapY; i++)
            {
                for (var j = 0; j < MapX; j++)
                {
                    Map[i, j] = GetTileCode(j, i, start, end);
                }
                start--;
                end++;
            }
        }

        private TileCode GetTileCode(int x, int y, int start, int end)
        {
            
            if (x < start || end < x) { return TileCode.VoidTile; }
            return TileCode.Borderless;
            /*
            if (x <= start + 1 || end - 1 <= x) { return TileCode.Borderless; }
            if (y == MapY - 1) { return TileCode.Borderless; }
            return TileCode.Wall;
            */
        }
    }
}
