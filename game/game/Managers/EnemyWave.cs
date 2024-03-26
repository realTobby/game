using game.Entities;
using game.Entities.Enemies;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game.Managers
{
    public class EnemyWave
    {
        private List<Vector2f> spawnPositions = new List<Vector2f>();
        private float spawnInterval;
        private float enemySpeed;

        public bool IsSpawned = false;

        public EnemyWave(float spawnInterval, float enemySpeed)
        {
            this.spawnInterval = spawnInterval;
            this.enemySpeed = enemySpeed;
        }

        public void AddSpawnPosition(Vector2f position)
        {
            spawnPositions.Add(position);
        }

        public void SpawnEnemies()
        {
            foreach (Vector2f position in spawnPositions)
            {
                var enemy = EntityManager.Instance.CreateEnemy(position);

                enemy.ResetFromPool(position);
                enemy.IsActive = true;

                //// Create an enemy at each spawn position
                //TestEnemy enemy = new TestEnemy(position, enemySpeed);
                //EntityManager.Instance.AddEntity(enemy);
            }
        }
    }
}
