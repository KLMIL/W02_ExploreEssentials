using UnityEngine;

public class D2_TurnelGimmikBoost : D2__TurnelGimmik
{
    // ���� �ӵ� �������� �����



    // �ӽ� �Լ� �����
    /*
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //��ġ �̵�
        collision.gameObject.transform.position = transform.position;

        //���� ���� & �Ŀ� ����
        float rad = transform.eulerAngles.z * Mathf.Deg2Rad;
        Vector2 dir = new Vector2(-Mathf.Sin(rad), Mathf.Cos(rad));

        collision.GetComponent<Rigidbody2D>().linearVelocity = power * dir;

        //SoundsPlayer.Instance.PlaySFX(sfx);
    }
    */
}
