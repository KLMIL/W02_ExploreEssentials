using UnityEngine;

public class D3_OreGenerator : MonoBehaviour
{
    [SerializeField] private D3_GameManager D3_gameManager;

    public void MakeOre()
    {
        if (D2_GameManager.Instance.rerollCoin > 0)
        {
            D3_gameManager.MakeRandomOre();
            D2_GameManager.Instance.rerollCoin--;
            D2_GameManager.Instance.rerollText.text = $"Reroll\nCoin\nX{D2_GameManager.Instance.rerollCoin}";
        }
        else
        {
            Debug.Log("reroll coin is 0");
        }
    }
}
