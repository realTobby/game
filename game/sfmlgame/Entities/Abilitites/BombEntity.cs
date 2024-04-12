using sfmglame.Helpers;
using SFML.Graphics;
using SFML.System;
using sfmlgame.Assets;
using sfmlgame.Entities.Enemies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sfmlgame.Entities.Abilitites
{
    public class BombEntity : AbilityEntity
    {
        public Vector2f targetPos;

        public BombEntity(string name, Vector2f initialPosition, Vector2f targetPos) : base(name, initialPosition, new Sprite(GameAssets.Instance.TextureLoader.GetTexture("bomb", "Entities/Abilities")))
        {
            CanCheckCollision = true;

            this.targetPos = targetPos;

            SetPosition(initialPosition);

            Damage = 1;



        }

        public override void Draw(RenderTexture renderTexture, float deltaTime)
        {
            if (!IsActive) return;

            base.Draw(renderTexture, deltaTime);
        }


        public override void Update(Player player, float deltaTime)
        {
            //UniversalLog.LogInfo("updating bomb...");

            // move y position towards targetPos
            var pos = GetPosition();



            if (pos.Y < targetPos.Y)
            {
                pos.Y += 25f * deltaTime;

                SetPosition(pos);
            }
            else
            {
                CanCheckCollision = true;
            }



            base.Update(player, deltaTime);
        }

        public override void CollidedWith(Entity collision)
        {
            if (collision.GetType() == typeof(Enemy))
            {
                IsActive = false;
                CanCheckCollision = false;
            }

        }
    }

}

