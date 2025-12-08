using UnityEngine;

public class ItemLogic : MonoBehaviour
{
    public static ItemLogic instance { get; private set; }

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

    #region Ball
    // 공 중력 변경
    public void GravaityChange(string itemName)
    {
        Rigidbody2D ballRb = GameManager.instance.spawnedBall.GetComponent<Rigidbody2D>();

        if (itemName == "중력감소") { ballRb.gravityScale -= 0.1f; }

        if (itemName == "중력증가") { ballRb.gravityScale += 0.1f; }
    }
    #endregion

    #region Score
    // 일정 장소 도착 할때 마다 점수 획득
    public void GetScoreToSpot(string itemName)
    {
        Rigidbody2D ballRb = GameManager.instance.spawnedBall.GetComponent<Rigidbody2D>();
        Vector3 ballPos = GameManager.instance.spawnedBall.transform.position;
        Vector3 startPos = GameManager.instance.spawnedBall.transform.position;

        if (itemName == "시작점오면점수드림") // 시작점 도착
        {
            if (ballPos == startPos && ballRb.velocity == Vector2.zero)
            {
                GameManager.instance.UpdateScore(10, 0);
            }
        }
    }
    #endregion
}