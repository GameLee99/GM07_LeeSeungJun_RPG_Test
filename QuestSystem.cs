using System;
using System.Collections.Generic;

namespace RPG_Test
{
    internal enum QuestCategory
    {
        Main,
        Sub,
        Repeat
    }

    internal enum QuestType
    {
        DefeatAnyMonster,
        DefeatStageBoss,
        DefeatNamedMonster
    }

    internal class Quest
    {
        private string title;
        private string description;
        private string giver;
        private QuestCategory category;
        private QuestType questType;
        private int targetCount;
        private int currentCount;
        private int rewardGold;
        private int rewardExp;
        private int requiredStageNumber;
        private string targetMonsterName;
        private bool isRepeatable;
        private bool isAccepted;
        private bool isCompleted;
        private bool isRewardReceived;

        public string Title
        {
            get { return title; }
        }

        public string Description
        {
            get { return description; }
        }

        public string Giver
        {
            get { return giver; }
        }

        public QuestCategory Category
        {
            get { return category; }
        }

        public QuestType Type
        {
            get { return questType; }
        }

        public int TargetCount
        {
            get { return targetCount; }
        }

        public int CurrentCount
        {
            get { return currentCount; }
        }

        public int RewardGold
        {
            get { return rewardGold; }
        }

        public int RewardExp
        {
            get { return rewardExp; }
        }

        public int RequiredStageNumber
        {
            get { return requiredStageNumber; }
        }

        public string TargetMonsterName
        {
            get { return targetMonsterName; }
        }

        public bool IsRepeatable
        {
            get { return isRepeatable; }
        }

        public bool IsAccepted
        {
            get { return isAccepted; }
        }

        public bool IsCompleted
        {
            get { return isCompleted; }
        }

        public bool IsRewardReceived
        {
            get { return isRewardReceived; }
        }

        public Quest(
            string title,
            string description,
            string giver,
            QuestCategory category,
            QuestType questType,
            int targetCount,
            int rewardGold,
            int rewardExp,
            int requiredStageNumber,
            string targetMonsterName,
            bool isRepeatable)
        {
            this.title = title;
            this.description = description;
            this.giver = giver;
            this.category = category;
            this.questType = questType;
            this.targetCount = targetCount;
            this.rewardGold = rewardGold;
            this.rewardExp = rewardExp;
            this.requiredStageNumber = requiredStageNumber;
            this.targetMonsterName = targetMonsterName;
            this.isRepeatable = isRepeatable;

            currentCount = 0;
            isAccepted = false;
            isCompleted = false;
            isRewardReceived = false;
        }

        public void Accept()
        {
            if (isAccepted || isRewardReceived)
            {
                return;
            }

            isAccepted = true;
        }

        public void AddProgress()
        {
            if (isAccepted == false || isCompleted || isRewardReceived)
            {
                return;
            }

            currentCount++;

            if (currentCount >= targetCount)
            {
                currentCount = targetCount;
                isCompleted = true;
            }
        }

        public void ReceiveReward()
        {
            if (isCompleted == false || isRewardReceived)
            {
                return;
            }

            isRewardReceived = true;
        }

        public void ResetForRepeat()
        {
            currentCount = 0;
            isAccepted = false;
            isCompleted = false;
            isRewardReceived = false;
        }

        public string GetCategoryText()
        {
            if (category == QuestCategory.Main)
            {
                return "메인";
            }
            else if (category == QuestCategory.Sub)
            {
                return "서브";
            }
            else
            {
                return "반복";
            }
        }

        public string GetStatusText()
        {
            if (isRewardReceived && isRepeatable == false)
            {
                return "[완료]";
            }

            if (isCompleted)
            {
                return "[보상 받기]";
            }

            if (isAccepted)
            {
                return "[진행 중]";
            }

            return "[미수락]";
        }
    }

