using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game
{
    public class Player
    {
        RenderWindow _window;

        private Sprite sprite;
        private float speed = 150f; // Change this value to adjust player's speed

        public Player(RenderWindow window)
        {
            _window = window;
            var playerTexture = new Texture("Assets/playerPlaceholder.png");
            sprite = new Sprite(playerTexture);
        }

        public void Update(Time deltaTime)
        {
            Vector2f movement = new Vector2f(0, 0);

            if (Keyboard.IsKeyPressed(Keyboard.Key.W))
            {
                //movement.X -= speed * deltaTime.AsSeconds();
                movement.Y -= speed * deltaTime.AsSeconds();
            }
            if (Keyboard.IsKeyPressed(Keyboard.Key.S))
            {
                //movement.X += speed * deltaTime.AsSeconds();
                movement.Y += speed * deltaTime.AsSeconds();
            }
            if (Keyboard.IsKeyPressed(Keyboard.Key.A))
            {
                movement.X -= speed * deltaTime.AsSeconds();
                //movement.Y += speed * deltaTime.AsSeconds();
            }
            if (Keyboard.IsKeyPressed(Keyboard.Key.D))
            {
                movement.X += speed * deltaTime.AsSeconds();
                //movement.Y -= speed * deltaTime.AsSeconds();
            }

            sprite.Position += movement;
        }

        public void Draw()
        {
            Vector2f roundedPosition = new Vector2f((float)Math.Round(sprite.Position.X), (float)Math.Round(sprite.Position.Y));
            sprite.Position = roundedPosition;
            _window.Draw(sprite);
        }

        public Vector2f Position
        {
            get { return sprite.Position; }
            set { sprite.Position = value; }
        }
    }
}
