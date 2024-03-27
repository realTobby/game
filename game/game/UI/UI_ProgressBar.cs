using game.Entities;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game.UI
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

        public UI_ProgressBar(Vector2f position, UIBinding<int> currentValue, UIBinding<int> maxValue, Vector2f size, View view, Color color) : base(position, view)
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
            if(FollowUI) base.StartUIDraw();

            if(_currentValue != _maxValue)
            {
                // Update the position relative to the camera view
                if (base.cameraView != null)
                {
                    Vector2f offsetPosition = Position + cameraView.Center - cameraView.Size / 2f;

                    backgroundShape.Position = offsetPosition;
                    fillShape.Position = offsetPosition;
                }

                if (FollowUI == false)
                {
                    backgroundShape.Position = new Vector2f(parent.Position.X, parent.Position.Y - 20);
                    fillShape.Position = new Vector2f(parent.Position.X, parent.Position.Y - 20);
                }

                Vector2f size = fillShape.Size;
                size.X = backgroundShape.Size.X * ((float)_currentValue / (float)_maxValue);
                fillShape.Size = new Vector2f(size.X, size.Y);

                renderTexture.Draw(backgroundShape);
                renderTexture.Draw(fillShape);
            }

            

            if(FollowUI) base.EndUIDraw();
            
        }

        public override void Update()
        {
            // calculate progress bar size values

            // MaxValue
            // CurrentValue
            // Size
            


        }
    }
}
