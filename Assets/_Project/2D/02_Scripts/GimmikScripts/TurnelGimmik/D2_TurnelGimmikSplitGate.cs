using UnityEngine;

public class D2_TurnelGimmikSplitGate : D2__TurnelGimmik
{
    // ����ϸ� ���� �ΰ��� �Ǵ� ���

    // �Լ� �����
    /*
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //��ġ �̵�
        collision.gameObject.transform.position = transform.position;

        //���� ����
        float rad = transform.eulerAngles.z * Mathf.Deg2Rad;
        Vector2 dir = new Vector2(-Mathf.Sin(rad), Mathf.Cos(rad));

        float power = collision.GetComponent<Rigidbody2D>().linearVelocity.magnitude;
        collision.GetComponent<Rigidbody2D>().linearVelocity = power * dir;
    }
    */
}
