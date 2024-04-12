using sfmlgame.Abilities;
using sfmlgame.Entities.Enemies;
using System.Reflection;


namespace sfmlgame.Entities.Abilitites
{
    internal class AbilityEntityFactory
    {
        public Ability CreateEntityForMatchingAbility(Player player, Type targetAbilityType, Enemy target = null)
        {
            // Get all Ability types
            var abilityTypes = Assembly.GetAssembly(typeof(Ability)).GetTypes()
                .Where(t => t.IsSubclassOf(typeof(Ability)) && !t.IsAbstract);

            // Known constructor signatures
            object[] constructorArgs = null;
            if (targetAbilityType == typeof(FireballAbility))
            {
                // create fireball entity
                constructorArgs = new object[] { player, 1.25f, 25f, 5f };
            }
            else if (targetAbilityType == typeof(OrbitalAbility))
            {
                // create orbitalEntity
                constructorArgs = new object[] { player, 5f, 5f, 100f, 3 }; // Assuming default values
            }
            else if (targetAbilityType == typeof(ThunderStrikeAbility))
            {
                // create thunderstrike entity
                constructorArgs = new object[] { 5f }; // Assuming default cooldown
            }

            if (constructorArgs != null)
            {
                return (Ability)Activator.CreateInstance(targetAbilityType, constructorArgs);
            }

            throw new InvalidOperationException("No suitable constructor found for the ability type.");
        }
    }
}
