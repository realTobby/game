
using game.Managers;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game.Entities
{
    public class ViewCamera
    {
        private RenderWindow _window;

        public View view;

        float cameraSpeed = 150f;

        public Vector2f TargetPosition;

        public bool IsFlyToggled = false;

        public ViewCamera(RenderWindow window)
        {
            _window = window;
            view = new View(new FloatRect(0, 0, _window.Size.X/2, _window.Size.Y/2));
        }

        public void Update(float deltaTime, Vector2f targetPos)
        {
            TargetPosition = targetPos;

            //if (DebugHelper.Instance.IsDebugMode && IsFlyToggled)
            //{
            //    DebugFreeCam(deltaTime);
            //}
            //else
            //{
            //    TrackPlayer();
            //}
            TrackPlayer();

            //GameManager.Instance.OnRedrawUI?.Invoke();

            _window.SetView(view);
        }

        private void DebugFreeCam(Time deltaTime)
        {
            Vector2f desiredPosition = view.Center;

            if (Keyboard.IsKeyPressed(Keyboard.Key.Left))
                //view.Move(new Vector2f(-cameraSpeed * deltaTime.AsSeconds(), 0));
                desiredPosition.X = -cameraSpeed * deltaTime.AsSeconds();
            if (Keyboard.IsKeyPressed(Keyboard.Key.Right))
                //view.Move(new Vector2f(cameraSpeed * deltaTime.AsSeconds(), 0));
                desiredPosition.X = cameraSpeed * deltaTime.AsSeconds();
            if (Keyboard.IsKeyPressed(Keyboard.Key.Up))
                //view.Move(new Vector2f(0, -cameraSpeed * deltaTime.AsSeconds()));
                desiredPosition.Y = -cameraSpeed * deltaTime.AsSeconds();
            if (Keyboard.IsKeyPressed(Keyboard.Key.Down))
                //view.Move(new Vector2f(0, cameraSpeed * deltaTime.AsSeconds()));
                desiredPosition.Y = cameraSpeed * deltaTime.AsSeconds();

            Vector2f roundedPosition = new Vector2f((float)Math.Round(desiredPosition.X), (float)Math.Round(desiredPosition.Y));
            view.Center += roundedPosition;

        }

        private void TrackPlayer()
        {
            Vector2f roundedPosition = new Vector2f((float)Math.Round(TargetPosition.X), (float)Math.Round(TargetPosition.Y));

            roundedPosition.X += 16;
            roundedPosition.Y += 32;

            view.Center = roundedPosition;
        }

        public void Draw()
        {
            // draw ui



        }

    }
}
