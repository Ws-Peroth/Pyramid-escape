using System;
using System.Collections;
using System.Collections.Generic;
using MainStage.MapMaker;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class SpriteOptimizer : MonoBehaviour
{
    public const float DistanceX = 10;
    public const float DistanceY = 6;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private ShadowCaster2D shadowCaster2D;
    private Vector3 _position;

    private static float Abs(float x) => x < 0 ? -x : x;
    
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
    
    public void ActivateSprite(float x1, float y1, bool generateStatus)
    {
        if(!generateStatus) return;
        
        _position = transform.position;
        var x = _position.x - x1;
        var y = _position.y - y1;
        SetActivate(Abs(x) < DistanceX && Abs(y) < DistanceY);
    }
}
