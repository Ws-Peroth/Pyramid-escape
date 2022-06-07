using UnityEditor;
using UnityEngine;

public partial class Mummy
{
    public class IdleState : State<Mummy>
    {
        public override State<Mummy> InputHandle(Mummy t)
        {
            if (t.isDead)
                return new DeadState();

            var col = Physics2D.OverlapBox(t.transform.position, t._searchRange, t._tileLayer);
            if (!col.CompareTag("Player")) return this;

            return new AttackState();
        }

        protected override void Update(Mummy t)
        {
            base.Update(t);
            Move(t);
        }
        
        private void Move(Mummy t)
        {
            var direction = GetDirection(t);

            t.mummySpriteRenderer.flipX = direction.x < 0;
            t.transform.Translate(direction * IdleMoveSpeed * Time.deltaTime);
        }
        private Vector2 GetDirection(Mummy t)
        {
            var reverseDirection = CheckEndOfDirection(t.transform.position, t._tileLayer, t);
            var direction = t.mummySpriteRenderer.flipX ? Vector2.left : Vector2.right;
            return reverseDirection ? -direction : direction;
        }

        private bool CheckEndOfDirection(Vector2 currentPosition, int layer, Mummy t)
        {
            const float radius = 0.2f;
            var positionX = t.mummySpriteRenderer.flipX ? -t._spriteX / 2 : t._spriteX / 2;
            
            var position = new Vector2(currentPosition.x + positionX, currentPosition.y);
            var col = Physics2D.OverlapCircle(position, radius, layer);
            t.isWall = !(col is null);
            
            position =new Vector2(currentPosition.x + positionX, currentPosition.y - t._spriteY / 2);
            col = Physics2D.OverlapCircle(position, radius, layer);
            t.isEndOfTile = col is null;

            return t.isWall || t.isEndOfTile;
        }
    }
}