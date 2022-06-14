using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles HealthBonus behaviour
/// </summary>
public class HealthBonus : MonoBehaviour
{
    private float rotationSpeed = 45.0f;

    void Update()
    {
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }
}
