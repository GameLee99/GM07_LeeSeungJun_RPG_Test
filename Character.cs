using System;
using System.Collections.Generic;

namespace RPG_Test
{
    internal enum PlayerClass
    {
        Warrior,
        Mage,
        Archer,
        Thief,
        Cheat
    }

    internal enum StatusEffectType
    {
        None,
        Poison
    }

    internal abstract class Character
    {
        protected string name;
        protected int maxHp;
        protected int hp;
        protected int baseAttack;
        protected int baseDefense;

        public string Name
        {
            get { return name; }
        }

        public int MaxHp
        {
            get { return maxHp; }
        }

        public int Hp
        {
            get { return hp; }
        }

        public Character(string name, int maxHp, int attack, int defense)
        {
            this.name = name;
            this.maxHp = maxHp;
            this.hp = maxHp;
            this.baseAttack = attack;
            this.baseDefense = defense;
        }

        public virtual int GetAttackPower()
        {
            return baseAttack;
        }

        public virtual int GetDefense()
        {
            return baseDefense;
        }

        public void Heal(int amount)
        {
            hp += amount;

            if (hp > maxHp)
            {
                hp = maxHp;
            }
        }

        public void RestoreFullHealth()
        {
            hp = maxHp;
        }

        public void TakeDamage(int damage)
        {
            int realDamage = damage - GetDefense();

            if (realDamage < 1)
            {
                realDamage = 1;
            }

            hp -= realDamage;

            if (hp < 0)
            {
                hp = 0;
            }

            Console.WriteLine(name + "이(가) " + realDamage + "의 피해를 입었습니다. (남은 HP: " + hp + ")");
        }

        public void TakeGuardDamage(int damage)
        {
            int realDamage = damage - GetDefense();

            if (realDamage < 1)
            {
                realDamage = 1;
            }

            realDamage = realDamage / 2;

            if (realDamage < 1)
            {
                realDamage = 1;
            }

            hp -= realDamage;

            if (hp < 0)
            {
                hp = 0;
            }

            Console.WriteLine(name + "이(가) 방어하여 " + realDamage + "의 피해를 입었습니다. (남은 HP: " + hp + ")");
        }

        public bool IsDead()
        {
            return hp <= 0;
        }

        public virtual void PrintStatus()
        {
            Console.WriteLine(name + " / HP: " + hp + "/" + maxHp + " / 공격력: " + GetAttackPower() + " / 방어력: " + GetDefense());
        }

        public abstract void Attack(Character target);
    }

    internal class Monster : Character
    {
        private int rewardExp;
        private int rewardGold;

        public int RewardExp
        {
            get { return rewardExp; }
        }

        public int RewardGold
        {
            get { return rewardGold; }
        }

        public Monster(string name, int maxHp, int attack, int defense, int rewardExp, int rewardGold)
            : base(name, maxHp, attack, defense)
        {
            this.rewardExp = rewardExp;
            this.rewardGold = rewardGold;
        }

        public override void Attack(Character target)
        {
            Console.WriteLine(name + "이(가) 공격했다!");
            target.TakeDamage(GetAttackPower());
        }
    }

    internal class Player : Character
    {
        private PlayerClass playerClass;
        private int level;
        private int currentExp;
        private int gold;
        private int maxMp;
        private int mp;
        private Inventory inventory;
        private StatusEffectType statusEffect;

        private HeadGear equippedHead;
        private Armor equippedArmor;
        private Weapon equippedWeapon;
        private Shield equippedShield;
        private Greaves equippedGreaves;
        private Shoes equippedShoes;
        private Ring equippedRing;

        private List<Skill> skills = new List<Skill>();

        public string ClassName
        {
            get { return GetClassName(playerClass); }
        }

        public int Level
        {
            get { return level; }
        }

        public int CurrentExp
        {
            get { return currentExp; }
        }

        public int Gold
        {
            get { return gold; }
        }

        public int MaxMp
        {
            get { return maxMp; }
        }

        public int Mp
        {
            get { return mp; }
        }

        public Inventory Inventory
        {
            get { return inventory; }
        }

        public bool IsPoisoned
        {
            get { return statusEffect == StatusEffectType.Poison; }
        }

        public Weapon EquippedWeapon
        {
            get { return equippedWeapon; }
        }

        public Armor EquippedArmor
        {
            get { return equippedArmor; }
        }

        public Player(string name, PlayerClass playerClass)
            : base(name, GetStartHp(playerClass), GetStartAttack(playerClass), GetStartDefense(playerClass))
        {
            this.playerClass = playerClass;
            level = 1;
            currentExp = 0;
            gold = 100;
            maxMp = GetStartMp(playerClass);
            mp = maxMp;
            inventory = new Inventory();
            statusEffect = StatusEffectType.None;

            UnlockSkillByLevel();
        }

