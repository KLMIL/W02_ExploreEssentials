using UnityEngine;

public class D3_OreGenerator : MonoBehaviour
{
    [SerializeField] private D3_GameManager D3_gameManager;

    public void MakeOre()
    {
        D3_gameManager.MakeRandomOre();
    }
}
