using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sfmlgame.World;
using sfmlgame.Abilities;
using System.Numerics;

namespace sfmlgame.Entities
{
    public class Player
    {
        private WorldManager world; // Reference to the World object

        public Sprite Sprite;
        private float speed = 200f;

        public Vector2i CurrentChunkIndex { get; private set; }

        public List<Ability> Abilities { get; private set; } = new List<Ability>();

        public Player(Texture texture, Vector2f position, WorldManager world)
        {
            this.world = world; // Store the World reference
            Sprite = new Sprite(texture) { Position = position };

            // Calculate the center of the texture
            Vector2f center = new Vector2f(texture.Size.X / 2f, texture.Size.Y / 2f);
            // Set origin point to the center of the sprite
            Sprite.Origin = center;

            Abilities.Add(new OrbitalAbility(this, 10f, 3f, 50f, 20));


        }

        public Vector2i PreviousChunkIndex { get; private set; }

        public void Update(float deltaTime)
        {
            Vector2f movement = new Vector2f();
            if (Keyboard.IsKeyPressed(Keyboard.Key.W)) movement.Y -= speed * deltaTime;
            if (Keyboard.IsKeyPressed(Keyboard.Key.S)) movement.Y += speed * deltaTime;
            if (Keyboard.IsKeyPressed(Keyboard.Key.A)) movement.X -= speed * deltaTime;
            if (Keyboard.IsKeyPressed(Keyboard.Key.D)) movement.X += speed * deltaTime;

            Sprite.Position += movement;

            if (world == null) return;

            PreviousChunkIndex = CurrentChunkIndex;
            CurrentChunkIndex = world.CalculateChunkIndex(Sprite.Position);

            // Check if the player has moved to a new chunk
            if (CurrentChunkIndex != PreviousChunkIndex)
            {
                world.ManageChunks(Sprite.Position);
            }

            UpdatePlayerAbilities(deltaTime);

        }

        private void UpdatePlayerAbilities(float deltaTime)
        {
            foreach (Ability ability in Abilities)
            {
                ability.Update();

                if (ability.abilityClock.ElapsedTime.AsSeconds() >= ability.Cooldown)
                {
                    ability.Activate();
                    ability.LastActivatedTime = ability.abilityClock.Restart().AsSeconds();
                }
            }
        }
    }
}
