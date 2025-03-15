using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

using E_DataTypes;
using Unity.VisualScripting;

public class PlayerController : MonoBehaviour
{
    /* Component Field */
    private Rigidbody rb;

    /* Player State Field */
    [SerializeField] private float moveSpeed = 10f;

    private Vector3 moveDir;
    private float moveX;
    private float moveZ;
    private Quaternion lastRotation;

    /* Children Object */
    [SerializeField] private GameObject directionArrow; // 방향 확인을 위한 화살표 오브젝트
    //[SerializeField] private GameObject stackItemPoint; // 획득한 아이템이 쌓일 위치 오브젝트
    [SerializeField] private GameObject playerDummy;

    /* Interation Field */
    private List<GameObject> interacts = new List<GameObject>();
    [SerializeField] private GameObject closestObject = null;
    [SerializeField] private GameObject lastHighlightObject = null;

    /* Inventory Field */
    //[SerializeField] private InteractItem currentItem;
    //private GameObject[] itemPrefab = new GameObject[3];
    //int capacity = 3;
    //int currentAmount = 0;


    /* Other Objects */
    InteractManager interactManager;




    /****************************
     * Lifecycle Functions 
     ****************************/
    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        // 인벤토리 스크립트 추가 예정
        //stackItem = GetComponent<StackItem>();
        //stackItem.maxAmount = 3;
        //stackItem.currentAmount = 0;

