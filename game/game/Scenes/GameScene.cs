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
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace game.Scenes
{
    public class GameScene : Scene
    {
        [AllowNull]
        private static GameScene _instance;
        public static GameScene Instance => _instance;

        private Random rnd = new Random();

        public UIManager _uiManager;
        private OverworldManager _overworldManager;
        public ViewCamera _viewCamera;

        private InputManager _inputManager;

        public Player player;

        private GameManager gameManager;

        private SoundManager soundManager;

        public GameScene()
        {
            if (_instance == null) _instance = this;

            _inputManager = new InputManager();
            _uiManager = new UIManager();
            _overworldManager = new OverworldManager(100);
            _viewCamera = new ViewCamera();
            player = new Player(new Vector2f(0,0));


            waveTimer = new Clock();

            gameManager = GameManager.Instance;
            soundManager = new SoundManager();

            UI_ProgressBar xpBar = new UI_ProgressBar(new Vector2f(5, 0), new UIBinding<int>(() => player.CurrentXP), new UIBinding<int>(() => player.MaxXP), new Vector2f(128, 16), _viewCamera.view, SFML.Graphics.Color.Blue);
            _uiManager.AddComponent(xpBar);

            UI_Text xpText = new UI_Text("XP: ", 8, new Vector2f(10, 0), _viewCamera.view, new UIBinding<string>(() => player.GetUIXPString()));
            _uiManager.AddComponent(xpText);

            UI_Text debugSoundChannels = new UI_Text("SoundChannels: ", 8, new Vector2f(10,20), _viewCamera.view, new UIBinding<string>(() => soundManager.GetActiveChannels().ToString()));
            _uiManager.AddComponent(debugSoundChannels);

            EntityManager.Instance.StartUpdatingEntities();

        }

        private void GenerateNewWave()
        {
            if(GameManager.Instance.GetEntities(new Type[] { typeof(Enemy) }).Cast<Enemy>().ToList().Count < 125)
            {
                EnemyWave wave = new EnemyWave(1f, 25f);

                int num = rnd.Next(5, 10);
                Vector2f point = player.Position;
                float radius = 300f;

                for (int i = 0; i < num; i++)
                {
                    var radians = 2 * MathF.PI / num * i;
                    var vertical = MathF.Sin(radians);
                    var horizontal = MathF.Cos(radians);

                    var spawnDir = new Vector2f(horizontal, vertical);
                    var spawnPos = point + spawnDir * radius;
                    wave.AddSpawnPosition(new Vector2f(spawnPos.X, spawnPos.Y));
                }
                gameManager._waveManager.AddWave(wave);
            }
        }

        private void StartNextWave()
        {
            gameManager._waveManager.StartWave();
        }

        public override void LoadContent()
        {
            // load resources if needed
        }

        private void HandlePlayerMovement(float deltaTime)
        {
            if (GameManager.Instance.IsGamePaused == false)
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

                if (_inputManager.IsKeyPressed(Keyboard.Key.Space))
                {
                    GameManager.Instance.MagnetizeGems();
                }
            }
        }

        public override void Update(float deltaTime)
        {
            _inputManager.Update();

            HandlePlayerMovement(deltaTime);

            _uiManager.Update();
            player.Update(deltaTime);
            _viewCamera.Update(player.Position);
            //gameManager._waveManager.Update(player, deltaTime);

            UpdateEnemyWave();

            UpdatePlayerAbilities(deltaTime);

            

        }

        public override void Draw(float deltaTime)
        {
            _overworldManager?.Draw();

            //HandleAnimations();
            gameManager._waveManager.DrawEnemies(deltaTime);

            player.Draw(deltaTime);

            gameManager.Draw(deltaTime);


            _uiManager.Draw(_viewCamera.view);
            //Game.Instance.GetRenderWindow().Display();
        }

        private void UpdatePlayerAbilities(float deltaTime)
        {
            if (GameManager.Instance.IsGamePaused == false)
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
        }

        public Enemy FindNearestEnemy(Vector2f position, List<Enemy> enemies)
        {
            Enemy nearestEnemy = null;
            float nearestDistance = float.MaxValue;
            int lowestHealth = int.MaxValue;

            foreach (Enemy enemy in enemies)
            {
                if (enemy.HP <= lowestHealth && enemy.HP > 0) // Check if enemy has lower health and is not defeated
                {
                    float distance = CalculateDistance(position, enemy.Position);
                    if (distance < nearestDistance)
                    {
                        nearestEnemy = enemy;
                        nearestDistance = distance;
                        lowestHealth = enemy.HP;
                    }
                }
            }

            return nearestEnemy;
        }

        public Enemy FindNearestEnemy(Vector2f position, List<Enemy> enemies, List<Enemy> avoidEnemy)
        {
            Enemy nearestEnemy = null;
            float nearestDistance = float.MaxValue;

            // create a foreach-loop that iterates through the enemies list but skips the avoidEnemy

            foreach (Enemy enemy in enemies.ToList())
            {
                if (avoidEnemy.Contains(enemy)) continue;

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

        public override void UnloadContent()
        {
            // unload resources
        }


    }
}
