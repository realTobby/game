using game.Entities.Enemies;
using game.Entities.Pickups;
using game.Entities;
using game.Scenes;
using SFML.System;
using System.Collections.Generic;
using System.Threading;
using System;
using System.Linq;

namespace game.Managers
{
    public class EntityManager
    {
        private static readonly Lazy<EntityManager> _instance = new Lazy<EntityManager>(() => new EntityManager());
        private static readonly object _lock = new object();

        private GemManager gemManager;

        private List<Entity> entities = new List<Entity>();
        private List<Enemy> enemies = new List<Enemy>();

        private Thread updateThread;
        private bool isUpdating;

        public static EntityManager Instance => _instance.Value;

        private EntityManager()
        {
            gemManager = new GemManager();
            updateThread = new Thread(UpdateLoop) { IsBackground = true };
        }

        public IEnumerable<Entity> Entities
        {
            get
            {
                lock (_lock)
                {
                    return entities.ToArray();
                }
            }
        }

        public IEnumerable<Enemy> Enemies
        {
            get
            {
                lock (_lock)
                {
                    return enemies.ToArray();
                }
            }
        }

        public void AddEntity(Entity entity)
        {
            lock (_lock)
            {
                entities.Add(entity);

                switch (entity)
                {
                    case Gem gem:
                        gemManager.ActiveGems.Add(gem);
                        break;
                    case Enemy enemy:
                        enemies.Add(enemy);
                        break;
                }
            }
        }

        public void RemoveEntity(Entity entity)
        {
            lock (_lock)
            {
                entities.Remove(entity);

                switch (entity)
                {
                    case Gem gem:
                        gemManager.ActiveGems.Remove(gem);
                        break;
                    case Enemy enemy:
                        //enemies.Remove(enemy);
                        enemy.IsActive = false;
                        break;
                }
            }
        }

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
            var clock = new Clock();

            while (isUpdating)
            {
                lock (_lock)
                {
                    var deltaTime = clock.Restart().AsSeconds();

                    gemManager.Update();

                    foreach (var entity in entities.ToArray())
                    {
                        entity?.Update();
                    }

                    foreach (var enemy in enemies.ToArray())
                    {
                        if (!enemy.IsActive) continue;
                        enemy?.Update(GameScene.Instance.player, deltaTime);
                    }
                }

                Thread.Sleep(10);
            }
        }

        public void DrawEntities(float deltaTime)
        {
            lock (_lock)
            {
                foreach (var entity in entities)
                {
                    entity?.Draw(deltaTime);
                }
            }
        }

        public bool EntityExists(Entity entityToCheck)
        {
            lock (_lock)
            {
                return entities.Contains(entityToCheck);
            }
        }

        public Enemy CreateEnemy(Vector2f pos)
        {
            // get free enemy from pool
            var freeEnemy = Enemies.Where(x => x.IsActive == false).FirstOrDefault();
            
            if(freeEnemy == null)
            {
                freeEnemy = new TestEnemy(pos, 25);
                AddEntity(freeEnemy);
            }
            else
            {
                freeEnemy.ResetFromPool(pos);
            }

            return freeEnemy;
        }


    }
}