        interactManager = InteractManager.Instance;
    }

    private void Update()
    {
        InputMove();
        InputRotate();
        InputKeys();

        FindClosetObject();
    }


    /**********************************
     * Input Functions 
     **********************************/
    private void InputMove()
    {
        moveX = Input.GetAxisRaw("Horizontal");
        moveZ = Input.GetAxisRaw("Vertical");

        moveDir = new Vector3(moveX, 0, moveZ).normalized;

        rb.linearVelocity = moveDir * moveSpeed;

        gameObject.transform.rotation = lastRotation;
    }

    private void InputRotate()
    {
        if (moveDir.magnitude > 0)
        {
            lastRotation = Quaternion.LookRotation(moveDir);
        }
    }

    private void InputKeys()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("[E] key down");
            InteractKeyDownEvent();
        }
    }


    private void InteractKeyDownEvent()
    {
        /*
         * 조건 리스트
         * 1. 가까운 물체가 있는가? -> closestObject
            * 2. 해당 물체가 어떤 종류인가? -> closestObject.GetComponent
         * 2. 손에 물건을 들고 있는가? -> currentItem
         */


        if (closestObject == null) // 1. 상호작용 중인 오브젝트가 없다면
        {
            Debug.Log("Case 1");
            if (playerDummy.GetComponent<StackDummy>().GetItemAmount() == 0) // 1-1. 손에 물건이 없다면
            {
                Debug.Log("Case 1-1");
                /* Do Nothing */
            }
            else // 1-2. 손에 물건이 있다면
            {
                Debug.Log("Case 1-2");
                // 바닥에 물건 내려놓기

                // 내려놓을 위치 계산. 플레이어 1칸 앞 격자.
                Vector3 dropPos = gameObject.transform.position;

                dropPos += gameObject.transform.forward;

                dropPos.x = Mathf.Round(dropPos.x);
                dropPos.z = Mathf.Round(dropPos.z);

                Debug.Log($"DropPos: {dropPos}");

                //StackDummy playerDummyScript = playerDummy.GetComponent<StackDummy>();
                interactManager.MakeNewStackDummy(playerDummy.GetComponent<StackDummy>().GetItem().itemName,
                        dropPos, playerDummy.GetComponent<StackDummy>().GetItemAmount()
                    );
                playerDummy.GetComponent<StackDummy>().RemoveFromDummyPlayer(playerDummy.GetComponent<StackDummy>().GetItemAmount());


                FindClosetObject();
            }
        }
        else // 2. 상호작용 중인 오브젝트가 있다면
        {
            Debug.Log("Case 2");
            InteractItem interactItem = closestObject.GetComponent<StackDummy>().GetItem();
            InteractType interactType = closestObject.GetComponent<StackDummy>().GetItemType();


            if (playerDummy.GetComponent<StackDummy>().GetItemAmount() == 0) // 2-1. 손에 물건이 없다면
            {
                Debug.Log("Case 2-1");
                InteractTypeCategory category = InteractTypeExtensions.GetCategory(interactType);
                switch (category)
                {
                    case InteractTypeCategory.WORKBENCH: // 2-1-1. WORKBENCH
                        Debug.Log("Case 2-1-1");
                        /* Do Nothing */
                        break;
                    case InteractTypeCategory.ENVIRONMENT: // 2-1-2. ENVIRONMENT
                        Debug.Log("Case 2-1-2");
                        // 상호작용 -> 채집
                        string frictionName = interactManager.GetFrictionNameByEnvironmentName(interactItem.itemName);
                        interactManager.MakeNewStackDummy(frictionName, closestObject.transform.position, 1);

                        Destroy(closestObject);
                        interacts.Remove(closestObject);
                        closestObject = lastHighlightObject = null;
                        FindClosetObject();

                        break;
                    case InteractTypeCategory.FRICTION: // 2-1-3. FRICTION
                    case InteractTypeCategory.ITEM: // 2-1-4. ITEM
                        Debug.Log("Case 2-1-3,4");
                        // 상호작용 -> 가능한 수만큼 줍기
                        StackDummy targetDummy = closestObject.GetComponent<StackDummy>();
                        int targetAmount = targetDummy.GetItemAmount();

                        playerDummy.GetComponent<StackDummy>().SetInfo(targetDummy.GetItem());

                        int addedAmount = playerDummy.GetComponent<StackDummy>().AddToDummy(targetAmount);
                        targetDummy.RemoveFromDummy(addedAmount);

                        interacts.Remove(closestObject);
                        closestObject = lastHighlightObject = null;
                        FindClosetObject();

                        break;
                    default:
                        /* Do Nothing */
                        break;
                }
            }
            else // 2-2. 손에 물건이 있다면
            {
                InteractTypeCategory category = InteractTypeExtensions.GetCategory(interactType);
                // 2-2-0. 상호작용 물체인 경우
                if (category == InteractTypeCategory.WORKBENCH)
                {
                    // 상호작용 물체가 접근 가능한 상태라면(자리가 있다면) 물건 넣기
                }
                
                Debug.Log("Case 2-2-1");
                // 2-2-1. 손에 물건이 꽉 차 있다면
                if (playerDummy.GetComponent<StackDummy>().GetItemAmount() == playerDummy.GetComponent<StackDummy>().GetCapacity())
                {
                    // 일단 아무 행동도 하지 않지만, 원래는 버릴 수 있어야 한다. 
                }
                else // 2-2-2. 물건을 들 수 있다면
                {
                    switch (category)
                    {
                        //case InteractTypeCategory.WORKBENCH: // 2-2-1. WORKBENCH
                        // 바깥으로 빼서 일단 제외
                        //case InteractTypeCategory.ENVIRONMENT: // 2-2-2. ENVIRONMENT
                        //    Debug.Log("Case 2-2-2");
                        //    /* Do Nothing */
                        //    break;
                        case InteractTypeCategory.FRICTION: // 2-2-3. FRICTION
                        case InteractTypeCategory.ITEM: // 2-2-4. ITEM
                            Debug.Log("Case 2-2-3,4");
                            InteractItem targetItem = closestObject.GetComponent<StackDummy>().GetItem();
                            InteractItem playerItem = playerDummy.GetComponent<StackDummy>().GetItem();

                            if (targetItem.interactType == playerItem.interactType) // 같은 물건이라면
                            {
                                Debug.Log("Case 2-2-3,4-1");
                                int targetAmount = closestObject.GetComponent<StackDummy>().GetItemAmount();
                                int added = playerDummy.GetComponent<StackDummy>().AddToDummy(targetAmount);
                                closestObject.GetComponent<StackDummy>().RemoveFromDummy(added);

                                if (targetAmount == added)
                                {
                                    interacts.Remove(closestObject);
                                    closestObject = lastHighlightObject = null;
                                    FindClosetObject();
                                }
                            }
                            else // 다른 물건이라면
                            {
                                Debug.Log("Case 2-2-3,4-2");
                                /* Do Nothing */

                            }
                                break;
                        default:
                            /* Do Nothing */
                            break;
                    }
                }
            }
        }
    }



    /****************************************
     * Interaction Functions 
     ***********************************/
    public void AddInteractable(GameObject obj)
    {
        if (!interacts.Contains(obj))
        {
            interacts.Add(obj);
            //Debug.Log("Interactable Added");
        }
    }

    public void RemoveInteractable(GameObject obj)
    {
        if (interacts.Contains(obj))
        {
            interacts.Remove(obj);
            //Debug.Log("Interactable Removed");
        }
    }

    private void FindClosetObject()
    {
        float minDistance = Mathf.Infinity;
        closestObject = null;

        foreach (GameObject obj in interacts)
        {
            float distance = Vector3.Distance(transform.position, obj.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestObject = obj;
            }
        }

        if (lastHighlightObject != closestObject)
        {
            if (lastHighlightObject != null)
            {
                RemoveHighlight(lastHighlightObject);
                //Debug.Log("HighLight Removed");
            }
            if (closestObject != null)
            {
                ApplyHighlight(closestObject);
                //Debug.Log("HighLight Applied");
            }

            lastHighlightObject = closestObject;
        }
    }


    private void ApplyHighlight(GameObject obj)
    {
        StackDummy stackDummy = obj.GetComponent<StackDummy>();
        Renderer[] childObjRenderers = obj.GetComponentsInChildren<Renderer>();
        foreach (Renderer childRenderer in childObjRenderers)
        {
            if (childRenderer != null)
            {
                Color color = Color.Lerp(childRenderer.material.color, Color.white, 0.5f);
                childRenderer.material.SetColor("_EmissionColor", color);
                childRenderer.material.EnableKeyword("_EMISSION");
            }
        }
    }

    private void RemoveHighlight(GameObject obj)
    {
        StackDummy stackDummy = obj.GetComponent<StackDummy>();
        Renderer[] childObjRenderers = obj.GetComponentsInChildren<Renderer>();
        foreach (Renderer childRenderer in childObjRenderers)
        {
            if (childRenderer != null)
            {
                childRenderer.material.SetColor("_EmissionColor", Color.black);
                childRenderer.material.DisableKeyword("_EMMISION");
            }
        }
    }


    /************************************* 
     * Inventory Functions 
     *************************************/
    //private void PickUpItemEvent()
    //{
    //    // 가까운 물체가 없거나, 손에 들 수 없다면 return
    //    if (closestObject == null || !closestObject.GetComponent<StackableItem>().IsHandable())
    //    {
    //        return;
    //    }

    //    // 상호작용 중인 아이템 확인
    //    StackableItem targetItem = closestObject.gameObject.GetComponent<StackableItem>();

    //    if (currentAmount == 0) // 손에 든 게 없을 때
    //    {
    //        currentItem = targetItem.GetItem();
    //        PickUpItem(targetItem);
    //    }
    //    else // 손에 아이템을 들고 있을 때
    //    {
    //        // 같은 아이템이라면
    //        if (targetItem.GetItemName() == currentItem.itemName)
    //        {
    //            PickUpItem(targetItem);
    //        }
    //    } 
    //}

    //private void PickUpItem(StackableItem targetItem)
    //{
    //    int prevAmount = currentAmount;

    //    int pickAmount = targetItem.RemoveItem(capacity - currentAmount);
    //    currentAmount += pickAmount;

    //    if (pickAmount != 0)
    //    {
    //        MakeItemOnPlayerHand(targetItem, prevAmount, currentAmount);
    //    }
    //}

    // 플레이어 아이템 핸들 위치에 프리펩 생성
    //private void MakeItemOnPlayerHand(StackableItem targetItem, int startIndex, int endIndex)
    //{
    //    interactManager.MakeChildPrefabByName(
    //            targetItem.GetItemName(),
    //            stackItemPoint.transform.position,
    //            startIndex,
    //            endIndex,
    //            itemPrefab,
    //            stackItemPoint.transform
    //        );
    //}
}
