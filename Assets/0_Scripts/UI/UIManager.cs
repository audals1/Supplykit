using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GreyPanel greyPanel, smallButton, bigButton;
    public GameObject highScorePanel;
    public TMP_Text highScoreText;

    private static UIManager _instance;
    public static UIManager Instance
    {
        get
        {
            if (null == _instance) return null;
            return _instance;
        }
    }

    private void Awake()
    {
        if (null == _instance){
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public GreyPanel NewGreyPanel(GreyPanel original, Vector2 offset, Vector2 size, Transform parent)
    {
        GreyPanel newGreyPanel = Instantiate<GreyPanel>(original, parent);
        newGreyPanel.Initiate(offset, size);
        return newGreyPanel;
    }

    public void ShowHighScorePanel()
    {
        highScoreText.text = $"High Scores!\n{GameManager.Instance.score}\n";
        int[] highScores = GameManager.Instance.GetHighScore();
        for (int i = 0; i < highScores.Length; ++i)
        {
            highScoreText.text += $"{highScores[i]}\n";
        }
        highScorePanel.SetActive(true);
    }

    public void RetryBtn()
    {
        highScorePanel.SetActive(false);
        SceneManager.LoadScene("SampleScene");
    }

    public void QuitBtn()
    {
        highScorePanel.SetActive(false);
        SceneManager.LoadScene("TitleScene");
    }
}
