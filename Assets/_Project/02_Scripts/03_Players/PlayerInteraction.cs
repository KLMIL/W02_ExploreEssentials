using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    /*
     * Player의 자식객체 InteractionPoint에 할당되는 스크립트.
     * Trigger Box에 Object가 Enter, Exit 될 때 이벤트를 수행한다.
     *
     * 아이템에 상호작용한다. 
     *  - 자연물 -> 1차가공물
     */

    private PlayerController playerController;

    private void Start()
    {
        playerController = GetComponentInParent<PlayerController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Interactable"))
        {
            playerController.AddInteractable(other.gameObject);
            //Debug.Log($"Trigger Enterd: {other.gameObject.GetInstanceID()}");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Interactable"))
        {
            playerController.RemoveInteractable(other.gameObject);
            //Debug.Log($"Trigger Exit: {other.gameObject.GetInstanceID()}");
        }
    }
}
