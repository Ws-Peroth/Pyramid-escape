using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class PlayerMoveController : MonoBehaviour
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
    [SerializeField] private Rigidbody2D playerRigidbody2D;
    public float Speed { get; set; }
    public float JumpPower { get; set; }

    [SerializeField] private float radius = 0.2f;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpPower;
    [SerializeField] private float wallJumpPower;

    private float _spriteY;
    #endregion
    public void SetLight(bool status) => visionLight.gameObject.SetActive(status);
    
    private void Start()
    {
        _spriteY = playerSprite.bounds.size.y;
    }
    
    private void CheckStatus()
    {
        if (Input.GetKey(KeyCode.LeftArrow)) transform.Translate(Speed * Vector3.left);
        if (Input.GetKey(KeyCode.RightArrow)) transform.Translate(Speed * Vector3.right);  
        var layer = 1 << LayerMask.NameToLayer("Tile");
        var currentPosition = transform.position;
        CheckIsGround(layer, currentPosition);
        CheckIsWall(layer, currentPosition);
        CheckIsCeil(layer, currentPosition);
        CheckIsHold();
    }

    private void CheckIsGround(int layer, Vector3 currentPosition)
    {
        if (Input.GetKey(KeyCode.UpArrow)) playerRigidbody2D.AddForce(JumpPower * Vector2.up, ForceMode2D.Impulse);
        var position =new Vector2(currentPosition.x, currentPosition.y - _spriteY / 2);
        var col = Physics2D.OverlapCircle(position, radius, layer);
        isGround = !(col is null);
    }

    private void CheckIsWall(int layer, Vector3 currentPosition)
    {
        var positionX = playerSpriteRenderer.flipX ? -_spriteY / 2 : _spriteY / 2;
        var position = new Vector2(currentPosition.x + positionX, currentPosition.y);
        var col = Physics2D.OverlapCircle(position, radius, layer);
        isWall = !(col is null);
    }
    private void CheckIsCeil(int layer, Vector3 currentPosition)
    {
        visionLight.gameObject.SetActive(true);
        var position = new Vector2(currentPosition.x, currentPosition.y + _spriteY / 2);
        var col = Physics2D.OverlapCircle(position, radius, layer);
        isCeil = !(col is null);
    }

    private void CheckIsHold()
    {
        isHold = isWall && Input.GetKey(KeyCode.Space);

        var v = playerRigidbody2D.velocity;
        playerRigidbody2D.gravityScale = isHold ? 0.1f : 1;
        playerRigidbody2D.velocity = isHold ? Vector2.zero : v;
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
        if (!isGround) return;
        if(isCeil) return;
        if(isHold) return;
        if (Input.GetKey(UpKeyCode))
        {
            print("jump");
            playerRigidbody2D.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
        }
    }
    
    private void WallJump()
    {
        if(isCeil) return;
        if(!isHold) return;
        if (Input.GetKey(UpKeyCode))
        {
            print("wall jump");
            var jumpDirection = 0;
            
            if (Input.GetKey(RightKeyCode))
            {
                jumpDirection = 1;
            }
            else if (Input.GetKey(LeftKeyCode))
            {
                jumpDirection = -1;
            }

            if(jumpDirection == 0) return;
            if(jumpDirection == holdDirection) return;
            
            playerRigidbody2D.AddForce(new Vector2(0, wallJumpPower), ForceMode2D.Impulse);
        }
    }
}