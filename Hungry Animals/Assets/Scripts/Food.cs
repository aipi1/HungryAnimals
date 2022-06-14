using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles Food behavior
/// </summary>
public class Food : GameCharacter
{
    void Awake()
    {
        Speed = 40.0f;
        xRangeBound = 30.0f;
        zTopBound = 30.0f;
        zBotBound = -10.0f;
        characterRb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        CheckMapBounds();
    }

    void FixedUpdate()
    {
        Move(transform.forward);
    }

    protected override void CheckMapBounds()
    {
        Vector3 foodPos = transform.position;

        if (foodPos.z > zTopBound || foodPos.z < zBotBound)
        {
            gameObject.SetActive(false);
        }
        else if (foodPos.x < -xRangeBound || foodPos.x > xRangeBound)
        {
            gameObject.SetActive(false);
        }
    }
}
