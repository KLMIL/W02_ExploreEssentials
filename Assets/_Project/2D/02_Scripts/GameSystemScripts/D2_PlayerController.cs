using DataTypes;
using NUnit;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class D2_PlayerController : MonoBehaviour
{
    // �÷��̾��� ���۰� ���õ� ���
    public static D2_PlayerController Instance { get; private set; }

    D2_PlayerManager playerManager;

    GameObject bulletReference;


    // ���� ���� �Լ�
    KeyCode[] itemKeys = { KeyCode.Q, KeyCode.W, KeyCode.E, KeyCode.R };


    //public List<GameObject> ItemButtonImage;


    // Bullet ���� ����
    bool isDragging = false; // ���콺�� ������ ���϶�
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
        // playerManager �̺�Ʈ �����ؼ�, Bullet ��������� Reference �������� ����� ���ڴ�. 

        PlayerMouseEvent();
        PlayerKeyboardEvent();
    }


    void PlayerKeyboardEvent()
    {
        // QWER Ű�� �� ������ ��� ��� �߰�
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

        //// SpaceŰ�� �� �Ŀ� ��� �߰�
        //if (Input.GetKeyDown(KeyCode.R))
        //{
        //    GameManager.Instance.RetryNow();
        //}
    }

    void PlayerMouseEvent()
    {
        // ���콺 Ŭ������ ��
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

        // ���콺 �巡�� ��
        if (isDragging)
        {
            // ���� Ŭ���� ��ġ ������� LineRenderer�� �׸� ���� ȹ��
            Vector2 currentMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //Vector2 currentMousePosition = camera2D.ScreenToWorldPoint(Input.mousePosition);
            Vector2 dragVector = (Vector2)bulletReference.transform.position - currentMousePosition;

            float dragMagnitude = Mathf.Min(dragVector.magnitude, playerManager.lineLengthMax);
            Vector2 lineEndPosition = (Vector2)bulletReference.transform.position - dragVector.normalized * dragMagnitude;

            // LineRenderer �׸���
            bulletReference.GetComponent<D2_Bullet>().SetLineRenderer(bulletReference.transform.position, lineEndPosition);
        }

        // ���콺 ������ ��
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

                    // ���콺�� ������ ��ġ ������� ���� ���
                    Vector2 releasePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    //Vector2 releasePosition = camera2D.ScreenToWorldPoint(Input.mousePosition);

                    Vector2 dragDistance = (Vector2)bulletReference.transform.position - releasePosition;
                    float dragMagnitude = Mathf.Min(dragDistance.magnitude, playerManager.lineLengthMax);
                    float bulletPower = Mathf.Pow(1 + (dragMagnitude / playerManager.lineLengthMax), playerManager.powValue) * playerManager.bulletPowerMax;


                    // �Ѿ� �߻��ϰ� LineRenderer ��Ȱ��ȭ
                    playerManager.FireBullet(dragDistance.normalized, bulletPower);
                    bulletReference.GetComponent<D2_Bullet>().DisableLineRenderer();

                    // ������ ���� �ʱ�ȭ�ϰ� �Ѿ� �߻� ��� �Ͻ�����
                    
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
