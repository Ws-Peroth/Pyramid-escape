using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace MainStage.MapMaker
{
    public struct MapData
    {
        public const int ChunkSize = 5; // 청크 크기 : 반드시 홀수여야함
        public const int Y = 50; // Map y 크기 => ChunkSize * N + 6
        public const int X = 50; // MapY * 2 + 1
        public const TileCode Tile = TileCode.Borderless;
    }

    public class MapDataInitializer : Singleton<MapDataInitializer>
    {
        public GameObject[,] tileMapObjects;
        [SerializeField] protected PlayerMoveController playerMoveController;
        public bool GenerateFinish { get; set; } = false;
        [SerializeField] protected Optimizer optimizer;
        // [SerializeField] protected Material shader;
        [field: SerializeField] public bool UsingViewEffect { get; set; }
        public TileCode[,] Map { get; private set; }
        public int MapX { get; set; }
        public int MapY { get; set; }
        protected int ChunkSize { get; set; }

        private readonly Dictionary<string, Sprite> _sprites = new Dictionary<string, Sprite>();

        protected Sprite GetSprite(string key)
        {
            if (_sprites.ContainsKey(key))
            {
                return _sprites[key];
            }
            
            print("File name Not found");
            throw new NullReferenceException();
        }

        protected virtual void InitializeMapData(int x = MapData.X, int y = MapData.Y)
        {
            // ChunkSize is OddNumber
            ChunkSize = MapData.ChunkSize % 2 == 0 ? MapData.ChunkSize + 1 : MapData.ChunkSize;
            // MapY is ChunkSize * N + 6
            MapY = y + (y - 6) % ChunkSize;
            // MapX is MapY * 2 + 1
            MapX = MapY * 2 + 1;
            
            Map = new TileCode[MapY, MapX];
            
            // map의 값 초기화
            for (var i = 0; i < MapY; i++)
            {
                for (var j = 0; j < MapX; j++)
                {
                    // 초기의 맵은 전부 채워져있음
                    Map[i, j] = MapData.Tile;
                }
            }
            
            InitializeSpriteData();
        }

        private void InitializeSpriteData()
        {
            var spriteArray = Resources.LoadAll<Sprite>("TileMap/Pyramid_Tile");
            
            if (spriteArray == null)
            {
                Debug.LogError("Missing Resources : TileMap/Pyramid_Tile");
                throw new MissingReferenceException();
            }
            
            foreach (var sprite in spriteArray.ToList())
            {
                _sprites.Add(sprite.name, sprite);
            }
        }

        protected static string GetFileName(TileCode tileType) =>
            tileType switch
            {
                TileCode.Borderless => "Borderless",
                TileCode.VoidTile => "__Void",
                TileCode.Wall => "PyramidWall_Tile",
                _ => SetFileName(tileType)
            };

        private static string SetFileName(TileCode tileType)
        {
            var hasDot = ((int) tileType & 0b11110000) != 0;
            var spriteName = "";

            if ((tileType & TileCode.Left) != 0) spriteName += "L";
            if ((tileType & TileCode.Right) != 0) spriteName += "R";
            if ((tileType & TileCode.Up) != 0) spriteName += "U";
            if ((tileType & TileCode.Down) != 0) spriteName += "D";

            if (hasDot)
            {
                spriteName += "_";
                if ((tileType & TileCode.DotUpRight) != 0) spriteName += "1";
                if ((tileType & TileCode.DotUpLeft) != 0) spriteName += "2";
                if ((tileType & TileCode.DotDownLeft) != 0) spriteName += "3";
                if ((tileType & TileCode.DotDownRight) != 0) spriteName += "4";
            }
            
            return spriteName;
        }
        
        protected static TileCode GetBorderType(bool up, bool down, bool left, bool right)
        {
            var tileType =
                (up ? TileCode.Up : 0) |
                (down ? TileCode.Down : 0) |
                (left ? TileCode.Left : 0) |
                (right ? TileCode.Right : 0);
            return tileType;
        }
        
        protected static TileCode GetDotType(bool upLeft, bool upRight, bool downLeft, bool downRight)
        {
            var tileType =
                (upLeft ? TileCode.DotUpLeft : 0) |
                (upRight ? TileCode.DotUpRight : 0) |
                (downLeft ? TileCode.DotDownLeft : 0) |
                (downRight ? TileCode.DotDownRight : 0);
            
            return tileType;
        }

        public static bool IsEmptyTile(TileCode tile) => tile == TileCode.VoidTile || tile == TileCode.Wall;
        
        protected static TileCode FixTileType(TileCode tileType)
        {
            if (IsEmptyTile(tileType))
            {
                return tileType;
            }

            // Border과 Dot이 같은 방향에 있으며 Dot을 지워줌
            if ((tileType & TileCode.Up) != 0)
            {
                tileType &= ~(TileCode.DotUpLeft | TileCode.DotUpRight);
            }
            if ((tileType & TileCode.Down) != 0)
            {
                tileType &= ~(TileCode.DotDownLeft | TileCode.DotDownRight);
            }
            if ((tileType & TileCode.Left) != 0)
            {
                tileType &= ~(TileCode.DotUpLeft | TileCode.DotDownLeft);
            }
            if ((tileType & TileCode.Right) != 0)
            {
                tileType &= ~(TileCode.DotUpRight | TileCode.DotDownRight);
            }
            return tileType;
        }
    }
}