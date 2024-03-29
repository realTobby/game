

using game.Entities.Enemies;
using SFML.System;
using sfmlgame.Entities;

namespace sfmlgame.Entities
{

    // REFACTOR NEEDED
    // PERFORMANCE IS GONE WITH THIS
    // ENTITIES NEED TO BE UPDATED WITH GAMESPEED
    // SOMEWHERE IN THIS IS A HUGE BOTTLENECK
    // ENEMIES DO NOT MOVE (or just move by mini-pixels, game is very very slow with enemies on the screen)


    public class EntityManager
    {
        private static readonly object _lock = new object();

        private Lazy<List<Entity>> allEntities = new Lazy<List<Entity>>(() => new List<Entity>());

        private Thread updateThread;
        private bool isUpdating;

        public int GetEnemyPoolSize => AllEntities.Where(x => x.GetType() == typeof(TestEnemy)).Count();
        public int GetDisbaledEnemyCount => AllEntities.Where(x => x.GetType() == typeof(TestEnemy) && x.IsActive == false).Count();

        public EntityManager()
        {
            updateThread = new Thread(UpdateLoop) { IsBackground = true };
        }

        public IEnumerable<Entity> AllEntities
        {
            get
            {
                lock (_lock)
                {
                    return allEntities.Value.ToList();
                }
            }
        }

        public List<Enemy> Enemies
        {
            get
            {
                return AllEntities.ToList().OfType<Enemy>().ToList();
            }
        }

        public List<Entity> noEnemyEntities
        {
            get
            {
                return AllEntities.ToList().OfType<Entity>().Where(x => !(x is Enemy)).ToList();
            }
        }

        //public List<AbilityEntity> abilityEntities
        //{
        //    get
        //    {
        //        lock (_lock)
        //        {
        //            return AllEntities.ToList().OfType<AbilityEntity>().ToList();
        //        }
        //    }
        //}

        //public List<Gem> gemEntities
        //{
        //    get
        //    {
        //        lock (_lock)
        //        {
        //            return AllEntities.ToList().OfType<Gem>().ToList();
        //        }
        //    }
        //}


        public void StartUpdatingEntities()
        {
            lock (_lock)
            {
                if (!isUpdating)
                {
                    isUpdating = true;
                    updateThread.Start();
                }
            }
        }

        public void StopUpdatingEntities()
        {
            lock (_lock)
            {
                if (isUpdating)
                {
                    isUpdating = false;
                    updateThread.Join();
                }
            }
        }

        private void UpdateLoop()
        {

            while (isUpdating)
            {
                lock (_lock)
                {
                    foreach (Enemy enemy in Enemies)
                    {
                        enemy.Update(Game.Instance.PLAYER, Game.Instance.DELTATIME);
                    }

                    

                }

                Thread.Sleep(5);
            }
        }

        //public void UpdateEntities(Player player, float deltaTime)
        //{
        //    lock(_lock)
        //    {
        //        foreach (Enemy enemy in Enemies.ToList())
        //        {
        //            enemy.Update(player, deltaTime);
        //        }
        //    }
            
        //}
        
        public void DrawEntities(SFML.Graphics.RenderTexture renderTexture, float deltaTime)
        {
            foreach (var entity in AllEntities)
            {
                entity?.Draw(renderTexture, deltaTime);
            }
        }

        public bool EntityExists(Entity entityToCheck)
        {
            lock (_lock)
            {
                return AllEntities.Contains(entityToCheck);
            }
        }

        public Enemy CreateEnemy(Vector2f pos)
        {
            // get free enemy from pool
            var freeEnemy = AllEntities.Where(x => x.IsActive == false && x.GetType() == typeof(Enemy)).FirstOrDefault() as Enemy;
            
            if(freeEnemy == null)
            {
                freeEnemy = new TestEnemy(pos, 25);
                lock(_lock)
                {
                    allEntities.Value.Add(freeEnemy);
                }
                
            }
            else
            {
                freeEnemy.ResetFromPool(pos);
            }

            return freeEnemy;
        }
    }
}
