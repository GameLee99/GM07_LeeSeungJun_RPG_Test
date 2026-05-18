using System;
using System.Collections.Generic;

namespace RPG_Test
{
    internal class ShopSystem
    {
        private List<Item> shopItems = new List<Item>();

        public ShopSystem()
        {
            shopItems.Add(new Potion("소형 체력 포션", PotionType.Health, 30, 20));
            shopItems.Add(new Potion("중형 체력 포션", PotionType.Health, 50, 35));
            shopItems.Add(new Potion("소형 마나 포션", PotionType.Mana, 20, 25));
            shopItems.Add(new Potion("중형 마나 포션", PotionType.Mana, 40, 45));
            shopItems.Add(new Potion("해독제", PotionType.Antidote, 0, 30));
            shopItems.Add(new Potion("엘릭서", PotionType.Elixir, 60, 120));

            shopItems.Add(new Weapon("수련용 검", 120, 5, PlayerClass.Warrior));
            shopItems.Add(new Weapon("견습 지팡이", 120, 5, PlayerClass.Mage));
            shopItems.Add(new Weapon("사냥 활", 120, 5, PlayerClass.Archer));
            shopItems.Add(new Weapon("도적 단검", 120, 5, PlayerClass.Thief));

            shopItems.Add(new Armor("가죽 갑옷", 100, 3));
            shopItems.Add(new HeadGear("가죽 모자", 80, 1));
            shopItems.Add(new Shield("나무 방패", 90, 2));
            shopItems.Add(new Greaves("가죽 각반", 70, 1));
            shopItems.Add(new Shoes("가죽 신발", 60, 1));
            shopItems.Add(new Ring("구리 반지", 110, 1));
        }

        public void OpenShop(Player player)
        {
            string[] menus = { "구매하기", "판매하기", "돌아가기" };
            int selectedIndex = 0;

            while (true)
            {
                GameConsole.SafeClear();
                Console.WriteLine("=== 상점 ===");
                Console.WriteLine("보유 골드: " + player.Gold + "G");
                Console.WriteLine();

                for (int i = 0; i < menus.Length; i++)
                {
                    if (i == selectedIndex)
                    {
                        Console.WriteLine("> " + menus[i]);
                    }
                    else
                    {
                        Console.WriteLine("  " + menus[i]);
                    }
                }

                Console.WriteLine();
                Console.WriteLine("↑ ↓ : 이동 / Enter : 선택 / Backspace : 나가기");

                ConsoleKeyInfo keyInfo = Console.ReadKey(true);

                if (keyInfo.Key == ConsoleKey.UpArrow)
                {
                    selectedIndex--;

                    if (selectedIndex < 0)
                    {
                        selectedIndex = menus.Length - 1;
                    }
                }
                else if (keyInfo.Key == ConsoleKey.DownArrow)
                {
                    selectedIndex++;

                    if (selectedIndex >= menus.Length)
                    {
                        selectedIndex = 0;
                    }
                }
                else if (keyInfo.Key == ConsoleKey.Enter)
                {
                    if (selectedIndex == 0)
                    {
                        OpenBuyMenu(player);
                    }
                    else if (selectedIndex == 1)
                    {
                        OpenSellMenu(player);
                    }
                    else
                    {
                        return;
                    }
                }
                else if (keyInfo.Key == ConsoleKey.Backspace)
                {
                    return;
                }
            }
        }

        private void OpenBuyMenu(Player player)
        {
            int selectedIndex = 0;

            while (true)
            {
                GameConsole.SafeClear();
                Console.WriteLine("=== 아이템 구매 ===");
                Console.WriteLine("보유 골드: " + player.Gold + "G");
                Console.WriteLine();

                for (int i = 0; i < shopItems.Count; i++)
                {
                    if (i == selectedIndex)
                    {
                        Console.Write("> ");
                    }
                    else
                    {
                        Console.Write("  ");
                    }

                    shopItems[i].PrintInfo();
                    Console.WriteLine();
                }

                Console.WriteLine();

                if (selectedIndex == shopItems.Count)
                {
                    Console.WriteLine("> 돌아가기");
                }
                else
                {
                    Console.WriteLine("  돌아가기");
                }

                Console.WriteLine();
                Console.WriteLine("↑ ↓ : 이동 / Enter : 구매 수량 선택 / Backspace : 돌아가기");

                ConsoleKeyInfo keyInfo = Console.ReadKey(true);

                if (keyInfo.Key == ConsoleKey.UpArrow)
                {
                    selectedIndex--;

                    if (selectedIndex < 0)
                    {
                        selectedIndex = shopItems.Count;
                    }
                }
                else if (keyInfo.Key == ConsoleKey.DownArrow)
                {
                    selectedIndex++;

                    if (selectedIndex > shopItems.Count)
                    {
                        selectedIndex = 0;
                    }
                }
                else if (keyInfo.Key == ConsoleKey.Enter)
                {
                    if (selectedIndex == shopItems.Count)
                    {
                        return;
                    }

                    Item selectedItem = shopItems[selectedIndex];
                    int quantity = OpenBuyQuantityMenu(player, selectedItem);

                    if (quantity <= 0)
                    {
                        continue;
                    }

                    GameConsole.SafeClear();
                    BuyItem(player, selectedIndex, quantity);
                    Console.ReadKey();
                }
                else if (keyInfo.Key == ConsoleKey.Backspace)
                {
                    return;
                }
            }
        }

