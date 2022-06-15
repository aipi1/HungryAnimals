using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Manages game UI
/// </summary>
public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI finalScoreText;
    [SerializeField] private TextMeshProUGUI bestScoreText;
    [SerializeField] private GameObject mainMenuScreen;
    [SerializeField] private GameObject healthBar;
    [SerializeField] private GameObject hintText;
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private GameObject pauseScreen;

    public void HideMainMenuUI()
    {
        mainMenuScreen.SetActive(false);
    }

    public void ShowGameplayUI(int score)
    {
        if (GameManager.isFirstGame)
        {
            GameManager.isFirstGame = false;
            StartCoroutine(ShowHint());
        }
        healthBar.SetActive(true);
        scoreText.text = $"Score: {score}";
        scoreText.gameObject.SetActive(true);
    }

    public void HideGameplayUI()
    {
        hintText.SetActive(false);
        healthBar.SetActive(false);
        scoreText.gameObject.SetActive(false);
    }

    public void ShowGameOverUI(int score, int bestScore)
    {
        finalScoreText.text = $"Final score: {score}";
        bestScoreText.text = $"Best score: {bestScore}";
        gameOverScreen.SetActive(true);
    }

    public void HideGameOverUI()
    {
        gameOverScreen.SetActive(false);
    }

    public void ShowPauseMenu()
    {
        pauseScreen.SetActive(true);
    }

    public void HidePauseMenu()
    {
        pauseScreen.SetActive(false);
    }

    public void UpdateScoreText(int score, int maxScore)
    {
        if (score < maxScore)
        {
            scoreText.text = $"Score: {score}";
        }
        else
        {
            scoreText.color = new Color32(0xB2, 0x35, 0x35, 0xFF);
        }
    }

    private IEnumerator ShowHint()
    {
        hintText.SetActive(true);
        yield return new WaitForSeconds(7.0f);
        hintText.SetActive(false);
    }
}
