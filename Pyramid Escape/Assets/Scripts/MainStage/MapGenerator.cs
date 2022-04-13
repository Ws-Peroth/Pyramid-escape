using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

// Tile. [Type] [Number]

// [Type]
// Full / Blank / Etc.

// [Number]
// Block Dot Number
//  2  |  1
//  3  |  4
public enum Tile
{
    BlackTile,
    GreenBlock,
    VoidTile,
    BorderFull,
    // D (4)
    D,
    D1,
    D2,
    D12,
    // Full (16)
    Full,
    Full1,
    Full2,
    Full3,
    Full4,
    Full12,
    Full13,
    Full14,
    Full23,
    Full24,
    Full34,
    Full123,
    Full124,
    Full134,
    Full234,
    Full1234,
    // L (4)
    L,
    L1,
    L4,
    L14,
    // LD (2)
    Ld,
    Ld1,
    ////////
    Lr,
    Lrd,
    Lru,
    Lu,
    Lu4,
    Lud,
    // R (4)
    R,
    R2,
    R3,
    R23,
    // RD (2)
    Rd,
    Rd2,
    // RU (2)
    Ru,
    Ru3,
    ////////
    Rud,
    // U (4)
    U,
    U3,
    U4,
    U34,
    ////////
    Ud,
    Wall,
    ////////
    Max
}

public class MapGenerator : MapInitializer
{
    [SerializeField] private float mapGenerateProgress;

    // Start is called before the first frame update
    void Start()
    {
        InitializeMapData();
    }

    protected override void InitializeMapData(int X = MapData.X, int Y = MapData.Y, Tile tile = Tile.Full)
    {
        MapY = Y;
        MapX = MapY * 2 + 1;
        base.InitializeMapData(MapX, MapY, tile);

        var start = MapX / 2;
        var end = start - 1;
        Map[0, start] = Tile.Full;

        for (var i = 0; i < MapY; i++)
        {
            start--;
            end++;
            for (var j = 0; j < MapX; j++)
            {
                if (i == MapY - 1)
                {
                    Map[i, j] = Tile.Full;
                    continue;
                }
                
                if (j < start)
                {
                    Map[i, j] = Tile.VoidTile;
                    continue;
                }

                if (j == start || j == start + 1)
                {
                    Map[i, j] = Tile.Full;
                    continue;
                }

                if (j == end || j == end + 1)
                {
                    Map[i, j] = Tile.Full;
                    continue;
                }

                if (j > end)
                {
                    Map[i, j] = Tile.VoidTile;
                    continue;
                }
                
                Map[i, j] = Tile.Wall;
            }
        }
    }
    // [Normalize]						h - hn   ==> h + hn - 1
    // void0    ==>  void1        condition : void1  < x >=  void0
    // 
    // 

    // Update is called once per frame
    void Update()
    {
        
    }
}
