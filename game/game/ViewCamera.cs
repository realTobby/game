using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game
{
    public class ViewCamera
    {
        private RenderWindow _window;

        public View view;

        float cameraSpeed = 1.25f;

        public Vector2f TargetPosition;

        public bool IsFlyToggled = false;

        public ViewCamera(RenderWindow window)
        {
            _window = window;
            view = new View(new FloatRect(0, 0, _window.Size.X, _window.Size.Y));
        }

        public void Update(Time deltaTime, Vector2f targetPos)
        {
            TargetPosition = targetPos;

            if(DebugHelper.Instance.IsDebugMode && IsFlyToggled)
            {
                DebugFlyMode(deltaTime);
            }
            else
            {
                TrackPlayer();
            }
            _window.SetView(view);
        }

        private void DebugFlyMode(Time deltaTime)
        {
            if (Keyboard.IsKeyPressed(Keyboard.Key.Left))
                view.Move(new SFML.System.Vector2f(-cameraSpeed * deltaTime.AsSeconds(), 0));
            if (Keyboard.IsKeyPressed(Keyboard.Key.Right))
                view.Move(new SFML.System.Vector2f(cameraSpeed * deltaTime.AsSeconds(), 0));
            if (Keyboard.IsKeyPressed(Keyboard.Key.Up))
                view.Move(new SFML.System.Vector2f(0, -cameraSpeed * deltaTime.AsSeconds()));
            if (Keyboard.IsKeyPressed(Keyboard.Key.Down))
                view.Move(new SFML.System.Vector2f(0, cameraSpeed * deltaTime.AsSeconds()));
        }

        private void TrackPlayer()
        {
            Vector2f roundedPosition = new Vector2f((float)Math.Round(TargetPosition.X), (float)Math.Round(TargetPosition.Y));
            view.Center = roundedPosition;
        }
    }
}
