using System.Collections;
using UnityEngine;

public class Wormhole : MonoBehaviour
{
    public Wormhole destinationWormhole; // (주의) Transform 대신 Wormhole 스크립트를 직접 연결하세요
    public Transform spawnPoint;         // 공이 나올 위치 (웜홀 중심보다 약간 앞쪽에 빈 오브젝트 배치 추천)

    public float waitTime = 0.5f;
    public float launchForce = 15f; // 힘을 좀 더 세게 수정

    bool isCooldown = false; // 쿨타임 체크용

    void OnTriggerEnter2D(Collider2D other)
    {
        // 쿨타임 중이거나, 공이 아니면 무시
        if (isCooldown || !other.CompareTag("Player")) { return; }

        StartCoroutine(TeleportProcess(other));
    }

    // 외부에서 쿨타임을 걸 수 있게 함
    public void ActivateCooldown()
    {
        StartCoroutine(CooldownRoutine());
    }

    IEnumerator CooldownRoutine()
    {
        isCooldown = true;
        yield return new WaitForSeconds(1.0f); // 1초 동안은 들어와도 반응 안 함
        isCooldown = false;
    }

    IEnumerator TeleportProcess(Collider2D ballCollider)
    {
        GameObject ball = ballCollider.gameObject;
        Rigidbody2D rb = ball.GetComponent<Rigidbody2D>();
        SpriteRenderer sr = ball.GetComponent<SpriteRenderer>();
        TrailRenderer tr = ball.GetComponent<TrailRenderer>(); // 꼬리 효과가 있다면 가져오기

        // 1. 사라지는 연출
        sr.enabled = false;
        rb.simulated = false;
        rb.velocity = Vector2.zero;
        if (tr != null) { tr.Clear(); }  // 텔레포트 시 꼬리가 길게 늘어지는 것 방지

        // 2. 대기
        yield return new WaitForSeconds(waitTime);

        // 3. 반대편 웜홀에게 "너 1초간 작동 멈춰"라고 명령 (핵심!)
        if (destinationWormhole != null)
        {
            destinationWormhole.ActivateCooldown();
        }

        // 4. 이동 (spawnPoint가 없으면 그냥 웜홀 위치로)
        ball.transform.position = spawnPoint != null ? spawnPoint.position : destinationWormhole.transform.position;

        // 5. 다시 나타나기
        sr.enabled = true;
        rb.simulated = true;

        // 6. 발사 (spawnPoint의 위쪽 방향으로)
        Vector2 dir = spawnPoint != null ? spawnPoint.up : destinationWormhole.transform.up;
        rb.AddForce(dir * launchForce, ForceMode2D.Impulse);
    }
}