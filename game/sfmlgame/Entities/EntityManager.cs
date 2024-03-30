using System.Collections.Concurrent;
using System.Threading.Tasks;
using game.Entities.Enemies;
using SFML.System;
using sfmlgame.Entities;

namespace sfmlgame.Entities
{
    public class EntityManager
    {
        private ConcurrentBag<Entity> allEntities = new ConcurrentBag<Entity>();

        // Removed the separate thread for updating entities. Consider using Task or ThreadPool for background operations if needed.

        public EntityManager()
        {
        }

        // Simplified properties for accessing entities
        public IEnumerable<Entity> AllEntities => allEntities;

        public IEnumerable<Enemy> Enemies => allEntities.OfType<Enemy>();

        // Simplified property for non-enemy entities
        public IEnumerable<Entity> NoEnemyEntities => allEntities.Where(x => !(x is Enemy));

        // Method to start any background tasks for updating entities, if necessary.
        // Consider using async/await with Tasks for any heavy or IO-bound operations.

        // Removed the UpdateLoop method. Updates are now handled more efficiently in the main game loop.

        public void UpdateEntities(float frameTime)
        {
            //float lastFrameDeltaTime = Game.Instance.DELTATIME;

            // Using parallel foreach for thread-safe iteration and potential performance improvement
            Parallel.ForEach(Enemies, enemy =>
            {
                enemy.Update(Game.Instance.PLAYER, frameTime);
                // Implement other entity-specific updates as needed
            });
        }

        public void DrawEntities(SFML.Graphics.RenderTexture renderTexture, float deltaTime)
        {
            foreach (var entity in allEntities)
            {
                entity?.Draw(renderTexture, deltaTime);
            }
            Thread.Sleep(5);
        }

        public bool EntityExists(Entity entityToCheck)
        {
            // Efficiently checks for existence without locking
            return allEntities.Contains(entityToCheck);
        }

        public Enemy CreateEnemy(Vector2f pos)
        {
            // Attempt to reuse a disabled enemy from the pool
            var freeEnemy = allEntities.FirstOrDefault(x => !x.IsActive && x is Enemy) as Enemy;

            if (freeEnemy == null)
            {
                freeEnemy = new TestEnemy(pos, 50);
                allEntities.Add(freeEnemy);
            }
            else
            {
                freeEnemy.ResetFromPool(pos);
            }

            return freeEnemy;
        }
    }
}
