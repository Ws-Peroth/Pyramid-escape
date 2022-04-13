using System.Collections;
using UnityEngine;

public class TileMapModifier : TileMapGenerator
{
    public int loopTime = 100;

    protected override void Start()
    {
        base.Start();
        print($"Enum Tile count : Max = {((int) Tile.Max).ToString()}");
        
        if (IsDebugMode)
        {
            StartCoroutine(RepeatGenerateTileMap());
        }
        else
        {
            InitializeMapData();
            GenerateTileMapObject();
            StartCoroutine(ModifyTiles());

        }
    }

    private IEnumerator RepeatGenerateTileMap()
    {
        var mapNumber = 1;
        while (true)
        {
            InitializeMapData();
            GenerateTileMapObject();

            yield return new WaitForSeconds(1.5f);

            if (IsDebugMode)
            {
                ModifyFilledTiles();
            }

            print($"End [{mapNumber}]");
            mapNumber++;

            yield return new WaitForSeconds(2f);

            if (loopTime == mapNumber) yield break;
        }
    }

    private void ModifyFilledTiles()
    {
        // Debug Mode Only
        var blankTile = Tile.VoidTile;
        var debugTile = Tile.GreenBlock;

        for (var y = 0; y < MapY; y++)
        {
            for (var x = 0; x < MapX; x++)
            {
                var tileKind = Map[y, x];

                if (tileKind != debugTile) continue;

                Map[y, x] = blankTile;
                tileMapObjects[y, x].GetComponent<SpriteRenderer>().sprite = tileSprites[(int) blankTile];
            }
        }
    }

    private IEnumerator ModifyTiles()
    {
        for (var y = 0; y < MapY; y++)
        {
            for (var x = 0; x < MapX; x++)
            {
                if (IsEmptyTile(Map[y, x])) continue;
                
                var tileType = GetBorderTile(x, y);
                Map[y, x] = tileType;
                tileMapObjects[y, x].GetComponent<SpriteRenderer>().sprite = tileSprites[(int) tileType];
            }

            yield return null;
        }

        yield break;
    }

    private Tile GetBorderTile(int x, int y)
    {
        // 상하좌우 블럭 유무 체크
        var up = false;
        var down = false;
        var left = false;
        var right = false;

        // 죄표의 끝이면 해방 방향에 Border 생성
        // 방향에 블럭이 비어있으면 Border 생성
        //    = [좌표의 끝임]   or [방향에 있는 블럭이 비어있음]
        right = x == MapX - 1 || IsEmptyTile(Map[y, x + 1]);
        down = y == MapY - 1 || IsEmptyTile(Map[y + 1, x]);
        left = x == 0 || IsEmptyTile(Map[y, x - 1]);
        up = y == 0 || IsEmptyTile(Map[y - 1, x]);
        
        // true => Border 생성이 필요함
        return GetBorderType(up, down, left, right);
    }

    private static bool IsEmptyTile(Tile tile) => tile == Tile.VoidTile || tile == Tile.Wall;
    
    
    private Tile GetBorderType(bool up, bool down, bool left, bool right)
    {
        // true => Border 생성이 필요함 : 해당 방향에 Border을 설치해야함
        // Border의 개수
        var borderCount = 0 + (up ? 1 : 0) + (down ? 1 : 0) + (left ? 1 : 0) + (right ? 1 : 0);
        var result = Tile.Max;

        switch (borderCount)
        {
            // Border 없음
            case 0:
                result = Tile.Full;
                break;
            // 한 방향에만 Border
            case 1:
                if (up) result = Tile.U;
                if (down) result = Tile.D;
                if (left) result = Tile.L;
                if (right) result = Tile.R;
                break;
            // 두 방향에 Border
            case 2:
                if (left)
                {
                    if (right) result = Tile.Lr;
                    if (up) result = Tile.Lu;
                    if (down) result = Tile.Ld;
                    break;
                }
                if (right)
                {
                    if (up) result = Tile.Ru;
                    if (down) result = Tile.Rd;
                    break;
                }
                if (up && down) result = Tile.Ud;
                break;
            // 세 방향에 Border
            // false인 부분을 제외한 세 부분에 Border이 있음
            case 3:
                if (!up) result = Tile.Lrd;
                if (!down) result = Tile.Lru;
                if (!left) result = Tile.Rud;
                if (!right) result = Tile.Lud;
                break;
            // 모든 방향에 Border
            case 4:
                result = Tile.BorderFull;
                break;
        }

        return result;
    }
}