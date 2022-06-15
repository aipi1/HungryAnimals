using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

/// <summary>
/// Handles game flow and save/load of bestScore to a json file
/// </summary>
public class GameManager : MonoBehaviour
{
    public static bool isGameInProgress = false;
    public static bool isFirstGame = true;
    public static bool isGamePaused = false;
    [SerializeField] private AudioClip buttonSound;
    [SerializeField] private GameObject spawnManager;
    private Player player;
    private SoundManager soundManager;
    private UIManager uIManager;
    private IEnumerator updateScore;
    private float scoreUpdateDelay = 1.0f;
    private int score;
    private int bestScore = 0;
    private int maxScore = 1000000;
    private bool isQuitPressed = false;

    void Awake()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        uIManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        // When QuitButton is pressed from PauseScreen
        Time.timeScale = 1;
        isGamePaused = false;
    }

    void Update()
    {
        if (player.isDead && isGameInProgress)
        {
            GameOver();
        }
    }

    public void StartGame()
    {
        isGameInProgress = true;
        score = 0;
        soundManager.PlaySound(buttonSound);
        soundManager.PlayMusic();
        DestroyAllEnemies();
        spawnManager.SetActive(true);
        updateScore = UpdateScore();
        StartCoroutine(updateScore);
        uIManager.HideMainMenuUI();
        uIManager.ShowGameplayUI(score);
    }

    private void GameOver()
    {
        isGameInProgress = false;
        soundManager.StopMusic();
        spawnManager.SetActive(false);
        StopCoroutine(updateScore);
        uIManager.HideGameplayUI();
        LoadBestScore();
        if (score > bestScore)
        {
            SaveBestScore();
            uIManager.ShowGameOverUI(score, score);
        }
        else
        {
            uIManager.ShowGameOverUI(score, bestScore);
        }
    }

    public void RetryPressed()
    {
        ResetPlayer();
        StartGame();
        uIManager.HideGameOverUI();
    }

    public void QuitPressed()
    {
        if (!isQuitPressed)
        {
            // Make sure the whole sound is played before proceeding
            StartCoroutine(QuitAfterSound());
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        isGamePaused = true;
        soundManager.PlaySound(buttonSound);
        soundManager.PauseMusic();
        uIManager.ShowPauseMenu();
    }

    public void UnpauseGame()
    {
        Time.timeScale = 1;
        isGamePaused = false;
        soundManager.PlaySound(buttonSound);
        soundManager.PlayMusic();
        uIManager.HidePauseMenu();
    }

    private IEnumerator UpdateScore()
    {
        while (score < maxScore)
        {
            yield return new WaitForSeconds(scoreUpdateDelay);
            score += (int)SpawnManager.healthRepeatRate;
            uIManager.UpdateScoreText(score, maxScore);
        }
    }

    private IEnumerator QuitAfterSound()
    {
        isQuitPressed = true;
        isGameInProgress = false;
        spawnManager.SetActive(false);
        soundManager.PlaySound(buttonSound);
        yield return new WaitWhile(() => soundManager.IsEffectsSourcePlaying());
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void DestroyAllEnemies()
    {
        var enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in enemies)
        {
            Destroy(enemy);
        }
    }

    private void ResetPlayer()
    {
        player.isDead = false;
        player.characterAnimator.Rebind();
        player.characterAnimator.Update(0f);
        player.transform.position = Vector3.zero;
        player.currentHealth = player.maxHealth;
    }

    [System.Serializable]
    private class SaveData
    {
        public int bestScore;
    }

    public void SaveBestScore()
    {
        SaveData data = new SaveData();
        data.bestScore = score;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/he_savefile.json", json);
    }

    public void LoadBestScore()
    {
        string path = Application.persistentDataPath + "/he_savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            bestScore = data.bestScore;
        }
    }
}
