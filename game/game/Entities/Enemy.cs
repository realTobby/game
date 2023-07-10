using game.Controllers;
using game.Managers;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game.Entities
{
    public class Enemy : Entity
    {
        private float speed;

        public Enemy(string category, string entityName, int frameCount, Vector2f initialPosition, float speed)
            : base(category, entityName, frameCount, initialPosition)
        {
            this.speed = speed;
        }

        public Enemy(Texture texture, int rows, int columns, Time frameDuration, float speed) : base(texture, rows, columns, frameDuration)
        {
            this.speed = speed;
        }


        public void Update(Player player, float deltaTime)
        {
            base.Update();

            Vector2f direction = player.Position - Position;
            float magnitude = (float)Math.Sqrt(direction.X * direction.X + direction.Y * direction.Y);

            if (magnitude != 0)
            {
                direction = direction / magnitude; // Normalize the direction vector
            }
            else
            {
                direction = new Vector2f(0, 0); // Or handle this case as appropriate for your game
            }

            Position += direction * speed * deltaTime;

            // Debug: print the enemy's position
            Console.WriteLine("Enemy position: " + Position.X + ", " + Position.Y);

            SetPosition(Position);
        }

        public void UpdateTwo(Player player)
        {
           // float deltaTime = Game.Instance.GetDeltaTime();

            // Calculate direction from current to target
            Vector2f direction = player.Position - Position;

            // Calculate distance to check if we are close to target
            float distance = MathF.Sqrt(direction.X * direction.X + direction.Y * direction.Y);

            // If we are close enough, then we can stop moving
            if (distance < speed)
            {
                Position = player.Position;
                return;
            }

            // Normalize direction
            direction /= distance;

            // Move sprite
            Position += direction * speed ;
            SetPosition(Position);
        }

    }
}
