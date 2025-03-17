using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D2_PlayerManager : MonoBehaviour
{
    // 새로 만든 스크립트
    // 플레이어의 정보에 관련된 기능
    public static D2_PlayerManager Instance { get; private set; }

    public GameObject[] BulletPrefabs = new GameObject[3];
    public bool isItemSelected = false;
    public List<ParticleSystem> itemParticles;

    D2_GameManager gameManager;
    D2_PlayerController playerController;
    D2_PlayerHUDController playerHUDController;
    D2_ItemManager itemManager;


    public int selectedItem = -1;
    //private bool isSelectAvailable = false;
    //private bool isBulletDestroyed = false;
    //private bool isPlayAvailable = false;
    private bool isDragAvailable = false;

    // Bullet 관련 필드
    //public float maxBulletPower = 40f; // 변수 이름 변경
    public float bulletPowerMax = 1f;
    //public float maxLineLength = 1.5f; // 변수 이름 변경
    public float lineLengthMax = 1f;
    //public float speedDamping = 0.98f; // 변수 이름 변경
    public float dampingAmount = 2.0f;
    //public float stopThreshold = 0.1f;
    public float stopSpeed = 0.1f;  // 변수 이름 변경
    public float powValue = 6f;

    private GameObject bulletReference;




    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        gameManager = D2_GameManager.Instance;
        itemManager = D2_ItemManager.Instance;
        playerController = D2_PlayerController.Instance;
        playerHUDController = D2_PlayerHUDController.Instance;

        // 나중에는 Bullet을 만들어서 Object Pooling 해보자
        //MakeBullet();
    }


    public void MakeBullet(int bulletIndex)
    {
        if (bulletReference != null)
        {
            Destroy(bulletReference);
        }
        bulletReference = Instantiate(BulletPrefabs[bulletIndex], gameObject.transform.position, Quaternion.identity, transform);
    }

    public void FireBullet(Vector2 direction, float power)
    {
        bulletReference.GetComponent<Rigidbody2D>().linearVelocity = direction * power;
        bulletReference.GetComponent<D2_Bullet>().Fire();
    }

    public void DestroyBullet()
    {
        //Debug.Log("DestroyBullet by PlayerManager called");
        //if (selectedItem != -1)
        //{
            itemManager.UseItem(selectedItem);
        //    selectedItem = -1;
        //}
        StartCoroutine(LateDestroyBullet());

    }

    private IEnumerator LateDestroyBullet()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(bulletReference.gameObject);
        //gameManager.UseBullet();
        //MakeBullet();
    }

    public void CountDownBullet()
    {
        itemManager.UseItem(selectedItem);
    }


    public void SelectItem(int itemIndex)
    {
        if (!gameManager.HaveItem(itemIndex))
        {
            // 색 빨갛게 바꾸고 끝
        }
        else
        {
            // 색 파랗게 바꾸고, 적용
            selectedItem = itemIndex;
        }
    }

    public void UnselectItem()
    {
        selectedItem = -1;
    }


    // GETTER, SETTER
    public GameObject GetBulletReference()
    {
        return bulletReference;
    }


    public void SetDragAvailable(bool canDrag)
    {
        isDragAvailable = canDrag;
    }

    public bool GetDragAvailable()
    {
        return isDragAvailable;
    }
}
