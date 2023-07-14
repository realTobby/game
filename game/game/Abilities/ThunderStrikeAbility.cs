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

            GameManager.Instance.AddEntity(new ThunderStrikeEntity(randomPosition));
            abilityClock.Restart();
        }

    }




    //public class ThunderStrikeAbility
    //{
    //    public bool IsActive { get; set; }

    //    public Vector2f Position { get; set; }

    //    private Vector2f target;
    //    private float followDelay;
    //    private Clock followTimer;

    //    private Time spawnCooldown;
    //    private Clock spawnTimer;

    //    //public Action<ThunderStrike> OnSpawnThunder { get; set; }

    //    // Variable to track the last frame's spacebar state
    //    private bool wasSpacePressedLastFrame = false;

    //    public ThunderStrikeAbility(Vector2f initialPosition, Vector2f target, float followDelay, Time spawnCooldown)
    //    {
    //        Position = initialPosition;
    //        this.target = target;
    //        this.followDelay = followDelay;
    //        this.spawnCooldown = spawnCooldown;

    //        followTimer = new Clock();
    //        spawnTimer = new Clock();

    //        IsActive = true;
    //    }

    //    public void Update(Vector2f newTarget)
    //    {
    //        // Check if the spacebar is currently being pressed
    //        bool isSpacePressed = Keyboard.IsKeyPressed(Keyboard.Key.Space);

    //        // Only toggle IsActive when the spacebar key is first pressed
    //        if (isSpacePressed && !wasSpacePressedLastFrame)
    //        {
    //            IsActive = !IsActive;
    //        }

    //        // Update the last frame's spacebar state
    //        wasSpacePressedLastFrame = isSpacePressed;

    //        target = newTarget;

    //        // Follow the target with a delay
    //        if (followTimer.ElapsedTime.AsSeconds() > followDelay)
    //        {
    //            Position = target;
    //            followTimer.Restart();
    //        }

    //        // Spawn a new AnimatedSprite every few seconds
    //        if (spawnTimer.ElapsedTime > spawnCooldown)
    //        {
    //            if (IsActive) SpawnAnimatedSprite();


    //            spawnTimer.Restart();
    //        }
    //    }

    //    Random rnd = new Random();

    //    private void SpawnAnimatedSprite()
    //    {
    //        // Create a new AnimatedSprite at the current position
    //        // You'll need to provide the necessary arguments for the AnimatedSprite constructor
    //        //ThunderStrike newThunder = new ThunderStrike();
    //        //AnimatedSprite newSprite = new AnimatedSprite(TextureLoader.Instance.GetTexture("thunderStrike", "VFX"), 1, 13, Time.FromSeconds(0.1f));
    //        //newThunder.IsSingleShotAnimation = true;

    //        Position = new Vector2f(rnd.Next((int)Position.X - 100, (int)Position.X + 100), rnd.Next((int)Position.Y - 100, (int)Position.Y + 100));

    //        //newThunder.SetPosition(Position);

    //        // Add the new sprite to a list of sprites in your game
    //        // You'll need to provide this list
    //        //game.Controllers.Game.Instance.animatedSprites.Add(newThunder);
    //        //OnSpawnThunder?.Invoke(newThunder);
    //    }

    //    public void Draw(RenderWindow window)
    //    {
    //    }
    //}
}
