using Game.Inventory;
using GameFramework;
using System.Collections.Generic;
using UnityEngine;

namespace Game.DungeonModule
{
    public class DungeonPlayer
    {
        public MapLayout map;

        public Vector2Int Position;

        public Dictionary<ItemType,Dictionary<uint,ItemStack>> inventoryData = new Dictionary<ItemType, Dictionary<uint, ItemStack>>();
        public Dictionary<string, LocalItemStack> dungeonInventoryData = new Dictionary<string, LocalItemStack>();

        public void Gain(ItemType itemType,uint itemId,uint count)
        {
            if (!inventoryData.TryGetValue(itemType, out var itemDic))
            {
                itemDic = new Dictionary<uint, ItemStack>();
                inventoryData[itemType] = itemDic;
            }

            if (!itemDic.TryGetValue(itemId, out ItemStack itemStack))
            {
                itemStack = new ItemStack();
                itemStack.Id = itemId;
                itemDic[itemId] = itemStack;
            }
            itemStack.Count += count;
            itemDic[itemId] = itemStack;
            Events.Publish<InventoryEvents.Changed>();
        }

        public void GainLocalItem(string itemKey, uint count)
        {
            Debug.Log($"GainLocalItem,Key:{itemKey},Count:{count}");
            LocalItemStack localItemStack = default;
            if (!dungeonInventoryData.TryGetValue(itemKey, out localItemStack))
            {
                localItemStack = new LocalItemStack();
                localItemStack.Key = itemKey;
                dungeonInventoryData[itemKey] = localItemStack;
            }

            localItemStack.Count += count;
            dungeonInventoryData[itemKey] = localItemStack;
            Events.Publish<InventoryEvents.Changed>();
        }



    }

}