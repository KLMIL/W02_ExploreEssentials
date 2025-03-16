using E_DataTypes;
using System.Collections.Generic;

namespace E_DataTypes
{
    // 아이템의 상호작용 분류 정의
    public enum InteractType
    {
        /// <summary>
        /// N/A Object
        /// </summary>
        None = 0,
        /// <summary>
        /// 1 ~ 10: Workbench Objects -> Insert correct items
        /// </summary>
        Workbench = 1,
        /// <summary>
        /// 10 ~ 19: Environment object -> Make to item
        /// </summary>
        Environment = 10,
        Coal = 11,
        Gold = 12,
        Iron = 13,
        Stone = 14,
        /// <summary>
        /// 20 ~ 29: First processed object -> Can grabable
        /// </summary>
        Friction = 20,
        Coalbar = 21,
        Goldbar = 22,
        Ironbar = 23,
        Stonebar = 24,
        /// <summary>
        /// 30 ~ 39: Processed item object made by frictions -> Can grabable
        /// </summary>
        Item = 30,
        /// <summary>
        /// 40 ~ 59: Item Dummis
        /// </summary>
        Dummy = 40
    }

    public enum InteractTypeCategory
    {
        None = 0,
        Workbench = 1,
        Environment = 10,
        Friction = 20,
        Item = 30,
        Dummy = 40
    }
}


public static class InteractTypeExtensions
{
    public static InteractTypeCategory GetCategory(InteractType itemType)
    {
        if (0 <= (int)itemType && (int)itemType < 10)
        {
            return InteractTypeCategory.Workbench;
        }
        if (10 <= (int)itemType && (int)itemType < 20)
        {
            return InteractTypeCategory.Environment;
        }
        if (20 <= (int)itemType && (int)itemType < 30)
        {
            return InteractTypeCategory.Friction;
        }
        if (30 <= (int)itemType && (int)itemType < 40)
        {
            return InteractTypeCategory.Item;
        }
        if (40 <= (int)itemType && (int)itemType < 60)
        {
            return InteractTypeCategory.Dummy;
        }

        return InteractTypeCategory.None;
    }
}