using UnityEngine;

public class D2_TurnelGimmikPortal : D2__TurnelGimmik
{
    // ���� �����̵���

    // ���� ���� �˰��� ��Ͽ�
    /*
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.CompareTag(tagBullet))
        {
            bool isUsed = masterPortal.FindID(collision.gameObject.GetInstanceID());

            if (isUsed == false)
            {
                masterPortal.AddID(collision.gameObject.GetInstanceID());

                //��ġ �̵�
                collision.gameObject.transform.position = OutPortal.transform.position;

                //�� ���� ���� (���� ȸ��)
                float portalDeg = OutPortal.transform.eulerAngles.z - transform.eulerAngles.z;
                float rad = portalDeg * Mathf.Deg2Rad;
                float cos = Mathf.Cos(rad);
                float sin = Mathf.Sin(rad);

                Vector2 velocity = collision.GetComponent<Rigidbody2D>().linearVelocity;

                float x = velocity.x * cos - velocity.y * sin;
                float y = velocity.x * sin + velocity.y * cos;

                collision.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(x, y);

                DEP_SoundsPlayer.Instance.PlaySFX(sfx);
            }
        }
    }
    */
}