using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MainStage.MapMaker;
using UniRx;
using UnityEngine;

public class Optimizer : MonoBehaviour
{
    [SerializeField] private Transform target;
    private Vector3 _size = new Vector3(SpriteOptimizer.DistanceX * 2, SpriteOptimizer.DistanceY * 2, 0);
    private readonly ReactiveProperty<Vector3> _cameraPosition = new ReactiveProperty<Vector3>(Vector3.zero);

    private void Start()
    {
        _cameraPosition
            .Where(_ => MapDataInitializer.instance.GenerateFinish)
            .Subscribe(position =>
            {
                CallActivator(position.x, position.y);
            });
    }

    private void CallActivator(float x, float y)
    {
        const float xRange = SpriteOptimizer.DistanceX * 3;
        const float yRange = SpriteOptimizer.DistanceY * 3;
        
        var startX = (int) (x - xRange / 2);
        var startY = (int) (-y - yRange / 2);
        
        startX = startX < 0 ? 0 : startX;
        startY = startY < 0 ? 0 : startY;

        var endX = startX + xRange;
        var endY = startY + yRange;
        
        endX = MapDataInitializer.instance.MapX <= endX ? MapDataInitializer.instance.MapX - 1 : endX;
        endY = MapDataInitializer.instance.MapY <= endY ? MapDataInitializer.instance.MapY - 1 : endY;

        for (var i = startY; i <= endY; i++)
        {
            for (var j = startX; j <= endX; j++)
            {
                var temp = TileObjectGenerator.instance.tileMapObjects[i, j];
                if(temp ==  null) continue;
                var optimizer = temp.GetComponent<SpriteOptimizer>();
                optimizer.SwitchMonsterActive(x, y);
                
                if(MapDataInitializer.IsEmptyTile(MapDataInitializer.instance.Map[i, j])) continue;
                optimizer.ActivateSprite(x, y, MapDataInitializer.instance.GenerateFinish);
            }
        }

        _size = new Vector3(endX - startX, endY - startY, 0);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(target.position, _size);
        
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(target.position, new Vector3(SpriteOptimizer.DistanceX * 2, SpriteOptimizer.DistanceY * 2, 0));
    }
    private void Update()
    {
        if (!MapDataInitializer.instance.GenerateFinish) return;
        _cameraPosition.Value = target.position;
    }
}
