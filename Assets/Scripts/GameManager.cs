using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager game;

    private static bool isPaused;
    private static float score;
    private static int maxScore;

    //events
    public delegate void NewScore();
    public event NewScore OnNewMaxScore;

    public static GameManager Game { get => game; }
    public static bool IsPaused { get => isPaused; }
    public static float Score { get => score; }
    public static int MaxScore { get => maxScore; }

    //pauses the game
    public static void PauseGame(bool pause)
    {
        isPaused = pause;
        Time.timeScale = pause ? 0f : 1f;
    }

    //add to score
    public static void AddScore(int value)
    {
        score += value;
    }

    private void Awake()
    {
        game = this;
        isPaused = false;
        score = 0f;
    }

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1f;

        //setting maxScore
        if(!PlayerPrefs.HasKey("MaxScore"))
        {
            PlayerPrefs.SetInt("MaxScore", 0);
        }
        maxScore = PlayerPrefs.GetInt("MaxScore");
        
        PlayerBehavior.Instance.OnPlayerDeath += () =>
        {   
            if((int)Mathf.Round(score) > maxScore)
            {
                maxScore = (int)Mathf.Round(score);
                PlayerPrefs.SetInt("MaxScore", maxScore);

                OnNewMaxScore?.Invoke();
            }

            UiHandler.UI.SetupScorePanel();
        };
    }

    // Update is called once per frame
    void Update()
    {
        //pause
        if (Input.GetKeyDown(KeyCode.P))
        {
            PauseGame(!isPaused);
        }

        //reset
        if (PlayerBehavior.IsDead)
        {
            //Test Reload
            if (Input.GetKey(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }

        //quit
        //if (Input.GetKeyDown(KeyCode.Escape))
        //{
        //    Application.Quit();
        //}
    }

    private void FixedUpdate()
    {
        if (PlayerBehavior.IsDead)
        {
            if(Time.timeScale >= 0.2f)
                Time.timeScale = Mathf.Lerp(Time.timeScale, 0f, 0.1f);
        }
        else
        {
            score += Time.deltaTime * 10f;
        }
    }
}
