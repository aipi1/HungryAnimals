using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls what happens when difficulty button is pressed
/// </summary>
public class DifficultyButton : MonoBehaviour
{
    [SerializeField] private float enemyRepeatRate;
    [SerializeField] private float healthRepeatRate;
    private Button difficultyButton;
    private GameManager gameManager;

    void Awake()
    {
        difficultyButton = GetComponent<Button>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        difficultyButton.onClick.AddListener(SetDifficulty);
    }

    private void SetDifficulty()
    {
        SpawnManager.enemyRepeatRate = enemyRepeatRate;
        SpawnManager.healthRepeatRate = healthRepeatRate;
        gameManager.StartGame();
    }
}