        private static int GetStartHp(PlayerClass playerClass)
        {
            if (playerClass == PlayerClass.Warrior)
            {
                return 130;
            }
            else if (playerClass == PlayerClass.Mage)
            {
                return 90;
            }
            else if (playerClass == PlayerClass.Archer)
            {
                return 105;
            }
            else if (playerClass == PlayerClass.Thief)
            {
                return 95;
            }
            else
            {
                return 999;
            }
        }

        private static int GetStartMp(PlayerClass playerClass)
        {
            if (playerClass == PlayerClass.Warrior)
            {
                return 40;
            }
            else if (playerClass == PlayerClass.Mage)
            {
                return 80;
            }
            else if (playerClass == PlayerClass.Archer)
            {
                return 55;
            }
            else if (playerClass == PlayerClass.Thief)
            {
                return 50;
            }
            else
            {
                return 999;
            }
        }

        private static int GetStartAttack(PlayerClass playerClass)
        {
            if (playerClass == PlayerClass.Warrior)
            {
                return 15;
            }
            else if (playerClass == PlayerClass.Mage)
            {
                return 20;
            }
            else if (playerClass == PlayerClass.Archer)
            {
                return 17;
            }
            else if (playerClass == PlayerClass.Thief)
            {
                return 18;
            }
            else
            {
                return 120;
            }
        }

        private static int GetStartDefense(PlayerClass playerClass)
        {
            if (playerClass == PlayerClass.Warrior)
            {
                return 4;
            }
            else if (playerClass == PlayerClass.Mage)
            {
                return 1;
            }
            else if (playerClass == PlayerClass.Archer)
            {
                return 2;
            }
            else if (playerClass == PlayerClass.Thief)
            {
                return 1;
            }
            else
            {
                return 50;
            }
        }

        private string GetClassName(PlayerClass playerClass)
        {
            if (playerClass == PlayerClass.Warrior)
            {
                return "전사";
            }
            else if (playerClass == PlayerClass.Mage)
            {
                return "마법사";
            }
            else if (playerClass == PlayerClass.Archer)
            {
                return "궁수";
            }
            else if (playerClass == PlayerClass.Thief)
            {
                return "도적";
            }
            else
            {
                return "치트캐릭터";
            }
        }

        public int GetNextLevelExp()
        {
            return level * 50;
        }

        public override int GetAttackPower()
        {
            int attackBonus = 0;

            if (equippedWeapon != null)
            {
                attackBonus += equippedWeapon.BonusValue;
            }

            return baseAttack + attackBonus;
        }

        public override int GetDefense()
        {
            int defenseBonus = 0;

            if (equippedHead != null)
            {
                defenseBonus += equippedHead.BonusValue;
            }

            if (equippedArmor != null)
            {
                defenseBonus += equippedArmor.BonusValue;
            }

            if (equippedShield != null)
            {
                defenseBonus += equippedShield.BonusValue;
            }

            if (equippedGreaves != null)
            {
                defenseBonus += equippedGreaves.BonusValue;
            }

            if (equippedShoes != null)
            {
                defenseBonus += equippedShoes.BonusValue;
            }

            return baseDefense + defenseBonus;
        }

        public override void Attack(Character target)
        {
            Console.WriteLine(name + "이(가) 공격했다!");
            target.TakeDamage(GetAttackPower());
        }

        public string GetStatusText()
        {
            if (statusEffect == StatusEffectType.Poison)
            {
                return "중독";
            }

            return "정상";
        }

        public void ShowPlayerStatus()
        {
            Console.WriteLine("=== 플레이어 상태 ===");
            Console.WriteLine("이름: " + name);
            Console.WriteLine("직업: " + ClassName);
            Console.WriteLine("레벨: " + level);
            Console.WriteLine("HP: " + hp + "/" + maxHp);
            Console.WriteLine("MP: " + mp + "/" + maxMp);
            Console.WriteLine("공격력: " + GetAttackPower());
            Console.WriteLine("방어력: " + GetDefense());
            Console.WriteLine("상태: " + GetStatusText());
            Console.WriteLine("경험치: " + currentExp + "/" + GetNextLevelExp());
            Console.WriteLine("골드: " + gold + "G");
            Console.WriteLine("보유 스킬 수: " + skills.Count);
        }

