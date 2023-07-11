using game.Entities;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game.Abilities
{
    public abstract class Ability
    {
        public string Name { get; set; }
        public int Damage { get; set; }
        public float Cooldown { get; set; }
        public float LastActivatedTime { get; set; } // New property

        public Clock abilityClock = new Clock();

        public Ability(string name, int damage, float cooldown)
        {
            Name = name;
            Damage = damage;
            Cooldown = cooldown;
            LastActivatedTime = 0f; // Initialize to 0
            abilityClock.Restart();
        }

        public abstract void Activate();
    }
}
