using UnityEngine;

public class D2_TurnelGimmikSplitGate : D2__TurnelGimmik
{
    // 통과하면 공이 두개가 되는 기믹

    // 함수 저장용
    /*
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //위치 이동
        collision.gameObject.transform.position = transform.position;

        //방향 조정
        float rad = transform.eulerAngles.z * Mathf.Deg2Rad;
        Vector2 dir = new Vector2(-Mathf.Sin(rad), Mathf.Cos(rad));

        float power = collision.GetComponent<Rigidbody2D>().linearVelocity.magnitude;
        collision.GetComponent<Rigidbody2D>().linearVelocity = power * dir;
    }
    */
}
