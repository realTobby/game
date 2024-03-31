using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace sfmlgame.UI
{
    public class UI_Button : UIComponent
    {
        public Action ClickAction { get; set; }

        public UI_Text _text;
        int _textSize;
        int _width;
        int _height;
        Sprite _buttonSprite;
        string _buttonText;

        private Color normalColor = Color.White;
        private Color hoverColor = Color.Yellow;
        private Color clickedColor = Color.Green;

        


        public UI_Button(Vector2f pos, string buttonText, int textSize, int width, int height, Sprite buttonSprite) : base(pos)
        {
            _text = new UI_Text(buttonText, 16, pos);
            _textSize = textSize;
            _width = width;
            _height = height;
            _buttonText = buttonText;
            _buttonSprite = buttonSprite;
            _buttonSprite.Position = pos;
            _buttonSprite.Color = normalColor;
        }

        public override void Draw(RenderTexture renderTexture)
        {

            renderTexture.Draw(_buttonSprite);

            if (!string.IsNullOrEmpty(_buttonText)) _text.Draw(renderTexture);

        }

        public override void Update(float deltaTime)
        {
            //var mousePos = Mouse.GetPosition();

            //// Adjust mouse position based on the view offset
            //mousePos.X += (int)Game.Instance.GetRenderWindow().GetView().Center.X - (int)Game.Instance.GetRenderWindow().Size.X / 2;
            //mousePos.Y += (int)Game.Instance.GetRenderWindow().GetView().Center.Y - (int)Game.Instance.GetRenderWindow().Size.Y / 2;

            //FloatRect buttonBounds = _buttonSprite.GetGlobalBounds();

            //// Check if mouse is over the button
            //if (buttonBounds.Contains(mousePos.X, mousePos.Y))
            //{
            //    // Mouse is over the button
            //    _buttonSprite.Color = hoverColor;

            //    // Check if button is clicked
            //    if (Mouse.IsButtonPressed(Mouse.Button.Left))
            //    {
            //        _buttonSprite.Color = clickedColor;
            //        // Button click action here
            //        ClickAction?.Invoke();
            //    }
            //}
            //else
            //{
            //    _buttonSprite.Color = normalColor;
            //}

            //_text.Update();
        }

    }
}
