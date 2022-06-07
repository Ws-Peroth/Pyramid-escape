using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public partial class Bet
{
    public class IdleState : State<Bet>
    {
        private int[] directions = new int[4];
        public override State<Bet> InputHandle(Bet t)
        {
            if (t.isDead)
                return new DeadState();

            var col = Physics2D.OverlapBox(t.transform.position, t.searchRange, t._tileLayer);
            if (!col.CompareTag("Player")) return this;
            t._targetTransform = col.transform;

            print("Attack Status");
            return new AttackState();
        }

        protected override void Update(Bet t)
        {
            base.Update(t);
            Move(t);
        }

        enum Directions
        {
            None = 0,
            Up = 1,
            Down = 2,
            Left = 4,
            Right = 8,
        }

        private void Move(Bet t)
        {

        }
    }
}
