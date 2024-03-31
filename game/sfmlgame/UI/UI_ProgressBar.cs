
using SFML.Graphics;
using SFML.System;
using sfmlgame.Entities;


namespace sfmlgame.UI
{
    public class UI_ProgressBar : UIComponent
    {
        UIBinding<int> CurrentValue;
        UIBinding<int> MaxValue;

        public int MinValue;
        private int _currentValue => CurrentValue.Value;
        private int _maxValue => MaxValue.Value;

        private RectangleShape backgroundShape;
        private RectangleShape fillShape;

        private Vector2f size;
        
        private Vector2f pos;

        public Color color;

        public bool FollowUI;

        public Entity parent;

        public UI_ProgressBar(Vector2f position, UIBinding<int> currentValue, UIBinding<int> maxValue, Vector2f size, Color color) : base(position)
        {
            FollowUI = true;

            CurrentValue = currentValue;
            MaxValue = maxValue;

            this.size = size;
            pos = position;

            this.color = color;

            backgroundShape = new RectangleShape(size);
            backgroundShape.FillColor = Color.Black;
            backgroundShape.Position = position;

            fillShape = new RectangleShape(size);
            fillShape.FillColor = color;
            backgroundShape.Position = position;
        }

        public UI_ProgressBar(Vector2f position, UIBinding<int> currentValue, UIBinding<int> maxValue, Vector2f size, Color color, Entity parent) : base(position)
        {
            this.parent = parent;
            FollowUI = false;

            CurrentValue = currentValue;
            MaxValue = maxValue;

            this.size = size;
            pos = position;

            this.color = color;

            backgroundShape = new RectangleShape(size);
            backgroundShape.FillColor = Color.Black;
            backgroundShape.Position = position;

            fillShape = new RectangleShape(size);
            fillShape.FillColor = color;
            fillShape.Position = position;
        }

        public override void Draw(RenderTexture renderTexture)
        {
            if(_currentValue != _maxValue)
            {

                backgroundShape.Position = new Vector2f(parent.GetPosition().X, parent.GetPosition().Y - 20);
                fillShape.Position = new Vector2f(parent.GetPosition().X, parent.GetPosition().Y - 20);

                Vector2f size = fillShape.Size;
                size.X = backgroundShape.Size.X * ((float)_currentValue / (float)_maxValue);
                fillShape.Size = new Vector2f(size.X, size.Y);

                renderTexture.Draw(backgroundShape);
                renderTexture.Draw(fillShape);
            }
        }

        public override void Update(float deltaTime)
        {
            // calculate progress bar size values

            // MaxValue
            // CurrentValue
            // Size
            


        }
    }
}
