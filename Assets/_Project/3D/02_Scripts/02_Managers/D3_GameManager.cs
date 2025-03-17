using UnityEngine;

using E_DataTypes;
using System.Collections.Generic;

public class D3_GameManager : MonoBehaviour
{
    // X좌표 -7 ~ 7
    // Z좌표 -6 ~ 6

    [SerializeField] private List<GameObject> orePrefabs = new List<GameObject>();

    int rangeX = 14;
    int rangeZ = 12;
    int randomAmount = 5;
    //InteractType[,] objectMap = new InteractType[25, 13];
    //InteractType[,] objectMap;
    GameObject[,] objectMap;


    [SerializeField] private D3_PlayerController D3_playerController;


    private void Start()
    {
        //objectMap = new InteractType[rangeX, rangeZ];
        objectMap = new GameObject[rangeX, rangeZ];
    }

    //private void Update()
    //{
        //if (Input.GetKeyDown(KeyCode.F))
        //{
        //    MakeRandomOre();
        //}
    //}

   
    public void MakeRandomOre()
    {
        // 만들 개수
        int[] oresAmount = new int[4];
        for (int i = 0; i < 4; i++)
        {
            oresAmount[i] = Random.Range(1, randomAmount);
        }

        // 초기화
        for (int i = 0; i < objectMap.GetLength(0); i++)
        {
            for (int j = 0; j < objectMap.GetLength(1); j++)
            {
                //objectMap[i, j] = InteractType.None;
                if (objectMap[i, j] != null)
                {
                    Destroy(objectMap[i, j]);
                }
            }
        }

        // 플레이어가 참조하던 모든 프리펩 제거
        D3_playerController.RemoveAllInteractable();

        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < oresAmount[i]; j++)
            {
                int randomX = Random.Range(0, rangeX);
                int randomZ = Random.Range(0, rangeZ);

                //while (objectMap[randomX, randomZ] != InteractType.None)
                while (objectMap[randomX, randomZ] != null)
                {
                    randomX = Random.Range(0, rangeX);
                    randomZ = Random.Range(0, rangeZ);
                }

                //objectMap[randomX, randomZ] = (InteractType)(11 + i);
                Vector3 randomPos = new Vector3(randomX - (rangeX / 2), 0, randomZ - (rangeZ / 2));
                Debug.Log($"X: {randomX}, Z: {randomZ}, pos: {randomPos}");
                objectMap[randomX, randomZ] = Instantiate(orePrefabs[i], randomPos, Quaternion.identity);
            }
        }
    }
}
