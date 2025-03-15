using E_DataTypes;
using NUnit.Framework;
using UnityEngine;

public class StackDummy : MonoBehaviour
{
    [SerializeField] private InteractItem item;
    [SerializeField] private int capacity = 20;
    [SerializeField] private int currAmount = 0;

    private GameObject[] dummyPrefabs = new GameObject[20];

    InteractManager interactManager;

    private void Start()
    {
        interactManager = InteractManager.Instance;
    }


    public int AddToDummy(int amount)
    {
        int targetAmount = currAmount + amount <= capacity ? currAmount + amount : capacity;
        for (int i = currAmount; i < targetAmount; i++)
        {
            Vector3 pos = gameObject.transform.position;
            pos.y += 0.4f * i;
            dummyPrefabs[i] = interactManager.InstantiateByName(item.itemName, pos, gameObject.transform);
        }

        int addAmount = amount < capacity - currAmount ? amount : capacity - currAmount;
        currAmount = targetAmount;
        return addAmount;
    }

    public int RemoveFromDummy(int amount)
    {
        int targetAmount = currAmount > amount ? amount : currAmount;

        for (int i = 0; i < targetAmount; i++)
        {
            Destroy(dummyPrefabs[currAmount - i]);
        }

        currAmount -= targetAmount;
        return targetAmount;
    }


    public InteractType GetItemType()
    {
        return item.interactType;
    }
}
