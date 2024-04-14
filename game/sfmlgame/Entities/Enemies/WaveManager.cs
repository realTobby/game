
using SFML.System;
using System.Numerics;

namespace sfmlgame.Entities.Enemies
{
    public class WaveManager
    {
        private List<EnemyWave> enemyWaves = new List<EnemyWave>();

        public int EnemyHP = 1;

        public void AddWave(EnemyWave wave)
        {
            enemyWaves.Add(wave);
        }

        private void AddSpawnPoints()
        {
            if (Game.Instance.EntityManager.Enemies.Where(x => x.IsActive).Count() < 50)
            {
                //UniversalLog.LogInfo("can create new wave!");
                EnemyWave wave = new EnemyWave(1f, 25f);

                Vector2f point = Game.Instance.PLAYER.GetPosition();
                float radius = 300f;

                // TEST: wave.AddSpawnPosition(new Vector2f(100, 100));

                for (int i = 0; i < 10; i++)
                {
                    var radians = 2 * MathF.PI / 10 * i;
                    var vertical = MathF.Sin(radians);
                    var horizontal = MathF.Cos(radians);

                    var spawnDir = new Vector2f(horizontal, vertical);
                    var spawnPos = point + spawnDir * radius;
                    wave.AddSpawnPosition(new Vector2f(spawnPos.X, spawnPos.Y));
                }
                AddWave(wave);
            }
        }

        public void StartWave()
        {


            if (Game.Instance.EntityManager.Enemies.Where(x => x.IsActive).Count() < 75)
            {
                AddSpawnPoints();
                var toSpawnWaves = enemyWaves.Where(x => x.IsSpawned == false);

                EnemyHP += 2;

                foreach (var item in toSpawnWaves.ToList())
                {
                    item.SpawnEnemies(EnemyHP);
                }
            }
            enemyWaves.Clear();
            //Console.WriteLine("Current Enemy Count: " + CurrentEnemies.Count);
        }

    }
}
