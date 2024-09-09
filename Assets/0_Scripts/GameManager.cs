using System.Collections;
using System.Collections.Generic;
using Actor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Singleton
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (null == _instance) return null;
            return _instance;
        }
    }
    private void Awake()
    {
        if (null == _instance)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    #endregion
    private enum GameState
    {
        Ready,
        Play,
    }
    public int score;
    private int killCount, level;
    public int[] highScore;
    private float time;
    private GameState gameState;

    public PlayerController playerPref;
    public PlayerController player;

    public Character Character; 

    private void Start()
    {
        this.score = 0;
        this.killCount = 0;
        this.level = 1;
        this.time = 0f;
        gameState = GameState.Ready;
        LoadHighScore();
    }

    private void Update()
    {
        if (Character == null)
        {
            Character = FindObjectOfType<Character>();
        }

        if (gameState == GameState.Play)
        {
            this.time += Time.deltaTime;
            level = ((int)time + killCount) / 100;
        }
    }

    public void GameStart()
    {
        gameState = GameState.Play;
        SceneManager.LoadScene("SampleScene"); //���� ���� ����
        //player = Instantiate<PlayerController>(playerPref);
    }

    public void GameOver()
    {
        gameState = GameState.Ready;
        for(int i = 0; i < highScore.Length; ++i)
        {
            if (score > highScore[i])
            {
                for (int j = highScore.Length - 1; j > i; --j)
                {
                    highScore[j] = highScore[j - 1];
                }
                highScore[i] = score;
                SaveHighScore();
            }
        }
        UIManager.Instance.ShowHighScorePanel();
    }

    public void Pause()
    {
        Time.timeScale = 0;
    }

    public void UpPause()
    {
        Time.timeScale = 1f;
    }

    private void LoadHighScore()
    {
        highScore = new int[10];
        int temp;
        for (int i = 0; i < highScore.Length; ++i)
        {
            temp = PlayerPrefs.GetInt($"HighScore{i}", 0);
            highScore[i] = temp;
        }
    }

    public int[] GetHighScore()
    {
        LoadHighScore();
        return highScore;
    }

    private void SaveHighScore()
    {
        for(int i = 0; i < highScore.Length; ++i)
        {
            PlayerPrefs.SetInt($"HighScore{i}", highScore[i]);
        }
    }
}
