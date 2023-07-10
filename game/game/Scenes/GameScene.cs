using game.Controllers;
using game.Controllers.game.Controllers;
using game.Entities;
using game.Entities.Enemies;
using game.Managers;
using game.Models;
using game.UI;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game.Scenes
{
    public class GameScene : Scene
    {
        private UIManager _uiManager;
        private OverworldManager _overworldManager;
        private ViewCamera _viewCamera;

        private InputManager _inputManager;

        private Player player;
        private AnimatedSprite testThunder;
        private ZeusMode follower;

        public List<Entity> SpawnedEntities = new List<Entity>();

        public List<Enemy> CurrentEnemies = new List<Enemy>();

        TestEnemy worm;

        

        public GameScene()
        {
            _inputManager = new InputManager();

            _uiManager = new UIManager();
            // add UI components to the manager

            _overworldManager = new OverworldManager(100);

            player = new Player(new Vector2f(0,0));

            _viewCamera = new ViewCamera();

            //testThunder = new AnimatedSprite(TextureLoader.Instance.GetTexture("thunderStrike", "VFX"), 1, 13, Time.FromSeconds(0.1f));
            //testThunder.IsSingleShotAnimation = true;
            //animatedSprites.Add(testThunder);

            follower = new ZeusMode(player.Position, player.Position, 0, Time.FromSeconds(.5f));

            //AnimatedSprite explosion = new AnimatedSprite(TextureLoader.Instance.GetTexture("EXPLOSION", "VFX"), 1, 12, Time.FromSeconds(0.1f));
            //animatedSprites.Add(explosion);

            _uiManager = new UIManager();

            //follower.OnSpawnThunder += SpawnThunder;

            //worm = new TestEnemy(player.Position, 100f);

            //animatedSprites.Add(worm);
            RandomlySpawnEnemies(1f, 3f, 100f); // Example usage with a minimum interval of 2 seconds, maximum interval of 5 seconds, and a radius of 200 units
        }

        private void SpawnEnemiesAroundPlayer(Vector2f playerPosition, float radius)
        {
            int numEnemies = 5; // Change the number of enemies as desired
            float angleIncrement = 360f / numEnemies;
            float currentAngle = 0f;

            for (int i = 0; i < numEnemies; i++)
            {
                // Calculate the position of the enemy based on the angle and radius
                float angleRad = MathF.PI * currentAngle / 180f;
                float x = playerPosition.X + radius * MathF.Cos(angleRad);
                float y = playerPosition.Y + radius * MathF.Sin(angleRad);

                // Create and add the enemy to the list
                TestEnemy enemy = new TestEnemy(new Vector2f(x, y), 100f);
                CurrentEnemies.Add(enemy);

                // Increment the angle for the next enemy
                currentAngle += angleIncrement;
            }
        }




        private void RandomlySpawnEnemies(float minInterval, float maxInterval, float radius)
        {
            Random random = new Random();
            float interval = random.Next((int)(minInterval * 1000), (int)(maxInterval * 1000)) / 1000f;

            Task.Delay((int)(interval * 1000)).ContinueWith(_ =>
            {
                SpawnEnemiesAroundPlayer(player.Position, radius); // Pass the player's position here
                RandomlySpawnEnemies(minInterval, maxInterval, radius);
            });
        }

        //private void SpawnThunder(ThunderStrike obj)
        //{
        //    //SpawnedEntities.Add(obj);
        //}

        public override void LoadContent()
        {
            // load resources if needed

        }

        private void HandlePlayerMovement(float deltaTime)
        {
            Vector2f movement = new Vector2f(0, 0);

            if (_inputManager.IsKeyPressed(Keyboard.Key.W))
            {
                player.MoveUp(deltaTime);
            }
            if (_inputManager.IsKeyPressed(Keyboard.Key.S))
            {
                player.MoveDown(deltaTime);
            }
            if (_inputManager.IsKeyPressed(Keyboard.Key.A))
            {
                player.MoveLeft(deltaTime);
            }
            if (_inputManager.IsKeyPressed(Keyboard.Key.D))
            {
                player.MoveRight(deltaTime);
            }
        }

        public override void Update(float deltaTime)
        {
            _inputManager.Update();

            HandlePlayerMovement(deltaTime);

            _uiManager.Update();
            player.Update(deltaTime);
            _viewCamera.Update(/*CurrentEnemies?.FirstOrDefault()?.Position ??*/ player.Position);
            follower.Update(player.Position);
            //worm.Update(player, deltaTime);
            UpdateEnemies(deltaTime);

        }

        private void UpdateEnemies(float deltaTime)
        {
            foreach (Enemy enemy in CurrentEnemies.ToList())
            {
                enemy.Update(player, deltaTime);
            }
        }

        private void DrawEnemies()
        {
            foreach (Enemy enemy in CurrentEnemies.ToList())
            {
                enemy.Draw();
            }
        }

        public override void Draw()
        {
            _uiManager.Draw();
            _overworldManager?.Draw();
            player.Draw();
            //HandleAnimations();
            DrawEnemies();
        }


        public override void UnloadContent()
        {
            // unload resources
        }

        private void HandleAnimations()
        {
            foreach (var sprite in SpawnedEntities.ToList())
            {
                if (sprite.IsFinished)
                {
                    SpawnedEntities.Remove(sprite);
                }
                else
                {
                    sprite.Update();
                    sprite.Draw();
                }
            }
        }

    }
}
