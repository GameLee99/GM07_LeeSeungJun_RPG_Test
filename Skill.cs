using System;

namespace RPG_Test
{
    internal class Skill
    {
        private string name;
        private string description;
        private int requiredLevel;
        private int bonusDamage;
        private int manaCost;

        public string Name
        {
            get { return name; }
        }

        public string Description
        {
            get { return description; }
        }

        public int RequiredLevel
        {
            get { return requiredLevel; }
        }

        public int BonusDamage
        {
            get { return bonusDamage; }
        }

        public int ManaCost
        {
            get { return manaCost; }
        }

        public Skill(string name, string description, int requiredLevel, int bonusDamage, int manaCost)
        {
            this.name = name;
            this.description = description;
            this.requiredLevel = requiredLevel;
            this.bonusDamage = bonusDamage;
            this.manaCost = manaCost;
        }

        public void PrintInfo()
        {
            Console.WriteLine(
                name +
                " / 설명: " + description +
                " / 필요 레벨: " + requiredLevel +
                " / 추가 데미지: +" + bonusDamage +
                " / 소모 MP: " + manaCost
            );
        }
    }
}