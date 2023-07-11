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
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace game.Scenes
{
    public class GameScene : Scene
    {
        private Random rnd = new Random();

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

        private WaveManager _waveManager;

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

            _waveManager = new WaveManager();

           

            waveTimer = new Clock();

            ////animatedSprites.Add(worm);
            //RandomlySpawnEnemies(1f, 3f, 100f); // Example usage with a minimum interval of 2 seconds, maximum interval of 5 seconds, and a radius of 200 units
        }

        private void CreateWaves()
        {
            EnemyWave wave = new EnemyWave(1f, 35f); // Adjust the spawn interval and enemy speed as desired

            int num = rnd.Next(5,20);
            Vector2f point = player.Position;
            float radius = 250f;

            for (int i = 0; i < num; i++)
            {

                /* Distance around the circle */
                var radians = 2 * MathF.PI / num * i;

                /* Get the vector direction */
                var vertical = MathF.Sin(radians);
                var horizontal = MathF.Cos(radians);

                var spawnDir = new Vector2f(horizontal, vertical);
                var spawnPos = point + spawnDir * radius; // Radius is just the distance away from the point

                /* Now spawn */
                wave.AddSpawnPosition(new Vector2f(spawnPos.X, spawnPos.Y));
            }
            _waveManager.AddWave(wave);
        }

        private void StartNextWave()
        {
            _waveManager.StartWave();
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
            _waveManager.Update(player, deltaTime);
            //UpdateEnemies(deltaTime);

            UpdateEnemyWave();

        }

        private Clock waveTimer;
        private float waveCooldown = 5f;
        private void UpdateEnemyWave()
        {
            if (waveTimer.ElapsedTime.AsSeconds() > waveCooldown)
            {
                waveTimer.Restart();
                CreateWaves();
                StartNextWave();
                waveCooldown = rnd.Next(5, 15);
            }
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
            foreach (Enemy enemy in _waveManager.CurrentEnemies)
            {
                enemy.Draw();
            }
        }

        public override void Draw()
        {
            _uiManager.Draw();
            _overworldManager?.Draw();
            
            //HandleAnimations();
            DrawEnemies();

            player.Draw();
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
