using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPSCheckManager : MonoBehaviour
{
    private float _deltaTime;
    [SerializeField] private Text fpsText;

    private void Start()
    {
        // Application.targetFrameRate = 30;
    }

    void Update()
    {
        _deltaTime += (Time.unscaledDeltaTime - _deltaTime) * 0.1f;
    }

    private void OnGUI()
    {
        var time = _deltaTime * 1000.0f;
        var fps = 1.0f / _deltaTime;
        fpsText.text = $"{time:0.0} ms ({fps:0.} fps)";
    }
}