using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonSmoke : MonoBehaviour
{
    [SerializeField] private CircleCollider2D _particleCollider2D;
    [SerializeField] private Renderer sprite;
    [SerializeField] private ParticleSystem poisonSmokeParticle;
    public float Duration { get; set; }
    public float Damage { get; set; }

    private void Awake()
    {
        sprite.sortingOrder = Player.PlayerSortOrder + 1;
    }

    public void PoisonSmokeOn()
    {
        Duration = poisonSmokeParticle.startLifetime;
        _particleCollider2D.enabled = true;
        Invoke(nameof(ColliderActivate), Duration - 0.5f);
    }

    private void ColliderActivate() => _particleCollider2D.enabled = false;
    
    private void OnDisable()
    {
        PoolManager.instance.DestroyPrefab(gameObject, PoolCode.PoisonSmoke);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            var entity = other.GetComponent<Entity>();
            entity.GetDamage(Damage);
        }
    }
}