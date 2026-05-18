using System;
using System.Collections.Generic;

namespace RPG_Test
{
    internal enum Place
    {
        Town,
        Shop,
        Hospital,
        Dungeon
    }

    internal class Program
    {
        static void Main()
        {
            PlayerClass selectedClass = SelectPlayerClass();
            string playerName = InputPlayerName(selectedClass);

            Player player = new Player(playerName, selectedClass);
            ShopSystem shopSystem = new ShopSystem();
            StageManager stageManager = new StageManager();
            BattleSystem battleSystem = new BattleSystem();
            QuestSystem questSystem = new QuestSystem();

            Place currentPlace = Place.Town;
            bool isPlaying = true;

            if (selectedClass == PlayerClass.Cheat)
            {
                SetupCheatCharacter(player);
            }
            else
            {
                player.Inventory.AddItem(new Potion("소형 체력 포션", PotionType.Health, 30, 20));
            }

            while (isPlaying)
            {
                if (currentPlace == Place.Town)
                {
                    int selectedTownMenu = OpenTownMenu(player);

                    if (selectedTownMenu == 0)
                    {
                        GameConsole.SafeClear();
                        player.ShowPlayerStatus();
                        Console.ReadKey();
                    }
                    else if (selectedTownMenu == 1)
                    {
                        OpenInventoryMenu(player);
                    }
                    else if (selectedTownMenu == 2)
                    {
                        OpenEquipMenu(player);
                    }
                    else if (selectedTownMenu == 3)
                    {
                        questSystem.OpenQuestMenu(player, stageManager);
                    }
                    else if (selectedTownMenu == 4)
                    {
                        currentPlace = Place.Shop;
                    }
                    else if (selectedTownMenu == 5)
                    {
                        currentPlace = Place.Hospital;
                    }
                    else if (selectedTownMenu == 6)
                    {
                        currentPlace = Place.Dungeon;
                    }
                    else if (selectedTownMenu == 7)
                    {
                        isPlaying = false;
                    }
                }
                else if (currentPlace == Place.Shop)
                {
                    int selectedShopMenu = OpenSimplePlaceMenu(
                        "=== 상점 ===",
                        new string[] { "물건 사기", "마을로 돌아가기" }
                    );

                    if (selectedShopMenu == 0)
                    {
                        shopSystem.OpenShop(player);
                    }
                    else
                    {
                        currentPlace = Place.Town;
                    }
                }
                else if (currentPlace == Place.Hospital)
                {
                    int selectedHospitalMenu = OpenSimplePlaceMenu(
                        "=== 치료소 ===",
                        new string[] { "치료 받기 (30G)", "마을로 돌아가기" }
                    );

                    if (selectedHospitalMenu == 0)
                    {
                        HealPlayer(player);
                    }
                    else
                    {
                        currentPlace = Place.Town;
                    }
                }
                else if (currentPlace == Place.Dungeon)
                {
                    int selectedStage = OpenDungeonMenu(player, stageManager);

                    if (selectedStage == 0)
                    {
                        currentPlace = Place.Town;
                    }
                    else
                    {
                        if (player.IsDead())
                        {
                            GameConsole.SafeClear();
                            Console.WriteLine("체력이 없습니다.");
                            Console.WriteLine("치료소에서 회복한 뒤 다시 도전하세요.");
                            Console.ReadKey();
                            currentPlace = Place.Hospital;
                            continue;
                        }

                        Monster monster = stageManager.CreateMonsterByStage(selectedStage);

                        bool win = battleSystem.StartBattle(
                            player,
                            monster,
                            selectedStage,
                            stageManager,
                            questSystem
                        );

                        if (win == false && player.IsDead())
                        {
                            currentPlace = Place.Hospital;
                        }
                        else if (win)
                        {
                            if (stageManager.ConsumeGameClearFlag())
                            {
                                bool keepPlaying = AskContinueAfterClear();

                                if (keepPlaying == false)
                                {
                                    isPlaying = false;
                                }
                                else
                                {
                                    currentPlace = Place.Town;
                                }
                            }
                        }
                    }
                }
            }

            GameConsole.SafeClear();
            Console.WriteLine("게임을 종료합니다.");
        }

