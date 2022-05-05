using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace MainStage.MapMaker
{
    public class TileObjectGenerator : MapDesigner
    {

        [field: SerializeField] 
        protected List<Sprite> tileSprites;
        protected GameObject[,] tileMapObjects;

        // Dummy Tile Object
        [SerializeField] private GameObject tile;
        private static Vector3 _worldStart;
        private float _tileSize;
        private TileCode _tileCodeKind;

        protected virtual void Start()
        {
            // 타일 생성 시작 지점 설정
            if (Camera.main is { })
            {
                var position = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height));
                _worldStart = new Vector3((int) position.x, (int) position.y, (int) position.z);
            }
        }

        protected override void InitializeMapData(int x = MapData.X, int y = MapData.Y)
        {
            base.InitializeMapData(x, y);

            // 타일 크기 설정
            _tileSize = tile.GetComponent<SpriteRenderer>().sprite.bounds.size.x;

            // 타일맵 배열 할당
            tileMapObjects = new GameObject[MapY, MapX];

            StartCoroutine(InitializeTiles());
        }

        private IEnumerator InitializeTiles()
        {
            // 타일맵 배열 초기화
            for (var y = 0; y < MapY; y++)
            {
                for (var x = 0; x < MapX; x++)
                {
                    if (Map[y, x] == TileCode.VoidTile) continue;
                    GenerateTileObject(x, y);
                }

                yield return null;
            }
        }

        private void GenerateTileObject(int x, int y)
        {
            // Dummy Tile 생성
            var newTile =
                Instantiate(tile,
                    new Vector3(_worldStart.x + (_tileSize * x), _worldStart.y - (_tileSize * y), 0),
                    Quaternion.identity,
                    transform
                );

            tileMapObjects[y, x] = newTile;
            SetupMapObject(x, y);
        }
        
        private void SetupMapObject(int x, int y)
        {
            // map에서 생성할 타일과 타일의 종류를 가져옴
            _tileCodeKind = Map[y, x];
            var targetTile = tileMapObjects[y, x];

            // 타일 종류에 맞는 Sprite 부여
            targetTile.GetComponent<SpriteRenderer>().sprite = GetSprite(GetFileName(_tileCodeKind));
            if (!IsEmptyTile(Map[y, x]))
            {
                var boxCollider = targetTile.AddComponent<BoxCollider2D>();
                boxCollider.usedByComposite = true;

                if (UsingViewEffect)
                {
                    var shadowCaster = targetTile.AddComponent<ShadowCaster2D>();
                    shadowCaster.selfShadows = false;
                    shadowCaster.castsShadows = true;
                    shadowCaster.useRendererSilhouette = true;
                }

            }
            targetTile.SetActive(false);
        }
    }
}