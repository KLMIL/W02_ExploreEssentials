using E_DataTypes;
using NUnit.Framework;
using UnityEngine;
    
public class D3_StackDummy : MonoBehaviour
{
    [SerializeField] private InteractItem item;
    [SerializeField] private int capacity = 20;
    [SerializeField] private int currAmount = 0;

    [SerializeField] private GameObject[] dummyPrefabs = new GameObject[20];

    D3_InteractManager interactManager;

    private void Start()
    {
        interactManager = D3_InteractManager.Instance;
    }

    public void SetInfo(InteractItem item)
    {
        this.item = item;
    }


    // 추가 성공한 개수 반환
    public int AddToDummy(int amount)
    {
        int targetAmount = currAmount + amount <= capacity ? currAmount + amount : capacity;
        for (int i = currAmount; i < targetAmount; i++)
        {
            Vector3 pos = gameObject.transform.position;
            pos.y += 0.4f * i;
            dummyPrefabs[i] = D3_InteractManager.Instance.InstantiateByName(item.itemName, pos, gameObject.transform);
        }

        int addAmount = amount < capacity - currAmount ? amount : capacity - currAmount;
        currAmount = targetAmount;

        return addAmount;
    }

    public int AddToWorkbenchDummy(int amount, InteractItem item)
    {
        int targetAmount = currAmount + amount <= capacity ? currAmount + amount : capacity;
        for (int i = currAmount; i < targetAmount; i++)
        {
            Vector3 pos = gameObject.transform.position;
            pos.y += 0.2f;
            pos.y += 0.4f * i;
            //dummyPrefabs[i] = D3_InteractManager.Instance.InstantiateByName(item.itemName, pos, gameObject.transform);
            dummyPrefabs[i] = D3_InteractManager.Instance.InstantiateByName(item.itemName, pos); // 너무 크게 만들어지는 문제때문에 임시
        }

        int addAmount = amount < capacity - currAmount ? amount : capacity - currAmount;
        currAmount = targetAmount;

        return addAmount;
    }

    // 제거 성공한 개수 반환
    public int RemoveFromDummy(int amount)
    {
        int targetAmount = currAmount > amount ? amount : currAmount;

        for (int i = 1; i <= targetAmount; i++)
        {
            Destroy(dummyPrefabs[currAmount - i]);
        }

        currAmount -= targetAmount;

        if (currAmount == 0)
        {
            Destroy(gameObject);
        }

        return targetAmount;
    }

    // 플레이어용 제거 함수
    public int RemoveFromDummyPlayer(int amount)
    {
        int targetAmount = currAmount > amount ? amount : currAmount;

        for (int i = 1; i <= targetAmount; i++)
        {
            Debug.Log("Destroyed Prefab");
            Destroy(dummyPrefabs[currAmount - i]);
        }

        currAmount -= targetAmount;

        if (currAmount == 0)
        {
            item.itemName = "None";
            item.interactType = InteractType.None;
        }
    

        return targetAmount;
    }


    public InteractType GetItemType()
    {
        return item.interactType;
    }

    public InteractItem GetItem()
    {
        return item;
    }

    public int GetItemAmount()
    {
        return currAmount;
    }

    public int GetCapacity()
    {
        return capacity;
    }
}
