using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Player : MonoBehaviour
{
    [SerializeField] private Light2D visionLight;
    [SerializeField] private Rigidbody2D _rigidbody2D;
    public float Speed { get; set; }
    public float JumpPower { get; set; }
    public bool Movable()
    {
        // Add Conditions
        return true;
    }
    public bool Jumpable()
    {
        // Add Conditions
        return true;
    }

    private void Move()
    {
        if(!Movable()) return;
        if (Input.GetKey(KeyCode.LeftArrow)) transform.Translate(Speed * Vector3.left);
        if (Input.GetKey(KeyCode.RightArrow)) transform.Translate(Speed * Vector3.right);  
    }

    private void Jump()
    {
        if(!Movable()) return;
        if(!Jumpable()) return;
        if (Input.GetKey(KeyCode.UpArrow)) _rigidbody2D.AddForce(JumpPower * Vector2.up, ForceMode2D.Impulse);
    }

    public void LightOn()
    {
        visionLight.gameObject.SetActive(true);
    }
    
    // Update is called once per frame
    void Update()
    {
        Move();
        Jump();
    }
}
