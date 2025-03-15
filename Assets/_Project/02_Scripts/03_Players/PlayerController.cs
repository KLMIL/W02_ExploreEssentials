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
    [SerializeField] private GameObject stackItemPoint; // 획득한 아이템이 쌓일 위치 오브젝트

    /* Interation Field */
    private List<GameObject> interacts = new List<GameObject>();
    private GameObject closestObject = null;
    private GameObject lastHighlightObject = null;

    /* Inventory Field */
    [SerializeField] private InteractItem currentItem;
    private GameObject[] itemPrefab = new GameObject[3];
    int capacity = 3;
    int currentAmount = 0;


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
            if (currentAmount == 0) // 1-1. 손에 물건이 없다면
            {
                Debug.Log("Case 1-1");
                /* Do Nothing */
            }
            else // 1-2. 손에 물건이 있다면
            {
                Debug.Log("Case 1-2");
                // 바닥에 물건 내려놓기
            }
        }
        else // 2. 상호작용 중인 오브젝트가 있다면
        {
            Debug.Log("Case 2");
            InteractType interactType = closestObject.GetComponent<StackDummy>().GetItemType();

            if (currentAmount == 0) // 2-1. 손에 물건이 없다면
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

                        // 더미 객체를 만들어서, 거기에 아이템을 넣고
                        // 현재 물체를 삭제
                        GameObject dummy = interactManager.MakeNewStackDummy();

                        break;
                    case InteractTypeCategory.FRICTION: // 2-1-3. FRICTION
                    case InteractTypeCategory.ITEM: // 2-1-4. ITEM
                        Debug.Log("Case 2-1-3,4");
                        // 가능한 개수만큼 물건 들기
                        break;
                    default:
                        /* Do Nothing */
                        break;
                }
            }
            else // 2-2. 손에 물건이 있다면
            {
                InteractTypeCategory category = InteractTypeExtensions.GetCategory(interactType);
                switch (category)
                {
                    case InteractTypeCategory.WORKBENCH: // 2-2-1. WORKBENCH
                        Debug.Log("Case 2-2-1");
                        // 가능한 상태라면 물건 넣기
                        break;
                    case InteractTypeCategory.ENVIRONMENT: // 2-2-2. ENVIRONMENT
                        Debug.Log("Case 2-2-2");
                        /* Do Nothing */
                        break;
                    case InteractTypeCategory.FRICTION: // 2-2-3. FRICTION
                    case InteractTypeCategory.ITEM: // 2-2-4. ITEM
                        Debug.Log("Case 2-2-3,4");
                        // 같은 물건이라면 더 들거나 내려놓기
                        break;
                    default:
                        /* Do Nothing */
                        break;
                }
            }
        }
    }


    //private void DEP_InteractKeyDownEvent()
    //{
    //    if (closestObject == null) // 1-1. 가까운 물체가 없다면
    //    {
    //        if (currentAmount == 0) // 1-1-1. 손에 든 물체가 없다면
    //        {
    //            /* Do Nothing */
    //        }
    //        else // 1-1-2. 손에 든 물체가 있다면
    //        {
    //            /*
    //            GameObject currDummy = DropItemEvent();
    //            int stackedSize = currDummy.GetComponent<StackDummy>().AddItemToDummy(currentAmount);
    //            int prevAmount = currentAmount;
    //            currentAmount -= stackedSize;

    //            for (int i = currentAmount; i > currentAmount; i--)
    //            {
    //                Destroy(itemPrefab[i]);
    //            }
    //            */
    //            // 아이템 더미가 아니라 StackableItem으로 다시 작성예정
    //        }
    //    }
    //    else // 1-2. 가까운 물체가 있다면
    //    {
    //        StackableItem targetItem = closestObject.GetComponent<StackableItem>();
    //        //StackDummy targetDummy = closestObject.GetComponent<StackDummy>();
    //        InteractTypeCategory category = InteractTypeCategory.NONE;
    //        if (targetItem != null)
    //        {
    //            category = InteractTypeExtensions.GetCategory(targetItem.GetInteractType());
    //        }
    //        //else if (targetDummy != null)
    //        //{
    //        //    category = InteractTypeExtensions.GetCategory(targetDummy.GetInteractType());
    //        //}
            

    //        if (currentAmount == 0) // 1-2-1. 손에 든 물체가 없다면
    //        {
    //            if (category == InteractTypeCategory.WORKBENCH) // 1-2-1-1. 상호작용 물체라면
    //            {
    //                /* Do Nothing */
    //            }
    //            if (category == InteractTypeCategory.ENVIRONMENT)  // 1-2-1-2. 광물이라면
    //            {
    //                // 해당 위치에 상호작용 결과물 프리펩 생성
    //                interactManager.MakePrefabInteract(targetItem.GetInteractType(), closestObject.transform.position);

    //                // 상호작용 물체 제거 후 가장 가까운 물체 검색
    //                Destroy(closestObject);
    //                interacts.Remove(closestObject);
    //                closestObject = lastHighlightObject = null;
    //                FindClosetObject();
    //            }
    //            if (category == InteractTypeCategory.FRICTION
    //                || category == InteractTypeCategory.ITEM) // 1-2-1-3. 주괴, 아이템이라면
    //            {
    //                //PickUpItemEvent();
    //                if (targetItem.GetAmount() <= 0)
    //                {
    //                    Destroy(closestObject);
    //                    interacts.Remove(closestObject);
    //                    closestObject = lastHighlightObject = null;
    //                }
    //                FindClosetObject();
    //            }
    //        }
    //        else // 1-2-2. 손에 든 물체가 있다면
    //        {
    //            if (category == InteractTypeCategory.WORKBENCH) // 1-2-2-1. 상호작용 물체라면
    //            {
    //                // 1-2-2-1-1. 상호작용 물체에 넣을 수 있다면
    //                // 아이템 넣기

    //                // 1-2-2-1-2. 상호작용 물체에 넣을 수 없다면
    //                /* Do Nothing */
    //            }
    //            if (category == InteractTypeCategory.ENVIRONMENT) // 1-2-2-2. 광물이라면
    //            {
    //                /* Do Nothing */
    //            }
    //            if (category == InteractTypeCategory.FRICTION
    //                                || category == InteractTypeCategory.ITEM) // 1-2-2-3. 주괴, 아이템이라면
    //            {
    //                if (currentItem.itemName == targetItem.GetItemName()) // 1-2-2-3-1. 같은 종류라면
    //                {
    //                    if (currentAmount == capacity) // 1-2-2-3-1-1. 손이 꽉 차있다면
    //                    {
    //                        // 아이템 내려놓기
    //                    }
    //                    else // 1-2-2-3-1-2. 비어있다면 부족한 만큼 들기
    //                    {
    //                        //PickUpItemEvent();
    //                        if (targetItem.GetAmount() <= 0)
    //                        {
    //                            Destroy(closestObject);
    //                            interacts.Remove(closestObject);
    //                            closestObject = lastHighlightObject = null;
    //                        }
    //                        FindClosetObject();
    //                    }

    //                }
    //                else // 1-2-2-3-2. 다른 종류라면
    //                {
    //                    /* Do Nothing */
    //                }
    //            }
    //        }
    //    }
    //}




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

        foreach(GameObject obj in interacts)
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
        Renderer objRenderer = obj.GetComponent<Renderer>();
        if (objRenderer != null)
        {
            Color color = Color.Lerp(objRenderer.material.color, Color.white, 0.5f);
            objRenderer.material.SetColor("_EmissionColor", color);
            objRenderer.material.EnableKeyword("_EMISSION");
        }
    }

    private void RemoveHighlight(GameObject obj)
    {
        Renderer objRenderer = obj.GetComponent<Renderer>();
        if (objRenderer != null)
        {
            objRenderer.material.SetColor("_EmissionColor", Color.black);
            objRenderer.material.DisableKeyword("_EMMISION");
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
