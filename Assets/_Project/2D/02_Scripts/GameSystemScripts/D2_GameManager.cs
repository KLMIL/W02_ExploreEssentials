using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

using DataTypes;
using System;
using UnityEngine.UI;
using UnityEngine.Experimental.AI;


public class D2_GameManager : MonoBehaviour
{
    public static D2_GameManager Instance { get; private set; }

    // MonoBehaviour ����


    // ���� �����Ȳ ����
    int currentStage = 0;
    bool isGameInProgress = true;
    bool isGameReset = true;
    bool isStageEnd = false;

    [SerializeField] public int currentCoin; // remainCoin���� ����
    //[SerializeField] public int currentBullet; // ���� PlayerController�� �־�� �ϴ� ���� 
    [SerializeField] public int[] currentItems; // ���� ItemController(�� ItemManager)�� �ִ� ����


    // ���� �ý��� ����
    //public List<GameObject> stagePrefabs;
    //public List<GameObject> stageEventPanels;
    public TextMeshProUGUI textCoin;
    //public TextMeshProUGUI textBullet;
    public List<TextMeshProUGUI> textItems;

    GameObject currentStageReference;


    public GameObject GameEndPanel;
    public GameObject StageClearPanel;
    public GameObject StageFailPanel;

    bool[][] isGameEnded = new bool[3][] {
    new bool[] { false, false, false, false },  // ù ��° ��
    new bool[] { false, false, false, false },  // �� ��° ��
    new bool[] { false, false, false }           // �� ��° ��
    };


    public Button[] ButtonGorup1 = new Button[4];
    public Button[] ButtonGorup2 = new Button[4];
    public Button[] ButtonGorup3 = new Button[3];



