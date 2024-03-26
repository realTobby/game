
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game.Abilities
{
    using System;
    using System.Linq;
    using System.Reflection;
    using game.Abilities;
    using game.Entities;

    public class AbilityFactory
    {
        private Random rnd = new Random();

        public Ability CreateRandomAbility(Player player)
        {
            // Get all Ability types
            var abilityTypes = Assembly.GetAssembly(typeof(Ability)).GetTypes()
                .Where(t => t.IsSubclassOf(typeof(Ability)) && !t.IsAbstract);

            // Randomly pick one ability type
            var abilityType = abilityTypes.ElementAt(rnd.Next(abilityTypes.Count()));

            // Known constructor signatures
            object[] constructorArgs = null;
            if (abilityType == typeof(FireballAbility))
            {
                constructorArgs = new object[] { player, 1.25f, 25f, 5f };
            }
            else if (abilityType == typeof(OrbitalAbility))
            {
                constructorArgs = new object[] { player, 5f, 5f, 50f, 3 }; // Assuming default values
            }
            else if (abilityType == typeof(ThunderStrikeAbility))
            {
                constructorArgs = new object[] { 5f }; // Assuming default cooldown
            }


            if(abilityType == typeof(OrbitalAbility))
            {
                if(player.Abilities.Any(x => x.GetType() == typeof(OrbitalAbility)))
                {
                    abilityType = typeof(FireballAbility);
                    constructorArgs = new object[] { player, 1.25f, 25f, 5f };
                }
            }

            if (constructorArgs != null)
            {
                return (Ability)Activator.CreateInstance(abilityType, constructorArgs);
            }

            throw new InvalidOperationException("No suitable constructor found for the ability type.");
        }
    }

}
