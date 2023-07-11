using game.Entities;
using game.Entities.Enemies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game.Managers
{
    public class WaveManager
    {
        private List<EnemyWave> enemyWaves = new List<EnemyWave>();

        public List<Enemy> CurrentEnemies = new List<Enemy>();
        
        public void AddWave(EnemyWave wave)
        {
            enemyWaves.Add(wave);
        }

        public void StartWave()
        {
            if (CurrentEnemies.Count < 125)
            {
                var toSpawnWaves = enemyWaves.Where(x => x.IsSpawned == false);

                foreach (var item in toSpawnWaves.ToList())
                {
                    item.SpawnEnemies(CurrentEnemies);
                }
            }
            //Console.WriteLine("Current Enemy Count: " + CurrentEnemies.Count);
        }

        public void Update(Player player, float deltaTime)
        {
            // Update enemies in the current wave
            foreach (Enemy enemy in CurrentEnemies)
            {
                enemy.Update(player, deltaTime);
            }
        }

        public void AddEnemy(Enemy enemy)
        {
            CurrentEnemies.Add(enemy);
        }

        public void RemoveEnemy(Enemy enemy)
        {
            CurrentEnemies.Remove(enemy);
        }

        public void DrawEnemies(float deltaTime)
        {
            foreach (Enemy enemy in CurrentEnemies)
            {
                enemy.Draw(deltaTime);
            }
        }

    }
}
