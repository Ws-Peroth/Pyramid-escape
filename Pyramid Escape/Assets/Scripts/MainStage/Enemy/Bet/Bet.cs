using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Bet : Enemy
{
    [SerializeField] private Sprite betSprite;
    [SerializeField] private SpriteRenderer betSpriteRenderer;
    [SerializeField] private bool isWall;
    [SerializeField] private bool isEndOfTile;
    [SerializeField] private bool isDead;

    private int _tileLayer;
    private Vector2 searchRange;
    private const float IdleMoveSpeed = 2f;
    private const float AttackDelay = 2.5f;
    private float _currentAttackDelay = AttackDelay;
    private State<Bet> _betState;
    private Transform _targetTransform;
    private float _spriteX;
    private float _spriteY;
    
    public override void Initialize()
    {
        _tileLayer = 1 << LayerMask.NameToLayer("Tile");
        betSpriteRenderer.flipX = Random.Range(0, 2) == 0;
        _spriteX = betSprite.bounds.size.x;
        _spriteY = betSprite.bounds.size.y;
        searchRange = new Vector2(_spriteX + 1, _spriteY + 1);
        isDead = false;
        _betState = new IdleState();
    }
    private void Update()
    {
        AttackDelayTime();
        var nowState = _betState.InputHandle(this);
        _betState.action(this);

        if (!nowState.Equals(_betState))
        {
            _betState = nowState;
        }
    }
    private void AttackDelayTime()
    {
        _currentAttackDelay += Time.deltaTime;
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0.5f, 0.7f, 1, 0.5f);
         
        const float radius = 0.2f;
        var spriteX = _spriteX;
        var spriteY = _spriteY;
        var positionX = betSpriteRenderer.flipX ? -spriteX / 2 : spriteX / 2;
        var currentPosition = transform.position;
        
        var position = new Vector2(currentPosition.x + positionX, currentPosition.y);
        Gizmos.DrawSphere(position, radius);
            
        position =new Vector2(currentPosition.x + positionX, currentPosition.y - spriteY / 2);
        Gizmos.DrawSphere(position, radius);
        
        Gizmos.DrawCube(currentPosition, searchRange);
    }
}
