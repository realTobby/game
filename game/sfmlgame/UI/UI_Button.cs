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


        private RenderTexture lastRenderTarget;

        public UI_Button(Vector2f pos, string buttonText, int textSize, int width, int height, Sprite buttonSprite) : base(pos)
        {
            _buttonSprite = buttonSprite;
            _buttonSprite.Position = pos;
            _buttonSprite.Color = normalColor;
            _buttonSprite.Origin = new Vector2f(width / 2, height / 2);

            _text = new sfmlgame.UI.UI_Text(_buttonSprite.Origin, buttonText, textSize, new Vector2f(pos.X + _buttonSprite.Texture.Size.X/2, pos.Y+ _buttonSprite.Texture.Size.Y/ 2), null);
            _textSize = textSize;
            _width = width;
            _height = height;
            _buttonText = buttonText;
            
        }

        public override void Draw(RenderTexture renderTexture)
        {
            lastRenderTarget = renderTexture;

            renderTexture.Draw(_buttonSprite);

            if (!string.IsNullOrEmpty(_buttonText)) _text.Draw(renderTexture);

        }

        public override void Update(float deltaTime)
        {
            var mousePos = Mouse.GetPosition();

            if(lastRenderTarget != null)
            {
                // Adjust mouse position based on the view offset
                mousePos.X += (int)lastRenderTarget.GetView().Center.X - (int)lastRenderTarget.GetView().Size.X / 2;
                mousePos.Y += (int)lastRenderTarget.GetView().Center.Y - (int)lastRenderTarget.GetView().Size.Y / 2;

                FloatRect buttonBounds = _buttonSprite.GetGlobalBounds();

                // Check if mouse is over the button
                if (buttonBounds.Contains(mousePos.X, mousePos.Y))
                {
                    // Mouse is over the button
                    _buttonSprite.Color = hoverColor;

                    // Check if button is clicked
                    if (Mouse.IsButtonPressed(Mouse.Button.Left))
                    {
                        _buttonSprite.Color = clickedColor;
                        // Button click action here
                        ClickAction?.Invoke();
                    }
                }
                else
                {
                    _buttonSprite.Color = normalColor;
                }
            }

            //_text.SetPosition(new Vector2f(pos.X + _buttonSprite.Texture.Size.X / 2, pos.Y + _buttonSprite.Texture.Size.Y / 2));

            _text.Update(deltaTime);
        }

    }
}
