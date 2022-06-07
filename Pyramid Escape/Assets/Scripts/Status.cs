using System;
using UnityEngine;

public abstract class State<T>
{
    public Action<T> action;
    public abstract State<T> InputHandle(T t);

    protected State()
    {
        action = Enter;
    }

    protected virtual void Enter(T t)
    {
        action = Update;
    }

    protected virtual void Update(T t)
    {
        
    }
    
    public virtual void Exit(T t)
    {

    }
}