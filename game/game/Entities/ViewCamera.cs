
using game.Controllers;
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

        public View view;

        float cameraSpeed = 150f;

        public Vector2f TargetPosition;

        public bool IsFlyToggled = false;

        public ViewCamera()
        {

            view = new View(new FloatRect(0, 0, game.Controllers.Game.Instance.GetRenderWindow().Size.X/2, game.Controllers.Game.Instance.GetRenderWindow().Size.Y/2));
        }

        public void Update(Vector2f targetPos)
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

            Game.Instance.GetRenderWindow().SetView(view);
        }

        private void DebugFreeCam()
        {
            //float deltaTime = Game.Instance.GetDeltaTime();

            //Vector2f desiredPosition = view.Center;

            //if (Keyboard.IsKeyPressed(Keyboard.Key.Left))
            //    //view.Move(new Vector2f(-cameraSpeed * deltaTime.AsSeconds(), 0));
            //    desiredPosition.X = -cameraSpeed * deltaTime;
            //if (Keyboard.IsKeyPressed(Keyboard.Key.Right))
            //    //view.Move(new Vector2f(cameraSpeed * deltaTime.AsSeconds(), 0));
            //    desiredPosition.X = cameraSpeed * deltaTime;
            //if (Keyboard.IsKeyPressed(Keyboard.Key.Up))
            //    //view.Move(new Vector2f(0, -cameraSpeed * deltaTime.AsSeconds()));
            //    desiredPosition.Y = -cameraSpeed * deltaTime;
            //if (Keyboard.IsKeyPressed(Keyboard.Key.Down))
            //    //view.Move(new Vector2f(0, cameraSpeed * deltaTime.AsSeconds()));
            //    desiredPosition.Y = cameraSpeed * deltaTime;

            //Vector2f roundedPosition = new Vector2f((float)Math.Round(desiredPosition.X), (float)Math.Round(desiredPosition.Y));
            //view.Center += roundedPosition;

        }

        private void TrackPlayer()
        {
            Vector2f roundedPosition = new Vector2f((float)Math.Round(TargetPosition.X), (float)Math.Round(TargetPosition.Y));

            roundedPosition.X += 16;
            roundedPosition.Y += 32;

            view.Center = roundedPosition;
        }


    }
}
