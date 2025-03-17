using UnityEngine;

namespace DataTypes
{
    public enum GamePanel
    {
        /// <summary> �������� ���� </summary>
        GAME_OVER = 0,
        /// <summary> �������� ���� </summary>
        NEXT_STAGE = 1,
        /// <summary> ��� �������� ���� </summary>
        GAME_END = 2
    }

    public enum GameItem
    {
        Normal = 0,
        Bomb = 1,
        Magnet = 2


        ///// <summary> ��ź </summary> 
        //BOMB = 0,
        ///// <summary> �ڼ� </summary> 
        //MAGNET = 1,
        ///// <summary> ��ġ�� </summary> 
        //KNOCKBACK = 2,
        ///// <summary> </summary> 
        //GHOTS = 3,
        ///// <summary> </summary> 
        //CLEANER = 4,
        ///// <summary> </summary> 
        //ZEROGRAVITY = 5
    }

    public enum GameSound
    {
        BOUNCE_BALL = 0,
        BOMB_SOUND = 1,
        MAGNET_SOUND = 2,
        COIN_SOUND = 3,
        RESET_SOUND = 4,
        BUTTON_SOUND = 5
    }
}
