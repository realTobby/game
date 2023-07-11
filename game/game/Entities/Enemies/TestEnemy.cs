using game.Managers;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace game.Entities.Enemies
{
    public class TestEnemy : Enemy
    {
        private RectangleShape debugDraw;

        public TestEnemy(Vector2f initialPosition, float speed) : base(TextureLoader.Instance.GetTexture("TestEnemy", "Entities/Enemies"), 1, 9, Time.FromSeconds(0.1f), speed, initialPosition)
        {
            

            base.MinDistance = 45f;

            var width = sprites[0].TextureRect.Width;
            var height = sprites[0].TextureRect.Height;

            debugDraw = new RectangleShape(new Vector2f(width/2, height/2))
            {
                Position = this.Position,
                FillColor = Color.Transparent,
                OutlineColor = Color.Red,
                OutlineThickness = 1,
                Origin = new Vector2f(0, 0)
            };

            

            SetPosition(initialPosition);
        }

        public override void Update(Player player, float deltaTime)
        {
            base.Update(player, deltaTime);


            var width = sprites[0].TextureRect.Width;
            var height = sprites[0].TextureRect.Height;
            if(debugDraw != null)
            {
                debugDraw.Position = new Vector2f(Position.X - width / 4, Position.Y - height / 4);
            }

            HitBoxDimensions = new FloatRect(Position.X, Position.Y, 32, 32);

            
        }

        private void ShowDebugBoundaries()
        {
            if (debugDraw == null) return;
            Game.Instance.GetRenderWindow().Draw(debugDraw);
        }

        public override void Draw(float deltaTime)
        {
            base.Draw(deltaTime);
            //ShowDebugBoundaries();
        }

    }
}