    /*
     * Lifecycle Functions: MonoBehaviour �Լ�
     */
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        //DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        // �ʱ�ȭ �Լ� �ۼ� ����
        //currentCoin = 50;
        //currentBullet = 50;
        //currentItems = new int[] { 50, 50, 50, 50 };
        InitText();
    }



    /*
     * Game Progress Functions: ���� �����Ȳ�� ���õ� �Լ�
     */
    //public void CollectCoin() // GainCoin���� ����
    //{
    //    // ���� ���� �����ϰ� UI�� �ݿ�
    //    currentCoin--;
    //    textCoin.SetText($"Rest Coin \n X {currentCoin}");

    //    if (currentCoin == 0)
    //    {
    //        isGameInProgress = false;
    //        stageEventPanels[(int)GamePanel.NEXT_STAGE].SetActive(true);
    //    }
    //}

    //public void UsedBullet() // NoBullet
    //{
    //    // Bullet ��� ���� ��������
    //    if (currentCoin != 0)
    //    {
    //        //isStageEnd = true; // �ӽ� ����. ��� �ǰ� ��������.
    //        isGameInProgress = false;
    //        RetryNow();
    //    }
    //    else
    //    {
    //        isGameInProgress = true;
    //        stageEventPanels[(int)GamePanel.NEXT_STAGE].SetActive(true);
    //    }
    //}




    /*
     * Game System Functions: ���� �ý��۰� ���õ� �Լ�
     */
    public void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    //public void NextStage()
    //{
    //    StopAllCoroutines(); // �� ���� ���� ���׶����� �־��� �ڵ�

    //    /* ���� ���� ����� */
    //    if (currentStage == 4)
    //    {
    //        // ��� panel ��Ȱ��ȭ
    //        for (int i = 0; i < stageEventPanels.Count; i++)
    //        {
    //            stageEventPanels[i].SetActive(false);
    //        }
    //        stageEventPanels[(int)GamePanel.GAME_END].SetActive(true);
    //    }

    //    /* ���� �������� Ȱ��ȭ */
    //    stageEventPanels[(int)GamePanel.NEXT_STAGE].SetActive(true);

    //    Destroy(currentStageReference);
    //    currentStageReference = Instantiate(stagePrefabs[++currentStage]);

    //    isGameInProgress = true;
    //    isGameReset = true;

    //    // PlayerController���� Bullet ����� �Լ� ȣ���ϰ�, ������ ������ index �ʱ�ȭ
    //    // ItemController���� ������ ��� �Լ� ȣ��
    //}

    //public void RetryNow()
    //{
    //    //if (canRetry) // �� ���� �ʿ���°� ���Ƽ� �ӽ� ����
    //    //{
    //    StopAllCoroutines(); // �� ���� ���� ���׶����� �־��� �ڵ�

    //    //stageEventPanels[(int)GamePanel.GAME_OVER].SetActive(true);
    //    Destroy(currentStageReference);
    //    currentStageReference = Instantiate(stagePrefabs[currentStage]);

    //    isGameInProgress = true;
    //    //isGameReset = true; // ��𾲴°��� 1
    //    //isStageEnd = false; // ��𾲴°��� 2

    //    // PlayerController Bullet ����� �Լ� ȣ��, SelectItem�� PlayerAvailable ���� �ʱ�ȭ
    //    // ItemController���� UseItem ȣ��

    //    //}
    //}


    public void QuitGame()
    {
        Application.Quit();
    }






    /* Getter */
    public int[] GetCurrentItems() // const ���� �ؾ���
    {
        return currentItems;
    }

    public void UseCurrentItems(int itemIndex)
    {
        currentItems[itemIndex]--;
    }


    public bool HaveItem(int itemIndex)
    {
        return (currentItems[itemIndex] != 0);
    }
 
    

    public void InitText()
    {
        //textBullet.text = $"Rest Bullet\n X {currentBullet}";
        //textCoin.text = $"Rest Coin\n X {currentCoin}";
        //textItems[0].text = $"{D2_ItemManager.Instance.itemStr[0]} X {currentItems[0]}";
        //textItems[1].text = $"{D2_ItemManager.Instance.itemStr[1]} X {currentItems[1]}";
        Debug.Log("Text Init");

        Debug.Log($"Is Null text? {textItems[0].text == null}");

        textItems[0].text = $"Normal \n Bullet \n X {currentItems[0]}";
        textItems[1].text = $"Bomb \n Bullet \n X {currentItems[1]}";
        textItems[2].text = $"Magnet \n Bullet \n X {currentItems[2]}";
    }

    //public void UseBullet()
    //{
    //    currentBullet--;
    //    textBullet.text = $"Rest Bullet\n X {currentBullet}";

    //    if (currentBullet == 0)
    //    {
    //        // ���� ���� �г� Ȱ��ȭ�ϰ� �̰����� ó��
    //        StageFailPanel.SetActive(true);
    //    }
    //}

    public void SetClear(int group, int stage)
    {
        isGameEnded[group][stage] = true;

        if (group == 0)
        {
            ButtonGorup1[stage].GetComponent<Image>().color = Color.gray;
        }
        else if (group == 1)
        {
            ButtonGorup2[stage].GetComponent<Image>().color = Color.gray;
        }
        else
        {
            ButtonGorup3[stage].GetComponent<Image>().color = Color.gray;
        }
    }

    public void CollectCoin()
    {
        currentCoin--;
        textCoin.text = $"Rest Coin\n X {currentCoin}";

        if (currentCoin == 0)
        {
            // �������� Ŭ���� �г� Ȱ��ȭ�ϰ� �̰����� ����
            StageClearPanel.SetActive(true);
            D2_StageManager.Instance.SetClear();


            bool flag = true;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < isGameEnded[i].Length; j++)
                {
                    if (isGameEnded[i][j] == false)
                    {
                        flag = false;
                    }
                }
            }

            if (flag)
            {
                GameEndPanel.SetActive(true);
            }
        }
    }

    public void UseItem(int itemIndex)
    {
        //currentItems[itemIndex]--;
        textItems[itemIndex].text = $"{D2_ItemManager.Instance.itemStr[itemIndex]} X {currentItems[itemIndex]}";
    }


    public void SelectItem(int itemIndex)
    {
        D2_PlayerController.Instance.SelectItem(itemIndex);
    }




}
