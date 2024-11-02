using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class HUDManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI gameTimerText;
    public TextMeshProUGUI ghostScaredTimerText;

    private float gameTime;
    private int score;
    private float ghostScaredTime;
    private bool ghostScaredActive = false;

    void Start()
    {
        score = 0;
        gameTime = 0;
        UpdateScore(0);
        UpdateGameTimer(0);
        ghostScaredTimerText.gameObject.SetActive(false); // Start invisible
    }

    void Update()
    {
        gameTime += Time.deltaTime;
        UpdateGameTimer(gameTime);

        if (ghostScaredActive)
        {
            ghostScaredTime -= Time.deltaTime;
            ghostScaredTimerText.text = Mathf.CeilToInt(ghostScaredTime).ToString();

            if (ghostScaredTime <= 0)
            {
                ghostScaredActive = false;
                ghostScaredTimerText.gameObject.SetActive(false);
            }
        }
    }

    public void UpdateScore(int value)
    {
        score += value;
        scoreText.text = "Score: " + score;
    }

    public void StartGhostScaredTimer(float duration)
    {
        ghostScaredTime = duration;
        ghostScaredActive = true;
        ghostScaredTimerText.gameObject.SetActive(true);
    }

    private void UpdateGameTimer(float time)
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(time);
        gameTimerText.text = string.Format("{0:D2}:{1:D2}:{2:D2}",
            timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds / 10);
    }
}