        static PlayerClass SelectPlayerClass()
        {
            string[] classMenus = { "전사", "마법사", "궁수", "도적", "치트캐릭터" };
            int selectedIndex = 0;

            while (true)
            {
                GameConsole.SafeClear();
                Console.WriteLine("=== 캐릭터 선택 ===");
                Console.WriteLine();

                for (int i = 0; i < classMenus.Length; i++)
                {
                    if (i == selectedIndex)
                    {
                        Console.WriteLine("> " + classMenus[i]);
                    }
                    else
                    {
                        Console.WriteLine("  " + classMenus[i]);
                    }
                }

                Console.WriteLine();
                ShowClassDescription(selectedIndex);
                Console.WriteLine();
                Console.WriteLine("↑ ↓ : 이동 / Enter : 선택");

                ConsoleKeyInfo keyInfo = Console.ReadKey(true);

                if (keyInfo.Key == ConsoleKey.UpArrow)
                {
                    selectedIndex--;

                    if (selectedIndex < 0)
                    {
                        selectedIndex = classMenus.Length - 1;
                    }
                }
                else if (keyInfo.Key == ConsoleKey.DownArrow)
                {
                    selectedIndex++;

                    if (selectedIndex >= classMenus.Length)
                    {
                        selectedIndex = 0;
                    }
                }
                else if (keyInfo.Key == ConsoleKey.Enter)
                {
                    if (selectedIndex == 0)
                    {
                        return PlayerClass.Warrior;
                    }
                    else if (selectedIndex == 1)
                    {
                        return PlayerClass.Mage;
                    }
                    else if (selectedIndex == 2)
                    {
                        return PlayerClass.Archer;
                    }
                    else if (selectedIndex == 3)
                    {
                        return PlayerClass.Thief;
                    }
                    else
                    {
                        return PlayerClass.Cheat;
                    }
                }
            }
        }

        static void ShowClassDescription(int selectedIndex)
        {
            if (selectedIndex == 0)
            {
                Console.WriteLine("전사 : 체력과 방어력이 높은 기본형 클래스");
            }
            else if (selectedIndex == 1)
            {
                Console.WriteLine("마법사 : 체력은 낮지만 강한 스킬 공격을 가진 클래스");
            }
            else if (selectedIndex == 2)
            {
                Console.WriteLine("궁수 : 안정적인 공격력과 균형 잡힌 능력치의 클래스");
            }
            else if (selectedIndex == 3)
            {
                Console.WriteLine("도적 : 체력은 낮지만 빠르고 강한 공격을 가진 클래스");
            }
            else
            {
                Console.WriteLine("치트캐릭터 : 테스트용 초고성능 캐릭터");
            }
        }

        static string InputPlayerName(PlayerClass selectedClass)
        {
            GameConsole.SafeClear();
            Console.WriteLine("선택한 직업: " + GetClassName(selectedClass));
            Console.Write("이름을 입력하세요 (엔터만 누르면 기본 이름 사용): ");

            string input = Console.ReadLine();

            if (input == null || input == "")
            {
                return GetDefaultName(selectedClass);
            }

            return input;
        }

        static string GetClassName(PlayerClass selectedClass)
        {
            if (selectedClass == PlayerClass.Warrior)
            {
                return "전사";
            }
            else if (selectedClass == PlayerClass.Mage)
            {
                return "마법사";
            }
            else if (selectedClass == PlayerClass.Archer)
            {
                return "궁수";
            }
            else if (selectedClass == PlayerClass.Thief)
            {
                return "도적";
            }
            else
            {
                return "치트캐릭터";
            }
        }

        static string GetDefaultName(PlayerClass selectedClass)
        {
            if (selectedClass == PlayerClass.Warrior)
            {
                return "아르곤";
            }
            else if (selectedClass == PlayerClass.Mage)
            {
                return "엘리나";
            }
            else if (selectedClass == PlayerClass.Archer)
            {
                return "리안";
            }
            else if (selectedClass == PlayerClass.Thief)
            {
                return "카일";
            }
            else
            {
                return "GM";
            }
        }

        static int OpenTownMenu(Player player)
        {
            string[] menus =
            {
                "상태 보기",
                "인벤토리",
                "장비 관리",
                "퀘스트 확인",
                "상점으로 이동",
                "치료소로 이동",
                "던전으로 이동",
                "게임 종료"
            };

            int selectedIndex = 0;

            while (true)
            {
                GameConsole.SafeClear();
                Console.WriteLine("=== 마을 ===");
                Console.WriteLine("이름: " + player.Name + " / 직업: " + player.ClassName);
                Console.WriteLine("HP: " + player.Hp + "/" + player.MaxHp + " / MP: " + player.Mp + "/" + player.MaxMp + " / 골드: " + player.Gold + "G");
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
                Console.WriteLine("↑ ↓ : 이동 / Enter : 선택");

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
                    return selectedIndex;
                }
            }
        }

