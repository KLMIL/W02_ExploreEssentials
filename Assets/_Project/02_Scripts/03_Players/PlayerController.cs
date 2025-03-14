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
        if (closestObject == null) // 1-1. 가까운 물체가 없다면
        {
            if (currentAmount == 0) // 1-1-1. 손에 든 물체가 없다면
            {
                /* Do Nothing */
            }
            else // 1-1-2. 손에 든 물체가 있다면
            {
                // 아이템 드롭
            }
        }
        else // 1-2. 가까운 물체가 있다면
        {
            StackableItem targetItem = closestObject.GetComponent<StackableItem>();
            InteractTypeCategory category = InteractTypeExtensions.GetCategory(targetItem.GetInteractType());

            if (currentAmount == 0) // 1-2-1. 손에 든 물체가 없다면
            {
                if (category == InteractTypeCategory.WORKBENCH) // 1-2-1-1. 상호작용 물체라면
                {
                    /* Do Nothing */
                }
                if (category == InteractTypeCategory.ENVIRONMENT)  // 1-2-1-2. 광물이라면
                {
                    // 해당 위치에 상호작용 결과물 프리펩 생성
                    interactManager.MakePrefabInteract(targetItem.GetInteractType(), closestObject.transform.position);

                    // 상호작용 물체 제거 후 가장 가까운 물체 검색
                    Destroy(closestObject);
                    interacts.Remove(closestObject);
                    closestObject = lastHighlightObject = null;
                    FindClosetObject();
                }
                if (category == InteractTypeCategory.FRICTION
                    || category == InteractTypeCategory.ITEM) // 1-2-1-3. 주괴, 아이템이라면
                {
                    PickUpItemEvent();
                    if (targetItem.GetAmount() <= 0)
                    {
                        Destroy(closestObject);
                        interacts.Remove(closestObject);
                        closestObject = lastHighlightObject = null;
                    }
                    FindClosetObject();
                }
            }
            else // 1-2-2. 손에 든 물체가 있다면
            {
                if (category == InteractTypeCategory.WORKBENCH) // 1-2-2-1. 상호작용 물체라면
                {
                    // 1-2-2-1-1. 상호작용 물체에 넣을 수 있다면
                    // 아이템 넣기

                    // 1-2-2-1-2. 상호작용 물체에 넣을 수 없다면
                    /* Do Nothing */
                }
                if (category == InteractTypeCategory.ENVIRONMENT) // 1-2-2-2. 광물이라면
                {
                    /* Do Nothing */
                }
                if (category == InteractTypeCategory.FRICTION
                                    || category == InteractTypeCategory.ITEM) // 1-2-2-3. 주괴, 아이템이라면
                {
                    if (currentItem.itemName == targetItem.GetItemName()) // 1-2-2-3-1같은 종류라면
                    {
                        PickUpItemEvent();
                        if (targetItem.GetAmount() <= 0)
                        {
                            Destroy(closestObject);
                            interacts.Remove(closestObject);
                            closestObject = lastHighlightObject = null;
                        }
                        FindClosetObject();
                    }
                    else // 1-2-2-3-2다른 종류라면
                    {
                        /* Do Nothing */
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
    public void PickUpItemEvent()
    {
        // 가까운 물체가 없거나, 손에 들 수 없다면 return
        if (closestObject == null || !closestObject.GetComponent<StackableItem>().IsHandable())
        {
            return;
        }

        // 상호작용 중인 아이템 확인
        StackableItem targetItem = closestObject.gameObject.GetComponent<StackableItem>();

        if (currentAmount == 0) // 손에 든 게 없을 때
        {
            currentItem = targetItem.GetItem();
            PickUpItem(targetItem);
        }
        else // 손에 아이템을 들고 있을 때
        {
            // 같은 아이템이라면
            if (targetItem.GetItemName() == currentItem.itemName)
            {
                PickUpItem(targetItem);
            }
        } 
    }

    private void PickUpItem(StackableItem targetItem)
    {
        int prevAmount = currentAmount;

        int pickAmount = targetItem.RemoveItem(capacity - currentAmount);
        currentAmount += pickAmount;

        if (pickAmount != 0)
        {
            MakeItemOnPlayerHand(targetItem, prevAmount, currentAmount);
        }
    }

    // 플레이어 아이템 핸들 위치에 프리펩 생성
    private void MakeItemOnPlayerHand(StackableItem targetItem, int startIndex, int endIndex)
    {
        interactManager.MakeChildPrefabByName(
                targetItem.GetItemName(),
                stackItemPoint.transform.position,
                startIndex,
                endIndex,
                stackItemPoint.transform
            );
    }


    public void DropItemEvent()
    {

    }
}
