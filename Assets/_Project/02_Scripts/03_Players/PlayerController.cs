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
    [SerializeField] private GameObject stackItemPoint; // ȹ���� �������� ���� ��ġ ������Ʈ

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
        if (closestObject == null) // 1-1. ����� ��ü�� ���ٸ�
        {
            if (currentAmount == 0) // 1-1-1. �տ� �� ��ü�� ���ٸ�
            {
                /* Do Nothing */
            }
            else // 1-1-2. �տ� �� ��ü�� �ִٸ�
            {
                // ������ ���
            }
        }
        else // 1-2. ����� ��ü�� �ִٸ�
        {
            StackableItem targetItem = closestObject.GetComponent<StackableItem>();
            InteractTypeCategory category = InteractTypeExtensions.GetCategory(targetItem.GetInteractType());

            if (currentAmount == 0) // 1-2-1. �տ� �� ��ü�� ���ٸ�
            {
                if (category == InteractTypeCategory.WORKBENCH) // 1-2-1-1. ��ȣ�ۿ� ��ü���
                {
                    /* Do Nothing */
                }
                if (category == InteractTypeCategory.ENVIRONMENT)  // 1-2-1-2. �����̶��
                {
                    // �ش� ��ġ�� ��ȣ�ۿ� ����� ������ ����
                    interactManager.MakePrefabInteract(targetItem.GetInteractType(), closestObject.transform.position);

                    // ��ȣ�ۿ� ��ü ���� �� ���� ����� ��ü �˻�
                    Destroy(closestObject);
                    interacts.Remove(closestObject);
                    closestObject = lastHighlightObject = null;
                    FindClosetObject();
                }
                if (category == InteractTypeCategory.FRICTION
                    || category == InteractTypeCategory.ITEM) // 1-2-1-3. �ֱ�, �������̶��
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
            else // 1-2-2. �տ� �� ��ü�� �ִٸ�
            {
                if (category == InteractTypeCategory.WORKBENCH) // 1-2-2-1. ��ȣ�ۿ� ��ü���
                {
                    // 1-2-2-1-1. ��ȣ�ۿ� ��ü�� ���� �� �ִٸ�
                    // ������ �ֱ�

                    // 1-2-2-1-2. ��ȣ�ۿ� ��ü�� ���� �� ���ٸ�
                    /* Do Nothing */
                }
                if (category == InteractTypeCategory.ENVIRONMENT) // 1-2-2-2. �����̶��
                {
                    /* Do Nothing */
                }
                if (category == InteractTypeCategory.FRICTION
                                    || category == InteractTypeCategory.ITEM) // 1-2-2-3. �ֱ�, �������̶��
                {
                    if (currentItem.itemName == targetItem.GetItemName()) // 1-2-2-3-1���� �������
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
                    else // 1-2-2-3-2�ٸ� �������
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
        // ����� ��ü�� ���ų�, �տ� �� �� ���ٸ� return
        if (closestObject == null || !closestObject.GetComponent<StackableItem>().IsHandable())
        {
            return;
        }

        // ��ȣ�ۿ� ���� ������ Ȯ��
        StackableItem targetItem = closestObject.gameObject.GetComponent<StackableItem>();

        if (currentAmount == 0) // �տ� �� �� ���� ��
        {
            currentItem = targetItem.GetItem();
            PickUpItem(targetItem);
        }
        else // �տ� �������� ��� ���� ��
        {
            // ���� �������̶��
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

    // �÷��̾� ������ �ڵ� ��ġ�� ������ ����
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
