using UnityEngine;

using DataTypes;
using System.Collections.Generic;
using UnityEngine.UI;


public class D2_ItemManager : MonoBehaviour
{
    public static D2_ItemManager Instance { get; private set; }

    public string[] itemStr =
    {
        //"Bomb", "Magnet", "KnockBack", "Ghost", "Cleaner", "ZeroGravity"
        "Normal", "Bomb", "Magnet"
    };


    D2_GameManager gameManager;
    D2_PlayerManager playerManager;


    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        gameManager = D2_GameManager.Instance;
        playerManager = D2_PlayerManager.Instance;
    }



    public void UseItem(int itemIndex)
    {
        //gameManager.UseCurrentItems(itemIndex);
        //playerHUDController.RefreshItemDisplay();
        switch(itemIndex)
        {
            case (int)GameItem.Normal:
                gameManager.UseItem(0);
                break;
            case (int)GameItem.Bomb:
                gameObject.GetComponent<ItemBomb>().Use();
                //Debug.Log(playerManager.GetBulletReference().transform.Find("DEP_Bomb Effect"));
                //playerManager.GetBulletReference().transform.Find("DEP_Bomb Effect").gameObject.SetActive(true);
                playerManager.GetBulletReference().GetComponent<D2_Bullet>().MakeParticle(0);
                //D2_GameManager.Instance.GetComponent<D2_SoundManager>().PlaySFX(D2_SoundManager.Instance.audios[(int)GameSound.BOMB_SOUND]);
                //playerManager.GetBulletReference().GetComponent<Material>().color = Color.blue;
                //GameObject.Find("Item0").gameObject.GetComponent<Image>().color = Color.white;
                gameManager.UseItem(1);
                break;
            case (int)GameItem.Magnet:
                gameObject.GetComponent<ItemMagnet>().Use();
                //playerManager.GetBulletReference().transform.Find("DEP_Magnetic Effect").gameObject.SetActive(true);
                playerManager.GetBulletReference().GetComponent<D2_Bullet>().MakeParticle(1);
                //D2_GameManager.Instance.GetComponent<D2_SoundManager>().PlaySFX(D2_SoundManager.Instance.audios[(int)GameSound.MAGNET_SOUND]);
                //playerManager.GetBulletReference().GetComponent<Material>().color = Color.red;
                //GameObject.Find("Item1").gameObject.GetComponent<Image>().color = Color.white;
                gameManager.UseItem(2);
                break;
            default:
                break;
        }

        //Debug.Log($"Item Used: {itemStr[itemIndex]}");
        //playerManager.UnselectItem();
        //playerManager.DestroyBullet();
        //Debug.Log("ItemManager :: UseItem End");
    }

    public void ApplyItemEffect(int itemIndex)
    {
        // 아이템 능력 적용지점
    }


    // GETTER, SETTER
    public string GetItemNameByIndex(int index)
    {
        return itemStr[index];
    }
}
