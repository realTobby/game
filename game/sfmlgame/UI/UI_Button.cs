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
        public int _width;
        public int _height;
        Sprite _buttonSprite;
        Texture _buttonTexture;
        string _buttonText;

        private Color normalColor = Color.White;
        private Color hoverColor = Color.Yellow;
        private Color clickedColor = Color.Green;

        private RenderTexture lastRenderTarget;

        private RenderTexture perfectSizeButtonSprite;

        private bool isButtonPressed; // Flag to check if the button is already pressed


        public UI_Button(Vector2f pos, string buttonText, int textSize, int width, int height, Color color) : base(pos)
        {
            isButtonPressed = false; // Initialize the button press state

            _width = width;
            _height = height;

            // Create a RenderTexture with the desired dimensions
            perfectSizeButtonSprite = new RenderTexture((uint)width, (uint)height);
            perfectSizeButtonSprite.Clear(color); // Optional, if you want transparency
            perfectSizeButtonSprite.Display(); // Finish rendering

            // Create a new texture from the RenderTexture
            _buttonTexture = new Texture(perfectSizeButtonSprite.Texture);
            _buttonSprite = new Sprite(_buttonTexture);
            _buttonSprite.Color = normalColor;
            _buttonSprite.Position = new Vector2f(pos.X, pos.Y);
            //_buttonSprite.Origin = new Vector2f(pos.X + width / 2f, pos.Y + height / 2f);

            _text = new UI_Text(pos, buttonText, textSize, new Vector2f(), null);
            _textSize = textSize;
            _buttonText = buttonText;

            // Center the text within the button
            FloatRect textRect = _text.textComp.GetLocalBounds();
            _text.textComp.Origin = new Vector2f(textRect.Left + textRect.Width / 2.0f, textRect.Top + textRect.Height / 2.0f);
            _text.textComp.Position = new Vector2f(pos.X + width / 2.0f, pos.Y + height / 2.0f);
        }

        public override void Draw(RenderTexture renderTexture)
        {
            lastRenderTarget = renderTexture;

            renderTexture.Draw(_buttonSprite);

            if (!string.IsNullOrEmpty(_buttonText))
                _text.Draw(renderTexture);
        }

        public override void Update(float deltaTime)
        {
            var mousePos = Mouse.GetPosition();

            if (lastRenderTarget != null)
            {
                // Adjust mouse position based on the view offset...
                FloatRect buttonBounds = _buttonSprite.GetGlobalBounds();

                if (buttonBounds.Contains(mousePos.X, mousePos.Y))
                {
                    _buttonSprite.Color = hoverColor;

                    if (Mouse.IsButtonPressed(Mouse.Button.Left))
                    {
                        if (!isButtonPressed)
                        {
                            _buttonSprite.Color = clickedColor;
                            ClickAction?.Invoke();
                            isButtonPressed = true; // Set the flag when the button is pressed
                        }
                    }
                    else
                    {
                        isButtonPressed = false; // Reset the flag when the mouse button is released
                    }
                }
                else
                {
                    _buttonSprite.Color = normalColor;
                    isButtonPressed = false; // Also reset here to handle edge cases
                }
            }
        }

        public override void SetPosition(Vector2f newPosition)
        {
            Position = newPosition;
            _buttonSprite.Position = newPosition;
            _text.textComp.Position = new Vector2f(newPosition.X + _width / 2.0f, newPosition.Y + _height / 2.0f);
        }

    }
}
