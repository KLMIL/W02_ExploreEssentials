using UnityEngine;

public class ItemBomb : _InstantItem
{
    // 터지는 효과. collider 충돌 이용
    // 기존 코드에서는 collider.enable = true를 이용함

    public override void Use()
    {
        //Debug.Log("Bomb Used");
        GameObject bulletRef = D2_PlayerManager.Instance.GetBulletReference();
        bulletRef.transform.Find("ExplosionRange")?.gameObject.SetActive(true);
    }
}
