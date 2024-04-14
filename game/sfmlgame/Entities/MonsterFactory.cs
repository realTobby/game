using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System; // Needed for Vector2f

namespace sfmlgame.Entities
{
    public class MonsterFactory
    {
        private const float spawnRadius = 300f; // Define the radius around the player where monsters can spawn

        public static void SpawnMonsterPack(int hpAmount)
        {
            var playerPos = Game.Instance.PLAYER.GetPosition();
            int monsterCount = Random.Shared.Next(6, 9); // Decide how many monsters to spawn

            for (int i = 0; i < monsterCount; i++)
            {
                // Randomize the position around the player within the spawnRadius
                double angle = Random.Shared.NextDouble() * Math.PI * 2; // Random angle
                float distance = (float)Random.Shared.NextDouble() * spawnRadius; // Random distance within the radius
                Vector2f spawnPos = new Vector2f(
                    playerPos.X + distance * (float)Math.Cos(angle),
                    playerPos.Y + distance * (float)Math.Sin(angle)
                );

                // Optional: Randomize hpAmount for each monster for more variety
                int variedHpAmount = hpAmount + Random.Shared.Next(-10, 11); // Variation by up to 10

                var enemy = Game.Instance.EntityManager.CreateEnemy(spawnPos, variedHpAmount);
            }
        }
    }
}