    internal class QuestSystem
    {
        private List<Quest> mainQuests = new List<Quest>();
        private List<Quest> boardQuests = new List<Quest>();

        public QuestSystem()
        {
            mainQuests.Add(new Quest(
                "슬라임 킹 토벌",
                "슬라임의 숲 깊숙한 곳에 있는 슬라임 킹을 처치하자.",
                "촌장",
                QuestCategory.Main,
                QuestType.DefeatStageBoss,
                1,
                100,
                80,
                1,
                "",
                false
            ));

            mainQuests.Add(new Quest(
                "고블린 대장 토벌",
                "고블린 야영지를 지배하는 고블린 대장을 처치하자.",
                "촌장",
                QuestCategory.Main,
                QuestType.DefeatStageBoss,
                1,
                150,
                120,
                2,
                "",
                false
            ));

            mainQuests.Add(new Quest(
                "오우거 족장 토벌",
                "오우거 협곡의 오우거 족장을 쓰러뜨리자.",
                "촌장",
                QuestCategory.Main,
                QuestType.DefeatStageBoss,
                1,
                220,
                180,
                3,
                "",
                false
            ));

            mainQuests.Add(new Quest(
                "고룡 토벌",
                "드래곤 둥지의 고룡을 처치해 마을의 위협을 끝내자.",
                "촌장",
                QuestCategory.Main,
                QuestType.DefeatStageBoss,
                1,
                400,
                320,
                4,
                "",
                false
            ));

            boardQuests.Add(new Quest(
                "마을 근처 정리",
                "마을 주변의 몬스터를 3마리 처치하자.",
                "게시판",
                QuestCategory.Sub,
                QuestType.DefeatAnyMonster,
                3,
                60,
                40,
                0,
                "",
                false
            ));

            boardQuests.Add(new Quest(
                "사냥 훈련",
                "몬스터를 6마리 처치해 실력을 증명하자.",
                "게시판",
                QuestCategory.Sub,
                QuestType.DefeatAnyMonster,
                6,
                120,
                90,
                0,
                "",
                false
            ));

            boardQuests.Add(new Quest(
                "희귀한 슬라임 토벌",
                "희귀한 슬라임을 3마리 처치하자.",
                "게시판",
                QuestCategory.Repeat,
                QuestType.DefeatNamedMonster,
                3,
                90,
                60,
                1,
                "희귀한 슬라임",
                true
            ));

            boardQuests.Add(new Quest(
                "희귀한 고블린 토벌",
                "희귀한 고블린을 3마리 처치하자.",
                "게시판",
                QuestCategory.Repeat,
                QuestType.DefeatNamedMonster,
                3,
                130,
                90,
                2,
                "희귀한 고블린",
                true
            ));

            boardQuests.Add(new Quest(
                "희귀한 오우거 토벌",
                "희귀한 오우거를 3마리 처치하자.",
                "게시판",
                QuestCategory.Repeat,
                QuestType.DefeatNamedMonster,
                3,
                180,
                130,
                3,
                "희귀한 오우거",
                true
            ));

            boardQuests.Add(new Quest(
                "희귀한 와이번 토벌",
                "희귀한 와이번을 3마리 처치하자.",
                "게시판",
                QuestCategory.Repeat,
                QuestType.DefeatNamedMonster,
                3,
                260,
                190,
                4,
                "희귀한 와이번",
                true
            ));
        }

