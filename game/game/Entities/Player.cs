using game.Controllers;
using game.Models;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game.Entities
{
    public class Player : DrawableEntity
    {
        private Vector2f precisePosition;

        private float speed = 150f; // Change this value to adjust player's speed



        public Player() : base("Entities", "priestess", 4)
        {
        }

        public void Update()
        {
            float deltaTime = Game.Instance.GetDeltaTime();

            Vector2f movement = new Vector2f(0, 0);

            if (Keyboard.IsKeyPressed(Keyboard.Key.W))
            {
                //movement.X -= speed * deltaTime.AsSeconds();
                movement.Y -= speed * deltaTime;
            }
            if (Keyboard.IsKeyPressed(Keyboard.Key.S))
            {
                //movement.X += speed * deltaTime.AsSeconds();
                movement.Y += speed * deltaTime;
            }
            if (Keyboard.IsKeyPressed(Keyboard.Key.A))
            {
                movement.X -= speed * deltaTime;
                //movement.Y += speed * deltaTime.AsSeconds();
            }
            if (Keyboard.IsKeyPressed(Keyboard.Key.D))
            {
                movement.X += speed * deltaTime;
                //movement.Y -= speed * deltaTime.AsSeconds();
            }

            precisePosition += movement;

            AnimatedSprite.Update();
            AnimatedSprite.SetPosition(new Vector2f((float)Math.Round(precisePosition.X), (float)Math.Round(precisePosition.Y)));
        }

        public void Draw()
        {
            Vector2f roundedPosition = new Vector2f((float)Math.Round(Position.X), (float)Math.Round(Position.Y));
            //base.Sprite.Position = roundedPosition;
            AnimatedSprite.SetPosition(roundedPosition);

            AnimatedSprite.Draw();
        }

        public Vector2f Position
        {
            get { return precisePosition; }  // return precise position when asked
            set
            {
                precisePosition = value;  // set both precise position
                //base.Sprite.Position = new Vector2f((float)Math.Round(value.X), (float)Math.Round(value.Y));  // and sprite position
                base.AnimatedSprite.SetPosition(new Vector2f((float)Math.Round(value.X), (float)Math.Round(value.Y)));
            }
        }
    }
}
