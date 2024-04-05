

using sfmlgame.Entities;

using System.Reflection;


namespace sfmlgame.Abilities
{

    public class AbilityFactory
    {
        private Random rnd = new Random();

        public Ability? CreateRandomAbility(Player player)
        {
            // Get all Ability types
            var abilityTypes = Assembly.GetAssembly(typeof(Ability)).GetTypes()
                .Where(t => t.IsSubclassOf(typeof(Ability)) && !t.IsAbstract);

            // Randomly pick one ability type
            var abilityType = abilityTypes.ElementAt(rnd.Next(abilityTypes.Count()));

            // Known constructor signatures
            object[]? constructorArgs = null;
            if (abilityType == typeof(FireballAbility))
            {
                constructorArgs = new object[] { player, 1.25f};
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
                    constructorArgs = new object[] { player, 1.25f };
                }
            }

            if (constructorArgs != null)
            {
                return Activator.CreateInstance(abilityType, constructorArgs) as Ability;
            }

            throw new InvalidOperationException("No suitable constructor found for the ability type.");
        }
    }

}
