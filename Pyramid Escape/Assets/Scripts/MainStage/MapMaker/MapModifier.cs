using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace MainStage.MapMaker
{
    public class MapModifier : TileObjectGenerator
    {
        private const TileCode HasWall = TileCode.Down | TileCode.Up | TileCode.Left | TileCode.Right;
        private Camera _selectedCamera; 
        private DateTime time0;
        private Action _nextLogic;
        
        protected void StartMapGenerate(Action nextAction)
        {
            _nextLogic = nextAction;
            InitializeMapData();
        }

        protected override void InitializeMapData(int x = MapData.X, int y = MapData.Y)
        {
            _selectedCamera = Camera.main;
            time0 = DateTime.Now;
            base.Start();
            base.InitializeMapData(x, y);
            StartCoroutine(ModifyTiles());
        }

        private IEnumerator ModifyTiles()
        {
            for (var y = 0; y < MapY; y++)
            {
                for (var x = 0; x < MapX; x++)
                {
                    if (tileMapObjects[y, x] == null) continue;
                    
                    tileMapObjects[y, x].SetActive(true);
                    
                    if (IsEmptyTile(Map[y, x]))
                    {
                        Destroy(tileMapObjects[y, x].GetComponent<ShadowCaster2D>());
                        Destroy(tileMapObjects[y, x].GetComponent<BoxCollider2D>());
                        continue;
                    }
                    var tileType = GetTileCode(x, y);
                    Map[y, x] = tileType;
                    HideShadowCast(x, y);
                    
                    var tileShadowCaster = tileMapObjects[y, x].GetComponent<ShadowCaster2D>();
                    var tileSpriteRenderer = tileMapObjects[y, x].GetComponent<SpriteRenderer>();
                    tileSpriteRenderer.sprite = GetSprite(GetFileName(tileType));
                    tileSpriteRenderer.enabled = false;
                    tileShadowCaster.enabled = false;
                }

                yield return null;
            }
            gameObject.AddComponent<CompositeCollider2D>();
            
            if (UsingViewEffect)
            {
                gameObject.AddComponent<CompositeShadowCaster2D>();
            }
            yield return null;
            if (Camera.main is null)
            {
                yield break;
            }
            GenerateFinish = true;
            PrintGenerateTime();
            PlayerSpawn();
            playerMoveController.SetLight(true);

            _nextLogic();
        }

        private void PrintGenerateTime()
        {
            var time1 = DateTime.Now;
            var spanStart = new TimeSpan(time0.Day, time0.Hour, time0.Minute, time0.Second, time0.Millisecond);
            var spanEnd = new TimeSpan(time1.Day, time1.Hour, time1.Minute, time1.Second, time1.Millisecond);
            var gap = spanEnd.Subtract(spanStart);
            print($"Map generate End\ntotal time : {gap}");
        }
        
        private void PlayerSpawn()
        {
            var startChunk = criticalChunks[0].Start;
            var indexX = (int) Calc.Avg(startChunk.x, startChunk.x + ChunkSize);
            var indexY = (int) Calc.Avg(startChunk.y, startChunk.y + ChunkSize);
            print($"{indexX}, {indexY}");
            var setPosition = tileMapObjects[indexY, indexX].transform.position;
            setPosition = new Vector3(setPosition.x, setPosition.y, -3);
            playerMoveController.transform.position = setPosition;
        }
        private void HideShadowCast(int x, int y)
        {
            if (!UsingViewEffect) return;
            
            var tileType = Map[y, x];
            var isCastOff = false;
            isCastOff = (tileType & HasWall) == 0;
            isCastOff = isCastOff || x == 0 || x == MapX - 1;
            isCastOff = isCastOff || y == 0 || y == MapY - 1;
            isCastOff = isCastOff || tileMapObjects[y, x - 1] == null || tileMapObjects[y, x + 1] == null;

            if (!isCastOff) return;
            
            var tileShadowCaster2D = tileMapObjects[y, x].GetComponent<ShadowCaster2D>();
            if ((tileType & HasWall) == 0)
            {
                tileShadowCaster2D.useRendererSilhouette = false;
                tileShadowCaster2D.selfShadows = false;
            }

            tileShadowCaster2D.castsShadows = false;
        }

        private TileCode GetTileCode(int x, int y)
        {
            return FixTileType(GetBorderTile(x, y) | GetDotTile(x, y));
        }
        private TileCode GetBorderTile(int x, int y)
        {
            // ???????????? ?????? ?????? ??????
            // ????????? ????????? ?????? ????????? Border ??????
            // ????????? ????????? ??????????????? Border ??????
            //    = [????????? ??????]   or [????????? ?????? ????????? ????????????]
            var right = x == MapX - 1 || IsEmptyTile(Map[y, x + 1]);
            var down = y == MapY - 1 || IsEmptyTile(Map[y + 1, x]);
            var left = x == 0 || IsEmptyTile(Map[y, x - 1]);
            var up = y == 0 || IsEmptyTile(Map[y - 1, x]);

            // true => Border ????????? ?????????
            return GetBorderType(up, down, left, right);
        }
        
        private TileCode GetDotTile(int x, int y)
        {
            // ???, ???, ???, ???, ??????, ??????, ??????, ?????? ?????? ??????
            // ????????? ????????? ?????? ????????? dot ??????
            // ??? / ??? / ??? / ??? ??? ????????? ??????, ?????? / ?????? / ?????? / ?????? ??? ????????? ??????
            
            var isRightBlockExist = x < MapX - 1 && !IsEmptyTile(Map[y, x + 1]);
            var isDownBlockExist = y < MapY - 1 && !IsEmptyTile(Map[y + 1, x]);
            var isLeftBlockExist = x > 0 && !IsEmptyTile(Map[y, x - 1]);
            var isUpBlockExist = y > 0 && !IsEmptyTile(Map[y - 1, x]);

            var downRight = isDownBlockExist && isRightBlockExist && IsEmptyTile(Map[y + 1, x + 1]);
            var downLeft = isDownBlockExist && isLeftBlockExist && IsEmptyTile(Map[y + 1, x - 1]);
            var upRight = isUpBlockExist && isRightBlockExist && IsEmptyTile(Map[y - 1, x + 1]);
            var upLeft = isUpBlockExist && isLeftBlockExist && IsEmptyTile(Map[y - 1, x - 1]);

            // true => dot ????????? ?????????
            return GetDotType(upLeft, upRight, downLeft, downRight);
        }
    }
}