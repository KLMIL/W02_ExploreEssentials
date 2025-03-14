using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    /*
     * Player�� �ڽİ�ü InteractionPoint�� �Ҵ�Ǵ� ��ũ��Ʈ.
     * Trigger Box�� Object�� Enter, Exit �� �� �̺�Ʈ�� �����Ѵ�.
     *
     * �����ۿ� ��ȣ�ۿ��Ѵ�. 
     *  - �ڿ��� -> 1��������
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
