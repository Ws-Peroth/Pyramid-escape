using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Bet
{
    public class DeadState : State<Bet>
    {
        public override State<Bet> InputHandle(Bet t)
        {
            /*
            if (t.isDead)
                return new DeadState();

            var col = Physics2D.OverlapBox(t.transform.position, t.searchRange, t._tileLayer);
            if (!col.CompareTag("Player")) return this;
            t._targetTransform = col.transform;

            print("Attack Status");
            return new AttackState();
            */
            
            return this;
            
        }

        protected override void Update(Bet t)
        {
            /*
            t.RemoveSpawnMonster();
            PoolManager.instance.DestroyPrefab(t.gameObject, PoolCode.Bet);
            */
        }

    }
}