        private int OpenBuyQuantityMenu(Player player, Item item)
        {
            if (item.Price <= 0)
            {
                return 1;
            }

            int maxQuantity = player.Gold / item.Price;

            if (maxQuantity <= 0)
            {
                GameConsole.SafeClear();
                Console.WriteLine("골드가 부족합니다.");
                Console.ReadKey();
                return 0;
            }

            int selectedQuantity = 1;

            while (true)
            {
                GameConsole.SafeClear();
                Console.WriteLine("=== 구매 수량 선택 ===");
                Console.WriteLine("보유 골드: " + player.Gold + "G");
                Console.WriteLine();

                item.PrintInfo();
                Console.WriteLine();
                Console.WriteLine("최대 구매 가능 수량: " + maxQuantity);
                Console.WriteLine("선택 수량: " + selectedQuantity);
                Console.WriteLine("총 가격: " + (item.Price * selectedQuantity) + "G");
                Console.WriteLine();
                Console.WriteLine("← → : 수량 조절 / Enter : 구매 / Backspace : 취소");

                ConsoleKeyInfo keyInfo = Console.ReadKey(true);

                if (keyInfo.Key == ConsoleKey.LeftArrow)
                {
                    selectedQuantity--;

                    if (selectedQuantity < 1)
                    {
                        selectedQuantity = maxQuantity;
                    }
                }
                else if (keyInfo.Key == ConsoleKey.RightArrow)
                {
                    selectedQuantity++;

                    if (selectedQuantity > maxQuantity)
                    {
                        selectedQuantity = 1;
                    }
                }
                else if (keyInfo.Key == ConsoleKey.Enter)
                {
                    return selectedQuantity;
                }
                else if (keyInfo.Key == ConsoleKey.Backspace)
                {
                    return 0;
                }
            }
        }

        private void OpenSellMenu(Player player)
        {
            int selectedIndex = 0;

            while (true)
            {
                GameConsole.SafeClear();
                Console.WriteLine("=== 아이템 판매 ===");
                Console.WriteLine("보유 골드: " + player.Gold + "G");
                Console.WriteLine();

                int groupCount = player.Inventory.GetGroupCount();

                if (groupCount == 0)
                {
                    Console.WriteLine("판매할 아이템이 없습니다.");
                    Console.WriteLine();
                    Console.WriteLine("Enter 또는 Backspace : 돌아가기");

                    ConsoleKeyInfo emptyKeyInfo = Console.ReadKey(true);

                    if (emptyKeyInfo.Key == ConsoleKey.Enter || emptyKeyInfo.Key == ConsoleKey.Backspace)
                    {
                        return;
                    }

                    continue;
                }

                for (int i = 0; i < groupCount; i++)
                {
                    Item item = player.Inventory.GetGroupRepresentative(i);
                    int totalCount = player.Inventory.GetGroupQuantity(i);
                    int sellableCount = player.Inventory.GetGroupSellableCount(i, player);

                    if (i == selectedIndex)
                    {
                        Console.Write("> ");
                    }
                    else
                    {
                        Console.Write("  ");
                    }

                    item.PrintInfo();
                    Console.Write(" / 보유: " + totalCount + "개");
                    Console.Write(" / 판매 가능: " + sellableCount + "개");
                    Console.Write(" / 개당 판매가: " + (item.Price / 2) + "G");

                    if (player.Inventory.GroupContainsEquippedItem(i, player))
                    {
                        Console.Write(" [장착 중 포함]");
                    }

                    Console.WriteLine();
                }

                Console.WriteLine();

                if (selectedIndex == groupCount)
                {
                    Console.WriteLine("> 돌아가기");
                }
                else
                {
                    Console.WriteLine("  돌아가기");
                }

                Console.WriteLine();
                Console.WriteLine("장착 중인 아이템은 판매할 수 없습니다.");
                Console.WriteLine("↑ ↓ : 이동 / Enter : 판매 수량 선택 / Backspace : 돌아가기");

                ConsoleKeyInfo keyInfo = Console.ReadKey(true);

                if (keyInfo.Key == ConsoleKey.UpArrow)
                {
                    selectedIndex--;

                    if (selectedIndex < 0)
                    {
                        selectedIndex = groupCount;
                    }
                }
                else if (keyInfo.Key == ConsoleKey.DownArrow)
                {
                    selectedIndex++;

                    if (selectedIndex > groupCount)
                    {
                        selectedIndex = 0;
                    }
                }
                else if (keyInfo.Key == ConsoleKey.Enter)
                {
                    if (selectedIndex == groupCount)
                    {
                        return;
                    }

                    Item selectedItem = player.Inventory.GetGroupRepresentative(selectedIndex);
                    int sellableCount = player.Inventory.GetGroupSellableCount(selectedIndex, player);

                    if (sellableCount <= 0)
                    {
                        GameConsole.SafeClear();
                        Console.WriteLine("판매할 수 있는 수량이 없습니다.");
                        Console.ReadKey();
                        continue;
                    }

                    int quantity = OpenSellQuantityMenu(selectedItem, sellableCount);

                    if (quantity <= 0)
                    {
                        continue;
                    }

                    GameConsole.SafeClear();
                    SellItem(player, selectedItem, quantity);
                    Console.ReadKey();
                }
                else if (keyInfo.Key == ConsoleKey.Backspace)
                {
                    return;
                }
            }
        }

