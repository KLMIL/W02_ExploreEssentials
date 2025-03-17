using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

using E_DataTypes;
using Unity.VisualScripting;
using System.Collections;
using NUnit.Framework.Constraints;

public class D3_PlayerController : MonoBehaviour
{
    /* Component Field */
    private Rigidbody rb;

    /* Player State Field */
    [SerializeField] private float moveSpeed = 10f;

    private Vector3 moveDir;
    private float moveX;
    private float moveZ;
    private Quaternion lastRotation;

    private bool isMining = false;

    /* Children Object */
    [SerializeField] private GameObject directionArrow; // ���� Ȯ���� ���� ȭ��ǥ ������Ʈ
    [SerializeField] private GameObject playerDummy;

    /* Interation Field */
    private List<GameObject> interacts = new List<GameObject>();
    [SerializeField] private GameObject closestObject = null;
    [SerializeField] private GameObject lastHighlightObject = null;


    /* Other Objects */
    D3_InteractManager interactManager;




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

        interactManager = D3_InteractManager.Instance;
    }

    private void Update()
    {
        if (!isMining)
        {
            InputMove();
            InputRotate();
            InputKeys();

            FindClosetObject();
        }
        
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
            if (playerDummy.GetComponent<D3_StackDummy>().GetItemAmount() == 0) // 1-1. �տ� ������ ���ٸ�
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
                interactManager.MakeNewStackDummy(playerDummy.GetComponent<D3_StackDummy>().GetItem().itemName,
                        dropPos, playerDummy.GetComponent<D3_StackDummy>().GetItemAmount()
                    );
                playerDummy.GetComponent<D3_StackDummy>().RemoveFromDummyPlayer(playerDummy.GetComponent<D3_StackDummy>().GetItemAmount());


                FindClosetObject();
            }
        }
        else // 2. ��ȣ�ۿ� ���� ������Ʈ�� �ִٸ�
        {
            Debug.Log("Case 2");
            InteractItem interactItem = closestObject.GetComponent<D3_StackDummy>().GetItem();
            InteractType interactType = closestObject.GetComponent<D3_StackDummy>().GetItemType();

            if (interactType == InteractType.OreGenerator) // 2-0. ���ʷ������� ���
            {
                Debug.Log("Generate Ores");
                closestObject.GetComponent<D3_OreGenerator>().MakeOre();
            }
            else if (interactType == InteractType.GoToShootingPang)
            {
                Debug.Log("Switch Dimention");
                closestObject.GetComponent<D3_DimentionSwitch>().Switch();
            }
            else
            {
                if (playerDummy.GetComponent<D3_StackDummy>().GetItemAmount() == 0) // 2-1. �տ� ������ ���ٸ�
                {
                    Debug.Log("Case 2-1");
                    InteractTypeCategory category = InteractTypeExtensions.GetCategory(interactType);

                    if (category == InteractTypeCategory.Workbench)
                    {

                    }

                    switch (category)
                    {
                        case InteractTypeCategory.Workbench: // 2-1-1. WORKBENCH
                            Debug.Log("Case 2-1-1");

                            // WorkBench�� ������ �ִٸ� ������
                            //InteractItem targetItem = closestObject.GetComponent<D3_StackDummy>().GetItem();
                            InteractItem targettItem = closestObject.GetComponent<D3_WorkbenchInput>().item;
                            int targettAmount = closestObject.GetComponent<D3_StackDummy>().GetItemAmount();

                            playerDummy.GetComponent<D3_StackDummy>().SetInfo(targettItem);

                            int added = playerDummy.GetComponent<D3_StackDummy>().AddToDummy(targettAmount);

                            closestObject.GetComponent<D3_StackDummy>().RemoveFromDummyWorkbench(added);

                            closestObject.GetComponent<D3_WorkbenchInput>().RemoveByByPlayer();

                            // ������ ����
                            break;
                        case InteractTypeCategory.Environment: // 2-1-2. ENVIRONMENT
                            Debug.Log("Case 2-1-2");
                            // ��ȣ�ۿ� -> ä��

                            StartCoroutine(MiningCoroutine(closestObject));



                            break;
                        case InteractTypeCategory.Friction: // 2-1-3. FRICTION
                        case InteractTypeCategory.Item: // 2-1-4. ITEM
                            Debug.Log("Case 2-1-3,4");
                            // ��ȣ�ۿ� -> ������ ����ŭ �ݱ�
                            D3_StackDummy targetDummy = closestObject.GetComponent<D3_StackDummy>();
                            int targetAmount = targetDummy.GetItemAmount();

                            playerDummy.GetComponent<D3_StackDummy>().SetInfo(targetDummy.GetItem());

                            int addedAmount = playerDummy.GetComponent<D3_StackDummy>().AddToDummy(targetAmount);
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
                    if (category == InteractTypeCategory.Workbench)
                    {
                        // ��ȣ�ۿ� ��ü�� ���� ������ ���¶��(�ڸ��� �ִٸ�) ���� �ֱ�
                        Debug.Log("Workbench connected");
                        InteractItem currItem = playerDummy.GetComponent<D3_StackDummy>().GetItem();
                        int currAmount = playerDummy.GetComponent<D3_StackDummy>().GetItemAmount();

                        int added = closestObject.GetComponent<D3_WorkbenchInput>().InputWorkbench(currItem, currAmount);

                        playerDummy.GetComponent<D3_StackDummy>().RemoveFromDummyPlayer(added);

                        // ������ ����
                    }

                    Debug.Log("Case 2-2-1");
                    // 2-2-1. �տ� ������ �� �� �ִٸ�
                    if (playerDummy.GetComponent<D3_StackDummy>().GetItemAmount() == playerDummy.GetComponent<D3_StackDummy>().GetCapacity())
                    {
                        // �ش� ���̿� ������ �������´�
                        Debug.Log("Case 2-2-1-1");
                        InteractItem targetItem = closestObject.GetComponent<D3_StackDummy>().GetItem();
                        InteractItem playerItem = playerDummy.GetComponent<D3_StackDummy>().GetItem();
                        if (targetItem.interactType == playerItem.interactType) // ���� �������� ���
                        {
                            Debug.Log("Case 2-2-1-1-1");
                            int targetAmount = playerDummy.GetComponent<D3_StackDummy>().GetItemAmount();
                            int added = closestObject.GetComponent<D3_StackDummy>().AddToDummy(targetAmount);
                            playerDummy.GetComponent<D3_StackDummy>().RemoveFromDummyPlayer(added);

                            //interacts.Remove(closestObject);
                            //closestObject = lastHighlightObject = null;
                            RemoveHighlight(closestObject);
                            ApplyHighlight(closestObject);
                            FindClosetObject();
                        }
                        else // �ٸ� �������� ���
                        {
                            Debug.Log("Case 2-2-1-1-2");
                            // ���� ����� �� ������ �������´�. 

                            Vector3 playerPos = transform.position;
                            Vector3 nearestPos = Vector3.zero;
                            float minDistance = Mathf.Infinity;

                            // �÷��̾� ���� ��ǥ Ȯ��
                            for (int x = -1; x <= 1; x++)
                            {
                                for (int z = -1; z <= 1; z++)
                                {
                                    if (x == 0 && z == 0) continue;

                                    Vector3 candidatePos = new Vector3(
                                            Mathf.Round(playerPos.x) + x,
                                            playerPos.y,
                                            Mathf.Round(playerPos.z) + z);

                                    bool isValid = true;

                                    Collider[] colliders = Physics.OverlapSphere(candidatePos, 0.1f);
                                    foreach (var collider in colliders)
                                    {
                                        if (collider.gameObject != gameObject && collider.GetComponent<D3_StackDummy>() != null)
                                        {
                                            isValid = false;
                                            break;
                                        }
                                    }

                                    if (isValid)
                                    {
                                        float distance = Vector3.Distance(playerPos, candidatePos);
                                        if (distance < minDistance)
                                        {
                                            minDistance = distance;
                                            nearestPos = candidatePos;
                                        }
                                    }
                                }
                            }

                            if (nearestPos != Vector3.zero)
                            {
                                interactManager.MakeNewStackDummy(
                                        playerDummy.GetComponent<D3_StackDummy>().GetItem().itemName,
                                        nearestPos,
                                        playerDummy.GetComponent<D3_StackDummy>().GetItemAmount()
                                        );
                                playerDummy.GetComponent<D3_StackDummy>().RemoveFromDummyPlayer(
                                        playerDummy.GetComponent<D3_StackDummy>().GetItemAmount()
                                        );
                                FindClosetObject();
                            }
                        }
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
                            case InteractTypeCategory.Friction: // 2-2-3. FRICTION
                            case InteractTypeCategory.Item: // 2-2-4. ITEM
                                Debug.Log("Case 2-2-3,4");
                                InteractItem targetItem = closestObject.GetComponent<D3_StackDummy>().GetItem();
                                InteractItem playerItem = playerDummy.GetComponent<D3_StackDummy>().GetItem();

                                if (targetItem.interactType == playerItem.interactType) // ���� �����̶��
                                {
                                    Debug.Log("Case 2-2-3,4-1");
                                    //int targetAmount = closestObject.GetComponent<D3_StackDummy>().GetItemAmount();
                                    //int added = playerDummy.GetComponent<D3_StackDummy>().AddToDummy(targetAmount);
                                    //closestObject.GetComponent<D3_StackDummy>().RemoveFromDummy(added);

                                    //if (targetAmount == added)
                                    //{
                                    //    interacts.Remove(closestObject);
                                    //    closestObject = lastHighlightObject = null;
                                    //    FindClosetObject();
                                    //}

                                    int targetAmount = playerDummy.GetComponent<D3_StackDummy>().GetItemAmount();
                                    int added = closestObject.GetComponent<D3_StackDummy>().AddToDummy(targetAmount);
                                    playerDummy.GetComponent<D3_StackDummy>().RemoveFromDummyPlayer(added);

                                    //interacts.Remove(closestObject);
                                    //closestObject = lastHighlightObject = null;
                                    RemoveHighlight(closestObject);
                                    ApplyHighlight(closestObject);
                                    FindClosetObject();
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
    }


    private IEnumerator MiningCoroutine(GameObject target)
    {
        isMining = true;
        yield return new WaitForSeconds(1f);

        if (target != null)
        {
            string frictionName = interactManager.GetFrictionNameByEnvironmentName(target.GetComponent<D3_StackDummy>().GetItem().itemName);
            interactManager.MakeNewStackDummy(frictionName, closestObject.transform.position, 1);

            Destroy(closestObject);
            interacts.Remove(closestObject);
            closestObject = lastHighlightObject = null;
            FindClosetObject();
        }

        isMining = false;
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

    public void RemoveAllInteractable()
    {
        interacts.Clear();
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
        D3_StackDummy stackDummy = obj.GetComponent<D3_StackDummy>();
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
        D3_StackDummy stackDummy = obj.GetComponent<D3_StackDummy>();
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



    /* Getter, Setter */
    public D3_StackDummy GetPlayerStackDummy()
    {
        return playerDummy.GetComponent<D3_StackDummy>();
    }
}
