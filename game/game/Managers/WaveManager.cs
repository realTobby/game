﻿using game.Entities;
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
        
        public void AddWave(EnemyWave wave)
        {
            enemyWaves.Add(wave);
        }

        public void StartWave()
        {
            if (EntityManager.Instance.Enemies.Count < 125)
            {
                var toSpawnWaves = enemyWaves.Where(x => x.IsSpawned == false);

                foreach (var item in toSpawnWaves.ToList())
                {
                    item.SpawnEnemies();
                }
            }
            //Console.WriteLine("Current Enemy Count: " + CurrentEnemies.Count);
        }

    }
}