        private int OpenSellQuantityMenu(Item item, int maxQuantity)
        {
            int selectedQuantity = 1;
            int sellPrice = item.Price / 2;

            while (true)
            {
                GameConsole.SafeClear();
                Console.WriteLine("=== 판매 수량 선택 ===");
                Console.WriteLine();

                item.PrintInfo();
                Console.WriteLine();
                Console.WriteLine("판매 가능 수량: " + maxQuantity);
                Console.WriteLine("선택 수량: " + selectedQuantity);
                Console.WriteLine("총 판매가: " + (sellPrice * selectedQuantity) + "G");
                Console.WriteLine();
                Console.WriteLine("← → : 수량 조절 / Enter : 판매 / Backspace : 취소");

                ConsoleKeyInfo keyInfo = Console.ReadKey(true);

                if (keyInfo.Key == ConsoleKey.LeftArrow)
                {
                    selectedQuantity--;

                    if (selectedQuantity < 1)
                    {
                        selectedQuantity = maxQuantity;
                    }
                }
                else if (keyInfo.Key == ConsoleKey.RightArrow)
                {
                    selectedQuantity++;

                    if (selectedQuantity > maxQuantity)
                    {
                        selectedQuantity = 1;
                    }
                }
                else if (keyInfo.Key == ConsoleKey.Enter)
                {
                    return selectedQuantity;
                }
                else if (keyInfo.Key == ConsoleKey.Backspace)
                {
                    return 0;
                }
            }
        }

        private void BuyItem(Player player, int index, int quantity)
        {
            Item item = shopItems[index];
            int totalPrice = item.Price * quantity;

            if (player.SpendGold(totalPrice) == false)
            {
                Console.WriteLine("골드가 부족합니다.");
                return;
            }

            for (int i = 0; i < quantity; i++)
            {
                player.Inventory.AddItem(CreateCopy(item), false);
            }

            Console.WriteLine(item.Name + "을(를) " + quantity + "개 구매했습니다.");
            Console.WriteLine("총 가격: " + totalPrice + "G");
        }

        private Item CreateCopy(Item item)
        {
            if (item is Potion)
            {
                Potion potion = (Potion)item;
                return new Potion(potion.Name, potion.PotionType, potion.HealAmount, potion.Price);
            }
            else if (item is Weapon)
            {
                Weapon weapon = (Weapon)item;
                return new Weapon(weapon.Name, weapon.Price, weapon.BonusValue, weapon.RequiredClass);
            }
            else if (item is Armor)
            {
                Armor armor = (Armor)item;
                return new Armor(armor.Name, armor.Price, armor.BonusValue);
            }
            else if (item is HeadGear)
            {
                HeadGear headGear = (HeadGear)item;
                return new HeadGear(headGear.Name, headGear.Price, headGear.BonusValue);
            }
            else if (item is Shield)
            {
                Shield shield = (Shield)item;
                return new Shield(shield.Name, shield.Price, shield.BonusValue);
            }
            else if (item is Greaves)
            {
                Greaves greaves = (Greaves)item;
                return new Greaves(greaves.Name, greaves.Price, greaves.BonusValue);
            }
            else if (item is Shoes)
            {
                Shoes shoes = (Shoes)item;
                return new Shoes(shoes.Name, shoes.Price, shoes.BonusValue);
            }
            else if (item is Ring)
            {
                Ring ring = (Ring)item;
                return new Ring(ring.Name, ring.Price, ring.BonusValue);
            }

            return null;
        }

        private void SellItem(Player player, Item selectedItem, int quantity)
        {
            int removedCount = player.Inventory.RemoveMatchingItems(selectedItem, quantity, player);

            if (removedCount <= 0)
            {
                Console.WriteLine("판매할 수 있는 아이템이 없습니다.");
                return;
            }

            int totalSellPrice = (selectedItem.Price / 2) * removedCount;
            player.GainGold(totalSellPrice);

            Console.WriteLine(selectedItem.Name + "을(를) " + removedCount + "개 판매했습니다.");
            Console.WriteLine("총 판매가: " + totalSellPrice + "G");
        }
    }
}