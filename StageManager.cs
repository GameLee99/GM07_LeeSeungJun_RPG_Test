using System;

namespace RPG_Test
{
    internal class StageManager
    {
        private int[] clearCounts = new int[4];
        private Random random = new Random();
        private bool gameClearTriggered = false;

        public string GetStageTitle(int stageNumber)
        {
            if (stageNumber == 1)
            {
                return "Stage 1 : 슬라임의 숲";
            }
            else if (stageNumber == 2)
            {
                return "Stage 2 : 고블린 야영지";
            }
            else if (stageNumber == 3)
            {
                return "Stage 3 : 오우거 협곡";
            }
            else
            {
                return "Stage 4 : 드래곤 둥지";
            }
        }

        public string GetStageProgressText(int stageNumber)
        {
            int count = clearCounts[stageNumber - 1];

            if (count >= 10)
            {
                return "진행도 : 클리어 완료 / 자유 전투 가능";
            }

            return "진행도 : " + count + " / 10";
        }

        public string GetBattleStatusText(int stageNumber)
        {
            int count = clearCounts[stageNumber - 1];

            if (count >= 10)
            {
                return "자유 전투";
            }

            if (count == 9)
            {
                return "보스 전투 : 10 / 10";
            }

            return "현재 전투 : " + (count + 1) + " / 10";
        }

        public bool IsStageCleared(int stageNumber)
        {
            return clearCounts[stageNumber - 1] >= 10;
        }

        public bool IsBossBattle(int stageNumber)
        {
            return clearCounts[stageNumber - 1] == 9;
        }

        public bool ConsumeGameClearFlag()
        {
            if (gameClearTriggered == false)
            {
                return false;
            }

            gameClearTriggered = false;
            return true;
        }

        public Monster CreateMonsterByStage(int stageNumber)
        {
            if (IsBossBattle(stageNumber))
            {
                return CreateBossMonster(stageNumber);
            }

            string name = "";
            int maxHp = 0;
            int attack = 0;
            int defense = 0;
            int rewardExp = 0;
            int rewardGold = 0;

            int randomNumber = random.Next(2);

            if (stageNumber == 1)
            {
                if (randomNumber == 0)
                {
                    name = "슬라임";
                    maxHp = 40;
                    attack = 8;
                    defense = 0;
                    rewardExp = 30;
                    rewardGold = 20;
                }
                else
                {
                    name = "숲늑대";
                    maxHp = 55;
                    attack = 10;
                    defense = 1;
                    rewardExp = 35;
                    rewardGold = 25;
                }
            }
            else if (stageNumber == 2)
            {
                if (randomNumber == 0)
                {
                    name = "고블린";
                    maxHp = 75;
                    attack = 12;
                    defense = 2;
                    rewardExp = 50;
                    rewardGold = 35;
                }
                else
                {
                    name = "고블린 궁수";
                    maxHp = 65;
                    attack = 14;
                    defense = 1;
                    rewardExp = 55;
                    rewardGold = 38;
                }
            }
            else if (stageNumber == 3)
            {
                if (randomNumber == 0)
                {
                    name = "오우거";
                    maxHp = 130;
                    attack = 19;
                    defense = 4;
                    rewardExp = 80;
                    rewardGold = 60;
                }
                else
                {
                    name = "오크 전사";
                    maxHp = 115;
                    attack = 21;
                    defense = 3;
                    rewardExp = 85;
                    rewardGold = 65;
                }
            }
            else
            {
                if (randomNumber == 0)
                {
                    name = "와이번";
                    maxHp = 210;
                    attack = 28;
                    defense = 6;
                    rewardExp = 150;
                    rewardGold = 120;
                }
                else
                {
                    name = "드레이크";
                    maxHp = 230;
                    attack = 30;
                    defense = 7;
                    rewardExp = 160;
                    rewardGold = 130;
                }
            }

            bool isRare = random.Next(100) < 10;

            if (isRare)
            {
                name = "희귀한 " + name;
                maxHp = (int)(maxHp * 2.5);
                attack = (int)(attack * 2.5);
                defense = (int)(defense * 2.5);
                rewardExp = (int)(rewardExp * 2.5);
                rewardGold = (int)(rewardGold * 2.5);
            }

            return new Monster(name, maxHp, attack, defense, rewardExp, rewardGold);
        }

