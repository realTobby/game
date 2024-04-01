using game.Entities.Abilitites;

using SFML.System;
using sfmlgame.Assets;


namespace sfmlgame.Abilities
{
    public class ThunderStrikeAbility : Ability
    {
        public ThunderStrikeAbility(float cooldown) : base("´ThunderStrike", 1, cooldown)
        {
            this.Icon = new SFML.Graphics.Sprite(GameAssets.Instance.TextureLoader.GetTexture("thunderIcon", "UI/Abilities"));
        }

        public override void Activate()
        {
            // get a random position in a radius arround the player
            Vector2f playerPosition = Game.Instance.PLAYER.GetPosition();

            // get a random position in a radius arround the player
            Random random = new Random();
            float radius = 100;
            float angle = (float)random.NextDouble() * 360;
            float x = (float)Math.Cos(angle) * radius;
            float y = (float)Math.Sin(angle) * radius;
            Vector2f randomPosition = new Vector2f(playerPosition.X + x, playerPosition.Y + y);


            var thunderStrikeEntity = Game.Instance.EntityManager.CreateAbilityEntity(randomPosition, typeof(ThunderStrikeEntity));
            thunderStrikeEntity.SetPosition(randomPosition);
            thunderStrikeEntity.IsActive = true;

            abilityClock.Restart();
        }

        public override void Update()
        {

        }
    }
}

