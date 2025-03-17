using UnityEngine;

public class ItemMagnet : _InstantItem
{
    [SerializeField] float radius;
    [SerializeField] LayerMask layerMask;

    public override void Use()
    {
        Debug.Log("Used Magnet");

        Vector3 bulletPos = D2_PlayerManager.Instance.GetBulletReference().transform.position;

        //스캔된 오브젝트들
        RaycastHit2D[] targets2D = Physics2D.CircleCastAll(bulletPos, radius, Vector2.up, 0, layerMask);
        

        //Debug.Log(targets2D.Length);

        foreach (RaycastHit2D target2D in targets2D)
        {
            if (target2D.transform.CompareTag("Coin"))
            {
                //Vector2 dir = transform.position - target2D.transform.position;
                Vector2 dir = bulletPos - target2D.transform.position;
                float pullPower = dir.magnitude;
                dir = dir.normalized * pullPower * 2.5f;

                Rigidbody2D _rigid2D = target2D.transform.GetComponent<Rigidbody2D>();
                _rigid2D.AddForce(dir, ForceMode2D.Impulse);
            }
        }
    }
}
