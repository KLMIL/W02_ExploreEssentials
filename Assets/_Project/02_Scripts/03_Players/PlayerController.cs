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
    [SerializeField] private GameObject directionArrow; // ���� Ȯ���� ���� ȭ��ǥ ������Ʈ
    //[SerializeField] private GameObject stackItemPoint; // ȹ���� �������� ���� ��ġ ������Ʈ
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

        // �κ��丮 ��ũ��Ʈ �߰� ����
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
         * ���� ����Ʈ
         * 1. ����� ��ü�� �ִ°�? -> closestObject
            * 2. �ش� ��ü�� � �����ΰ�? -> closestObject.GetComponent
         * 2. �տ� ������ ��� �ִ°�? -> currentItem
         */


        if (closestObject == null) // 1. ��ȣ�ۿ� ���� ������Ʈ�� ���ٸ�
        {
            Debug.Log("Case 1");
            if (playerDummy.GetComponent<StackDummy>().GetItemAmount() == 0) // 1-1. �տ� ������ ���ٸ�
            {
                Debug.Log("Case 1-1");
                /* Do Nothing */
            }
            else // 1-2. �տ� ������ �ִٸ�
            {
                Debug.Log("Case 1-2");
                // �ٴڿ� ���� ��������

                // �������� ��ġ ���. �÷��̾� 1ĭ �� ����.
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
        else // 2. ��ȣ�ۿ� ���� ������Ʈ�� �ִٸ�
        {
            Debug.Log("Case 2");
            InteractItem interactItem = closestObject.GetComponent<StackDummy>().GetItem();
            InteractType interactType = closestObject.GetComponent<StackDummy>().GetItemType();


            if (playerDummy.GetComponent<StackDummy>().GetItemAmount() == 0) // 2-1. �տ� ������ ���ٸ�
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
                        // ��ȣ�ۿ� -> ä��
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
                        // ��ȣ�ۿ� -> ������ ����ŭ �ݱ�
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
            else // 2-2. �տ� ������ �ִٸ�
            {
                InteractTypeCategory category = InteractTypeExtensions.GetCategory(interactType);
                // 2-2-0. ��ȣ�ۿ� ��ü�� ���
                if (category == InteractTypeCategory.WORKBENCH)
                {
                    // ��ȣ�ۿ� ��ü�� ���� ������ ���¶��(�ڸ��� �ִٸ�) ���� �ֱ�
                }
                
                Debug.Log("Case 2-2-1");
                // 2-2-1. �տ� ������ �� �� �ִٸ�
                if (playerDummy.GetComponent<StackDummy>().GetItemAmount() == playerDummy.GetComponent<StackDummy>().GetCapacity())
                {
                    // �ϴ� �ƹ� �ൿ�� ���� ������, ������ ���� �� �־�� �Ѵ�. 
                }
                else // 2-2-2. ������ �� �� �ִٸ�
                {
                    switch (category)
                    {
                        //case InteractTypeCategory.WORKBENCH: // 2-2-1. WORKBENCH
                        // �ٱ����� ���� �ϴ� ����
                        //case InteractTypeCategory.ENVIRONMENT: // 2-2-2. ENVIRONMENT
                        //    Debug.Log("Case 2-2-2");
                        //    /* Do Nothing */
                        //    break;
                        case InteractTypeCategory.FRICTION: // 2-2-3. FRICTION
                        case InteractTypeCategory.ITEM: // 2-2-4. ITEM
                            Debug.Log("Case 2-2-3,4");
                            InteractItem targetItem = closestObject.GetComponent<StackDummy>().GetItem();
                            InteractItem playerItem = playerDummy.GetComponent<StackDummy>().GetItem();

                            if (targetItem.interactType == playerItem.interactType) // ���� �����̶��
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
                            else // �ٸ� �����̶��
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
    //    // ����� ��ü�� ���ų�, �տ� �� �� ���ٸ� return
    //    if (closestObject == null || !closestObject.GetComponent<StackableItem>().IsHandable())
    //    {
    //        return;
    //    }

    //    // ��ȣ�ۿ� ���� ������ Ȯ��
    //    StackableItem targetItem = closestObject.gameObject.GetComponent<StackableItem>();

    //    if (currentAmount == 0) // �տ� �� �� ���� ��
    //    {
    //        currentItem = targetItem.GetItem();
    //        PickUpItem(targetItem);
    //    }
    //    else // �տ� �������� ��� ���� ��
    //    {
    //        // ���� �������̶��
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

    // �÷��̾� ������ �ڵ� ��ġ�� ������ ����
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
