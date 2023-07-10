using game.Entities;
using game.Managers;
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
        private TextureLoader _textureLoader;
        private RenderWindow _gameWindow;

        private UIManager _uiManager;

        private OverworldManager _overworldManager;

        private Clock _uniClock;

        private Player player;

        private ViewCamera _viewCamera;

        public void Init()
        {
            _uniClock = new Clock();
            _uniClock.Restart();

            _textureLoader = new TextureLoader();

            _uiManager = new UIManager();
            _overworldManager = new OverworldManager(50);

            var mode = new VideoMode(800, 600);
            _gameWindow = new RenderWindow(mode, "Game");
            _gameWindow.Closed += (sender, e) => _gameWindow.Close();

            player = new Player(_gameWindow);

            _viewCamera = new ViewCamera(_gameWindow);

            
        }

        public void Run()
        {
            while(_gameWindow.IsOpen)
            {
                float deltaTime = _uniClock.Restart().AsSeconds();

                // Process events
                _gameWindow.DispatchEvents();

                // Clear screen
                _gameWindow.Clear(SFML.Graphics.Color.Black);

                Update(deltaTime);
                Draw(deltaTime);

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
        }

        private void Draw(float deltaTime)
        { 
            _overworldManager.Draw(_gameWindow);

            player.Draw();

            _viewCamera.Draw();

        }


    }
}
