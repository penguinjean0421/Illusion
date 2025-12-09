using UnityEngine;
using System.Collections;

public class FadeEffect : MonoBehaviour
{
    // Canvas Group 컴포넌트를 연결할 변수
    public CanvasGroup DarkGroup;
    public CanvasGroup TextGroup;

    // 효과가 완료될 때까지 걸리는 시간 (초)
    public float FadeDuration = 3.0f;

    // 버튼 클릭 이벤트에 연결될 함수
    public void StartGameOverEffect()
    {
        // 두 개의 페이드인 코루틴을 동시에 시작합니다.
        StartCoroutine(FadeInGroup(DarkGroup, FadeDuration));
        StartCoroutine(FadeInGroup(TextGroup, FadeDuration));
    }

    IEnumerator FadeInGroup(CanvasGroup group, float duration)
    {
        // Inspector 연결 여부 확인
        if (group == null) { yield break; }

        group.alpha = 0f;

        float time = 0f;

        while (time < duration)
        {
            time += Time.unscaledDeltaTime;
            float t = time / duration;

            // Lerp를 사용하여 알파 값을 0에서 1로 변화시킵니다.
            group.alpha = Mathf.Lerp(0f, 1f, t);

            yield return null;
        }

        // 완료 후 최종 알파 값을 1로 고정합니다.
        group.alpha = 1f;
    }
}