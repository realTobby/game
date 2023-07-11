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
        float cameraSpeed = 150f;

        public View view;
        public Vector2f TargetPosition;
        public bool IsFlyToggled = false;

        public ViewCamera()
        {
            view = new View(new FloatRect(0, 0, Game.Instance.GetRenderWindow().Size.X/2, Game.Instance.GetRenderWindow().Size.Y/2));
        }

        public void Update(Vector2f targetPos)
        {
            TargetPosition = targetPos;
            TrackPlayer();

            //GameManager.Instance.OnRedrawUI?.Invoke();

            Game.Instance.GetRenderWindow().SetView(view);
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
