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
        NONE= 0,
        /// <summary>
        /// 1 ~ 10: Workbench Objects -> Insert correct items
        /// </summary>
        WORKBENCH = 1,
        /// <summary>
        /// 10 ~ 19: Environment object -> Make to item
        /// </summary>
        ENVIRONMENT = 10,
        /// <summary>
        /// 20 ~ 29: First processed object -> Can grabable
        /// </summary>
        FRICTION = 20,
        /// <summary>
        /// 20 ~ 29: Processed item object made by frictions -> Can grabable
        /// </summary>
        ITEM = 30
    }

    public enum InteractTypeCategory
    {
        NONE = 0,
        WORKBENCH = 1,
        ENVIRONMENT = 10,
        FRICTION = 20,
        ITEM = 30
    }
}


public static class InteractTypeExtensions
{
    public static InteractTypeCategory GetCategory(InteractType itemType)
    {
        if (0 <= (int)itemType && (int)itemType < 10)
        {
            return InteractTypeCategory.WORKBENCH;
        }
        if (10 <= (int)itemType && (int)itemType < 20)
        {
            return InteractTypeCategory.ENVIRONMENT;
        }
        if (20 <= (int)itemType && (int)itemType < 30)
        {
            return InteractTypeCategory.FRICTION;
        }
        if (30 <= (int)itemType && (int)itemType < 40)
        {
            return InteractTypeCategory.ITEM;
        }
        return InteractTypeCategory.NONE;
    }
}