        private void ApplyRingBonus(Ring ring)
        {
            if (ring == null)
            {
                return;
            }

            baseAttack += ring.BonusValue;
            baseDefense += ring.BonusValue;
            maxHp += ring.BonusValue;
            hp += ring.BonusValue;
            maxMp += ring.BonusValue;
            mp += ring.BonusValue;
        }

        private void RemoveRingBonus(Ring ring)
        {
            if (ring == null)
            {
                return;
            }

            baseAttack -= ring.BonusValue;
            baseDefense -= ring.BonusValue;
            maxHp -= ring.BonusValue;
            maxMp -= ring.BonusValue;

            if (hp > maxHp)
            {
                hp = maxHp;
            }

            if (mp > maxMp)
            {
                mp = maxMp;
            }
        }

        private string GetEquipmentName(Equipment equipment)
        {
            if (equipment == null)
            {
                return "(비어 있음)";
            }

            return equipment.Name;
        }

        public void ShowEquipment()
        {
            Console.WriteLine("머리 : " + GetEquipmentName(equippedHead));
            Console.WriteLine("갑옷 : " + GetEquipmentName(equippedArmor));
            Console.WriteLine("무기 : " + GetEquipmentName(equippedWeapon));
            Console.WriteLine("방패 : " + GetEquipmentName(equippedShield));
            Console.WriteLine("각반 : " + GetEquipmentName(equippedGreaves));
            Console.WriteLine("신발 : " + GetEquipmentName(equippedShoes));
            Console.WriteLine("반지 : " + GetEquipmentName(equippedRing));
        }

        public bool IsEquippedItem(Item item)
        {
            if (item == equippedHead) return true;
            if (item == equippedArmor) return true;
            if (item == equippedWeapon) return true;
            if (item == equippedShield) return true;
            if (item == equippedGreaves) return true;
            if (item == equippedShoes) return true;
            if (item == equippedRing) return true;

            return false;
        }

        public void GainGold(int amount)
        {
            gold += amount;
            Console.WriteLine(amount + " 골드를 획득했습니다.");
        }

        public bool SpendGold(int amount)
        {
            if (gold < amount)
            {
                return false;
            }

            gold -= amount;
            return true;
        }

        public bool UseMana(int amount)
        {
            if (mp < amount)
            {
                return false;
            }

            mp -= amount;
            return true;
        }

        public void RestoreMana(int amount)
        {
            mp += amount;

            if (mp > maxMp)
            {
                mp = maxMp;
            }
        }

        public void ApplyPoison()
        {
            if (statusEffect == StatusEffectType.Poison)
            {
                return;
            }

            statusEffect = StatusEffectType.Poison;
            Console.WriteLine(name + "이(가) 중독되었습니다!");
        }

        public void CureStatusEffect()
        {
            statusEffect = StatusEffectType.None;
        }

        public void ProcessStatusEffect()
        {
            if (statusEffect == StatusEffectType.Poison)
            {
                int poisonDamage = maxHp / 10;

                if (poisonDamage < 5)
                {
                    poisonDamage = 5;
                }

                hp -= poisonDamage;

                if (hp < 0)
                {
                    hp = 0;
                }

                Console.WriteLine(name + "이(가) 중독 피해로 " + poisonDamage + "의 피해를 입었습니다. (남은 HP: " + hp + ")");
            }
        }

        public void RestoreAll()
        {
            RestoreFullHealth();
            mp = maxMp;
            statusEffect = StatusEffectType.None;
        }

        public void GainExp(int amount)
        {
            Console.WriteLine(amount + " 경험치를 획득했습니다.");
            currentExp += amount;

            while (currentExp >= GetNextLevelExp())
            {
                currentExp -= GetNextLevelExp();
                level++;

                maxHp += 20;
                maxMp += 10;
                baseAttack += 5;
                baseDefense += 2;

                RestoreAll();

                Console.WriteLine("레벨업! 현재 레벨 : " + level);
                Console.WriteLine("체력과 마나가 모두 회복되고 능력치가 상승했습니다.");

                UnlockSkillByLevel();
            }
        }

        public bool UseHealthPotion()
        {
            for (int i = 0; i < inventory.Count; i++)
            {
                Item item = inventory.GetItem(i);

                if (item is Potion)
                {
                    Potion potion = (Potion)item;

                    if (potion.PotionType == PotionType.Health)
                    {
                        Heal(potion.HealAmount);
                        inventory.RemoveItem(item);

                        Console.WriteLine(potion.Name + "을(를) 사용했습니다.");
                        Console.WriteLine("체력이 " + potion.HealAmount + " 회복되었습니다.");
                        Console.WriteLine("현재 HP: " + hp + "/" + maxHp);
                        return true;
                    }
                }
            }

            return false;
        }

