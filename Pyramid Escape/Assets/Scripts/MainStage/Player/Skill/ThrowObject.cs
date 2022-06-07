using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowObject : MonoBehaviour
{
    private const PoolCode ObjectPoolType = PoolCode.ThrowSwordSkill;
    public float Range { get; set; }
    public float Speed { get; set; }
    public float Damage { get; set; }
    public float KnockBack { get; set; }

    private Vector3 _skillDirection;
    private Coroutine _moveCoroutine;

    public void Initialize(float range, float speed, float damage, float knockBack)
    {
        Range = range;
        Speed = speed;
        Damage = damage;
        KnockBack = knockBack;
        Active();
    }

    private void Active()
    {
        _moveCoroutine = StartCoroutine(Move());
    }

    private IEnumerator Move()
    {
        while (Range > 0)
        {
            var delta = Time.deltaTime;
            var move = new Vector3(1, 1, 0) * Speed * delta;
            transform.Translate(move);
            Range -= move.y;
            yield return null;
        }

        PoolManager.instance.DestroyPrefab(gameObject, ObjectPoolType);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // if hit
        if (other.CompareTag("Tile"))
        {
            StopCoroutine(_moveCoroutine);
            PoolManager.instance.DestroyPrefab(gameObject, ObjectPoolType);
        }

        if (other.CompareTag("Enemy"))
        {
            var enemy = other.GetComponent<Enemy>();
            var rotation = transform.rotation;
            enemy.GetKnockBack(KnockBack, rotation.y + rotation.z);
            enemy.GetDamage(Damage);
        }
    }
}
