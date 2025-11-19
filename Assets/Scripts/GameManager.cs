using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class GameManager : MonoBehaviour
{
  

    [SerializeField]
    GameObject ball, startButton, highScoreText, scoreText, quitButton, restartButton;

    [SerializeField]
    Text text;

    int score, highScore, minute, second;
    [SerializeField]
    float time, curTime;
    

    [SerializeField]
    Rigidbody2D left, right;

    [SerializeField]
    Vector3 startPos;

    public int multiplier;

    bool canPlay;

    public static GameManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        Time.timeScale = 1;
        score = 0;
        multiplier = 1;
        highScore = PlayerPrefs.HasKey("HighScore") ? PlayerPrefs.GetInt("HighScore") : 0;
        highScoreText.GetComponent<Text>().text = "HighScore : " + highScore;
        canPlay = false;
    }

    private void Update()
    {
        if (!canPlay) return;
        if (Input.GetKey(KeyCode.A))
        {
            left.AddTorque(25f);
        }
        else
        {
            left.AddTorque(-20f);
        }
        if (Input.GetKey(KeyCode.D))
        {
            right.AddTorque(-25f);
        }
        else
        {
            right.AddTorque(20f);
        }

    }

    public void UpdateScore(int point, int mullIncrease)
    {
        multiplier += mullIncrease;
        score += point * multiplier;
        scoreText.GetComponent<Text>().text = "Score : " + score;
    }

    public void GameEnd()
    {
        Time.timeScale = 0;
        highScoreText.SetActive(true);
        quitButton.SetActive(true);
        restartButton.SetActive(true);
        if (score > highScore)
        {
            PlayerPrefs.SetInt("HighScore", score);
            highScore = score;
        }
        highScoreText.GetComponent<Text>().text = "HighScore : " + highScore;
    }

    public void GameStart()
    {
        highScoreText.SetActive(false);
        startButton.SetActive(false);
        scoreText.SetActive(true);
        Instantiate(ball, startPos, Quaternion.identity);
        canPlay = true;
        StartCoroutine(StartTimer());
    }

    public void GameQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

    public void GameRestart()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    #region TimeCoroutine
    IEnumerator StartTimer()
    {
        curTime = time;
        while(curTime > 0)
        {
            curTime -= Time.deltaTime;
            minute = (int)curTime / 60;
            second = (int)curTime % 60;
            text.text = minute.ToString("00") + ":" + second.ToString("00");
            yield return null;

            if(curTime <= 0)
            {
                Debug.Log("라운드 종료");
                curTime = 0;
                yield break;
            }
        }
    }
    #endregion
}
