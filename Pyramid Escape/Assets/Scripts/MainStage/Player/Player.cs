using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
/*
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;
*/
public class Player : MonoBehaviour
{
    #region 필드정의
    private const KeyCode LeftKeyCode = KeyCode.LeftArrow;
    private const KeyCode RightKeyCode = KeyCode.RightArrow;
    private const KeyCode UpKeyCode = KeyCode.UpArrow;

    [SerializeField] private bool isGround;
    [SerializeField] private bool isWall;
    [SerializeField] private bool isCeil;
    [SerializeField] private bool isHold;
    [SerializeField] private int holdDirection;
    
    [SerializeField] private Sprite playerSprite;
    [SerializeField] private SpriteRenderer playerSpriteRenderer;
    [SerializeField] private Light2D visionLight;
    [SerializeField] private Rigidbody2D _rigidbody2D;

    [SerializeField] private float radius = 0.2f;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpPower;
    [SerializeField] private float wallJumpPower;
    
    [SerializeField] private float maxDashCoolTime;
    [SerializeField] private float currentDashCool;
    [SerializeField] private float dashPower;
    [SerializeField] private float dashingTime;
    
    public bool canControl;

    private float spriteX;
    private float spriteY;
    #endregion

    private void Start()
    {
        spriteX = playerSprite.bounds.size.x;
        spriteY = playerSprite.bounds.size.y;
    }

    public void SetLight(bool status) => visionLight.gameObject.SetActive(status);
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        
        var currentPosition = transform.position;
        var position = new Vector3(currentPosition.x, currentPosition.y - spriteY / 2, 0);
        Gizmos.DrawSphere(position, radius);
        
        var positionX = playerSpriteRenderer.flipX ? -spriteY / 2 : spriteY / 2;
        position = new Vector2(currentPosition.x + positionX, currentPosition.y);
        Gizmos.DrawSphere(position, radius);
    }

    private void CheckStatus()
    {
        var layer = 1 << LayerMask.NameToLayer("Tile");
        var currentPosition = transform.position;
        CheckIsGround(layer, currentPosition);
        CheckIsWall(layer, currentPosition);
        CheckIsCeil(layer, currentPosition);
        CheckIsHold();
    }

    private void CheckIsGround(int layer, Vector3 currentPosition)
    {
        var position =new Vector2(currentPosition.x, currentPosition.y - spriteY / 2);
        var col = Physics2D.OverlapCircle(position, radius, layer);
        isGround = !(col is null);
    }

    private void CheckIsWall(int layer, Vector3 currentPosition)
    {
        var positionX = playerSpriteRenderer.flipX ? -spriteY / 2 : spriteY / 2;
        var position = new Vector2(currentPosition.x + positionX, currentPosition.y);
        var col = Physics2D.OverlapCircle(position, radius, layer);
        isWall = !(col is null);
    }
    private void CheckIsCeil(int layer, Vector3 currentPosition)
    {
        var position = new Vector2(currentPosition.x, currentPosition.y + spriteY / 2);
        var col = Physics2D.OverlapCircle(position, radius, layer);
        isCeil = !(col is null);
    }

    private void CheckIsHold()
    {
        isHold = isWall && Input.GetKey(KeyCode.Space);

        var v = _rigidbody2D.velocity;
        _rigidbody2D.gravityScale = isHold ? 0.1f : 1;
        _rigidbody2D.velocity = isHold ? Vector2.zero : v;
        isGround = isHold || isGround;

        if (isHold)
        {
            holdDirection = playerSpriteRenderer.flipX ? -1 : 1;
        }
    }
    
    private void FixedUpdate()
    {
        CheckStatus();
        Move();
        Jump();
        WallJump();
    }

    private void Move()
    {
        if (Input.GetKey(LeftKeyCode))
        {
            playerSpriteRenderer.flipX = true;
            transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
        }

        if (Input.GetKey(RightKeyCode))
        {
            playerSpriteRenderer.flipX = false;
            transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
        }
    }

    private void Jump()
    {
        if(isCeil) return;
        if (!isGround) return;
        if(isHold) return;
        if (Input.GetKey(UpKeyCode))
        {
            print("jump");
            _rigidbody2D.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
        }
    }
    
    private void WallJump()
    {
        if(isCeil) return;
        if(!isHold) return;
        if (Input.GetKey(UpKeyCode))
        {
            var jumpDirection = 0;
            
            if (Input.GetKey(KeyCode.RightArrow))
            {
                jumpDirection = 1;
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                jumpDirection = -1;
            }

            if(jumpDirection == 0) return;
            if(jumpDirection == holdDirection) return;
            
            var direction = jumpDirection;
            _rigidbody2D.AddForce(new Vector2(0, wallJumpPower), ForceMode2D.Impulse);
        }
    }

    private void Dash()
    {
        if(isWall) return;
        
        transform.Translate(Vector3.right * dashPower);
    }
}
