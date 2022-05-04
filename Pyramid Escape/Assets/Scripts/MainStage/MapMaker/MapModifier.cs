using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace MainStage.MapMaker
{
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
    public class MapModifier : TileObjectGenerator
    {
        public DateTime time0;
        protected override void Start()
        {
            time0 = DateTime.Now;
            base.Start();

            InitializeMapData();
            StartCoroutine(ModifyTiles());
        }

        private IEnumerator ModifyTiles()
        {
            for (var y = 0; y < MapY; y++)
            {
                for (var x = 0; x < MapX; x++)
                {
                    if (tileMapObjects[y, x] == null)
                    {
                        continue;
                    }

                    tileMapObjects[y, x].SetActive(true);

                    if (IsEmptyTile(Map[y, x]))
                    {
                        Destroy(tileMapObjects[y, x].GetComponent<ShadowCaster2D>());
                        Destroy(tileMapObjects[y, x].GetComponent<BoxCollider2D>());
                        continue;
                    }
                    var tileType = GetTileCode(x, y);

                    if (tileType == TileCode.VoidTile)
                    {
                        print($"x: {x}, y: {y}");
                    }

                    Map[y, x] = tileType;
                    tileMapObjects[y, x].GetComponent<SpriteRenderer>().sprite = GetSprite(GetFileName(tileType));
                }

                yield return null;
            }

            gameObject.AddComponent<CompositeCollider2D>();
            gameObject.AddComponent<CompositeShadowCaster2D>();
            
            yield return null;
            
            var time1 = DateTime.Now;
            var spanStart = new TimeSpan(time0.Day, time0.Hour, time0.Minute, time0.Second, time0.Millisecond);
            var spanEnd = new TimeSpan(time1.Day, time1.Hour, time1.Minute, time1.Second, time1.Millisecond);
            var gap = spanEnd.Subtract(spanStart);
            
            print("Map generate End");
            print($"Start Time\t{time0:h:mm:ss tt}");
            print($"Finish Time\t{time1:h:mm:ss tt}");
            print($"total time : {gap}");

            if (Camera.main is { })
            {
                Camera.main.transform.position = new Vector3(176, 7, -10);
            }
        }

        private TileCode GetTileCode(int x, int y)
        {
            return FixTileType(GetBorderTile(x, y) | GetDotTile(x, y));
        }
        private TileCode GetBorderTile(int x, int y)
        {
            // 상하좌우 블럭 유무 체크
            // 죄표의 끝이면 해방 방향에 Border 생성
            // 방향에 블럭이 비어있으면 Border 생성
            //    = [좌표의 끝임]   or [방향에 있는 블럭이 비어있음]
            var right = x == MapX - 1 || IsEmptyTile(Map[y, x + 1]);
            var down = y == MapY - 1 || IsEmptyTile(Map[y + 1, x]);
            var left = x == 0 || IsEmptyTile(Map[y, x - 1]);
            var up = y == 0 || IsEmptyTile(Map[y - 1, x]);

            // true => Border 생성이 필요함
            return GetBorderType(up, down, left, right);
        }
        
        private TileCode GetDotTile(int x, int y)
        {
            // 상, 하, 좌, 우, 좌상, 우상, 좌하, 우하 방향 확인
            // 죄표의 끝이면 해방 방향에 dot 없음
            // 상 / 하 / 좌 / 우 에 블럭이 있고, 좌상 / 우상 / 좌하 / 우하 에 블럭이 없음
            
            var isRightBlockExist = x < MapX - 1 && !IsEmptyTile(Map[y, x + 1]);
            var isDownBlockExist = y < MapY - 1 && !IsEmptyTile(Map[y + 1, x]);
            var isLeftBlockExist = x > 0 && !IsEmptyTile(Map[y, x - 1]);
            var isUpBlockExist = y > 0 && !IsEmptyTile(Map[y - 1, x]);

            var downRight = isDownBlockExist && isRightBlockExist && IsEmptyTile(Map[y + 1, x + 1]);
            var downLeft = isDownBlockExist && isLeftBlockExist && IsEmptyTile(Map[y + 1, x - 1]);
            var upRight = isUpBlockExist && isRightBlockExist && IsEmptyTile(Map[y - 1, x + 1]);
            var upLeft = isUpBlockExist && isLeftBlockExist && IsEmptyTile(Map[y - 1, x - 1]);

            // true => dot 생성이 필요함
            return GetDotType(upLeft, upRight, downLeft, downRight);
        }
    }
}