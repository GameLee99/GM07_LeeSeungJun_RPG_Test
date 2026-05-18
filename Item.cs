using System;

namespace RPG_Test
{
    internal abstract class Item
    {
        protected string name;
        protected int price;

        public string Name
        {
            get { return name; }
        }

        public int Price
        {
            get { return price; }
        }

        public Item(string name, int price)
        {
            this.name = name;
            this.price = price;
        }

        public virtual void PrintInfo()
        {
            Console.Write(name + " / 가격: " + price + "G");
        }
    }

    internal abstract class Equipment : Item
    {
        protected int bonusValue;

        public int BonusValue
        {
            get { return bonusValue; }
        }

        public Equipment(string name, int price, int bonusValue)
            : base(name, price)
        {
            this.bonusValue = bonusValue;
        }
    }

    internal class Weapon : Equipment
    {
        private PlayerClass requiredClass;

        public PlayerClass RequiredClass
        {
            get { return requiredClass; }
        }

        public Weapon(string name, int price, int bonusValue, PlayerClass requiredClass)
            : base(name, price, bonusValue)
        {
            this.requiredClass = requiredClass;
        }

        private string GetRequiredClassName()
        {
            if (requiredClass == PlayerClass.Warrior)
            {
                return "전사";
            }
            else if (requiredClass == PlayerClass.Mage)
            {
                return "마법사";
            }
            else if (requiredClass == PlayerClass.Archer)
            {
                return "궁수";
            }
            else
            {
                return "도적";
            }
        }

        public override void PrintInfo()
        {
            Console.Write(name + " / 무기 / " + GetRequiredClassName() + " 전용 / 공격력 +" + bonusValue + " / 가격: " + price + "G");
        }
    }

    internal class Armor : Equipment
    {
        public Armor(string name, int price, int bonusValue)
            : base(name, price, bonusValue)
        {
        }

        public override void PrintInfo()
        {
            Console.Write(name + " / 갑옷 / 방어력 +" + bonusValue + " / 가격: " + price + "G");
        }
    }

    internal class HeadGear : Equipment
    {
        public HeadGear(string name, int price, int bonusValue)
            : base(name, price, bonusValue)
        {
        }

        public override void PrintInfo()
        {
            Console.Write(name + " / 머리 / 방어력 +" + bonusValue + " / 가격: " + price + "G");
        }
    }

    internal class Shield : Equipment
    {
        public Shield(string name, int price, int bonusValue)
            : base(name, price, bonusValue)
        {
        }

        public override void PrintInfo()
        {
            Console.Write(name + " / 방패 / 방어력 +" + bonusValue + " / 가격: " + price + "G");
        }
    }

    internal class Greaves : Equipment
    {
        public Greaves(string name, int price, int bonusValue)
            : base(name, price, bonusValue)
        {
        }

        public override void PrintInfo()
        {
            Console.Write(name + " / 각반 / 방어력 +" + bonusValue + " / 가격: " + price + "G");
        }
    }

    internal class Shoes : Equipment
    {
        public Shoes(string name, int price, int bonusValue)
            : base(name, price, bonusValue)
        {
        }

        public override void PrintInfo()
        {
            Console.Write(name + " / 신발 / 방어력 +" + bonusValue + " / 가격: " + price + "G");
        }
    }

    internal class Ring : Equipment
    {
        public Ring(string name, int price, int bonusValue)
            : base(name, price, bonusValue)
        {
        }

        public override void PrintInfo()
        {
            Console.Write(name + " / 반지 / 모든 능력치 +" + bonusValue + " / 가격: " + price + "G");
        }
    }

    internal enum PotionType
    {
        Health,
        Mana,
        Antidote,
        Elixir
    }

    internal class Potion : Item
    {
        private PotionType potionType;
        private int healAmount;

        public PotionType PotionType
        {
            get { return potionType; }
        }

        public int HealAmount
        {
            get { return healAmount; }
        }

        public Potion(string name, PotionType potionType, int healAmount, int price)
            : base(name, price)
        {
            this.potionType = potionType;
            this.healAmount = healAmount;
        }

        public override void PrintInfo()
        {
            if (potionType == PotionType.Health)
            {
                Console.Write(name + " / 체력 포션 / 회복량: " + healAmount + " / 가격: " + price + "G");
            }
            else if (potionType == PotionType.Mana)
            {
                Console.Write(name + " / 마나 포션 / 회복량: " + healAmount + " / 가격: " + price + "G");
            }
            else if (potionType == PotionType.Antidote)
            {
                Console.Write(name + " / 해독제 / 중독 치료 / 가격: " + price + "G");
            }
            else
            {
                Console.Write(name + " / 엘릭서 / HP, MP +" + healAmount + " / 가격: " + price + "G");
            }
        }
    }
}