using game.Entities;
using game.Entities.Enemies;
using game.Entities.Pickups;
using game.Scenes;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace game.Managers
{
    public class EntityManager
    {
        [AllowNull]
        private static EntityManager _instance;
        public static EntityManager Instance => _instance;

        private GemManager gemManager;

        private List<Entity> entities;
        private List<Enemy> enemies;

        private Thread updateThread;
        private bool isUpdating;

        public EntityManager()
        {
            if (_instance == null) _instance = this;

            entities = new List<Entity>();
            enemies = new List<Enemy>();
            gemManager = new GemManager();

            updateThread = new Thread(UpdateLoop);
            isUpdating = false;

        }

        public List<Entity> Entities => entities;
        public List<Enemy> Enemies => enemies;

        public void AddEntity(Entity entity)
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

        public void RemoveEntity(Entity entity)
        {
            if (entity is Gem || entity is MaxiGem)
            {
                gemManager.ActiveGems.Remove((Gem)entity);
            }

            entities.Remove(entity);

            if (entity is Enemy enemy)
            {
                enemies.Remove(enemy);
            }
        }

        public void StartUpdatingEntities()
        {
            if (!isUpdating)
            {
                isUpdating = true;
                updateThread.Start();
            }
        }

        public void StopUpdatingEntities()
        {
            if (isUpdating)
            {
                isUpdating = false;
                updateThread.Join();
            }
        }

        private void UpdateLoop()
        {
            float deltaTime = 0f;
            Clock clock = new Clock();

            while (isUpdating)
            {
                deltaTime = clock.Restart().AsSeconds();

                gemManager.Update();

                Parallel.ForEach(entities.ToList(), entity =>
                {
                    if(entity != null) entity.Update();

                });

                UpdateEnemyEntities(GameScene.Instance.player, deltaTime);

                Thread.Sleep(10); // Adjust the sleep duration as needed
            }
        }

        public void DrawEntities(float deltaTime)
        {
            foreach (Entity entity in entities.ToList())
            {
                if(entity != null) entity.Draw(deltaTime);

            }
        }

        public void UpdateEnemyEntities(Player player, float deltaTime)
        {
            foreach (Enemy enemy in GameManager.Instance.GetEntities(new Type[] { typeof(Enemy) }))
            {
                if(enemy != null) enemy.Update(player, deltaTime);

            }
        }


    }
}
