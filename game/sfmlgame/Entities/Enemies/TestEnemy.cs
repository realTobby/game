
using SFML.Graphics;
using SFML.System;
using sfmlgame.Assets;

namespace sfmlgame.Entities.Enemies
{
    public class TestEnemy : Enemy
    {
        

        Random rnd = new Random();

        public TestEnemy(Vector2f initialPosition, float speed) : base(GameAssets.Instance.TextureLoader.GetTexture("TestEnemy", "Entities/Enemies"), 1, 9, Time.FromSeconds(0.1f), speed, initialPosition)
        {
            HP = 10;
            Damage = 1;

            base.MinDistance = 50f;

            

            

            SetPosition(initialPosition);

            //base.animateSpriteComponent = new Models.AnimatedSprite(texLoad.GetSpriteFromSheet(rnd.Next(0, 69), rnd.Next(0, 47)), initialPosition);
        }

        public override void Update(Player player, float deltaTime)
        {
            base.Update(player, deltaTime);


            

            //base.animateSpriteComponent.HitBoxDimensions = new FloatRect(GetPosition().X, GetPosition().Y, 32, 32);

            

        }

        
        public override void Draw(RenderTexture renderTexture, float deltaTime)
        {
            base.Draw(renderTexture, deltaTime);
        }

    }
}
