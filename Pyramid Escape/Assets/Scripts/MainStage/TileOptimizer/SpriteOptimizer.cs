using System;
using System.Collections;
using System.Collections.Generic;
using MainStage.MapMaker;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class SpriteOptimizer : MonoBehaviour
{
    public bool monsterActivateStatus; 
    public List<GameObject> spawnMonster = new List<GameObject>(); 
    public const float DistanceX = 10;
    public const float DistanceY = 6;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private ShadowCaster2D shadowCaster2D;
    private Vector3 _position;

    private void GetTileComponents()
    {
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        if (MapDataInitializer.instance.UsingViewEffect && shadowCaster2D == null)
        {
            shadowCaster2D = GetComponent<ShadowCaster2D>();
        }
    }
    private void SetActivate(bool status)
    {
        GetTileComponents();
        
        spriteRenderer.enabled = status;
        
        if(!MapDataInitializer.instance.UsingViewEffect) return;
        
        shadowCaster2D.enabled = status;
    }

    public bool IsNear(float x1, float y1)
    {
        _position = transform.position;
        var x = _position.x - x1;
        var y = _position.y - y1;
        return Calc.Abs(x) < DistanceX && Calc.Abs(y) < DistanceY;
    }
    
    public bool IsNear(float x0, float y0, float x1, float y1)
    {
        return Calc.Abs(x0 - x1) < DistanceX && Calc.Abs(y0 - y1) < DistanceY;
    }
    
    public void ActivateSprite(float x1, float y1, bool generateStatus)
    {
        if(!generateStatus) return;

        SetActivate(IsNear(x1, y1));
    }

    public void SwitchMonsterActive(float x1, float y1)
    {
        var status = false;
        foreach (var o in spawnMonster)
        {
            var position = o.transform.position;
            var temp = IsNear(position.x, position.y, x1, y1);
            status = status || temp;
            o.SetActive(temp);
        }

        monsterActivateStatus = status;
    }
}
