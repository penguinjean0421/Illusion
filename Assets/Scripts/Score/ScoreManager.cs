using UnityEngine;
public class ScoreManager : MonoBehaviour
{
    public GameObject textEffectPrefab;
    public Transform canvasTransform;

    public void ShowScore(Vector3 worldPosition, int score)
    {

        // 화면 좌표로 변환
        Vector3 screenPos = Vector3.zero; // (위치 사이드로 변경 요망)

        // UI 생성
        GameObject obj = Instantiate(textEffectPrefab, screenPos, Quaternion.identity, canvasTransform);

        // 텍스트 넣기
        obj.GetComponent<EffectText>().SetText("+" + score);
    }
}