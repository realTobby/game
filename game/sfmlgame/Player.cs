using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sfmlgame
{
    public class Player
    {
        private World world; // Reference to the World object

        public Sprite Sprite;
        private float speed = 200f;

        public Vector2i CurrentChunkIndex { get; private set; }

        public Player(Texture texture, Vector2f position, World world)
        {
            this.world = world; // Store the World reference
            Sprite = new Sprite(texture) { Position = position };

            // Calculate the center of the texture
            Vector2f center = new Vector2f(texture.Size.X / 2f, texture.Size.Y / 2f);
            // Set origin point to the center of the sprite
            Sprite.Origin = center;

        }

        public void Update(float deltaTime)
        {
            Vector2f movement = new Vector2f();
            if (Keyboard.IsKeyPressed(Keyboard.Key.W)) movement.Y -= speed * deltaTime;
            if (Keyboard.IsKeyPressed(Keyboard.Key.S)) movement.Y += speed * deltaTime;
            if (Keyboard.IsKeyPressed(Keyboard.Key.A)) movement.X -= speed * deltaTime;
            if (Keyboard.IsKeyPressed(Keyboard.Key.D)) movement.X += speed * deltaTime;

            Sprite.Position += movement;

            if (world == null) return;

            CurrentChunkIndex = world.CalculateChunkIndex(this.Sprite.Position);

        }
    }
}
