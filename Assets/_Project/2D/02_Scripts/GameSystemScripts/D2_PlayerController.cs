using DataTypes;
using NUnit;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class D2_PlayerController : MonoBehaviour
{
    // 플레이어의 동작과 관련된 기능
    public static D2_PlayerController Instance { get; private set; }

    D2_PlayerManager playerManager;

    GameObject bulletReference;


    // 동작 관련 함수
    KeyCode[] itemKeys = { KeyCode.Q, KeyCode.W, KeyCode.E, KeyCode.R };


    //public List<GameObject> ItemButtonImage;


    // Bullet 관련 변수
    bool isDragging = false; // 마우스가 누르는 중일때
    //bool isDragAvailable = true;


    /* LifeCycle Function */
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
        playerManager = D2_PlayerManager.Instance;
    }

    private void Update()
    {
        // playerManager 이벤트 구독해서, Bullet 만들어지면 Reference 가져오는 방식이 낫겠다. 

        PlayerMouseEvent();
        PlayerKeyboardEvent();
    }


    void PlayerKeyboardEvent()
    {
        // QWER 키에 각 아이템 사용 기능 추가
        for (int i = 0; i < itemKeys.Length; i++)
        {
            //if (Input.GetKeyDown(itemKeys[i]))
            //{
            //    if (i == 0 && D2_GameManager.Instance.currentItems[0] > 0)
            //    {
            //        GameObject.Find("Item1").gameObject.GetComponent<Image>().color = Color.white;
            //        GameObject.Find("Item0").gameObject.GetComponent<Image>().color = Color.cyan;
            //    }
            //    if (i == 1 && D2_GameManager.Instance.currentItems[1] > 0)
            //    {
            //        GameObject.Find("Item0").gameObject.GetComponent<Image>().color = Color.white;
            //        GameObject.Find("Item1").gameObject.GetComponent<Image>().color = Color.cyan;
            //    }
            //    //Debug.Log($"Key Down: {itemKeys[i]}");
            //    SelectItem(i);
            //}
        }

        //// Space키에 공 파워 기능 추가
        //if (Input.GetKeyDown(KeyCode.R))
        //{
        //    GameManager.Instance.RetryNow();
        //}
    }

    void PlayerMouseEvent()
    {
        // 마우스 클릭했을 때
        if (Input.GetMouseButtonDown(0))
        {
            bulletReference = playerManager.GetBulletReference();

            if (bulletReference != null && !bulletReference.GetComponent<D2_Bullet>().GetFired())
            {
                if (playerManager.GetDragAvailable())
                {
                    isDragging = true;
                    bulletReference.GetComponent<LineRenderer>().enabled = true;
                }
            }
        }

        // 마우스 드래그 중
        if (isDragging)
        {
            // 현재 클릭한 위치 기반으로 LineRenderer를 그릴 벡터 획득
            Vector2 currentMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //Vector2 currentMousePosition = camera2D.ScreenToWorldPoint(Input.mousePosition);
            Vector2 dragVector = (Vector2)bulletReference.transform.position - currentMousePosition;

            float dragMagnitude = Mathf.Min(dragVector.magnitude, playerManager.lineLengthMax);
            Vector2 lineEndPosition = (Vector2)bulletReference.transform.position - dragVector.normalized * dragMagnitude;

            // LineRenderer 그리기
            bulletReference.GetComponent<D2_Bullet>().SetLineRenderer(bulletReference.transform.position, lineEndPosition);
        }

        // 마우스 놓았을 때
        if (Input.GetMouseButtonUp(0))
        {
            if (isDragging)
            {
                if (!playerManager.GetDragAvailable())
                {
                    //playerManager.CountDownBullet();
                    D2_GameManager.Instance.UseCurrentItems2();


                    isDragging = false;
                    bulletReference.GetComponent<D2_Bullet>().Fire();
                    //GameManager.Instance.GetComponent<SoundManager>().PlaySFX(SoundManager.Instance.audios[(int)GameSound.BOUNCE_BALL]);

                    // 마우스가 놓아진 위치 기반으로 벡터 계산
                    Vector2 releasePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    //Vector2 releasePosition = camera2D.ScreenToWorldPoint(Input.mousePosition);

                    Vector2 dragDistance = (Vector2)bulletReference.transform.position - releasePosition;
                    float dragMagnitude = Mathf.Min(dragDistance.magnitude, playerManager.lineLengthMax);
                    float bulletPower = Mathf.Pow(1 + (dragMagnitude / playerManager.lineLengthMax), playerManager.powValue) * playerManager.bulletPowerMax;


                    // 총알 발사하고 LineRenderer 비활성화
                    playerManager.FireBullet(dragDistance.normalized, bulletPower);
                    bulletReference.GetComponent<D2_Bullet>().DisableLineRenderer();

                    // 아이템 선택 초기화하고 총알 발사 기능 일시중지
                    
                }
                else
                {
                    isDragging = false;
                    bulletReference.GetComponent<D2_Bullet>().DisableLineRenderer();
                }
            }
        }
    }



    public void SelectItem(int itemIndex)
    {
        playerManager.SelectItem(itemIndex);
    }

}
