using E_DataTypes;
using UnityEngine;

public class D3_WorkbenchOutput : MonoBehaviour
{
    [SerializeField] private GameObject workbenchInput1;
    [SerializeField] private GameObject workbenchInput2;

    [SerializeField] private InteractType item1Type = InteractType.None;
    [SerializeField] private InteractType item2Type = InteractType.None;

    D3_InteractManager interactManager;


    public void SetItemType(InteractType type, int index)
    {
        if (index == 0)
        {
            item1Type = type;
        }
        if (index == 1)
        {
            item2Type = type;
        }

        if (item1Type != InteractType.None && item2Type != InteractType.None)
        {
            MakeItem(item1Type, item2Type);

            if (workbenchInput1.GetComponent<D3_StackDummy>().GetItemAmount() == 0)
            {
                item1Type = InteractType.None;
            }
            if (workbenchInput2.GetComponent<D3_StackDummy>().GetItemAmount() == 0)
            {
                item2Type = InteractType.None;
            }
        }
    }

    private void MakeItem(InteractType item1Type, InteractType item2Type)
    {
        interactManager = D3_InteractManager.Instance;

        bool[] checkType = { false, false, false, false };

        checkType[(int)item1Type % 10 - 1] = true;
        checkType[(int)item2Type % 10 - 1] = true;


        string itemName = ""; // 임시 아이템 이름 객체. 너무 하드코딩이긴 해 ~

        if (checkType[3] == true)
        {
            if (checkType[0] == true)
            {
                Debug.Log("Make Bomb Bullet");
                itemName = "BombBullet";
            }
            else if (checkType[1] == true)
            {
                Debug.Log("Make Normal Bullet");
                itemName = "NormalBullet";
            }
            else
            {
                Debug.Log("Make Magnet Bullet");
                itemName = "MagnetBullet";
            }
        }
        else
        {
            Debug.Log("Not combinable");
        }

        if (itemName != "")
        {
            Vector3 pos = gameObject.transform.position;
            pos.y += 2f;
            interactManager.MakeNewStackDummy(itemName, pos, 1);

            workbenchInput1.GetComponent<D3_WorkbenchInput>().RemoveOneItem();
            workbenchInput2.GetComponent<D3_WorkbenchInput>().RemoveOneItem();
        }
    }
}
