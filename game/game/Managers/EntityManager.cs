using game.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game.Managers
{
    public class EntityManager
    {
        private List<Entity> entities;

        public EntityManager()
        {
            entities = new List<Entity>();
        }

        public void AddEntity(Entity entity)
        {
            entities.Add(entity);
        }

        public void RemoveEntity(Entity entity)
        {
            entities.Remove(entity);
        }

        public void UpdateEntities(float deltaTime)
        {
            foreach (Entity entity in entities.ToList())
            {
                entity.Update();
            }
        }

        public void DrawEntities()
        {
            foreach (Entity entity in entities.ToList())
            {
                entity.Draw();
            }
        }
    }
}
