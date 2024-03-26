using game.Entities.Abilitites;
using game.Managers;
using game.Models;
using game.Scenes;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace game.Abilities
{
    public class ThunderStrikeAbility : Ability
    {
        public ThunderStrikeAbility(float cooldown) : base("´ThunderStrike", 1, cooldown)
        {
        }

        public override void Activate()
        {
            // get a random position in a radius arround the player
            Vector2f playerPosition = GameScene.Instance.player.Position;

            // get a random position in a radius arround the player
            Random random = new Random();
            float radius = 100;
            float angle = (float)random.NextDouble() * 360;
            float x = (float)Math.Cos(angle) * radius;
            float y = (float)Math.Sin(angle) * radius;
            Vector2f randomPosition = new Vector2f(playerPosition.X + x, playerPosition.Y + y);


            var thunderStrikeEntity = EntityManager.Instance.CreateAbilityEntity(randomPosition, typeof(ThunderStrikeEntity));
            thunderStrikeEntity.SetPosition(randomPosition);
            thunderStrikeEntity.IsActive = true;

            abilityClock.Restart();
        }

        public override void Update()
        {

        }
    }
}

