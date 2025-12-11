using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    public FadeEffect fadeEffect;

    public GameObject ball;
    public GameObject spawnedBall;

    public GameObject startPos;


    // Torque
    Rigidbody2D left, right;
    public float leftTorque = 1200f;
    public float rightTorque = 500f;

    //UI
    GameObject startButton;
    GameObject quitButton;
    GameObject restartButton;
    Text highScoreText, scoreText;
    Text timerText;

    // 점수 
    Text goalScoreText;
    int level;
    public int[] minScores;
    public int score;
    int highScore;

    // Charge Gauge
    public Slider chargeGauge;
    float Max = 45f;
    float Min = 21f;

    [SerializeField] float curForce;


    // 상점
    Store store;
    GameObject storeObj;
    Text goldUI;
    Text bought;

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
        Time.timeScale = 1f;
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

                curForce -= 25f * Time.deltaTime;
            }
            else if (tempPoint == -1)
            {
                curForce += 25f * Time.deltaTime;
            }

            // 💡 (슬라이더의 값이 Min/Max 범위를 벗어나지 않도록 강제로 고정)
            curForce = Mathf.Clamp(curForce, Min, Max);

            // 💡 (현재 curForce 값을 슬라이더의 value에 반영하여 UI 업데이트)
            if (chargeGauge != null) { chargeGauge.value = curForce; }
        }
    }

    public void UpdateGoalScore()
    {
        // level 변수는 현재 라운드를 나타내며, 배열 인덱스로 사용됨.
        int arrayIndex = level;

        if (arrayIndex >= 0 && arrayIndex < minScores.Length)
        {
            int goalScore = minScores[arrayIndex];

            // Round 1 Goal: 300 와 같이 표시
            goalScoreText.text = $"Round {level + 1} Goal: {goalScore}";
        }
        else
        {
            // 배열 범위를 벗어났을 때의 처리 (모든 라운드 완료)
            goalScoreText.text = "All Rounds Completed!";
        }
    }

    void Update()
    {
        //if(SceneManager.GetActiveScene().name == "Tutorial")
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                EndTutorial();
            }
        }

        if (!isCanPlay) { return; }

        Slider(); // 매 프레임마다 Slider 게이지 충전/방전 로직을 실행하도록 호출

        if (Input.GetKeyUp(KeyCode.Space) && isCanLaunched)
        {
            Launch();
            isCanLaunched = false;
        }

        if (Input.GetKey(KeyCode.A)) { left.AddTorque(leftTorque); }
        else { left.AddTorque(-rightTorque); }

        if (Input.GetKey(KeyCode.L)) { right.AddTorque(-leftTorque); }
        else { right.AddTorque(rightTorque); }

        if (Input.GetKey(KeyCode.LeftShift)) { Cheat(); }
    }

    public void UpdateScore(int point, float mullIncrease)
    {
        // multiplier = mullIncrease;
        score += (int)(point * mullIncrease);
        scoreText.text = $"Score : {score}";
        Debug.Log($"multiplier : {multiplier}");
    }

    public void GameEnd()
    {
        Time.timeScale = 0f;

        if (curTime > 0 || score < minScores[level])
        {
            GameOver();
            Destroy(spawnedBall);
        }
        else
        {
            storeObj.SetActive(true);
            store.SetupShopUI();
        }
    }

    void GameOver()
    {
        isCanPlay = false;

        if (fadeEffect != null) { fadeEffect.StartGameOverEffect(); }

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
        // curTime = time[level]; // Lv마다 다르게 할거면 이걸로 변경

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
                GameEnd();
                yield break;
            }
        }
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
        UpdateGoalScore();
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

        // 다시 시작 누르면 씬 재로딩 하지말고 게임이 다시 시작되게 해도 될거 같은데 님들 생각은 어떰?
        /* 
        highScoreText.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false);
        level = 0;
        score = 0;
        spawnedBall = Instantiate(ball, startPos.transform.position, Quaternion.identity);
        Time.timeScale = 1f;
        isCanPlay = true;
        StartCoroutine(StartTimer()); 
        */
    }

    public void StoreClose()
    {
        level++;
        spawnedBall.transform.position = startPos.transform.position;
        storeObj.SetActive(false);
        UpdateGoalScore();
        StartCoroutine(StartTimer());
    }

    public void Launch()
    {
        Rigidbody2D ballRb = spawnedBall.GetComponent<Rigidbody2D>();
        ballRb.AddForce(Vector2.up * curForce, ForceMode2D.Impulse);


        // 발사 후 게이지 초기 위치로 돌리기
        curForce = Min + 1f;

        // UI 초기화
        chargeGauge.value = curForce;
        Debug.Log($"Rb : {ballRb.velocity}");
    }

    public void EndTutorial()
    {
        SceneManager.LoadScene("Title");
    }
    #endregion

    #region UI
    public void MoneyUpdate(int money)
    {
        goldUI.text = $"Gold : {money} G";
    }

    public void BuyItem(string name)
    {
        bought.text = $"{name} 구매 완료";
    }

    public void BoughtItem(string itemID)
    {
        ItemCounter itemCounter = GameObject.Find(itemID).GetComponent<ItemCounter>();
        itemCounter.Count(itemID);
    }
    #endregion

    #region Initialize
    void OnInitialize()
    {
        highScoreText = GameObject.Find("HighScore").GetComponent<Text>();

        scoreText = GameObject.Find("Score").GetComponent<Text>();
        scoreText.gameObject.SetActive(false);

        timerText = GameObject.Find("Timer").GetComponent<Text>();

        goalScoreText = GameObject.Find("GoalScore").GetComponent<Text>();

        left = GameObject.Find("Left").GetComponent<Rigidbody2D>();
        right = GameObject.Find("Right").GetComponent<Rigidbody2D>();

        startButton = GameObject.Find("Start");

        quitButton = GameObject.Find("Quit");
        quitButton.gameObject.SetActive(false);

        restartButton = GameObject.Find("Restart");
        restartButton.gameObject.SetActive(false);

        storeObj = GameObject.Find("Store");
        store = storeObj.GetComponent<Store>();
        goldUI = GameObject.Find("Gold").GetComponent<Text>();
        bought = GameObject.Find("Bought").GetComponent<Text>();
        storeObj.SetActive(false);

        chargeGauge = GameObject.Find("ChargeSlider").GetComponent<Slider>();
        chargeGauge.minValue = Min;
        chargeGauge.maxValue = Max;
        chargeGauge.value = curForce;
    }
    #endregion

    #region Developer Cheat
    void Cheat()
    {
        if (Input.GetKeyDown(KeyCode.P)) { Reset(); }
        if (Input.GetKeyDown(KeyCode.O)) { AddScore(); }
        if (Input.GetKeyDown(KeyCode.Alpha9)) { ShutDown(); }
    }


    void Reset()
    {
        PlayerPrefs.DeleteKey("HighScore");
        Debug.Log("기록말살");
    }

    void AddScore()
    {
        UpdateScore(100, 1);
    }

    void ShutDown()
    {
        curTime = 0f;
    }

    #endregion
}