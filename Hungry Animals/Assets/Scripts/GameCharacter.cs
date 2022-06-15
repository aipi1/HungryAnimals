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

    protected virtual void CheckMapBounds()
    {
        //Horizontal boundaries
        if (transform.position.x < -xRangeBound)
        {
            transform.position = new Vector3(-xRangeBound, transform.position.y, transform.position.z);
        }
        else if (transform.position.x > xRangeBound)
        {
            transform.position = new Vector3(xRangeBound, transform.position.y, transform.position.z);
        }
        //Vertical boundaries
        if (transform.position.z < zBotBound)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, zBotBound);
        }
        else if (transform.position.z > zTopBound)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, zTopBound);
        }
    }
}
