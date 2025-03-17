using UnityEngine;
using UnityEngine.UI;

public class DimensionSwitch : MonoBehaviour
{
    public Camera camera2D;
    public Camera camera3D;
    //public GameObject player2D;
    //public GameObject player3D;

    public GameObject stagePanel;

    private bool is2DMode = true;


    private void Start()
    {
        SwitchTo2D();
    }

    public void SwitchTo2D()
    {
        is2DMode = true;

        camera2D.gameObject.SetActive(true);
        camera3D.gameObject.SetActive(false);
        stagePanel.SetActive(true);

        //player2D.SetActive(true);
        //player3D.SetActive(false);xx
    }

    public void SwitchTo3D()
    {
        is2DMode = false;

        camera2D.gameObject.SetActive(false);
        camera3D.gameObject.SetActive(true);
        stagePanel.SetActive(false);

        //player2D.SetActive(false);
        //player3D.SetActive(true);
    }

    public void Switch()
    {
        if (!is2DMode)
        {
            SwitchTo2D();
        }
        else
        {
            SwitchTo3D();
        }
    }

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Tab))
    //    {
    //        if (!is2DMode)
    //        {
    //            SwitchTo2D();
    //        }
    //        else
    //        {
    //            SwitchTo3D();
    //        }
    //    }
    //}
}