        private Monster CreateBossMonster(int stageNumber)
        {
            if (stageNumber == 1)
            {
                return new Monster("슬라임 킹", 170, 28, 5, 150, 100);
            }
            else if (stageNumber == 2)
            {
                return new Monster("고블린 대장", 240, 38, 8, 230, 150);
            }
            else if (stageNumber == 3)
            {
                return new Monster("오우거 족장", 380, 55, 12, 330, 220);
            }
            else
            {
                return new Monster("고룡", 700, 85, 22, 700, 500);
            }
        }

        public string ProcessVictory(int stageNumber)
        {
            int index = stageNumber - 1;

            if (clearCounts[index] >= 10)
            {
                return GetStageTitle(stageNumber) + "\n자유 전투에서 승리했습니다.";
            }

            bool wasBossBattle = clearCounts[index] == 9;
            clearCounts[index]++;

            if (wasBossBattle)
            {
                if (stageNumber == 4)
                {
                    gameClearTriggered = true;
                    return GetStageTitle(stageNumber) + "\n고룡을 쓰러뜨렸습니다!\n게임을 클리어했습니다!";
                }

                return GetStageTitle(stageNumber) + "\n보스를 쓰러뜨렸습니다!\n스테이지를 클리어했습니다.";
            }

            return GetStageTitle(stageNumber) + "\n전투에서 승리했습니다.\n현재 진행도 : " + clearCounts[index] + " / 10";
        }

        public Item CreateDropItem(int stageNumber)
        {
            int chance = random.Next(100);

            if (chance >= 35)
            {
                return null;
            }

            int randomNumber;

            if (stageNumber == 1)
            {
                randomNumber = random.Next(3);

                if (randomNumber == 0)
                {
                    return new Potion("소형 체력 포션", PotionType.Health, 30, 20);
                }
                else if (randomNumber == 1)
                {
                    return CreateRandomBasicWeapon();
                }
                else
                {
                    return new HeadGear("천 모자", 50, 1);
                }
            }
            else if (stageNumber == 2)
            {
                randomNumber = random.Next(3);

                if (randomNumber == 0)
                {
                    return new Potion("중형 체력 포션", PotionType.Health, 50, 35);
                }
                else if (randomNumber == 1)
                {
                    return new Armor("사슬 조끼", 90, 2);
                }
                else
                {
                    return new Shield("나무 방패", 80, 2);
                }
            }
            else if (stageNumber == 3)
            {
                randomNumber = random.Next(3);

                if (randomNumber == 0)
                {
                    return new Greaves("강철 각반", 120, 3);
                }
                else if (randomNumber == 1)
                {
                    return new Shoes("가죽 장화", 100, 2);
                }
                else
                {
                    return new Ring("힘의 반지", 140, 2);
                }
            }
            else
            {
                randomNumber = random.Next(4);

                if (randomNumber == 0)
                {
                    return CreateRandomAdvancedWeapon();
                }
                else if (randomNumber == 1)
                {
                    return new Armor("판금 갑옷", 220, 5);
                }
                else if (randomNumber == 2)
                {
                    return new Ring("수호 반지", 180, 3);
                }
                else
                {
                    return new Potion("대형 체력 포션", PotionType.Health, 80, 60);
                }
            }
        }

        private Weapon CreateRandomBasicWeapon()
        {
            int randomNumber = random.Next(4);

            if (randomNumber == 0)
            {
                return new Weapon("낡은 검", 60, 2, PlayerClass.Warrior);
            }
            else if (randomNumber == 1)
            {
                return new Weapon("낡은 지팡이", 60, 2, PlayerClass.Mage);
            }
            else if (randomNumber == 2)
            {
                return new Weapon("낡은 활", 60, 2, PlayerClass.Archer);
            }
            else
            {
                return new Weapon("낡은 단검", 60, 2, PlayerClass.Thief);
            }
        }

        private Weapon CreateRandomAdvancedWeapon()
        {
            int randomNumber = random.Next(4);

            if (randomNumber == 0)
            {
                return new Weapon("기사의 검", 200, 5, PlayerClass.Warrior);
            }
            else if (randomNumber == 1)
            {
                return new Weapon("현자의 지팡이", 200, 5, PlayerClass.Mage);
            }
            else if (randomNumber == 2)
            {
                return new Weapon("강궁", 200, 5, PlayerClass.Archer);
            }
            else
            {
                return new Weapon("암살자 단검", 200, 5, PlayerClass.Thief);
            }
        }
    }
}