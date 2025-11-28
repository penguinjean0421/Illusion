using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine.Rendering;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    public GameObject ball;
    GameObject spawnedBall;

    public GameObject startPos;

    public ScoreManager scoreManager;

    Rigidbody2D left, right;

    //UI
    GameObject startButton;
    GameObject quitButton;
    GameObject restartButton;
    Text highScoreText, scoreText;
    Text timerText;

    // 상점
    GameObject store;

    // 점수
    int level;
    public int[] minScores;
    int score, highScore;



    float Max = 45f;
    float Min = 21f;

    [SerializeField]
    float curForce;

    // 타이머
    public float time;
    // public float[] time; // Lv마다 다르게 할거면
    int minute, second;
    float curTime;

    internal int multiplier;
    bool isCanPlay;

    public bool canLaunched = false;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        OnInitialize();
    }

    void Start()
    {
        Time.timeScale = 1;
        score = 0;
        multiplier = 1;
        highScore = PlayerPrefs.HasKey("HighScore") ? PlayerPrefs.GetInt("HighScore") : 0;
        highScoreText.text = $"HighScore : {highScore}";
        isCanPlay = false;
        curForce = Min + 1f;
    }

    int tempPoint = 1;

    void Update()
    {
        if (!isCanPlay) { return; }

        if (Input.GetKey(KeyCode.Space) && canLaunched)
        {

            
             if (curForce >= Max)
            {
                tempPoint = 1;
            }
            else if (curForce <= Min)
            {
                tempPoint = -1;
            }


            // charge sibal nom a
            if (tempPoint ==1)
            {
                
                curForce -= 16f * Time.deltaTime;
            }
            else if (tempPoint == -1)
            {
                curForce += 16f * Time.deltaTime;
            }

            

        }
        if (Input.GetKeyUp(KeyCode.Space) && canLaunched)
        {
            
            Launch();
            canLaunched = false;
        }

        if (Input.GetKey(KeyCode.A))
        {
            left.AddTorque(50f);
        }
        else
        {
            left.AddTorque(-20f);
        }

        if (Input.GetKey(KeyCode.D))
        {
            right.AddTorque(-50f);
        }
        else
        {
            right.AddTorque(20f);
        }

        if (Input.GetKeyDown(KeyCode.L)) { OnReset(); }
    }

    public void UpdateScore(int point, int mullIncrease)
    {
        multiplier += mullIncrease;
        score += point * multiplier;
        scoreText.text = $"Score : {score}";
        scoreManager.ShowScore(transform.position, point);
        Debug.Log($"multiplier : {multiplier}");
    }

    public void GameEnd()
    {
        Time.timeScale = 0f;
        Destroy(spawnedBall);

        if (curTime <= 0 && score >= minScores[level])
        {
            LevelUp();
            store.SetActive(true);
        }
        else
        {
            GameOver();
        }
    }

    void GameOver()
    {
        isCanPlay = false;

        highScoreText.gameObject.SetActive(true);
        quitButton.SetActive(true);
        restartButton.SetActive(true);

        if (score > highScore)
        {
            PlayerPrefs.SetInt("HighScore", score);
            highScore = score;
        }

        highScoreText.text = $"HighScore : {highScore}";
    }

    #region TimeCoroutine
    IEnumerator StartTimer()
    {
        curTime = time;
        /* curTime = time[level]; */ // Lv마다 다르게 할거면 이걸로 변경

        while (curTime > 0)
        {
            curTime -= Time.deltaTime;
            minute = (int)curTime / 60;
            second = (int)curTime % 60;
            timerText.text = minute.ToString("00") + ":" + second.ToString("00");
            yield return null;

            if (curTime <= 0)
            {
                Debug.Log("라운드 종료");
                curTime = 0;
                Time.timeScale = 0f;

                GameEnd();
                yield break;
            }
        }
    }

    void LevelUp()
    {
        level++;
    }
    #endregion

    #region Buttons
    public void GameStart()
    {
        highScoreText.gameObject.SetActive(false);
        startButton.SetActive(false);

        scoreText.gameObject.SetActive(true);

        spawnedBall = Instantiate(ball, startPos.transform.position, Quaternion.identity);
        isCanPlay = true;
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void StoreClose()
    {
        store.SetActive(false);
        spawnedBall = Instantiate(ball, startPos.transform.position, Quaternion.identity);
        Time.timeScale = 1f;
        StartCoroutine(StartTimer());
    }

    void OnReset()
    {
        PlayerPrefs.DeleteKey("HighScore");
        Debug.Log("기록말살");
    }

    void Launch()
    {
        Rigidbody2D ballRb = spawnedBall.GetComponent<Rigidbody2D>();
        ballRb.AddForce(Vector2.up * curForce,ForceMode2D.Impulse);
    }

    #endregion

    #region Initialize
    void OnInitialize()
    {
        highScoreText = GameObject.Find("HighScore").GetComponent<Text>();

        scoreText = GameObject.Find("Score").GetComponent<Text>();
        scoreText.gameObject.SetActive(false);

        timerText = GameObject.Find("Timer").GetComponent<Text>();

        left = GameObject.Find("Left").GetComponent<Rigidbody2D>();
        right = GameObject.Find("Right").GetComponent<Rigidbody2D>();

        startButton = GameObject.Find("Start");

        quitButton = GameObject.Find("Quit");
        quitButton.gameObject.SetActive(false);

        restartButton = GameObject.Find("Restart");
        restartButton.gameObject.SetActive(false);

        store = GameObject.Find("Store");
        store.SetActive(false);
    }
    #endregion


    
}
