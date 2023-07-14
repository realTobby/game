using game.Entities.Enemies;
using game.Entities.Pickups;
using game.Entities;
using game.Scenes;
using SFML.System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;

namespace game.Managers
{
    public class EntityManager
    {
        private static EntityManager _instance;
        private static readonly object _lock = new object();

        private GemManager gemManager;

        private List<Entity> entities;
        private List<Enemy> enemies;

        private Thread updateThread;
        private bool isUpdating;

        public static EntityManager Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                        _instance = new EntityManager();
                    return _instance;
                }
            }
        }

        private EntityManager()
        {
            entities = new List<Entity>();
            enemies = new List<Enemy>();
            gemManager = new GemManager();

            updateThread = new Thread(UpdateLoop);
            isUpdating = false;
        }

        public List<Entity> Entities
        {
            get
            {
                lock (_lock)
                {
                    return new List<Entity>(entities);
                }
            }
        }

        public List<Enemy> Enemies
        {
            get
            {
                lock (_lock)
                {
                    return new List<Enemy>(enemies);
                }
            }
        }

        public void AddEntity(Entity entity)
        {
            lock (_lock)
            {
                if (entity is Gem || entity is MaxiGem)
                {
                    gemManager.ActiveGems.Add((Gem)entity);
                }

                entities.Add(entity);

                if (entity is Enemy enemy)
                {
                    enemies.Add(enemy);
                }
            }
        }

        public void RemoveEntity(Entity entity)
        {
            lock (_lock)
            {
                if (entities.Contains(entity))
                {
                    entities.Remove(entity);

                    if (entity is Gem || entity is MaxiGem)
                    {
                        gemManager.ActiveGems.Remove((Gem)entity);
                    }

                    if (entity is Enemy enemy)
                    {
                        enemies.Remove(enemy);
                    }
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
            float deltaTime = 0f;
            Clock clock = new Clock();

            while (true)
            {
                lock (_lock)
                {
                    if (!isUpdating)
                        break;

                    deltaTime = clock.Restart().AsSeconds();

                    gemManager.Update();

                    foreach (Entity entity in entities.ToList())
                    {
                        if (entity != null)
                            entity.Update();
                    }
                }

                UpdateEnemyEntities(GameScene.Instance.player, deltaTime);

                Thread.Sleep(10); // Adjust the sleep duration as needed
            }
        }

        public void DrawEntities(float deltaTime)
        {
            lock (_lock)
            {
                foreach (Entity entity in entities.ToList())
                {
                    if (entity != null)
                        entity.Draw(deltaTime);
                }
            }
        }

        public void UpdateEnemyEntities(Player player, float deltaTime)
        {
            lock (_lock)
            {
                foreach (Enemy enemy in enemies.ToList())
                {
                    if (enemy != null)
                        enemy.Update(player, deltaTime);
                }
            }
        }
    }
}
