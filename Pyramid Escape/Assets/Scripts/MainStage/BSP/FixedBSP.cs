using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedBSP : MapInitializer
{

    #region 필드 변수 정의

    private const int BorderSizeX = 2;
    private const int BorderSizeY = 2;
    private const int WideMax = 3;
    private static int RoomDivideRatio => Random.Range(4, 7);
    private Tile GetTileNumber => IsDebugMode ? Tile.GreenBlock : Tile.VoidTile; // 디버그모드 : 통로 표시용 스프라이트
    
    #endregion

    private static int GetBorderLimit(int value) => value > 1 ? value : 1;

    protected void GenerateMapBsp()
    {
        DivideMap(10, 0, 0, MapX, MapY);
    }

    private MapLocationPosition DivideMap(int depth, int startX, int startY, int endX, int endY)
    {
        var lengthX = endX - startX;
        var lengthY = endY - startY;

        if (depth == 0 || lengthX <= 10 || lengthY <= 10)
        {
            return GenerateRoom(startX, startY, endX, endY);
        }

        MapLocationPosition firstRoom, secondRoom;

        int divideNum;

        // X의 길이가 더 길 경우 세로방향으로 분할
        if (lengthX > lengthY)
        {
            divideNum = RoomDivideRatio * lengthX / 10;
            firstRoom = DivideMap(depth - 1, startX, startY, startX + divideNum, endY);
            secondRoom = DivideMap(depth - 1, startX + divideNum, startY, endX, endY);
            MergeVerticalRoom(firstRoom, secondRoom);
        }
        else // 가로 방향으로 분할
        {
            divideNum = RoomDivideRatio * lengthY / 10;
            firstRoom = DivideMap(depth - 1, startX, startY, endX, startY + divideNum);
            secondRoom = DivideMap(depth - 1, startX, startY + divideNum, endX, endY);
            MergeHorizonRoom(firstRoom, secondRoom);
        }

        return new MapLocationPosition(firstRoom.StartX, firstRoom.StartY, firstRoom.EndX, firstRoom.EndY);
    }

    private MapLocationPosition GenerateRoom(int startX, int startY, int endX, int endY)
    {
        // (startX, startY) ~ (endX, endY) 의 구역 중, border만큼을 띄우고 방을 생성
        for (var y = startY + BorderSizeY; y < endY - BorderSizeY; y++)
        {
            for (var x = startX + BorderSizeX; x < endX - BorderSizeX; x++)
            {
                Map[y, x] = Tile.VoidTile;
            }
        }

        // 실제 방의 생성된 위치를 반환함
        return new MapLocationPosition
        (
            startX + BorderSizeX,
            startY + BorderSizeY,
            endX - BorderSizeX - 1,
            endY - BorderSizeY - 1
        );
    }

    private void MergeVerticalRoom(MapLocationPosition firstRoom, MapLocationPosition secondRoom)
    {
        var pathWide = GetRandomVerticalWide(0, WideMax, firstRoom.StartY, firstRoom.EndY);
        ConnectVerticalRoom(firstRoom.EndX, secondRoom.StartX, firstRoom.StartY, pathWide);

        pathWide = GetRandomVerticalWide(0, WideMax, firstRoom.StartY, firstRoom.EndY);
        ConnectVerticalRoom(firstRoom.EndX, secondRoom.StartX, firstRoom.EndY, pathWide);
    }

    private int GetRandomVerticalWide(int min, int max, int startLimit, int endLimit, int defaultValue = 0)
    {
        var pathWide = defaultValue;
        var borderLimit = GetBorderLimit(BorderSizeY / 2);

        for (var i = 0; i < 3; i++)
        {
            pathWide = Random.Range(min, max + 1);

            // 통로 범위가 map을 초과하는지 검사
            // 통로 범위가 map을 초과하면 기본값을 대입
            if (startLimit - (pathWide / 2) <= borderLimit)
            {
                pathWide = defaultValue;
            }
            else if (endLimit + (pathWide / 2) >= MapY - borderLimit)
            {
                pathWide = defaultValue;
            }
        }

        return pathWide;
    }

    private void ConnectVerticalRoom(int start, int end, int lockedPosition, int connectWide = 0)
    {
        var tileNum = GetTileNumber;
        var pathWide = connectWide / 2;

        // 두 구역을 통로로 연결
        for (var x = start; x < end; x++)
        {
            for (var y = lockedPosition - pathWide; y <= lockedPosition + pathWide; y++)
            {
                Map[y, x] = tileNum;
            }
        }
    }

    private void MergeHorizonRoom(MapLocationPosition firstRoom, MapLocationPosition secondRoom)
    {
        var pathWide = GetRandomHorizonWide(0, WideMax, firstRoom.StartX, firstRoom.EndX);
        ConnectHorizonRoom(firstRoom.EndY, secondRoom.StartY, firstRoom.StartX, pathWide);

        pathWide = GetRandomHorizonWide(0, WideMax, firstRoom.StartX, firstRoom.EndX);
        ConnectHorizonRoom(firstRoom.EndY, secondRoom.StartY, firstRoom.EndX, pathWide);
    }

    private int GetRandomHorizonWide(int min, int max, int startLimit, int endLimit, int defaultValue = 0)
    {
        var wide = defaultValue;
        var borderLimit = GetBorderLimit(BorderSizeX / 2);

        for (var i = 0; i < 3; i++)
        {
            // 임의의 통로 범위 설정
            wide = Random.Range(min, max + 1);

            // 임의의 통로 범위가 map을 초과하는지 검사
            // 통로 범위가 map을 초과하면 기본값을 대입
            if (startLimit - (wide / 2) <= borderLimit)
            {
                wide = defaultValue;
            }
            else if (endLimit + (wide / 2) >= MapX - borderLimit)
            {
                wide = defaultValue;
            }
        }

        return wide;
    }

    private void ConnectHorizonRoom(int start, int end, int lockedPosition, int connectWide = 0)
    {
        var tileNum = GetTileNumber;
        var pathWide = connectWide / 2;

        // 두 구역을 통로로 연결
        for (var y = start; y < end; y++)
        {
            for (var x = lockedPosition - pathWide; x <= lockedPosition + pathWide; x++)
            {
                Map[y, x] = tileNum;
            }
        }
    }
}