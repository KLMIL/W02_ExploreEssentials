using UnityEngine;

using E_DataTypes;
using Mono.Cecil.Cil;

public class D3_WorkbenchInput : MonoBehaviour
{
    [SerializeField] private GameObject workbenchOutput;

    [SerializeField] private int workbenchIndex;
    //[SerializeField] private GameObject stackDummy;
    public InteractItem item;


    public int InputWorkbench(InteractItem item, int amount)
    {
        D3_StackDummy sdScript = GetComponent<D3_StackDummy>();

        if (sdScript.GetItemAmount() == 0)
        {
            this.item = item;
            int added = sdScript.AddToWorkbenchDummy(amount, this.item);
            workbenchOutput.GetComponent<D3_WorkbenchOutput>().SetItemType(item.interactType, workbenchIndex);

            return added;
        }
        else
        {
            // 현재 아이템과 종류가 다르다면 return 0
            if (this.item.interactType != item.interactType)
            {
                return 0;
            }
            else
            {
                int added = sdScript.AddToWorkbenchDummy(amount, this.item);
                workbenchOutput.GetComponent<D3_WorkbenchOutput>().SetItemType(item.interactType, workbenchIndex);

                return added;
            }
        }
    }

    public void RemoveOneItem()
    {
        GetComponent<D3_StackDummy>().RemoveFromDummyWorkbench(1);
        if (GetComponent<D3_StackDummy>().GetItemAmount() == 0)
        {
            this.item.itemName = "None";
            this.item.interactType = InteractType.None;
        }
    }

    public void RemoveByByPlayer()
    {
        int amount = GetComponent<D3_StackDummy>().GetItemAmount();
        if (amount == 0)
        {
            this.item.itemName = "None";
            this.item.interactType = InteractType.None;
        }
    }

    public void RemoveFromOutput()
    {
        workbenchOutput.GetComponent<D3_WorkbenchOutput>().SetItemType(item.interactType, workbenchIndex);
    }
}
