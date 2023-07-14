using game.Managers;
using game.Models;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game.Entities
{
    public abstract class Entity : AnimatedSprite
    {
        public Vector2f Position { get; set; }

        public int Damage = 0;

        public Entity(string category, string entityName, int frameCount, Vector2f initialPosition)
            : base(category, entityName, frameCount)
        {
            Position = initialPosition;
        }

        public Entity(Texture texture, int rows, int columns, Time frameDuration, Vector2f initialPosition) : base(texture, rows, columns, frameDuration, initialPosition)
        {
            Position = initialPosition;
        }

        public virtual void Update()
        {
            base.Update();
        }

        public virtual void Draw(float deltaTime)
        {
            base.Draw(deltaTime);
        }

        public FloatRect GetBounds()
        {
            return HitBoxDimensions;
        }

        public bool CheckCollision(Entity other)
        {
            return GetBounds().Intersects(other.GetBounds());
        }

    }
}
