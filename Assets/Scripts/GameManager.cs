using UnityEngine;
using UnityEngine.UI; // 💡 (슬라이더/UI 컴포넌트 사용을 위해 추가)
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    public FadeEffect fadeEffect;

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

    // 💡 (슬라이더 UI 컴포넌트를 에디터에서 연결하기 위해 추가)
    public Slider chargeGauge;
    float Max = 45f;
    float Min = 21f;

    [SerializeField]
    float curForce;

    // 상점
    GameObject store;

    // 점수
    int level;
    public int[] minScores;
    int score, highScore;

    // 타이머
    public float time;
    // public float[] time; // Lv마다 다르게 할거면
    int minute, second;
    float curTime;

    internal int multiplier;
    bool isCanPlay;

    public bool isCanLaunched = false;

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

    public void Slider()
    {
        if (!isCanPlay) { return; }

        if (Input.GetKey(KeyCode.Space) && isCanLaunched)
        {

            if (curForce >= Max)
            {
                // Max에 도달하면 방향을 바꿔서 힘을 감소시킴
                tempPoint = 1;
            }
            else if (curForce <= Min)
            {
                // Min에 도달하면 방향을 바꿔서 힘을 증가시킴
                tempPoint = -1;
            }

            // charge sibal nom a
            if (tempPoint == 1)
            {

                curForce -= 16f * Time.deltaTime;
            }
            else if (tempPoint == -1)
            {
                curForce += 16f * Time.deltaTime;
            }

            // 💡 (슬라이더의 값이 Min/Max 범위를 벗어나지 않도록 강제로 고정)
            curForce = Mathf.Clamp(curForce, Min, Max);

            // 💡 (현재 curForce 값을 슬라이더의 value에 반영하여 UI 업데이트)
            if (chargeGauge != null)
            {
                chargeGauge.value = curForce;
            }
        }
    }

    void Update()
    {
        if (!isCanPlay) { return; }

        // 💡 (매 프레임마다 Slider 게이지 충전/방전 로직을 실행하도록 호출)
        Slider();

        if (Input.GetKeyUp(KeyCode.Space) && isCanLaunched)
        {

            Launch();
            isCanLaunched = false;
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

        if (fadeEffect != null)
        {
            fadeEffect.StartGameOverEffect();
        }

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

    #endregion
    void LevelUp()
    {
        level++;
    }


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

    void Launch()
    {
        Rigidbody2D ballRb = spawnedBall.GetComponent<Rigidbody2D>();
        ballRb.AddForce(Vector2.up * curForce, ForceMode2D.Impulse);
        // 발사 후 게이지 초기 위치로 돌리기
        curForce = Min + 1f;

        // UI도 초기화 (Launch는 Update 바깥에서 호출될 가능성이 높으므로 명시적으로 업데이트)
        if (chargeGauge != null)
        {
            chargeGauge.value = curForce;
        }
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

        chargeGauge = GameObject.Find("ChargeSlider").GetComponent<Slider>();
        chargeGauge.minValue = Min;
        chargeGauge.maxValue = Max;
        chargeGauge.value = curForce;
    }
    #endregion

    #region Developer Cheat
    void OnReset()
    {
        PlayerPrefs.DeleteKey("HighScore");
        Debug.Log("기록말살");
    }
    #endregion
}