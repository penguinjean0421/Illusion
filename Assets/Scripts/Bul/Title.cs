using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Title : MonoBehaviour
{
    public GameObject ImageGameObject;
    public GameObject ButtonGameObject;
    public GameObject ButtonGameObject2;
    
    public GameObject ButtonGameObject3;
    public void TitleButton()
    {

        ImageGameObject.SetActive(false);
        ButtonGameObject.SetActive(false);
        ButtonGameObject3.SetActive(false);
        ButtonGameObject2.SetActive(true);
    }

    public void Tutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }

    public void GamePlay()
    {
        SceneManager.LoadScene("GamePlay");
    }
}
