using sfmglame.Helpers;
using SFML.System;
using sfmlgame.Managers;
using sfmlgame.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sfmlgame.Framework
{
    public class PlayerStats
    {

        public string Name = "";

        public int MaxHP = 0;
        public int CurrentHP = 0;

        public int Damage = 1;
        public float MovementSpeed = 75f;

        public int XP = 0;
        public int NeededXP = 4;

        public int Level = 1;

        public Action OnPlayerLevelUp;

        public UI_ProgressBar HPBar;

        public void SetHP(int maxHP)
        {
            MaxHP = maxHP;
            CurrentHP = maxHP;

        }

        public void LevelUp(int levels)
        {
            Damage += 1;

            MovementSpeed += 1.25f;

            Level += levels;

            NeededXP = NeededXP + 5;

            OnPlayerLevelUp?.Invoke();

            //var newAbility = abilityFactory.CreateRandomAbility(this);

            //Abilities.Add(newAbility);

            SoundManager.Instance.PlayLevelUp();
        }

        public void ProcessXP(int xpAmount)
        {
            while (xpAmount > 0)
            {
                if (XP + xpAmount >= NeededXP)
                {
                    xpAmount -= (NeededXP - XP);
                    XP = 0;
                    LevelUp(1);
                }
                else
                {
                    XP += xpAmount;
                    xpAmount = 0;
                }
            }
        }

        public void RandomStatUp()
        {
            UniversalLog.LogInfo("Random Stat up!");
        }

    }
}
