using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ball : MonoBehaviour
{
    Rigidbody2D rigid2D;

    public float maxPosSpeed = 50f;
    public float maxNegSpeed = -50f;

    // 계수 변동
    float coefficient = 0f;
    internal float coefficientMin = 0.3f;
    internal float coefficientMax = 1.3f;


    [Header("장애물별 점수")]
    internal int bouncerScore = 10;
    internal int pointScore = 20;
    internal int point2Score = 30;
    internal int sideScore = 10;
    internal int wormHole = 30;
    internal int startPoint = 0;

    void Awake()
    {
        rigid2D = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        float veloX = rigid2D.velocity.x;
        float veloY = rigid2D.velocity.y;

        // 현재 속도가 최대 속도를 초과하는지 확인 후 제어
        if (veloX > maxPosSpeed) { rigid2D.velocity = new Vector2(maxPosSpeed, rigid2D.velocity.y); }
        else if (veloX < maxNegSpeed) { rigid2D.velocity = new Vector2(maxNegSpeed, rigid2D.velocity.y); }

        if (veloY > maxPosSpeed) { rigid2D.velocity = new Vector2(rigid2D.velocity.x, maxPosSpeed); }
        else if (veloY < maxNegSpeed) { rigid2D.velocity = new Vector2(rigid2D.velocity.x, maxNegSpeed); }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.LogWarning($"velocity : {rigid2D.velocity}");

        coefficient = Random.Range(coefficientMin, coefficientMax);

        switch (collision.gameObject.tag)
        {
            case "Dead":

                if (SceneManager.GetActiveScene().name != "Tutorial")
                {
                    GameManager.instance.GameEnd();
                }
                else if (SceneManager.GetActiveScene().name == "Tutorial")
                {
                    this.gameObject.transform.position = GameManager.instance.startPos.transform.position;
                }
                break;

            case "Bouncer":
                GameManager.instance.UpdateScore(bouncerScore, coefficient);
                break;

            case "Point":
                GameManager.instance.UpdateScore(pointScore, coefficient);
                break;

            case "Point2":
                GameManager.instance.UpdateScore(point2Score, coefficient);
                break;

            case "Side":
                GameManager.instance.UpdateScore(sideScore, coefficient);
                break;

            case "Flipper":
                GameManager.instance.multiplier = 1;
                break;

            case "Blocker":
                StartCoroutine(BlockerJump());
                break;

            default:
                break;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        coefficient = Random.Range(coefficientMin, coefficientMax);

        if (collision.gameObject.tag == "Tunnel")
        {
            GameManager.instance.UpdateScore(wormHole, coefficient);
        }

        if (collision.gameObject.tag == "StartPoint")
        {
            GameManager.instance.isCanLaunched = true;
            GameManager.instance.UpdateScore(startPoint, coefficient);
        }
    }

    IEnumerator BlockerJump()
    {
        yield return new WaitForSeconds(1f);
        GameManager.instance.Launch();
        //Debug.Log("발사했어요 ㅅㅂ");
    }
}