        public void OpenQuestMenu(Player player, StageManager stageManager)
        {
            string[] menus = { "촌장 만나기", "게시판 확인", "돌아가기" };
            int selectedIndex = 0;

            while (true)
            {
                GameConsole.SafeClear();
                Console.WriteLine("=== 퀘스트 ===");
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
                Console.WriteLine("↑ ↓ : 이동 / Enter : 선택 / Backspace : 돌아가기");

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
                        OpenChiefMenu(player, stageManager);
                    }
                    else if (selectedIndex == 1)
                    {
                        OpenBoardMenu(player, stageManager);
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

        private void OpenChiefMenu(Player player, StageManager stageManager)
        {
            while (true)
            {
                Quest currentMainQuest = GetCurrentMainQuest();

                GameConsole.SafeClear();
                Console.WriteLine("=== 촌장 ===");
                Console.WriteLine();

                if (currentMainQuest == null)
                {
                    Console.WriteLine("모든 메인 퀘스트를 완료했습니다.");
                    Console.WriteLine();
                    Console.WriteLine("Enter 또는 Backspace : 돌아가기");

                    ConsoleKeyInfo endKeyInfo = Console.ReadKey(true);

                    if (endKeyInfo.Key == ConsoleKey.Enter || endKeyInfo.Key == ConsoleKey.Backspace)
                    {
                        return;
                    }

                    continue;
                }

                Console.WriteLine("메인 퀘스트");
                Console.WriteLine("제목: " + currentMainQuest.Title);
                Console.WriteLine("설명: " + currentMainQuest.Description);
                Console.WriteLine("진행도: " + currentMainQuest.CurrentCount + " / " + currentMainQuest.TargetCount);
                Console.WriteLine("보상: " + currentMainQuest.RewardGold + "G / 경험치 " + currentMainQuest.RewardExp);
                Console.WriteLine("상태: " + currentMainQuest.GetStatusText());
                Console.WriteLine();

                if (currentMainQuest.IsAccepted == false)
                {
                    Console.WriteLine("Enter : 퀘스트 수락");
                }
                else if (currentMainQuest.IsCompleted && currentMainQuest.IsRewardReceived == false)
                {
                    Console.WriteLine("Enter : 보상 받기");
                }
                else
                {
                    Console.WriteLine("Enter : 확인");
                }

                Console.WriteLine("Backspace : 돌아가기");

                ConsoleKeyInfo keyInfo = Console.ReadKey(true);

                if (keyInfo.Key == ConsoleKey.Enter)
                {
                    HandleQuestAction(player, currentMainQuest, stageManager);
                }
                else if (keyInfo.Key == ConsoleKey.Backspace)
                {
                    return;
                }
            }
        }

        private void OpenBoardMenu(Player player, StageManager stageManager)
        {
            int selectedIndex = 0;

            while (true)
            {
                GameConsole.SafeClear();
                Console.WriteLine("=== 게시판 ===");
                Console.WriteLine();

                for (int i = 0; i < boardQuests.Count; i++)
                {
                    if (i == selectedIndex)
                    {
                        Console.Write("> ");
                    }
                    else
                    {
                        Console.Write("  ");
                    }

                    Console.WriteLine("[" + boardQuests[i].GetCategoryText() + "] " + boardQuests[i].Title + " " + boardQuests[i].GetStatusText());
                }

                Console.WriteLine();

                Quest selectedQuest = boardQuests[selectedIndex];

                Console.WriteLine("퀘스트: " + selectedQuest.Title);
                Console.WriteLine("종류: " + selectedQuest.GetCategoryText());
                Console.WriteLine("설명: " + selectedQuest.Description);
                Console.WriteLine("진행도: " + selectedQuest.CurrentCount + " / " + selectedQuest.TargetCount);
                Console.WriteLine("보상: " + selectedQuest.RewardGold + "G / 경험치 " + selectedQuest.RewardExp);
                Console.WriteLine("상태: " + selectedQuest.GetStatusText());
                Console.WriteLine();

                if (selectedQuest.IsAccepted == false)
                {
                    Console.WriteLine("Enter : 퀘스트 수락");
                }
                else if (selectedQuest.IsCompleted && selectedQuest.IsRewardReceived == false)
                {
                    Console.WriteLine("Enter : 보상 받기");
                }
                else
                {
                    Console.WriteLine("Enter : 확인");
                }

                Console.WriteLine("Backspace : 돌아가기");

                ConsoleKeyInfo keyInfo = Console.ReadKey(true);

                if (keyInfo.Key == ConsoleKey.UpArrow)
                {
                    selectedIndex--;

                    if (selectedIndex < 0)
                    {
                        selectedIndex = boardQuests.Count - 1;
                    }
                }
                else if (keyInfo.Key == ConsoleKey.DownArrow)
                {
                    selectedIndex++;

                    if (selectedIndex >= boardQuests.Count)
                    {
                        selectedIndex = 0;
                    }
                }
                else if (keyInfo.Key == ConsoleKey.Enter)
                {
                    HandleQuestAction(player, selectedQuest, stageManager);
                }
                else if (keyInfo.Key == ConsoleKey.Backspace)
                {
                    return;
                }
            }
        }

        private Quest GetCurrentMainQuest()
        {
            for (int i = 0; i < mainQuests.Count; i++)
            {
                if (mainQuests[i].IsRewardReceived == false)
                {
                    return mainQuests[i];
                }
            }

            return null;
        }

        private void HandleQuestAction(Player player, Quest quest, StageManager stageManager)
        {
            GameConsole.SafeClear();

            if (quest.IsAccepted == false)
            {
                quest.Accept();
                Console.WriteLine("퀘스트 [" + quest.Title + "]를 수락했습니다.");

                if (quest.Type == QuestType.DefeatStageBoss && quest.RequiredStageNumber > 0)
                {
                    if (stageManager.IsStageCleared(quest.RequiredStageNumber))
                    {
                        quest.AddProgress();
                        Console.WriteLine("이미 해당 보스를 처치한 기록이 있어 즉시 완료되었습니다.");
                    }
                }
            }
            else if (quest.IsCompleted && quest.IsRewardReceived == false)
            {
                player.GainGold(quest.RewardGold);
                player.GainExp(quest.RewardExp);
                quest.ReceiveReward();

                Console.WriteLine("퀘스트 [" + quest.Title + "] 보상을 받았습니다.");

                if (quest.IsRepeatable)
                {
                    Console.WriteLine("반복 퀘스트가 초기화되었습니다. 다시 수락할 수 있습니다.");
                    quest.ResetForRepeat();
                }
            }
            else if (quest.IsRewardReceived)
            {
                Console.WriteLine("이미 완료한 퀘스트입니다.");
            }
            else
            {
                Console.WriteLine("아직 완료되지 않았습니다.");
            }

            Console.ReadKey();
        }

        public void UpdateProgress(int stageNumber, bool isBossBattle, string monsterName)
        {
            Quest currentMainQuest = GetCurrentMainQuest();

            if (currentMainQuest != null)
            {
                UpdateQuestProgress(currentMainQuest, stageNumber, isBossBattle, monsterName);
            }

            for (int i = 0; i < boardQuests.Count; i++)
            {
                UpdateQuestProgress(boardQuests[i], stageNumber, isBossBattle, monsterName);
            }
        }

        private void UpdateQuestProgress(Quest quest, int stageNumber, bool isBossBattle, string monsterName)
        {
            if (quest.IsAccepted == false || quest.IsCompleted || quest.IsRewardReceived)
            {
                return;
            }

            bool wasCompleted = quest.IsCompleted;

            if (quest.Type == QuestType.DefeatAnyMonster)
            {
                quest.AddProgress();
            }
            else if (quest.Type == QuestType.DefeatStageBoss)
            {
                if (isBossBattle && quest.RequiredStageNumber == stageNumber)
                {
                    quest.AddProgress();
                }
            }
            else if (quest.Type == QuestType.DefeatNamedMonster)
            {
                if (quest.TargetMonsterName == monsterName)
                {
                    quest.AddProgress();
                }
            }

            if (wasCompleted == false && quest.IsCompleted)
            {
                Console.WriteLine("퀘스트 [" + quest.Title + "]를 완료했습니다!");
            }
        }
    }
}