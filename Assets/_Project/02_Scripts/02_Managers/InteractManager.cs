using E_DataTypes;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class InteractManager : MonoBehaviour
{
    public static InteractManager Instance { get; private set; }

    //[SerializeField] private List<string> InteractablePrefabsName;
    //[SerializeField] private List<GameObject> InteractablePrefabs;
    [SerializeField] private string[] interactPrefabName = new string[40];
    [SerializeField] private GameObject[] interactPrefabs = new GameObject[40];
    [SerializeField] private InteractType[] interactPrefabTypes = new InteractType[40];

    [SerializeField] private GameObject dummyPrefab;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

    }


    public GameObject InstantiateByName(string name, Vector3 pos, Transform parent = null)
    {
        GameObject targetPrefab = null;
        for (int i = 0; i < interactPrefabName.Length; i++)
        {
            if (interactPrefabName[i] == name)
            {
                targetPrefab = interactPrefabs[i];
            }
        }

        if (targetPrefab == null)
        {
            return null;
        }

        GameObject madePrefab = Instantiate(targetPrefab, pos, Quaternion.identity, parent);

        return madePrefab;
    }




    //호출된 위치에, 호출된 부모의 아래로 프리팹 생성
    public void MakeChildPrefabByName(string name, Vector3 pos, int startIndex, int endIndex, GameObject[] itemPrefab, Transform parent = null)
    {
        for (int i = 0; i < interactPrefabName.Length; i++)
        {
            if (interactPrefabName[i] == name)
            {
                for (int j = startIndex + 1; j <= endIndex; j++)
                {
                    Vector3 stackPos = new Vector3(pos.x, pos.y + startIndex * 0.4f, pos.z);
                    GameObject currItem = Instantiate(interactPrefabs[i], stackPos, Quaternion.identity, parent);
                    itemPrefab[j] = currItem;
                    currItem.GetComponent<BoxCollider>().enabled = false;
                }
            }
        }
    }

    public GameObject MakePrefabInteract(InteractType type, Vector3 pos)
    {
        return Instantiate(interactPrefabs[(int)type + 10], pos, Quaternion.identity);
    }

    public void MakeNewStackDummy(string itemName, Vector3 pos, int amount) //, Transform parent = null)
    {
        GameObject dummy = Instantiate(dummyPrefab, pos, Quaternion.identity); //, parent);


        // 임시로 Name으로 Interact Item을 만들어서 넣을 수 있도록 했음. 추후 변경요망
        InteractType interactType = InteractType.None;
        //for (int i = 0; i < interactPrefabName.Length; i++)
        //{
        //    if (interactPrefabName[i] == itemName)
        //    {
        //        interactType = interactPrefabTypes[i];
        //        
        //    }
        //}

        Enum.TryParse<InteractType>(itemName, true, out interactType);

        InteractItem item;
        item.itemName = itemName;
        item.interactType = interactType;

        dummy.GetComponent<StackDummy>().SetInfo(item);

        for (int i = 0; i < interactPrefabName.Length; i++)
        {
            if (interactPrefabName[i] == item.itemName)
            {
                dummy.GetComponent<StackDummy>().AddToDummy(amount);
            }
        }

        //return dummy;
    }

    public string GetFrictionNameByEnvironmentName(string name)
    {
        for (int i = 0; i < interactPrefabName.Length; i++)
        {
            if (interactPrefabName[i] == name)
            {
                return interactPrefabName[i + 10];
            }
        }
        return null;
    }
}
