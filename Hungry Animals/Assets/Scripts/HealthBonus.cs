using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles HealthBonus behaviour
/// </summary>
public class HealthBonus : GameCharacter
{
    public override float Speed
    {
        get { return speed; }
        protected set { speed = value; }
    }
    private float speed = 45.0f;

    void Update()
    {
        Rotate(Speed);
    }

    private void Rotate(float rotationSpeed)
    {
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }
}
