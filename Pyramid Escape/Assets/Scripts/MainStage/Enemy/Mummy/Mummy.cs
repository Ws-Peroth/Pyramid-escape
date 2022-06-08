using System;using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public partial class Mummy : Enemy
{
    private const float HpBios = 0.1f;
    private const float AttackBios = 0.1f;
    private const int DefaultHp = 5;
    private const int DefaultAttack = 1;
    
    [SerializeField] private Sprite mummySprite;
    [SerializeField] private SpriteRenderer mummySpriteRenderer;
    [SerializeField] private bool isWall;
    [SerializeField] private bool isEndOfTile;
    
    private const float IdleMoveSpeed = 2f;
    private const float AttackDelay = 2.5f;
    
    private State<Mummy> _mummyState;
    private Vector2 _searchRange;
    private int _tileLayer;
    private float _spriteX;
    private float _spriteY;
    [SerializeField] private float _currentAttackDelay = AttackDelay;
    
    public override void Initialize()
    {
        _tileLayer = 1 << LayerMask.NameToLayer("Tile");
        mummySpriteRenderer.flipX = Random.Range(0, 2) == 0;
        _spriteX = mummySprite.bounds.size.x;
        _spriteY = mummySprite.bounds.size.y;
        _searchRange = new Vector2(_spriteX + 1, _spriteY + 1);
        IsDead = false;
        
        Hp = DefaultHp;
        Hp += DefaultHp * Level * HpBios;
        
        Attack = DefaultAttack;
        Attack += DefaultAttack * Level * AttackBios;
        
        _mummyState = new IdleState();
    }

    private void Update()
    {
        AttackDelayTime();
        var nowState = _mummyState.InputHandle(this);
        _mummyState.action(this);

        if (!nowState.Equals(_mummyState))
        {
            _mummyState = nowState;
        }
    }
    private void AttackDelayTime()
    {
        _currentAttackDelay += Time.deltaTime;
    }

    private void CreatPoisonSmoke()
    {
        var poisonSmoke = PoolManager.instance.CreatPrefab(PoolCode.PoisonSmoke);
        var currentPosition = transform.position;
        poisonSmoke.transform.position =
            new Vector2(
                currentPosition.x,
                currentPosition.y
            );
        poisonSmoke.SetActive(true);
        var smokeScript = poisonSmoke.GetComponent<PoisonSmoke>();
        smokeScript.Damage = Attack;
        smokeScript.PoisonSmokeOn();
    }

    public override void GetDamage(float damage)
    {
        base.GetDamage(damage);
        print($"Attack : {damage}");
        Hp -= damage;
        if (Hp <= 0)
        {
            IsDead = true;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0.5f, 0.7f, 1, 0.5f);
         
        const float radius = 0.2f;
        var spriteX = _spriteX;
        var spriteY = _spriteY;
        var positionX = mummySpriteRenderer.flipX ? -spriteX / 2 : spriteX / 2;
        var currentPosition = transform.position;
        
        var position = new Vector2(currentPosition.x + positionX, currentPosition.y);
        Gizmos.DrawSphere(position, radius);
            
        position =new Vector2(currentPosition.x + positionX, currentPosition.y - spriteY / 2);
        Gizmos.DrawSphere(position, radius);
        
        Gizmos.DrawCube(currentPosition, _searchRange);
    }
}
