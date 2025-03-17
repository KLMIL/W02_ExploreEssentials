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

    // MonoBehaviour 변수


    // 게임 진행상황 변수
    int currentStage = 0;
    bool isGameInProgress = true;
    bool isGameReset = true;
    bool isStageEnd = false;

    [SerializeField] public int currentCoin; // remainCoin에서 변경
    //[SerializeField] public int currentBullet; // 원래 PlayerController에 있어야 하는 변수 
    [SerializeField] public int[] currentItems; // 원래 ItemController(현 ItemManager)에 있던 변수


    // 게임 시스템 변수
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
    new bool[] { false, false, false, false },  // 첫 번째 행
    new bool[] { false, false, false, false },  // 두 번째 행
    new bool[] { false, false, false }           // 세 번째 행
    };


    public Button[] ButtonGorup1 = new Button[4];
    public Button[] ButtonGorup2 = new Button[4];
    public Button[] ButtonGorup3 = new Button[3];



    /*
     * Lifecycle Functions: MonoBehaviour 함수
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
        // 초기화 함수 작성 예정
        //currentCoin = 50;
        //currentBullet = 50;
        //currentItems = new int[] { 50, 50, 50, 50 };
        InitText();
    }



    /*
     * Game Progress Functions: 게임 진행상황과 관련된 함수
     */
    //public void CollectCoin() // GainCoin에서 변경
    //{
    //    // 코인 개수 변경하고 UI에 반영
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
    //    // Bullet 사용 로직 가져오기
    //    if (currentCoin != 0)
    //    {
    //        //isStageEnd = true; // 임시 제거. 없어도 되게 만들어야함.
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
     * Game System Functions: 게임 시스템과 관련된 함수
     */
    public void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    //public void NextStage()
    //{
    //    StopAllCoroutines(); // 공 제거 관련 버그때문에 넣었던 코드

    //    /* 게임 종료 컨디션 */
    //    if (currentStage == 4)
    //    {
    //        // 모든 panel 비활성화
    //        for (int i = 0; i < stageEventPanels.Count; i++)
    //        {
    //            stageEventPanels[i].SetActive(false);
    //        }
    //        stageEventPanels[(int)GamePanel.GAME_END].SetActive(true);
    //    }

    //    /* 다음 스테이지 활성화 */
    //    stageEventPanels[(int)GamePanel.NEXT_STAGE].SetActive(true);

    //    Destroy(currentStageReference);
    //    currentStageReference = Instantiate(stagePrefabs[++currentStage]);

    //    isGameInProgress = true;
    //    isGameReset = true;

    //    // PlayerController에서 Bullet 재생성 함수 호출하고, 선택한 아이템 index 초기화
    //    // ItemController에서 아이템 사용 함수 호출
    //}

    //public void RetryNow()
    //{
    //    //if (canRetry) // 이 조건 필요없는거 같아서 임시 제거
    //    //{
    //    StopAllCoroutines(); // 공 제거 관련 버그때문에 넣었던 코드

    //    //stageEventPanels[(int)GamePanel.GAME_OVER].SetActive(true);
    //    Destroy(currentStageReference);
    //    currentStageReference = Instantiate(stagePrefabs[currentStage]);

    //    isGameInProgress = true;
    //    //isGameReset = true; // 어디쓰는거지 1
    //    //isStageEnd = false; // 어디쓰는거지 2

    //    // PlayerController Bullet 재생성 함수 호출, SelectItem과 PlayerAvailable 변수 초기화
    //    // ItemController에서 UseItem 호출

    //    //}
    //}


    public void QuitGame()
    {
        Application.Quit();
    }






    /* Getter */
    public int[] GetCurrentItems() // const 생각 해야함
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
    //        // 게임 지는 패널 활성화하고 이것저것 처리
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
            // 스테이지 클리어 패널 활성화하고 이것저것 정리
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
