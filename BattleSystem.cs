using System;

namespace RPG_Test
{
    internal class BattleSystem
    {
        private Random random = new Random();

        public bool StartBattle(Player player, Monster monster, int stageNumber, StageManager stageManager, QuestSystem questSystem)
        {
            bool isBossBattle = stageManager.IsBossBattle(stageNumber);
            string[] battleMenus = { "공격", "방어", "스킬", "아이템", "도망가기" };
            int selectedIndex = 0;

            while (true)
            {
                GameConsole.SafeClear();

                Console.WriteLine(stageManager.GetStageTitle(stageNumber));
                Console.WriteLine(stageManager.GetBattleStatusText(stageNumber));
                Console.WriteLine();

                Console.WriteLine("[플레이어]");
                player.ShowPlayerStatus();
                Console.WriteLine();

                Console.WriteLine("[몬스터]");
                monster.PrintStatus();
                Console.WriteLine();

                for (int i = 0; i < battleMenus.Length; i++)
                {
                    if (i == selectedIndex)
                    {
                        Console.Write("[ " + battleMenus[i] + " ]  ");
                    }
                    else
                    {
                        Console.Write("  " + battleMenus[i] + "    ");
                    }
                }

                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("← → : 이동 / Enter : 선택");

                ConsoleKeyInfo keyInfo = Console.ReadKey(true);

                if (keyInfo.Key == ConsoleKey.LeftArrow)
                {
                    selectedIndex--;

                    if (selectedIndex < 0)
                    {
                        selectedIndex = battleMenus.Length - 1;
                    }
                }
                else if (keyInfo.Key == ConsoleKey.RightArrow)
                {
                    selectedIndex++;

                    if (selectedIndex >= battleMenus.Length)
                    {
                        selectedIndex = 0;
                    }
                }
                else if (keyInfo.Key == ConsoleKey.Enter)
                {
                    bool playerTurnEnd = false;
                    bool isDefending = false;

                    GameConsole.SafeClear();

                    if (selectedIndex == 0)
                    {
                        player.Attack(monster);
                        playerTurnEnd = true;
                    }
                    else if (selectedIndex == 1)
                    {
                        Console.WriteLine(player.Name + "이(가) 방어 자세를 취했습니다!");
                        isDefending = true;
                        playerTurnEnd = true;
                    }
                    else if (selectedIndex == 2)
                    {
                        Skill selectedSkill = OpenSkillMenu(player);

                        if (selectedSkill != null)
                        {
                            if (player.UseMana(selectedSkill.ManaCost) == false)
                            {
                                GameConsole.SafeClear();
                                Console.WriteLine("마나가 부족합니다.");
                                Console.WriteLine("현재 MP: " + player.Mp + " / 필요 MP: " + selectedSkill.ManaCost);
                                Console.ReadKey();
                                continue;
                            }

                            GameConsole.SafeClear();
                            Console.WriteLine(player.Name + "이(가) " + selectedSkill.Name + "을(를) 사용했다!");
                            Console.WriteLine("설명: " + selectedSkill.Description);
                            Console.WriteLine("소모 MP: " + selectedSkill.ManaCost);
                            monster.TakeDamage(player.GetAttackPower() + selectedSkill.BonusDamage);
                            playerTurnEnd = true;
                        }
                    }
                    else if (selectedIndex == 3)
                    {
                        int itemChoice = OpenItemMenu(player);

                        if (itemChoice == 0)
                        {
                            GameConsole.SafeClear();

                            if (player.UseHealthPotion() == false)
                            {
                                Console.WriteLine("사용할 수 있는 체력 포션이 없습니다.");
                                Console.ReadKey();
                                continue;
                            }

                            playerTurnEnd = true;
                        }
                        else if (itemChoice == 1)
                        {
                            GameConsole.SafeClear();

                            if (player.UseManaPotion() == false)
                            {
                                Console.WriteLine("사용할 수 있는 마나 포션이 없습니다.");
                                Console.ReadKey();
                                continue;
                            }

                            playerTurnEnd = true;
                        }
                        else if (itemChoice == 2)
                        {
                            GameConsole.SafeClear();

                            if (player.UseAntidote() == false)
                            {
                                Console.WriteLine("해독제를 사용할 수 없거나 중독 상태가 아닙니다.");
                                Console.ReadKey();
                                continue;
                            }

                            playerTurnEnd = true;
                        }
                        else if (itemChoice == 3)
                        {
                            GameConsole.SafeClear();

                            if (player.UseElixir() == false)
                            {
                                Console.WriteLine("사용할 수 있는 엘릭서가 없습니다.");
                                Console.ReadKey();
                                continue;
                            }

                            playerTurnEnd = true;
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else if (selectedIndex == 4)
                    {
                        Console.WriteLine("전투에서 도망쳤습니다.");
                        Console.ReadKey();
                        return false;
                    }

                    if (playerTurnEnd == false)
                    {
                        continue;
                    }

                    if (monster.IsDead())
                    {
                        ProcessVictory(player, monster, stageNumber, stageManager, questSystem, isBossBattle);
                        Console.ReadKey();
                        return true;
                    }

                    Console.WriteLine();

                    if (isDefending)
                    {
                        Console.WriteLine(monster.Name + "이(가) 공격했다!");
                        player.TakeGuardDamage(monster.GetAttackPower());
                    }
                    else
                    {
                        monster.Attack(player);
                    }

                    if (player.IsDead() == false)
                    {
                        TryApplyPoison(player, monster, stageNumber, isBossBattle);
                    }

                    if (player.IsDead() == false && player.IsPoisoned)
                    {
                        Console.WriteLine();
                        Console.WriteLine("=== 상태 이상 ===");
                        player.ProcessStatusEffect();
                    }

                    if (player.IsDead())
                    {
                        Console.WriteLine();
                        Console.WriteLine("플레이어가 쓰러졌습니다.");
                        Console.WriteLine("치료소로 이동하면 체력과 마나를 회복할 수 있습니다.");
                        Console.ReadKey();
                        return false;
                    }

                    Console.ReadKey();
                }
            }
        }

        private void TryApplyPoison(Player player, Monster monster, int stageNumber, bool isBossBattle)
        {
            if (player.IsPoisoned)
            {
                return;
            }

            int poisonChance = 0;

            if (isBossBattle)
            {
                poisonChance = 35;
            }
            else if (monster.Name.IndexOf("희귀한") >= 0)
            {
                poisonChance = 25;
            }
            else if (stageNumber >= 4)
            {
                poisonChance = 18;
            }
            else if (stageNumber >= 2)
            {
                poisonChance = 10;
            }

            if (poisonChance > 0 && random.Next(100) < poisonChance)
            {
                player.ApplyPoison();
            }
        }

        private void ProcessVictory(Player player, Monster monster, int stageNumber, StageManager stageManager, QuestSystem questSystem, bool isBossBattle)
        {
            Console.WriteLine();
            Console.WriteLine(monster.Name + "을(를) 처치했습니다!");

            player.GainGold(monster.RewardGold);
            player.GainExp(monster.RewardExp);

            Item dropItem = stageManager.CreateDropItem(stageNumber);

            if (dropItem != null)
            {
                Console.WriteLine("아이템 드랍!");
                player.Inventory.AddItem(dropItem);
            }
            else
            {
                Console.WriteLine("드랍된 아이템이 없습니다.");
            }

            Console.WriteLine();
            Console.WriteLine(stageManager.ProcessVictory(stageNumber));

            questSystem.UpdateProgress(stageNumber, isBossBattle, monster.Name);
        }

        private Skill OpenSkillMenu(Player player)
        {
            if (player.GetSkillCount() == 0)
            {
                GameConsole.SafeClear();
                Console.WriteLine("사용할 수 있는 스킬이 없습니다.");
                Console.ReadKey();
                return null;
            }

            int selectedIndex = 0;

            while (true)
            {
                GameConsole.SafeClear();
                Console.WriteLine("=== 스킬 선택 ===");
                Console.WriteLine("현재 MP: " + player.Mp + "/" + player.MaxMp);
                Console.WriteLine();

                for (int i = 0; i < player.GetSkillCount(); i++)
                {
                    Skill skill = player.GetSkill(i);

                    if (i == selectedIndex)
                    {
                        Console.WriteLine("> " + skill.Name);
                        Console.WriteLine("  설명: " + skill.Description);
                        Console.WriteLine("  추가 데미지: +" + skill.BonusDamage);
                        Console.WriteLine("  필요 레벨: " + skill.RequiredLevel);
                        Console.WriteLine("  소모 MP: " + skill.ManaCost);
                    }
                    else
                    {
                        Console.WriteLine("  " + skill.Name);
                    }

                    Console.WriteLine();
                }

                Console.WriteLine("↑ ↓ : 이동 / Enter : 선택 / Backspace : 뒤로가기");

                ConsoleKeyInfo keyInfo = Console.ReadKey(true);

                if (keyInfo.Key == ConsoleKey.UpArrow)
                {
                    selectedIndex--;

                    if (selectedIndex < 0)
                    {
                        selectedIndex = player.GetSkillCount() - 1;
                    }
                }
                else if (keyInfo.Key == ConsoleKey.DownArrow)
                {
                    selectedIndex++;

                    if (selectedIndex >= player.GetSkillCount())
                    {
                        selectedIndex = 0;
                    }
                }
                else if (keyInfo.Key == ConsoleKey.Enter)
                {
                    return player.GetSkill(selectedIndex);
                }
                else if (keyInfo.Key == ConsoleKey.Backspace)
                {
                    return null;
                }
            }
        }

        private int OpenItemMenu(Player player)
        {
            string[] itemMenus = { "체력 포션 사용", "마나 포션 사용", "해독제 사용", "엘릭서 사용", "뒤로가기" };
            int selectedIndex = 0;

            while (true)
            {
                GameConsole.SafeClear();
                Console.WriteLine("=== 아이템 선택 ===");
                Console.WriteLine("현재 HP: " + player.Hp + "/" + player.MaxHp);
                Console.WriteLine("현재 MP: " + player.Mp + "/" + player.MaxMp);
                Console.WriteLine("현재 상태: " + player.GetStatusText());
                Console.WriteLine();

                for (int i = 0; i < itemMenus.Length; i++)
                {
                    if (i == selectedIndex)
                    {
                        Console.WriteLine("> " + itemMenus[i]);
                    }
                    else
                    {
                        Console.WriteLine("  " + itemMenus[i]);
                    }
                }

                Console.WriteLine();
                Console.WriteLine("↑ ↓ : 이동 / Enter : 선택 / Backspace : 뒤로가기");

                ConsoleKeyInfo keyInfo = Console.ReadKey(true);

                if (keyInfo.Key == ConsoleKey.UpArrow)
                {
                    selectedIndex--;

                    if (selectedIndex < 0)
                    {
                        selectedIndex = itemMenus.Length - 1;
                    }
                }
                else if (keyInfo.Key == ConsoleKey.DownArrow)
                {
                    selectedIndex++;

                    if (selectedIndex >= itemMenus.Length)
                    {
                        selectedIndex = 0;
                    }
                }
                else if (keyInfo.Key == ConsoleKey.Enter)
                {
                    return selectedIndex;
                }
                else if (keyInfo.Key == ConsoleKey.Backspace)
                {
                    return itemMenus.Length - 1;
                }
            }
        }
    }

    internal class GameConsole
    {
        public static void SafeClear()
        {
            Console.Clear();
        }
    }
}