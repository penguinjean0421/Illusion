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
    // 공 중력 변경
    public void GravaityChange(string itemID)
    {
        Rigidbody2D ballRb = GameManager.instance.spawnedBall.GetComponent<Rigidbody2D>();

        if (itemID == "중력감소") { ballRb.gravityScale -= 0.1f; }

        if (itemID == "중력증가") { ballRb.gravityScale += 0.1f; }
    }

    // 프랍 점수 강화
    public void ObjeectScore(string itemID)
    {
        Ball ball = GameManager.instance.spawnedBall.GetComponent<Ball>();

        switch (itemID)
        {
            // 바운서
            case ("바운서"):
                ball.bouncerScore += 10;
                break;

            // 포인트
            case ("포인트"):
                ball.pointScore += 20;
                break;

            // 포인트2
            case ("포인트2"):
                ball.point2Score += 30;
                break;

            //사이드
            case ("사이드"):
                ball.sideScore += 10;
                break;

            default:
                break;
        }
    }
    #endregion

    #region Score
    // 일정 장소 도착 할때 마다 점수 획득
    public void GetScoreToSpot(string itemID)
    {
        Rigidbody2D ballRb = GameManager.instance.spawnedBall.GetComponent<Rigidbody2D>();
        Vector3 ballPos = GameManager.instance.spawnedBall.transform.position;
        Vector3 startPos = GameManager.instance.spawnedBall.transform.position;

        if (itemID == "시작점오면점수드림") // 시작점 도착
        {
            if (ballPos == startPos && ballRb.velocity == Vector2.zero)
            {
                GameManager.instance.UpdateScore(10, 0);
            }
        }
    }

    public void ObjectOnOff(string itemID)
    {

    }

    #endregion
}