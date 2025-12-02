using System.Collections;
using UnityEngine;

public class Ball : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
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


