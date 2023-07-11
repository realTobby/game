using game.Abilities;
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
        private static GameScene _instance;
        public static GameScene Instance => _instance;

        private Random rnd = new Random();

        private UIManager _uiManager;
        private OverworldManager _overworldManager;
        private ViewCamera _viewCamera;

        private InputManager _inputManager;

        public Player player;

        TestEnemy worm;

        

        private GameManager gameManager;

        public GameScene()
        {
            if (_instance == null) _instance = this;

            _inputManager = new InputManager();

            _uiManager = new UIManager();
            // add UI components to the manager

            _overworldManager = new OverworldManager(100);

            player = new Player(new Vector2f(0,0));

            _viewCamera = new ViewCamera();

            //testThunder = new AnimatedSprite(TextureLoader.Instance.GetTexture("thunderStrike", "VFX"), 1, 13, Time.FromSeconds(0.1f));
            //testThunder.IsSingleShotAnimation = true;
            //animatedSprites.Add(testThunder);

            //follower = new ZeusMode(player.Position, player.Position, 0, Time.FromSeconds(.5f));

            //AnimatedSprite explosion = new AnimatedSprite(TextureLoader.Instance.GetTexture("EXPLOSION", "VFX"), 1, 12, Time.FromSeconds(0.1f));
            //animatedSprites.Add(explosion);

            _uiManager = new UIManager();

            //follower.OnSpawnThunder += SpawnThunder;

            //worm = new TestEnemy(player.Position, 100f);


            waveTimer = new Clock();

            gameManager = GameManager.Instance;

            ////animatedSprites.Add(worm);
            //RandomlySpawnEnemies(1f, 3f, 100f); // Example usage with a minimum interval of 2 seconds, maximum interval of 5 seconds, and a radius of 200 units
        }

        private void GenerateNewWave()
        {
            if(gameManager._waveManager.CurrentEnemies.Count < 125)
            {
                EnemyWave wave = new EnemyWave(1f, 25f); // Adjust the spawn interval and enemy speed as desired

                int num = rnd.Next(5, 10);
                Vector2f point = player.Position;
                float radius = 300f;

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
                gameManager._waveManager.AddWave(wave);
            }
        }

        private void StartNextWave()
        {
            gameManager._waveManager.StartWave();
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
            //follower.Update(player.Position);
            //worm.Update(player, deltaTime);
            gameManager._waveManager.Update(player, deltaTime);
            //UpdateEnemies(deltaTime);

            UpdateEnemyWave();

            UpdatePlayerAbilities(deltaTime);

            gameManager.Update(deltaTime);

        }

        private void UpdatePlayerAbilities(float deltaTime)
        {
            foreach (Ability ability in player.Abilities)
            {
                if (ability.abilityClock.ElapsedTime.AsSeconds() >= ability.Cooldown)
                {
                    ability.Activate();
                    ability.LastActivatedTime = ability.abilityClock.Restart().AsSeconds();
                }
            }
        }

        public Enemy FindNearestEnemy(Vector2f position, List<Enemy> enemies)
        {
            Enemy nearestEnemy = null;
            float nearestDistance = float.MaxValue;

            foreach (Enemy enemy in enemies)
            {
                float distance = CalculateDistance(position, enemy.Position);
                if (distance < nearestDistance)
                {
                    nearestEnemy = enemy;
                    nearestDistance = distance;
                }
            }

            return nearestEnemy;
        }

        private float CalculateDistance(Vector2f a, Vector2f b)
        {
            float dx = b.X - a.X;
            float dy = b.Y - a.Y;
            return MathF.Sqrt(dx * dx + dy * dy);
        }

        private Clock waveTimer;
        private float waveCooldown = 5f;
        private void UpdateEnemyWave()
        {
            if (waveTimer.ElapsedTime.AsSeconds() > waveCooldown)
            {
                waveTimer.Restart();
                GenerateNewWave();
                StartNextWave();
                waveCooldown = rnd.Next(5, 15);
            }
        }

       

        public override void Draw(float deltaTime)
        {
            _uiManager.Draw();
            _overworldManager?.Draw();

            //HandleAnimations();
            gameManager._waveManager.DrawEnemies(deltaTime);


            player.Draw(deltaTime);

            gameManager.Draw(deltaTime);

        }


        public override void UnloadContent()
        {
            // unload resources
        }

        //private void HandleAnimations()
        //{
        //    foreach (var sprite in SpawnedEntities.ToList())
        //    {
        //        if (sprite.IsFinished)
        //        {
        //            SpawnedEntities.Remove(sprite);
        //        }
        //        else
        //        {
        //            sprite.Update();
        //            sprite.Draw();
        //        }
        //    }
        //}

    }
}