        public bool UseManaPotion()
        {
            for (int i = 0; i < inventory.Count; i++)
            {
                Item item = inventory.GetItem(i);

                if (item is Potion)
                {
                    Potion potion = (Potion)item;

                    if (potion.PotionType == PotionType.Mana)
                    {
                        RestoreMana(potion.HealAmount);
                        inventory.RemoveItem(item);

                        Console.WriteLine(potion.Name + "을(를) 사용했습니다.");
                        Console.WriteLine("마나가 " + potion.HealAmount + " 회복되었습니다.");
                        Console.WriteLine("현재 MP: " + mp + "/" + maxMp);
                        return true;
                    }
                }
            }

            return false;
        }

        public bool UseAntidote()
        {
            if (statusEffect == StatusEffectType.None)
            {
                return false;
            }

            for (int i = 0; i < inventory.Count; i++)
            {
                Item item = inventory.GetItem(i);

                if (item is Potion)
                {
                    Potion potion = (Potion)item;

                    if (potion.PotionType == PotionType.Antidote)
                    {
                        inventory.RemoveItem(item);
                        statusEffect = StatusEffectType.None;

                        Console.WriteLine(potion.Name + "을(를) 사용했습니다.");
                        Console.WriteLine("중독이 해제되었습니다.");
                        return true;
                    }
                }
            }

            return false;
        }

        public bool UseElixir()
        {
            for (int i = 0; i < inventory.Count; i++)
            {
                Item item = inventory.GetItem(i);

                if (item is Potion)
                {
                    Potion potion = (Potion)item;

                    if (potion.PotionType == PotionType.Elixir)
                    {
                        Heal(potion.HealAmount);
                        RestoreMana(potion.HealAmount);
                        inventory.RemoveItem(item);

                        Console.WriteLine(potion.Name + "을(를) 사용했습니다.");
                        Console.WriteLine("체력과 마나가 각각 " + potion.HealAmount + " 회복되었습니다.");
                        Console.WriteLine("현재 HP: " + hp + "/" + maxHp);
                        Console.WriteLine("현재 MP: " + mp + "/" + maxMp);
                        return true;
                    }
                }
            }

            return false;
        }

        public bool UsePotion()
        {
            return UseHealthPotion();
        }

        public void EquipItem(Item item)
        {
            if (item is HeadGear)
            {
                if (equippedHead == item)
                {
                    Console.WriteLine(item.Name + "을(를) 해제했습니다.");
                    equippedHead = null;
                }
                else
                {
                    equippedHead = (HeadGear)item;
                    Console.WriteLine(item.Name + "을(를) 장착했습니다.");
                }
            }
            else if (item is Armor)
            {
                if (equippedArmor == item)
                {
                    Console.WriteLine(item.Name + "을(를) 해제했습니다.");
                    equippedArmor = null;
                }
                else
                {
                    equippedArmor = (Armor)item;
                    Console.WriteLine(item.Name + "을(를) 장착했습니다.");
                }
            }
            else if (item is Weapon)
            {
                Weapon weapon = (Weapon)item;

                if (playerClass != PlayerClass.Cheat && weapon.RequiredClass != playerClass)
                {
                    Console.WriteLine("이 무기는 " + GetClassName(weapon.RequiredClass) + " 전용입니다.");
                    return;
                }

                if (equippedWeapon == item)
                {
                    Console.WriteLine(item.Name + "을(를) 해제했습니다.");
                    equippedWeapon = null;
                }
                else
                {
                    equippedWeapon = weapon;
                    Console.WriteLine(item.Name + "을(를) 장착했습니다.");
                }
            }
            else if (item is Shield)
            {
                if (equippedShield == item)
                {
                    Console.WriteLine(item.Name + "을(를) 해제했습니다.");
                    equippedShield = null;
                }
                else
                {
                    equippedShield = (Shield)item;
                    Console.WriteLine(item.Name + "을(를) 장착했습니다.");
                }
            }
            else if (item is Greaves)
            {
                if (equippedGreaves == item)
                {
                    Console.WriteLine(item.Name + "을(를) 해제했습니다.");
                    equippedGreaves = null;
                }
                else
                {
                    equippedGreaves = (Greaves)item;
                    Console.WriteLine(item.Name + "을(를) 장착했습니다.");
                }
            }
            else if (item is Shoes)
            {
                if (equippedShoes == item)
                {
                    Console.WriteLine(item.Name + "을(를) 해제했습니다.");
                    equippedShoes = null;
                }
                else
                {
                    equippedShoes = (Shoes)item;
                    Console.WriteLine(item.Name + "을(를) 장착했습니다.");
                }
            }
            else if (item is Ring)
            {
                Ring ring = (Ring)item;

                if (equippedRing == item)
                {
                    RemoveRingBonus(equippedRing);
                    Console.WriteLine(item.Name + "을(를) 해제했습니다.");
                    equippedRing = null;
                }
                else
                {
                    if (equippedRing != null)
                    {
                        RemoveRingBonus(equippedRing);
                    }

                    equippedRing = ring;
                    ApplyRingBonus(equippedRing);
                    Console.WriteLine(item.Name + "을(를) 장착했습니다.");
                }
            }
            else
            {
                Console.WriteLine("이 아이템은 장착할 수 없습니다.");
            }
        }

