using UnityEngine;

using E_DataTypes;

public class D3_SubmitHole : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("Submit Hole enter");
        if (collision.gameObject.CompareTag("Player"))
        {
            D3_StackDummy dummy = collision.gameObject.GetComponent<D3_PlayerController>().GetPlayerStackDummy();

            // dummy�� ������Ұ� �������̶��, �ش� �������� ������ ���� Bullet ���� ++
            if (dummy.GetItemType() == InteractType.NormalBullet)
            {
                D2_GameManager.Instance.currentItems[0]++;
                dummy.RemoveFromDummyPlayer(1);
            }
            else if (dummy.GetItemType() == InteractType.BombBullet)
            {
                D2_GameManager.Instance.currentItems[1]++;
                dummy.RemoveFromDummyPlayer(1);
            }
            else if (dummy.GetItemType() == InteractType.MagnetBullet)
            {
                D2_GameManager.Instance.currentItems[2]++;
                dummy.RemoveFromDummyPlayer(1);
            }

            D2_GameManager.Instance.InitText();
        }
    }
}
