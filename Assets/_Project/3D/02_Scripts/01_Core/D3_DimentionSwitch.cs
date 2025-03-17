using UnityEngine;

public class D3_DimentionSwitch : MonoBehaviour
{
    [SerializeField] private SuperGameManager superGameManager;

    public void Switch()
    {
        superGameManager.GetComponent<DimensionSwitch>().Switch();
    }
}
