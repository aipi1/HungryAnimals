using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controlls HealthBar behaviour
/// </summary>
public class HealthBar : MonoBehaviour
{
    private Image healthBar;
    private Player player;
    private float maxHealth;

    void Awake()
    {
        healthBar = GetComponent<Image>();
        player = GameObject.Find("Player").GetComponent<Player>();
        maxHealth = (float)player.maxHealth;
    }

    void Update()
    {
        healthBar.fillAmount = player.currentHealth / maxHealth;
    }
}
