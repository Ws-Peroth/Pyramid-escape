using UnityEngine;

public partial class Mummy
{
    public class DeadState : State<Mummy>
    {
        public override State<Mummy> InputHandle(Mummy t)
        {
            base.Update(t);
            return this;
        }

        protected override void Update(Mummy t)
        {
            base.Enter(t);
            //독가스 생성
            t.CreatPoisonSmoke();
            t.RemoveSpawnMonster();
            PoolManager.instance.DestroyPrefab(t.gameObject, PoolCode.Mummy);
        }
    }
}