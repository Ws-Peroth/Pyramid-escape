using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Mummy
{
    public class AttackState : State<Mummy>
    {
        private static readonly int TileLayer = 1 << LayerMask.NameToLayer("Tile");

        public override State<Mummy> InputHandle(Mummy t)
        {
            if (t.IsDead)
                return new DeadState();

            var col = Physics2D.OverlapBox(t.transform.position, t._searchRange, TileLayer);
            if (!col.CompareTag("Player")) return new IdleState();
            return this;
        }

        protected override void Update(Mummy t)
        {
            base.Update(t);
            
            if (t._currentAttackDelay > AttackDelay)
            {
                t._currentAttackDelay = 0;
                t.CreatPoisonSmoke();
            }
        }
    }
}
