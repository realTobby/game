using SFML.System;

namespace sfmlgame.Entities.Enemies
{
    public class EnemyWave
    {
        public int EnemyHP = 1;

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
            EnemyHP++;

            foreach (Vector2f position in spawnPositions)
            {
                var enemy = Game.Instance.EntityManager.CreateEnemy(position, EnemyHP);

                enemy.ResetFromPool(position);
                enemy.IsActive = true;

                //// Create an enemy at each spawn position
                //TestEnemy enemy = new TestEnemy(position, enemySpeed);
                //EntityManager.Instance.AddEntity(enemy);
            }
        }
    }
}
