﻿using game.Controllers;
using game.Managers;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game.Entities.Enemies
{
    public class TestEnemy : Enemy
    {
        // Texture texture, int rows, int columns, Time frameDuration

        public TestEnemy(Vector2f initialPosition, float speed) : base(TextureLoader.Instance.GetTexture("TestEnemy", "Entities/Enemies"), 1, 9, Time.FromSeconds(0.1f), speed, initialPosition)
        {
            base.MinDistance = 45f;
        }

        public override void Update(Player player, float deltaTime)
        {
            base.Update(player, deltaTime);
        }

        private void DebugDraw()
        {
            RectangleShape debugShape = new RectangleShape(new Vector2f(50, 50)) // Change this size to a suitable value for your game
            {
                Position = this.Position,
                FillColor = Color.Transparent,
                OutlineColor = Color.Red,
                OutlineThickness = 2,
                //Origin = new Vector2f(-25,-25)
            };
            Game.Instance.GetRenderWindow().Draw(debugShape);
        }

        public override void Draw()
        {
            base.Draw();
            DebugDraw();
        }

    }
}
