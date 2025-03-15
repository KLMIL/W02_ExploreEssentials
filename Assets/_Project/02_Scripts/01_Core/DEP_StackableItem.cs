using E_DataTypes;
using UnityEngine;

public class DEP_StackableItem : MonoBehaviour
{
    [SerializeField] private InteractItem item;
    [SerializeField] private int capacity = 20;
    [SerializeField] private int currentAmount = 1;

    // ���̿� ������ �߰�. ���� ������ return
    public int AddItem(int amount)
    {
        if (currentAmount + amount <= capacity)
        {
            currentAmount += amount;
            return 0;
        }
        else
        {
            int restAmount = currentAmount + amount - capacity;
            currentAmount = capacity;
            return restAmount;
        }
    }

    public int RemoveItem(int amount)
    {
        //Debug.Log($"RemoveItem Called: amount: {amount}, currentAmount: {currentAmount}");
        if (currentAmount <= amount)
        {
            //Debug.Log("Item is destroyed");
            //Destroy(gameObject);
            int returnAmount = currentAmount;
            currentAmount = 0;
            return returnAmount;
        }
        else
        {
            //Debug.Log("Item is left");
            currentAmount -= amount;
            return amount;
        }
    }

    /// <summary>
    /// �÷��̾ �տ� �� �� �ִ� ���������� ����
    /// </summary>
    public bool IsHandable()
    {
        return InteractTypeExtensions.GetCategory(item.interactType) == InteractTypeCategory.FRICTION
                || InteractTypeExtensions.GetCategory(item.interactType) == InteractTypeCategory.ITEM;
    }


    /* Getter, Setter */

    public string GetItemName()
    {
        return item.itemName;
    }

    public InteractItem GetItem()
    {
        return item;
    }

    public int GetAmount()
    {
        return currentAmount;
    }

    public InteractType GetInteractType()
    {
        return item.interactType;
    }
}
