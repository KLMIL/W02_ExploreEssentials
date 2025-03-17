using UnityEngine;

public class D2_TurnelGimmikBoost : D2__TurnelGimmik
{
    // 대충 속도 빨라지는 기믹임



    // 임시 함수 저장용
    /*
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //위치 이동
        collision.gameObject.transform.position = transform.position;

        //방향 조정 & 파워 충전
        float rad = transform.eulerAngles.z * Mathf.Deg2Rad;
        Vector2 dir = new Vector2(-Mathf.Sin(rad), Mathf.Cos(rad));

        collision.GetComponent<Rigidbody2D>().linearVelocity = power * dir;

        //SoundsPlayer.Instance.PlaySFX(sfx);
    }
    */
}
