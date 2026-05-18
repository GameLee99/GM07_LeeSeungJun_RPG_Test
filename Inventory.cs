using System;
using System.Collections.Generic;

namespace RPG_Test
{
    internal class Inventory
    {
        private List<Item> items = new List<Item>();

        public int Count
        {
            get { return items.Count; }
        }

        public void AddItem(Item item)
        {
            AddItem(item, true);
        }

        public void AddItem(Item item, bool showMessage)
        {
            if (item == null)
            {
                return;
            }

            items.Add(item);

            if (showMessage)
            {
                Console.WriteLine(item.Name + "을(를) 획득했습니다.");
            }
        }

        public bool RemoveItem(Item item)
        {
            return items.Remove(item);
        }

        public Item GetItem(int index)
        {
            if (index < 0 || index >= items.Count)
            {
                return null;
            }

            return items[index];
        }

        public int GetGroupCount()
        {
            List<Item> groupedItems = new List<Item>();
            List<int> groupedCounts = new List<int>();

            BuildGroups(groupedItems, groupedCounts);

            return groupedItems.Count;
        }

        public Item GetGroupRepresentative(int groupIndex)
        {
            List<Item> groupedItems = new List<Item>();
            List<int> groupedCounts = new List<int>();

            BuildGroups(groupedItems, groupedCounts);

            if (groupIndex < 0 || groupIndex >= groupedItems.Count)
            {
                return null;
            }

            return groupedItems[groupIndex];
        }

        public int GetGroupQuantity(int groupIndex)
        {
            List<Item> groupedItems = new List<Item>();
            List<int> groupedCounts = new List<int>();

            BuildGroups(groupedItems, groupedCounts);

            if (groupIndex < 0 || groupIndex >= groupedCounts.Count)
            {
                return 0;
            }

            return groupedCounts[groupIndex];
        }

        public bool GroupContainsEquippedItem(int groupIndex, Player player)
        {
            Item representative = GetGroupRepresentative(groupIndex);

            if (representative == null || player == null)
            {
                return false;
            }

            for (int i = 0; i < items.Count; i++)
            {
                if (IsSameItem(items[i], representative) && player.IsEquippedItem(items[i]))
                {
                    return true;
                }
            }

            return false;
        }

        public int GetGroupSellableCount(int groupIndex, Player player)
        {
            Item representative = GetGroupRepresentative(groupIndex);

            if (representative == null)
            {
                return 0;
            }

            int count = 0;

            for (int i = 0; i < items.Count; i++)
            {
                if (IsSameItem(items[i], representative))
                {
                    if (player == null || player.IsEquippedItem(items[i]) == false)
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        public int RemoveMatchingItems(Item targetItem, int quantity, Player player)
        {
            if (targetItem == null || quantity <= 0)
            {
                return 0;
            }

            int removedCount = 0;

            for (int i = items.Count - 1; i >= 0; i--)
            {
                if (IsSameItem(items[i], targetItem))
                {
                    if (player != null && player.IsEquippedItem(items[i]))
                    {
                        continue;
                    }

                    items.RemoveAt(i);
                    removedCount++;

                    if (removedCount >= quantity)
                    {
                        break;
                    }
                }
            }

            return removedCount;
        }

        public void ShowItems(Player player)
        {
            Console.WriteLine("=== 인벤토리 ===");

            int groupCount = GetGroupCount();

            if (groupCount == 0)
            {
                Console.WriteLine("인벤토리가 비어 있습니다.");
                return;
            }

            for (int i = 0; i < groupCount; i++)
            {
                Item representative = GetGroupRepresentative(i);
                int quantity = GetGroupQuantity(i);

                Console.Write((i + 1) + ". ");
                representative.PrintInfo();
                Console.Write(" x" + quantity);

                if (GroupContainsEquippedItem(i, player))
                {
                    Console.Write(" [장착 중 포함]");
                }

                Console.WriteLine();
            }
        }

        private void BuildGroups(List<Item> groupedItems, List<int> groupedCounts)
        {
            for (int i = 0; i < items.Count; i++)
            {
                bool found = false;

                for (int j = 0; j < groupedItems.Count; j++)
                {
                    if (IsSameItem(items[i], groupedItems[j]))
                    {
                        groupedCounts[j]++;
                        found = true;
                        break;
                    }
                }

                if (found == false)
                {
                    groupedItems.Add(items[i]);
                    groupedCounts.Add(1);
                }
            }
        }

        private bool IsSameItem(Item item1, Item item2)
        {
            if (item1 == null || item2 == null)
            {
                return false;
            }

            if (item1.GetType() != item2.GetType())
            {
                return false;
            }

            if (item1.Name != item2.Name)
            {
                return false;
            }

            if (item1.Price != item2.Price)
            {
                return false;
            }

            if (item1 is Potion && item2 is Potion)
            {
                Potion potion1 = (Potion)item1;
                Potion potion2 = (Potion)item2;

                return potion1.PotionType == potion2.PotionType &&
                       potion1.HealAmount == potion2.HealAmount;
            }

            if (item1 is Weapon && item2 is Weapon)
            {
                Weapon weapon1 = (Weapon)item1;
                Weapon weapon2 = (Weapon)item2;

                return weapon1.BonusValue == weapon2.BonusValue &&
                       weapon1.RequiredClass == weapon2.RequiredClass;
            }

            if (item1 is Equipment && item2 is Equipment)
            {
                Equipment equipment1 = (Equipment)item1;
                Equipment equipment2 = (Equipment)item2;

                return equipment1.BonusValue == equipment2.BonusValue;
            }

            return true;
        }
    }
}