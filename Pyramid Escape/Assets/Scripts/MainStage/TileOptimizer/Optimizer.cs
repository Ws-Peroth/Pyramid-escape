using System;
using System.Collections;
using System.Collections.Generic;
using MainStage.MapMaker;
using UniRx;
using UnityEngine;

public class Optimizer : MonoBehaviour
{
    [SerializeField] private GameObject target;

    public delegate void SpriteActivator(float x1, float y1, bool generateFinish);
    private readonly ReactiveProperty<Vector3> _cameraPosition = new ReactiveProperty<Vector3>(Vector3.zero);
    [field: SerializeField] public SpriteActivator Activator { get; set; }

    private void Start()
    {
        _cameraPosition.Subscribe(position =>
        {
            CallActivator(position.x, position.y);
        });
    }

    public void CallActivator(float x, float y)
    {
        if (Activator is null)
        {
            print("Activator is null");
            return;
        }
        Activator(x, y, MapDataInitializer.instance.GenerateFinish);
    }

    private void Update()
    {
        if (!MapDataInitializer.instance.GenerateFinish) return;
        _cameraPosition.Value = target.transform.position;
    }
}