        static int OpenSimplePlaceMenu(string title, string[] menus)
        {
            int selectedIndex = 0;

            while (true)
            {
                GameConsole.SafeClear();
                Console.WriteLine(title);
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
                Console.WriteLine("↑ ↓ : 이동 / Enter : 선택");

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
                    return selectedIndex;
                }
            }
        }

        static int OpenDungeonMenu(Player player, StageManager stageManager)
        {
            int selectedIndex = 1;

            while (true)
            {
                GameConsole.SafeClear();
                Console.WriteLine("=== 던전 ===");
                Console.WriteLine("HP: " + player.Hp + "/" + player.MaxHp + " / MP: " + player.Mp + "/" + player.MaxMp + " / 골드: " + player.Gold + "G");
                Console.WriteLine();

                for (int stageNumber = 1; stageNumber <= 4; stageNumber++)
                {
                    if (stageNumber == selectedIndex)
                    {
                        Console.WriteLine("> " + stageManager.GetStageTitle(stageNumber));
                    }
                    else
                    {
                        Console.WriteLine("  " + stageManager.GetStageTitle(stageNumber));
                    }

                    Console.WriteLine("   " + stageManager.GetStageProgressText(stageNumber));
                    Console.WriteLine("   " + stageManager.GetBattleStatusText(stageNumber));
                    Console.WriteLine();
                }

                if (selectedIndex == 5)
                {
                    Console.WriteLine("> 마을로 돌아가기");
                }
                else
                {
                    Console.WriteLine("  마을로 돌아가기");
                }

                Console.WriteLine();
                Console.WriteLine("↑ ↓ : 이동 / Enter : 선택 / Backspace : 돌아가기");

                ConsoleKeyInfo keyInfo = Console.ReadKey(true);

                if (keyInfo.Key == ConsoleKey.UpArrow)
                {
                    selectedIndex--;

                    if (selectedIndex < 1)
                    {
                        selectedIndex = 5;
                    }
                }
                else if (keyInfo.Key == ConsoleKey.DownArrow)
                {
                    selectedIndex++;

                    if (selectedIndex > 5)
                    {
                        selectedIndex = 1;
                    }
                }
                else if (keyInfo.Key == ConsoleKey.Enter)
                {
                    if (selectedIndex == 5)
                    {
                        return 0;
                    }

                    return selectedIndex;
                }
                else if (keyInfo.Key == ConsoleKey.Backspace)
                {
                    return 0;
                }
            }
        }

        static void OpenInventoryMenu(Player player)
        {
            int selectedIndex = 0;

            while (true)
            {
                GameConsole.SafeClear();
                Console.WriteLine("=== 인벤토리 ===");
                Console.WriteLine();

                int groupCount = player.Inventory.GetGroupCount();

                if (groupCount == 0)
                {
                    Console.WriteLine("인벤토리가 비어 있습니다.");
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
                    int quantity = player.Inventory.GetGroupQuantity(i);

                    if (i == selectedIndex)
                    {
                        Console.Write("> ");
                    }
                    else
                    {
                        Console.Write("  ");
                    }

                    item.PrintInfo();
                    Console.Write(" x" + quantity);

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
                Console.WriteLine("↑ ↓ : 이동 / Enter : 확인 / Backspace : 돌아가기");

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
                    int quantity = player.Inventory.GetGroupQuantity(selectedIndex);

                    GameConsole.SafeClear();
                    selectedItem.PrintInfo();
                    Console.WriteLine();
                    Console.WriteLine("보유 수량: " + quantity);

                    if (player.Inventory.GroupContainsEquippedItem(selectedIndex, player))
                    {
                        Console.WriteLine("장착 중인 같은 종류의 아이템이 포함되어 있습니다.");
                    }

                    Console.WriteLine();

                    if (selectedItem is Potion)
                    {
                        Console.WriteLine("포션은 전투 중 아이템 메뉴에서 사용할 수 있습니다.");
                    }
                    else if (selectedItem is Equipment)
                    {
                        Console.WriteLine("장비는 장비 관리 메뉴에서 장착할 수 있습니다.");
                    }

                    Console.ReadKey();
                }
                else if (keyInfo.Key == ConsoleKey.Backspace)
                {
                    return;
                }
            }
        }

        static void OpenEquipMenu(Player player)
        {
            int selectedIndex = 0;

            while (true)
            {
                GameConsole.SafeClear();
                Console.WriteLine("=== 장비 관리 ===");
                Console.WriteLine();

                player.ShowEquipment();
                Console.WriteLine();
                Console.WriteLine("=== 장착 가능한 아이템 ===");
                Console.WriteLine();

                List<Item> equipItems = new List<Item>();

                for (int i = 0; i < player.Inventory.Count; i++)
                {
                    Item item = player.Inventory.GetItem(i);

                    if (item is Equipment)
                    {
                        equipItems.Add(item);
                    }
                }

                if (equipItems.Count == 0)
                {
                    Console.WriteLine("장착할 수 있는 아이템이 없습니다.");
                    Console.WriteLine();
                    Console.WriteLine("Enter 또는 Backspace : 돌아가기");

                    ConsoleKeyInfo emptyKeyInfo = Console.ReadKey(true);

                    if (emptyKeyInfo.Key == ConsoleKey.Enter || emptyKeyInfo.Key == ConsoleKey.Backspace)
                    {
                        return;
                    }

                    continue;
                }

                for (int i = 0; i < equipItems.Count; i++)
                {
                    Item item = equipItems[i];

                    if (i == selectedIndex)
                    {
                        Console.Write("> ");
                    }
                    else
                    {
                        Console.Write("  ");
                    }

                    item.PrintInfo();

                    if (player.IsEquippedItem(item))
                    {
                        Console.Write(" [장착 중]");
                    }

                    Console.WriteLine();
                }

                Console.WriteLine();

                if (selectedIndex == equipItems.Count)
                {
                    Console.WriteLine("> 돌아가기");
                }
                else
                {
                    Console.WriteLine("  돌아가기");
                }

                Console.WriteLine();
                Console.WriteLine("같은 장비를 다시 선택하면 해제됩니다.");
                Console.WriteLine("↑ ↓ : 이동 / Enter : 선택 / Backspace : 돌아가기");

                ConsoleKeyInfo keyInfo = Console.ReadKey(true);

                if (keyInfo.Key == ConsoleKey.UpArrow)
                {
                    selectedIndex--;

                    if (selectedIndex < 0)
                    {
                        selectedIndex = equipItems.Count;
                    }
                }
                else if (keyInfo.Key == ConsoleKey.DownArrow)
                {
                    selectedIndex++;

                    if (selectedIndex > equipItems.Count)
                    {
                        selectedIndex = 0;
                    }
                }
                else if (keyInfo.Key == ConsoleKey.Enter)
                {
                    if (selectedIndex == equipItems.Count)
                    {
                        return;
                    }

                    Item selectedItem = equipItems[selectedIndex];

                    GameConsole.SafeClear();
                    player.EquipItem(selectedItem);
                    Console.ReadKey();
                }
                else if (keyInfo.Key == ConsoleKey.Backspace)
                {
                    return;
                }
            }
        }

        static void HealPlayer(Player player)
        {
            GameConsole.SafeClear();

            if (player.Hp >= player.MaxHp && player.Mp >= player.MaxMp)
            {
                Console.WriteLine("이미 체력과 마나가 가득 찼습니다.");
                Console.ReadKey();
                return;
            }

            if (player.SpendGold(30) == false)
            {
                Console.WriteLine("골드가 부족합니다.");
                Console.ReadKey();
                return;
            }

            player.RestoreAll();
            Console.WriteLine("치료가 완료되었습니다.");
            Console.WriteLine("체력과 마나가 모두 회복되었습니다.");
            Console.ReadKey();
        }

        static bool AskContinueAfterClear()
        {
            string[] menus = { "계속하기", "종료하기" };
            int selectedIndex = 0;

            while (true)
            {
                GameConsole.SafeClear();
                Console.WriteLine("=== 게임 클리어 ===");
                Console.WriteLine("고룡을 쓰러뜨렸습니다!");
                Console.WriteLine("마을에 평화가 찾아왔습니다!");
                Console.WriteLine("계속 하시겠습니까?");
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
                Console.WriteLine("↑ ↓ : 이동 / Enter : 선택");

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
                    return selectedIndex == 0;
                }
            }
        }

        static void SetupCheatCharacter(Player player)
        {
            Armor cheatArmor = new Armor("관리자 갑옷", 0, 20);
            HeadGear cheatHead = new HeadGear("관리자 투구", 0, 10);
            Shield cheatShield = new Shield("관리자 방패", 0, 10);
            Greaves cheatGreaves = new Greaves("관리자 각반", 0, 10);
            Shoes cheatShoes = new Shoes("관리자 신발", 0, 10);
            Ring cheatRing = new Ring("관리자 반지", 0, 5);

            player.GainGold(10000);
            player.GainExp(500);

            player.Inventory.AddItem(cheatArmor, false);
            player.Inventory.AddItem(cheatHead, false);
            player.Inventory.AddItem(cheatShield, false);
            player.Inventory.AddItem(cheatGreaves, false);
            player.Inventory.AddItem(cheatShoes, false);
            player.Inventory.AddItem(cheatRing, false);

            for (int i = 0; i < 20; i++)
            {
                player.Inventory.AddItem(new Potion("대형 체력 포션", PotionType.Health, 80, 60), false);
                player.Inventory.AddItem(new Potion("대형 마나 포션", PotionType.Mana, 60, 80), false);
            }

            player.EquipItem(cheatArmor);
            player.EquipItem(cheatHead);
            player.EquipItem(cheatShield);
            player.EquipItem(cheatGreaves);
            player.EquipItem(cheatShoes);
            player.EquipItem(cheatRing);
            player.RestoreAll();
        }
    }
}