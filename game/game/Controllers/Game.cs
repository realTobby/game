using game.Entities;
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

namespace game.Controllers
{
    public class Game
    {
        private static Game _instance;
        public static Game Instance => _instance;

        private TextureLoader _textureLoader;
        private RenderWindow _gameWindow;

        private UIManager _uiManager;

        private OverworldManager _overworldManager;

        private Clock _uniClock;

        private Player player;

        private ViewCamera _viewCamera;

        private AnimatedSprite testThunder;

        public Game()
        {
            if (_instance == null) _instance = this;
        }

        public void Init()
        {
            _uniClock = new Clock();
            _uniClock.Restart();

            _textureLoader = new TextureLoader();

            _uiManager = new UIManager();
            _overworldManager = new OverworldManager(100);

            var mode = new VideoMode(800, 600);
            _gameWindow = new RenderWindow(mode, "Game");
            _gameWindow.Closed += (sender, e) => _gameWindow.Close();

            player = new Player(_gameWindow);

            _viewCamera = new ViewCamera(_gameWindow);

            //testThunder = new AnimatedSprite(TextureLoader.Instance.GetTexture("thunderStrike", "VFX"), 1, 13, Time.FromSeconds(0.1f));
            //testThunder.IsSingleShotAnimation = true;
            //animatedSprites.Add(testThunder);

            follower = new Enemy(player.Position, player.Position, .5f, Time.FromSeconds(1), new AnimatedSprite(TextureLoader.Instance.GetTexture("thunderStrike", "VFX"), 1, 13, Time.FromSeconds(0.1f)));

        }

        public Enemy follower;

        List<Sprite> testDebug = new List<Sprite>();

        public void Run()
        {

            while (_gameWindow.IsOpen)
            {
                float deltaTime = _uniClock.Restart().AsSeconds();

                // Process events
                _gameWindow.DispatchEvents();

                // Clear screen
                _gameWindow.Clear(SFML.Graphics.Color.Black);

                Update(deltaTime);
                Draw(deltaTime);

                HandleAnimations();

                // Update the window
                _gameWindow.Display();
                //_uniClock.Restart();
            }
        }

        private void Update(float deltaTime)
        {
            player.Update(deltaTime);

            //_overworldManager.OnPlayerMove(player.Position);

            _viewCamera.Update(deltaTime, player.Position);

            follower.Update(player.Position);

        }

        public List<AnimatedSprite> animatedSprites = new List<AnimatedSprite>();

        private void HandleAnimations()
        {
            foreach (var sprite in animatedSprites.ToList())
            {
                if (sprite.IsFinished)
                {
                    animatedSprites.Remove(sprite);
                }
                else
                {
                    sprite.Update();
                    sprite.Draw(_gameWindow);
                }
            }
        }

        private void Draw(float deltaTime)
        { 
            _overworldManager.Draw(_gameWindow);

            player.Draw();

            _viewCamera.Draw();

            //testThunder.DebugDrawAllFrames(_gameWindow);


        }





    }
}
