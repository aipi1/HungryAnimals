using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base abstract class for Enemy, Player, Food
/// </summary>
public abstract class GameCharacter : MonoBehaviour, IMovable
{
    public virtual float Speed
    {
        get;
        protected set;
    }
    public Animator characterAnimator;
    protected Rigidbody characterRb;
    protected float xRangeBound;
    protected float zTopBound;
    protected float zBotBound;

    public virtual void Move(Vector3 direction)
    {
        characterRb.velocity = direction * Speed;
    }

    protected abstract void CheckMapBounds();
}
