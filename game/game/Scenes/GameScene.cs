using game.Abilities;
using game.Controllers;
using game.Controllers.game.Controllers;
using game.Entities;
using game.Entities.Enemies;
using game.Helpers;
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
using System.IO;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Color = SFML.Graphics.Color;

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

        private Sprite debugSprite = null;

        // Load your shader
        Shader CRTShader;

        // load every fucking sprite in memory
        SpriteSheetLoader texLoad = new SpriteSheetLoader("Assets/Sprites/spritesheet.png");

        public GameScene()
        {
            string shaderPath = "Assets/Shaders/CRT.frag";
            if (!File.Exists(shaderPath))
            {
                throw new FileNotFoundException($"The shader file was not found: {shaderPath}");
            }

            // If the file exists, try to load it
            CRTShader = new Shader(null, null, shaderPath);

            if (_instance == null) _instance = this;

            _inputManager = new InputManager();
            _uiManager = new UIManager();

            _overworldManager = new OverworldManager(300);
            _viewCamera = new ViewCamera();
            player = new Player(new Vector2f(25 * 16, 25 * 16));


            waveTimer = new Clock();

            gameManager = GameManager.Instance;
            soundManager = new SoundManager();

            UI_ProgressBar xpBar = new UI_ProgressBar(new Vector2f(5, 0), new UIBinding<int>(() => player.CurrentXP), new UIBinding<int>(() => player.MaxXP), new Vector2f(128, 16), _viewCamera.view, SFML.Graphics.Color.Blue);
            _uiManager.AddComponent(xpBar);

            UI_Text xpText = new UI_Text("XP: ", 8, new Vector2f(10, 0), _viewCamera.view, new UIBinding<string>(() => player.GetUIXPString()));
            xpText.SetColor(SFML.Graphics.Color.Magenta);
            _uiManager.AddComponent(xpText);

            UI_Text debugSoundChannels = new UI_Text("SoundChannels: ", 8, new Vector2f(10, 20), _viewCamera.view, new UIBinding<string>(() => soundManager.GetActiveChannels().ToString()));
            debugSoundChannels.SetColor(SFML.Graphics.Color.Red);
            _uiManager.AddComponent(debugSoundChannels);

            UI_Text debugPoolSizeEnemies = new UI_Text("Enemy Pool Size: ", 8, new Vector2f(10, 30), _viewCamera.view, new UIBinding<string>(() => EntityManager.Instance.GetEnemyPoolSize.ToString()));
            debugPoolSizeEnemies.SetColor(SFML.Graphics.Color.Red);
            _uiManager.AddComponent(debugPoolSizeEnemies);

            UI_Text debugDamageNumberPoolSize = new UI_Text("Damager Number Pool Size: ", 8, new Vector2f(10, 40), _viewCamera.view, new UIBinding<string>(() => GameScene.Instance._uiManager.GetDamagerNumberPoolSize.ToString()));
            debugDamageNumberPoolSize.SetColor(SFML.Graphics.Color.Red);
            _uiManager.AddComponent(debugDamageNumberPoolSize);

            UI_Text debugEntityCountWhole = new UI_Text("All Entities: ", 8, new Vector2f(10, 50), _viewCamera.view, new UIBinding<string>(() => EntityManager.Instance.AllEntities.Count().ToString()));
            debugEntityCountWhole.SetColor(SFML.Graphics.Color.Red);
            _uiManager.AddComponent(debugEntityCountWhole);

            UI_Text debugTest = new UI_Text("Debug Coords: ", 8, new Vector2f(10, 60), _viewCamera.view, new UIBinding<string>(() => string.Format("X:{0}/Y:{1}", debugX, debugY)));
            debugTest.SetColor(SFML.Graphics.Color.Red);
            _uiManager.AddComponent(debugTest);

            EntityManager.Instance.StartUpdatingEntities();

            //player.Abilities.Add(new OrbitalAbility(player, 10f, 10, 45, 5));


            
            debugSprite = texLoad.GetSpriteFromSheet(4,6);

            debugSprite.Position = new Vector2f(0, 25);

        }

        public Vector2i GetChunkPositionFromWorldPosition(Vector2f worldPosition)
        {
            return new Vector2i(
                (int)Math.Floor(worldPosition.X / (_overworldManager.ChunkSize * 16)),
                (int)Math.Floor(worldPosition.Y / (_overworldManager.ChunkSize * 16)));
        }

        private void GenerateNewWave()
        {
            UniversalLog.LogInfo("trying to generate new wave...");
            if(EntityManager.Instance.Enemies.Where(x => x.IsActive).Count() < 50)
            {
                UniversalLog.LogInfo("can create new wave!");
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

        public int debugX = 0;
        public int debugY = 0;

        

        public override void Update(float deltaTime)
        {
            
            

            _viewCamera.Update(player.Position, CRTShader);
            //gameManager._waveManager.Update(player, deltaTime);

            UpdateChunksBasedOnPlayerPosition();

            _inputManager.Update();

            HandlePlayerMovement(deltaTime);

            _uiManager.Update();
            player.Update(deltaTime);

            UpdateEnemyWave();

            UpdatePlayerAbilities(deltaTime);

            //particleSystem.Update(deltaTime);



            debugSprite = texLoad.GetSpriteFromSheet(debugX, debugY);

            debugX++;

            if(debugX == 79)
            {
                debugY++;
                debugX = 0;
            }

        }

        public override void Draw(RenderTexture renderTexture, float deltaTime)
        {
            

            //_overworldManager?.Draw();

            ////HandleAnimations();
            ////gameManager._waveManager.DrawEnemies(deltaTime);

            //player.Draw(deltaTime);

            //gameManager.Draw(deltaTime);

            ////particleSystem.Draw(_viewCamera.view);

            //_uiManager.Draw(_viewCamera.view);
            ////Game.Instance.GetRenderWindow().Display();

            //Game.Instance.GetRenderWindow().Draw(debugSprite);

            renderTexture.Clear(Color.Black);
            renderTexture.SetView(_viewCamera.view);
            _overworldManager?.Draw(renderTexture, _viewCamera);
            player.Draw(renderTexture, deltaTime);
            gameManager.Draw(renderTexture, deltaTime);
            _uiManager.Draw(renderTexture);
            renderTexture.Display(); // This is necessary to finalize the drawing on the renderTexture

            
            Sprite sceneSprite = new Sprite(renderTexture.Texture);
            //sceneSprite.Origin = _viewCamera.view.Center;
            //sceneSprite.Position = new Vector2f(sceneSprite.Position.X+renderTexture.Size.X/2, sceneSprite.Position.Y);
            Game.Instance.GetRenderWindow().Clear();
            //sceneSprite.Position = new Vector2f(-500, 0);


            Game.Instance.GetRenderWindow().Draw(sceneSprite, new RenderStates(CRTShader));

            //_uiManager.DrawDirectlyToWindow();
        }

        private void UpdatePlayerAbilities(float deltaTime)
        {
            if (GameManager.Instance.IsGamePaused == false)
            {
                foreach (Ability ability in player.Abilities)
                {
                    ability.Update();

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

        private void UpdateChunksBasedOnPlayerPosition()
        {
            Vector2i currentChunk = GetChunkPositionFromWorldPosition(player.Position);

            // Determine the range of chunks to load around the player
            // This example loads the immediate surrounding chunks in all directions
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    Vector2i chunkPos = new Vector2i(currentChunk.X + x, currentChunk.Y + y);
                    _overworldManager.GetOrCreateChunk(chunkPos);
                }
            }

            // Optionally, implement logic here to unload chunks that are far away from the player
        }

    }
}
