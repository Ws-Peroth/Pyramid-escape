using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : Entity
{
    public const int PlayerSortOrder = 2;
    [SerializeField] private int layer;
    void Start()
    {
        Hp = 50;
        GameUIManager.instance.UpdateHpUI(Hp);
        layer = 1 << LayerMask.NameToLayer("Objects");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            var col = Physics2D.OverlapBox(transform.position, new Vector2(1, 1), 0, layer);
            if (col == null)
            {
                return;
            }
            var n = col.GetComponentInChildren<ActiveObject>();
            if (n == null)
            {
                return;
            }
            n.Activate(gameObject);
        }
    }

    public override void GetDamage(float damage)
    {
        base.GetDamage(damage);
        if (GameManager.instance.IsShield)
        {
            // 실드를 키면 데미지 저항 10%
            Hp -= Hp / 10;
        }
        Hp -= damage;
        GameUIManager.instance.UpdateHpUI(Hp);


        if (Hp <= 0)
        {
            GameManager.instance.GameEnd();
        }
    }
}

