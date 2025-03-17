using UnityEngine;

public class ItemBomb : _InstantItem
{
    // ������ ȿ��. collider �浹 �̿�
    // ���� �ڵ忡���� collider.enable = true�� �̿���

    public override void Use()
    {
        //Debug.Log("Bomb Used");
        GameObject bulletRef = D2_PlayerManager.Instance.GetBulletReference();
        bulletRef.transform.Find("ExplosionRange")?.gameObject.SetActive(true);
    }
}
