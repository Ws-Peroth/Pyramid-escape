using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashObject : MonoBehaviour
{
    
    private const PoolCode ObjectPoolType = PoolCode.SlashSkill;
    [SerializeField] private ParticleSystem skillParticle; 
    private readonly WaitForSeconds _waitSecond = new WaitForSeconds(0.02f);
    public float Range { get; set; }
    public float Speed { get; set; }
    public float Damage { get; set; }
    public float KnockBack { get; set; }
    
    private Vector3 _skillDirection;
    private Coroutine _moveCoroutine;

    public void Initialize(float range, float speed, float damage, float knockBack, bool isLeft)
    {
        Range = range;
        Speed = speed;
        Damage = damage;
        KnockBack = knockBack;
        Active(isLeft);
    }
    
    private void Active(bool isLeft)
    {
        transform.rotation = Quaternion.Euler(new Vector3(0, isLeft ? 180 : 0, 0));
        skillParticle.gameObject.SetActive(true);
        skillParticle.Play();
        _moveCoroutine = StartCoroutine(Move());
    }

    private IEnumerator Move()
    {
        while (Range > 0)
        {
            var delta = Time.deltaTime;
            var move = Vector3.right * Speed * delta;
            transform.Translate(move);
            Range -= move.x;
            yield return null;
        }

        skillParticle.gameObject.SetActive(false);
        PoolManager.instance.DestroyPrefab(gameObject, ObjectPoolType);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            var enemy = other.GetComponent<Enemy>();
            enemy.GetDamage(Damage);
            enemy.GetKnockBack(KnockBack, transform.rotation.y);
        }
    }
}
