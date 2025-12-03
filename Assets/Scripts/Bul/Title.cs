using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Title : MonoBehaviour
{
    public GameObject ImageGameObject;
    public GameObject ButtonGameObject;
    public GameObject ButtonGameObject2;

    public void TitleButton()
    {
    // 이미지그림 및 버튼 SetActive False
        ImageGameObject.SetActive(false);
        ButtonGameObject.SetActive(false);
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
