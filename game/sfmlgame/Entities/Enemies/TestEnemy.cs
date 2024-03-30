
using SFML.Graphics;
using SFML.System;
using sfmlgame.Assets;
using sfmlgame.Entities;
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

        Random rnd = new Random();

        public TestEnemy(Vector2f initialPosition, float speed) : base(GameAssets.Instance.TextureLoader.GetTexture("TestEnemy", "Entities/Enemies"), 1, 9, Time.FromSeconds(0.1f), speed, initialPosition)
        {
            HP = 1;
            Damage = 5;

            base.MinDistance = 45f;

            var width = base.animateSpriteComponent.sprites[0].TextureRect.Width;
            var height = base.animateSpriteComponent.sprites[0].TextureRect.Height;

            debugDraw = new RectangleShape(new Vector2f(width/2, height/2))
            {
                Position = this.Position,
                FillColor = Color.Transparent,
                OutlineColor = Color.Red,
                OutlineThickness = 1,
                Origin = new Vector2f(0, 0)
            };

            

            SetPosition(initialPosition);

            //base.animateSpriteComponent = new Models.AnimatedSprite(texLoad.GetSpriteFromSheet(rnd.Next(0, 69), rnd.Next(0, 47)), initialPosition);
        }

        public override void Update(Player player, float deltaTime)
        {
            base.Update(player, deltaTime);


            var width = base.animateSpriteComponent.sprites[0].TextureRect.Width;
            var height = base.animateSpriteComponent.sprites[0].TextureRect.Height;
            if(debugDraw != null)
            {
                debugDraw.Position = new Vector2f(Position.X - width / 4, Position.Y - height / 4);
            }

            base.animateSpriteComponent.HitBoxDimensions = new FloatRect(Position.X, Position.Y, 32, 32);

            

        }

        private void ShowDebugBoundaries(RenderTexture target)
        {
            if (debugDraw == null) return;
            //target.Draw(debugDraw);
        }

        public override void Draw(RenderTexture renderTexture, float deltaTime)
        {
            base.Draw(renderTexture, deltaTime);
            ShowDebugBoundaries(renderTexture);
        }

    }
}
