using UnityEngine.UI;
using UnityEngine;
public class EffectText : MonoBehaviour
{
    public float moveSpeed = 50f;
    public float fadeSpeed = 1f;
    Text text;
    Color color;

    void Awake()
    {
        text = GetComponent<Text>();
        color = text.color;
    }

    void Update()
    {
        // 위로 이동
        transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);

        // 알파 감소 (서서히 사라짐)
        color.a -= fadeSpeed * Time.deltaTime;
        text.color = color;

        // 완전히 사라지면 삭제
        if (color.a <= 0)
            Destroy(gameObject);
    }

    public void SetText(string value)
    {
        text.text = value;
    }
}