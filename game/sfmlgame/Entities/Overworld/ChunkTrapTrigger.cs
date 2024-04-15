using SFML.Graphics;
using SFML.System;
using sfmlgame.Framework;
using System.Drawing;

namespace sfmlgame.Entities.Overworld
{
    public class ChunkTrapTrigger : Entity
    {
        public bool IsActivated { get; private set; }

        private RenderTexture perfectSizeButtonSprite;


        public ChunkTrapTrigger(Vector2f initialPosition, Sprite _buttonSprite) : base(_buttonSprite, initialPosition)
        {
            CanCheckCollision = true; // Enable collision checking for traps
            IsActivated = false;

           

        }

        public override void ResetFromPool(Vector2f position)
        {
            IsActive = true;
            IsActivated = false;
            SetPosition(position);
        }

        public override void CollidedWith(Entity collision)
        {
            // Ensure the trap is activated only once per game or reset
            if (!IsActivated && collision == Game.Instance.PLAYER)
            {
                IsActivated = true;
                MonsterFactory.SpawnMonsterPack(Game.Instance.PLAYER.Level * 2); // Example scaling factor
            }
        }

        public override void Draw(RenderTexture renderTexture, float deltaTime)
        {
            base.Draw(renderTexture, deltaTime);
        }

        public override void Update(Player player, float deltaTime)
        {
            base.Update(player, deltaTime);
        }


    }
}
