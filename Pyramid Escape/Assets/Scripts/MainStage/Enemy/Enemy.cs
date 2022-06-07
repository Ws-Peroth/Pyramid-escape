using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    public SpriteOptimizer optimizeObject;
    public virtual void Initialize()
    {
        throw new NotImplementedException();
    }

    public void RemoveSpawnMonster()
    {
        var index = optimizeObject.spawnMonster.FindIndex(x => gameObject);
        optimizeObject.spawnMonster.RemoveAt(index);
    }
}
