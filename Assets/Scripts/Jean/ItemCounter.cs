using UnityEngine;
using UnityEngine.UI;

public class ItemCounter : MonoBehaviour
{
    public Text text;
    int count;

    void Start()
    {
        count = 0;
    }

    public void Count(string itemID)
    {
        count++;
        text.text = count.ToString();
    }
}