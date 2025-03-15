using E_DataTypes;
using NUnit.Framework;
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

    private void Awake()
    {
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
            Debug.Log("Find prefab error");
            return null;
        }

        return Instantiate(targetPrefab, pos, Quaternion.identity, parent);
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

    public GameObject MakeNewStackDummy(string name, Vector3 pos, int amount, Transform parent = null)
    {
        for (int i = 0; i < interactPrefabName.Length; i++)
        {
            if (interactPrefabName[i] == name)
            {

                for (int j = 0; j < amount; j++)
                {

                }
            }
        }
    }
}
