using UnityEngine;

public class D2_TileGimmikInteractWall : D2__TileGimmik
{
    // ��ȣ�ۿ� ������ ��
    private void OnTriggerEnter2D(Collider2D collider)
    {
        //Debug.Log("Wall Trigger Enter Explosion");
        if (collider.CompareTag("Explosion"))
        {
            Destroy(gameObject);
        }
    }
}
