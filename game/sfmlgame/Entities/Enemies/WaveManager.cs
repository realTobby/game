
namespace sfmlgame.Entities.Enemies
{
    public class WaveManager
    {
        private List<EnemyWave> enemyWaves = new List<EnemyWave>();

        public int EnemyHP = 5;

        public void AddWave(EnemyWave wave)
        {
            enemyWaves.Add(wave);
        }

        public void StartWave()
        {
            if (Game.Instance.EntityManager.Enemies.Where(x => x.IsActive).Count() < 75)
            {
                var toSpawnWaves = enemyWaves.Where(x => x.IsSpawned == false);

                EnemyHP += 5;

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
