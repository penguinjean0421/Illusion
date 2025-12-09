using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager instance { get; private set; }

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
    }

    #region Upgrade
    // 프랍 기본 점수 강화
    public void ObjeectScoreUp(string itemID)
    {
        Ball ball = GameManager.instance.spawnedBall.GetComponent<Ball>();

        switch (itemID)
        {
            // 바운서
            case ("osuB"):
                ball.bouncerScore += 10;
                break;

            // 포인트
            case ("osuP"):
                ball.pointScore += 20;
                break;

            // // 포인트2
            // case ("osuP2"):
            //     ball.point2Score += 30;
            //     break;

            // //사이드
            // case ("osuS"):
            //     ball.sideScore += 10;
            //     break;

            default:
                break;
        }
    }

    // 터널 들어갈때 기본점수 강화
    public void InsertWormHole(string itemID)
    {
        Ball ball = GameManager.instance.spawnedBall.GetComponent<Ball>();
        if (itemID == "iwhUp") // 시작점 도착
        {
            ball.startPoint += 10;
        }

        if (itemID == "iwhDown") // 시작점 도착
        {
            ball.startPoint -= 10;
            if (ball.startPoint < 0) { ball.startPoint = 0; }
        }
    }

    // 일정 장소 도착 할때 마다 점수 획득 or 강화
    public void GetScoreToSpot(string itemID)
    {
        Ball ball = GameManager.instance.spawnedBall.GetComponent<Ball>();
        if (itemID == "gstsStartUp")
        {
            ball.startPoint += 10;
        }

        if (itemID == "gstsStartDown")
        {
            ball.startPoint -= 10;
            if (ball.startPoint < 0) { ball.startPoint = 0; }
        }
    }
    #endregion

    #region Object
    public void AddTorque(string itemID)
    {
        if (itemID == "atLeft")
        {
            GameManager.instance.leftTorque += 10f;
        }
        if (itemID == "atRight")
        {
            GameManager.instance.rightTorque += 10f;
        }
    }

    public void GravaityChange(string itemID)
    {
        Rigidbody2D ballRb = GameManager.instance.spawnedBall.GetComponent<Rigidbody2D>();

        if (itemID == "gsDown")
        {
            ballRb.gravityScale -= 0.1f;
            if (ballRb.gravityScale < 0f) { ballRb.gravityScale = 0f; }
        }

        if (itemID == "gsUp") { ballRb.gravityScale += 0.1f; }
    }

    public void CoefficientValueChange(string itemID)
    {
        Ball ball = GameManager.instance.spawnedBall.GetComponent<Ball>();
        if (itemID == "cvcMin") { ball.coefficientMin += 0.1f; }
        if (itemID == "cvcMax") { ball.coefficientMax += 0.1f; }
    }
    #endregion
}