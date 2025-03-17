using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class D2_StageManager : MonoBehaviour
{
    public static D2_StageManager Instance { get; private set; }

    public GameObject stagePanel;

    public List<GameObject> TutorialMapPrefab;
    public List<GameObject> BombMapPrefab;
    public List<GameObject> MagnetMapPrefab;


    //public List<List<GameObject>> MapPrefabs;

    GameObject stageReference;
    int group = -1;
    int curStage = -1;


    private void Awake()
    {
        Instance = this;
    }

    public void StartTutorialStage(int stage)
    {
        Destroy(stageReference);
        stageReference = Instantiate(TutorialMapPrefab[stage], transform.position, Quaternion.identity);
        stagePanel.SetActive(false);

        group = 0;
        curStage = stage;
    }

    public void StartBombStage(int stage)
    {
        Destroy(stageReference);
        stageReference = Instantiate(BombMapPrefab[stage], transform.position, Quaternion.identity);
        stagePanel.SetActive(false);

        group = 1;
        curStage = stage;
    }

    public void StartMagnetStage(int stage)
    {
        Destroy(stageReference);
        stageReference = Instantiate(MagnetMapPrefab[stage], transform.position, Quaternion.identity);
        stagePanel.SetActive(false);

        group = 2;
        curStage = stage;
    }

    public void BackToLobby()
    {
        if (D2_PlayerManager.Instance.GetBulletReference())
        {
            Destroy(D2_PlayerManager.Instance.GetBulletReference());
        }
        if (stageReference)
        {
            Destroy(stageReference);
        }
    }

    public void SetClear()
    {
        BackToLobby();
        D2_GameManager.Instance.SetClear(group, curStage);
    }
}