        public void ShowSkills()
        {
            Console.WriteLine("=== 스킬 목록 ===");

            if (skills.Count == 0)
            {
                Console.WriteLine("배운 스킬이 없습니다.");
                return;
            }

            for (int i = 0; i < skills.Count; i++)
            {
                Console.Write((i + 1) + ". ");
                skills[i].PrintInfo();
            }
        }

        public Skill GetSkill(int index)
        {
            if (index < 0 || index >= skills.Count)
            {
                return null;
            }

            return skills[index];
        }

        public int GetSkillCount()
        {
            return skills.Count;
        }

        private void UnlockSkillByLevel()
        {
            if (playerClass == PlayerClass.Warrior)
            {
                AddSkillIfNotExists(new Skill("강타", "강하게 내려쳐 추가 피해를 준다.", 1, 5, 5));

                if (level >= 2)
                {
                    AddSkillIfNotExists(new Skill("베쉬", "무기로 강하게 밀어붙인다.", 2, 10, 8));
                }

                if (level >= 3)
                {
                    AddSkillIfNotExists(new Skill("파워 슬래시", "강력한 참격으로 큰 피해를 준다.", 3, 18, 12));
                }
            }
            else if (playerClass == PlayerClass.Mage)
            {
                AddSkillIfNotExists(new Skill("파이어볼", "불덩이를 날려 적을 공격한다.", 1, 7, 8));

                if (level >= 2)
                {
                    AddSkillIfNotExists(new Skill("아이스 스피어", "얼음 창으로 적을 꿰뚫는다.", 2, 12, 12));
                }

                if (level >= 3)
                {
                    AddSkillIfNotExists(new Skill("메테오", "거대한 운석을 떨어뜨린다.", 3, 22, 18));
                }
            }
            else if (playerClass == PlayerClass.Archer)
            {
                AddSkillIfNotExists(new Skill("더블 샷", "화살 두 발을 빠르게 쏜다.", 1, 6, 6));

                if (level >= 2)
                {
                    AddSkillIfNotExists(new Skill("관통 화살", "적을 꿰뚫는 강한 화살을 쏜다.", 2, 11, 10));
                }

                if (level >= 3)
                {
                    AddSkillIfNotExists(new Skill("폭우 사격", "화살비를 퍼부어 큰 피해를 준다.", 3, 19, 14));
                }
            }
            else if (playerClass == PlayerClass.Thief)
            {
                AddSkillIfNotExists(new Skill("급소 찌르기", "적의 약점을 노려 찌른다.", 1, 6, 6));

                if (level >= 2)
                {
                    AddSkillIfNotExists(new Skill("그림자 베기", "그림자처럼 움직이며 벤다.", 2, 12, 10));
                }

                if (level >= 3)
                {
                    AddSkillIfNotExists(new Skill("암살", "순간적으로 큰 피해를 준다.", 3, 20, 15));
                }
            }
            else if (playerClass == PlayerClass.Cheat)
            {
                AddSkillIfNotExists(new Skill("관리자 일격", "압도적인 힘으로 적을 제압한다.", 1, 60, 0));

                if (level >= 2)
                {
                    AddSkillIfNotExists(new Skill("시스템 브레이크", "규칙을 무시하는 공격을 가한다.", 2, 120, 10));
                }

                if (level >= 3)
                {
                    AddSkillIfNotExists(new Skill("종결 선언", "전투를 끝내는 파괴적인 일격이다.", 3, 250, 20));
                }
            }
        }

        private void AddSkillIfNotExists(Skill newSkill)
        {
            for (int i = 0; i < skills.Count; i++)
            {
                if (skills[i].Name == newSkill.Name)
                {
                    return;
                }
            }

            skills.Add(newSkill);
            Console.WriteLine("새 스킬 [" + newSkill.Name + "]을(를) 배웠습니다!");
        }
    }
}