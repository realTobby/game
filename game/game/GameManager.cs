using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using SFML.Graphics;
using SFML.Window;
using System.Text;
using System.Threading.Tasks;
using SFML.System;
using System.Numerics;

namespace game
{
    public class GameManager
    {
        private static GameManager _instance = null;
        public static GameManager Instance => _instance;

        public Random RANDOM = new Random();

        private IsometricGrid _isometricGrid = null;
        public ViewCamera ViewCamera = null;

        private DebugHelper _debugHelper = null;

        public Player Player = null;

        private Clock _drawClock = new Clock();
        private Clock _updateClock = new Clock();

        private RenderWindow gameWindow;

        public GameManager()
        {
            if (_instance == null) _instance = this;

            _debugHelper = new DebugHelper();

        }

        public void Run()
        {

            // Create a new window
            var mode = new VideoMode(800, 600);
            gameWindow = new RenderWindow(mode, "Game");

            _isometricGrid = new IsometricGrid(gameWindow);
            ViewCamera = new ViewCamera(gameWindow);

            Player = new Player(gameWindow);

           // Player.Position = _isometricGrid.GetCenterIsoPos();

            gameWindow.SetFramerateLimit(60);

            gameWindow.Resized += AdjustViewPort;

            gameWindow.Closed += (sender, e) => gameWindow.Close();

            // Start the game loop
            while (gameWindow.IsOpen)
            {

                // Process events
                gameWindow.DispatchEvents();

                // Clear screen
                gameWindow.Clear(SFML.Graphics.Color.Black);

                

                Update();
                Draw();

                // Update the window
                gameWindow.Display();
                _drawClock.Restart();
            }


        }

        private void AdjustViewPort(object? sender, SizeEventArgs e)
        {
            float aspectRatio = (float)gameWindow.Size.X / gameWindow.Size.Y;
            float viewHeight = ViewCamera.view.Size.Y; // Keep a fixed view height
            float viewWidth = viewHeight * aspectRatio; // Calculate the new view width based on the aspect ratio

            // Update the view size
            ViewCamera.view.Size = new Vector2f(viewWidth, viewHeight);

            // Update the camera position to keep the player in center
            ViewCamera.Update(_updateClock.ElapsedTime, Player.Position);

            gameWindow.SetView(ViewCamera.view);

        }

        private void Update()
        {
            Player.Update(_updateClock.ElapsedTime);
            ViewCamera.Update(_updateClock.ElapsedTime, Player.Position);
            //_isometricGrid.UpdateVisibleTiles(ViewCamera.view);

            if (Keyboard.IsKeyPressed(Keyboard.Key.R))
            {
                _isometricGrid.ResetGrid();
            }

            _updateClock.Restart();
        }

        private void Draw()
        {
            _isometricGrid.Draw();
            Player.Draw();
            _drawClock.Restart();

        }
    }
}
