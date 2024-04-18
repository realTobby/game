using SFML.Graphics;
using SFML.System;

namespace sfmlgame.UI
{
    public abstract class UIComponent
    {
        public bool Hide = false;

        private Vector2f _position;

        public bool IsActive = false;

        public int Width;
        public int Height;

        public Vector2f Position
        {
            get { return _position; }
            set
            {
                _position = value;
            }
        }

        public virtual void SetPosition(Vector2f pos)
        {
            Position = pos;
        }



        public UIComponent(Vector2f position)
        {
            _position = position;
            IsActive = true;
        }

        

        public abstract void Update(float deltaTime);

        public abstract void Draw(RenderTexture renderTexture);


        public float GetCenterX => Position.X + Width / 2;
        public float GetCenterY => Position.Y + Height / 2;

    }
}
