using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interface for movable GameCharacter
/// </summary>
public interface IMovable
{
    public float Speed { get; }

    public void Move(Vector3 direction);
}
