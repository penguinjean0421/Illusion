using System.Collections;
using UnityEngine;

public class Ball : MonoBehaviour
{
    Rigidbody2D rigid2D;

    public float maxPosSpeed = 50f;
    public float maxNegSpeed = -50f;

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
        switch (collision.gameObject.tag)
        {
            case "Dead":
                GameManager.instance.GameEnd();
                break;

            case "Bouncer":
                GameManager.instance.UpdateScore(10, 0);

                break;

            case "Point":
                GameManager.instance.UpdateScore(20, 0);

                break;

            case "Point2":
                GameManager.instance.UpdateScore(30, 0);

                break;

            case "Side":
                GameManager.instance.UpdateScore(10, 0);

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
        if (collision.gameObject.tag == "Tunnel")
        {
            GameManager.instance.UpdateScore(30, 0);
        }

        if (collision.gameObject.tag == "StartPoint")
        {
            GameManager.instance.isCanLaunched = true;
        }
    }

    IEnumerator BlockerJump()
    {
        yield return new WaitForSeconds(1f);
        GameManager.instance.Launch();
        //Debug.Log("발사했어요 ㅅㅂ");
    }
}


