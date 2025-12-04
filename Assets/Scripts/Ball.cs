using UnityEngine;
using UnityEngine.SceneManagement;

public class Ball : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Dead":
                
                if (SceneManager.GetActiveScene().name != "Tutorial")
                {
                    GameManager.instance.GameEnd();
                }
                else if(SceneManager.GetActiveScene().name == "Tutorial")
                {
                    this.gameObject.transform.position = GameManager.instance.startPos.transform.position;
                }
                break;

            case "Bouncer":
                GameManager.instance.UpdateScore(10, 1);

                break;

            case "Point":
                GameManager.instance.UpdateScore(20, 1);

                break;

            case "Side":
                GameManager.instance.UpdateScore(10, 0);

                break;

            case "Flipper":
                GameManager.instance.multiplier = 1;
                break;

            default:
                break;
        }
    }
}
