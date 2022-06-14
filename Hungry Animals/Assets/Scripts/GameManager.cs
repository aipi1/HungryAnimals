using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.IO;

/// <summary>
/// Handles game flow, UI and save/load high score to a json
/// </summary>
public class GameManager : MonoBehaviour
{
    public static bool gameInProgress = false;
    private static bool isFirstGame = true;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI finalScoreText;
    [SerializeField] private TextMeshProUGUI bestScoreText;
    [SerializeField] private GameObject mainMenuScreen;
    [SerializeField] private GameObject healthBar;
    [SerializeField] private GameObject hintText;
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private GameObject spawnManager;
    [SerializeField] private Player player;
    [SerializeField] private AudioClip buttonSound;
    private AudioSource bgMusicSource;
    private AudioSource soundsSource;
    private IEnumerator updateScore;
    private float scoreUpdateDelay = 1.0f;
    private int score;
    private int bestScore = 0;
    private int maxScore = 1000000;
    private bool isQuitPressed = false;

    void Awake()
    {
        bgMusicSource = GameObject.Find("BgMusicSource").GetComponent<AudioSource>();
        soundsSource = GameObject.Find("SoundsSource").GetComponent<AudioSource>();
    }

    void Update()
    {
        if (player.isDead && gameInProgress)
        {
            GameOver();
        }
    }

    public void StartGame()
    {
        gameInProgress = true;
        soundsSource.PlayOneShot(buttonSound);
        bgMusicSource.Play();
        DestroyAllEnemies();
        HandleStartGameUI();
    }

    private void GameOver()
    {
        gameInProgress = false;
        bgMusicSource.Stop();
        HandleGameOverUI();
    }

    public void RetryPressed()
    {
        soundsSource.PlayOneShot(buttonSound);
        gameOverScreen.SetActive(false);
        ResetPlayer();
        StartGame();
    }

    public void QuitPressed()
    {
        if (!isQuitPressed)
        {
            // Make sure the whole sound is played before proceeding
            StartCoroutine(QuitAfterSound());
        }
    }

    private IEnumerator QuitAfterSound()
    {
        isQuitPressed = true;
        soundsSource.PlayOneShot(buttonSound);
        yield return new WaitWhile(() => soundsSource.isPlaying);
        isQuitPressed = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void HandleStartGameUI()
    {
        mainMenuScreen.SetActive(false);
        if (isFirstGame)
        {
            isFirstGame = false;
            StartCoroutine(ShowHint());
        }
        healthBar.SetActive(true);
        updateScore = UpdateScore();
        score = 0;
        scoreText.text = $"Score: {score}";
        StartCoroutine(updateScore);
        scoreText.gameObject.SetActive(true);
        spawnManager.SetActive(true);
    }

    private void HandleGameOverUI()
    {
        hintText.SetActive(false);
        spawnManager.SetActive(false);
        healthBar.SetActive(false);
        StopCoroutine(updateScore);
        scoreText.gameObject.SetActive(false);
        finalScoreText.text = $"Final score: {score}";
        LoadBestScore();
        if (score > bestScore)
        {
            SaveBestScore();
            bestScoreText.text = $"Best score: {score}";
        }
        else
        {
            bestScoreText.text = $"Best score: {bestScore}";
        }
        gameOverScreen.SetActive(true);
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

    private IEnumerator ShowHint()
    {
        hintText.SetActive(true);
        yield return new WaitForSeconds(7.0f);
        hintText.SetActive(false);
    }

    private IEnumerator UpdateScore()
    {
        while (score < maxScore)
        {
            yield return new WaitForSeconds(scoreUpdateDelay);
            score += (int)SpawnManager.healthRepeatRate;
            scoreText.text = $"Score: {score}";
        }
        scoreText.color = new Color32(0xB2, 0x35, 0x35, 0xFF);
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
