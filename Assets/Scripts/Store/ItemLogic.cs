using UnityEngine;

public class ItemLogic : MonoBehaviour
{
    public static ItemLogic instance { get; private set; }

    void Awake()
    {
        if (instance == null) { instance = this; }
        else { Destroy(gameObject); }
    }
}