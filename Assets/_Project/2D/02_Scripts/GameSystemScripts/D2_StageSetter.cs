using UnityEngine;

public class D2_StageSetter : MonoBehaviour
{
    //[SerializeField] private int[] items = new int[2];
    [SerializeField] private int numCoin;
    //[SerializeField] private int numBullet;
    //[SerializeField] private Vector3 playerPosition;
    //[SerializeField] private int cameraSize;

    D2_GameManager gameManager;


    private void Awake()
    {
        gameManager = D2_GameManager.Instance;
        
    }

    private void Start()
    {
        SetStage();
    }

    private void SetStage()
    {
        //DEP_ItemController.Instance.InitItem(items);
        //DEP_GameManager.Instance.InitCoin(numCoin);
        //DEP_PlayerController.Instance.InitBullet(numBullet);
        //DEP_PlayerController.Instance.InitPosition(playerPosition);
        //Camera.main.orthographicSize = cameraSize;

        gameManager.currentCoin = numCoin;
        gameManager.InitText();
        //gameManager.currentBullet = numBullet;
        //gameManager.currentItems = items;
        //gameManager.InitText();
    }

    public int GetCoinNum()
    {
        return numCoin;
    }

    //public int GetBulletNum()
    //{
    //    return numBullet;
    //}
}
