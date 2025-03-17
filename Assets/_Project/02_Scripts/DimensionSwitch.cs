using System.Collections;
using System.Transactions;
using UnityEngine;
using UnityEngine.UI;

public class DimensionSwitch : MonoBehaviour
{
    public Camera camera2D;
    public Camera camera3D;
    //public GameObject player2D;
    //public GameObject player3D;

    public GameObject stagePanel;

    public GameObject BackToLobbyButton;

    private bool is2DMode = true;


    private Vector3 origin3DPos = new Vector3(1.15f, 15f, -8.3f);
    private Vector3 origin3DRot = new Vector3(66f, 5f, 0);

    private Vector3 trans3DPos = new Vector3(-11f, 1.4f, -5f);
    private Vector3 trans3DRot = new Vector3(90f, 270f, 0);

    private float moveDuration = 1f;



    private void Start()
    {
        //SwitchTo2D();
    }

    public void SwitchTo2D()
    {
        if (is2DMode) return;

        is2DMode = true;

        BackToLobbyButton.SetActive(true);

        if (camera3D.gameObject.activeSelf)
        {
            StartCoroutine(MoveCameraAndSwitch(camera3D, trans3DPos, Quaternion.Euler(trans3DRot), Perform2DSwitch));
        }
        else
        {
            Perform2DSwitch();
        }
    }

    public void SwitchTo3D()
    {
        if (!is2DMode) return;

        is2DMode = false;

        BackToLobbyButton.SetActive(false);

        Perform3DSwitch();
        StartCoroutine(MoveCameraAndSwitch(camera3D, origin3DPos, Quaternion.Euler(origin3DRot), Perform3DSwitch));
    }

    public void Perform2DSwitch()
    {
        camera2D.gameObject.SetActive(true);
        camera3D.gameObject.SetActive(false);
        stagePanel.SetActive(true);

        //player2D.SetActive(true);
        //player3D.SetActive(false);
    }

    public void Perform3DSwitch()
    {
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


    private IEnumerator MoveCameraAndSwitch(Camera camera, Vector3 targetPos, Quaternion targetRot, System.Action onComplete)
    {
        float elapsedTime = 0f;
        Vector3 startPos = camera.transform.position;
        Quaternion startRot = camera.transform.rotation;

        while (elapsedTime < moveDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / moveDuration;

            t = t * t * (3f - 2f * t);

            camera.transform.position = Vector3.Lerp(startPos, targetPos, t);
            camera.transform.rotation = Quaternion.Lerp(startRot, targetRot, t);

            yield return null;
        }

        camera.transform.position = targetPos;
        camera.transform.rotation = targetRot;

        onComplete?.Invoke();
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
