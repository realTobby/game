using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game.UI
{
    public abstract class UIComponent
    {
        internal View cameraView;

        private Vector2f _position;

        public bool IsActive = false;

        public Vector2f Position
        {
            get { return _position; }
            set
            {
                _position = value;
            }
        }

        public UIComponent(Vector2f position)
        {
            _position = position;
            IsActive = true;
        }

        public UIComponent(Vector2f position, View view)
        {
            _position = position;
            cameraView = view;
            IsActive = true;
        }

        public abstract void Update();

        public abstract void Draw();

        View originalViewTMP;

        public void StartUIDraw()
        {
            RenderWindow window = Game.Instance.GetRenderWindow();

            // Store the original view
            originalViewTMP = window.GetView();

            // Set the view of the render target to the camera's view
            window.SetView(cameraView);
        }

        public void EndUIDraw()
        {
            // Restore the original view
            Game.Instance.GetRenderWindow().SetView(originalViewTMP);
        }

    }
}
