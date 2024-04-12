
namespace sfmlgame.Entities.Enemies
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
            if (Game.Instance.EntityManager.Enemies.Where(x => x.IsActive).Count() < 75)
            {
                var toSpawnWaves = enemyWaves.Where(x => x.IsSpawned == false);

                foreach (var item in toSpawnWaves.ToList())
                {
                    item.SpawnEnemies();
                }
            }
            enemyWaves.Clear();
            //Console.WriteLine("Current Enemy Count: " + CurrentEnemies.Count);
        }

    }
}
