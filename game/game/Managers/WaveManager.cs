﻿using game.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game.Managers
{
    public class WaveManager
    {
        public List<Enemy> CurrentEnemies = new List<Enemy>();
        private List<EnemyWave> enemyWaves = new List<EnemyWave>();

        public void AddWave(EnemyWave wave)
        {
            enemyWaves.Add(wave);
        }

        public void StartWave()
        {
           // CurrentEnemies.Clear();

            var toSpawnWaves = enemyWaves.Where(x => x.IsSpawned == false);

            foreach(var item in toSpawnWaves.ToList())
            {
                item.SpawnEnemies(CurrentEnemies);
            }
        }

        public void Update(Player player, float deltaTime)
        {
            Console.WriteLine("Current Enemy Count: " + CurrentEnemies.Count);
            // Update enemies in the current wave
            foreach (Enemy enemy in CurrentEnemies)
            {
                enemy.Update(player, deltaTime);
            }

            //// Check if all enemies in the current wave have been defeated
            //if (CurrentEnemies.Count == 0)
            //{
            //    isWaveActive = false;
            //    currentWaveIndex++;

            //    if (currentWaveIndex < enemyWaves.Count)
            //    {
            //        // Start the next wave
            //        StartWave();
            //    }
            //    else
            //    {
            //        // All waves have been completed
            //    }
            //}
        }
    }
}
