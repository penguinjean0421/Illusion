using UnityEngine;
public class ScoreEffect : MonoBehaviour
{
    public GameObject textEffectPrefab;
    public Transform canvasTransform;

    public void ShowScore(Vector3 worldPosition, int score)
    {
        // 화면 좌표로 변환
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPosition);

        // UI 생성
        GameObject obj = Instantiate(textEffectPrefab, screenPos, Quaternion.identity, canvasTransform);

        // 텍스트 넣기
        obj.GetComponent<EffectText>().SetText("+" + score);

        Debug.Log("생성");
    }
}