using sfmglame.Helpers;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using sfmlgame.Managers;

namespace sfmlgame.UI
{
    public class UI_Button : UIComponent
    {
        public Action ClickAction { get; set; }

        public UI_Text _text;
        int _textSize;
        
        Sprite _buttonSprite;
        Texture _buttonTexture;
        string _buttonText;

        private Color normalColor = Color.White;
        private Color hoverColor = Color.Yellow;
        private Color clickedColor = Color.Green;

        private RenderTexture lastRenderTarget;

        private RenderTexture perfectSizeButtonSprite;

        private bool isButtonPressed; // Flag to check if the button is already pressed

        private float rainbowHue; // Declare this in your class

        public UI_Button(Vector2f pos, string buttonText, int textSize, int width, int height, Color color) : base(pos)
        {
            isButtonPressed = false; // Initialize the button press state

            Width = width;
            Height = height;

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

            // Center the text within the button
            FloatRect buttonRect = _buttonSprite.GetLocalBounds();
            _buttonSprite.Origin = new Vector2f(buttonRect.Left + buttonRect.Width / 2.0f, buttonRect.Top + buttonRect.Height / 2.0f);

            _text = new UI_Text(buttonText, textSize, pos);
            _textSize = textSize;
            _buttonText = buttonText;

            FloatRect textRect = _text.textComp.GetLocalBounds();
            _text.textComp.Origin = new Vector2f(textRect.Left + textRect.Width / 2.0f, textRect.Top + textRect.Height / 2.0f);

            _text.textComp.Position = _text.textComp.Position;

            var pastelColor = RandomExtensions.GenerateRandomPastelColor();
            rainbowHue = RandomExtensions.RGBToHue(pastelColor.R, pastelColor.G, pastelColor.B);

            
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
            FloatRect buttonBounds = _buttonSprite.GetGlobalBounds();
            if (lastRenderTarget != null)
            {
                // Adjust mouse position based on the view offset...
                

                if (buttonBounds.Contains(mousePos.X, mousePos.Y))
                {
                    _buttonSprite.Color = hoverColor;

                    if (Mouse.IsButtonPressed(Mouse.Button.Left))
                    {
                        if (!isButtonPressed)
                        {
                            _buttonSprite.Color = clickedColor;
                            ClickAction?.Invoke();
                            SoundManager.Instance.PlaySelectSound();
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

            // Change the hue value at a constant rate
            rainbowHue += deltaTime * 0.2f;  // Adjust speed as needed
            if (rainbowHue > 1f) rainbowHue -= 1f;  // Wrap hue around if it exceeds 1

            // Convert the current hue to an RGB color with full saturation and lightness
            SFML.Graphics.Color rainbowColor = RandomExtensions.HSLToRGB(rainbowHue, 1.0f, 0.5f);
            perfectSizeButtonSprite.Clear(rainbowColor); // Optional, if you want transparency
            
        }

        public override void SetPosition(Vector2f newPosition)
        {
            Position = newPosition;
            _buttonSprite.Position = newPosition;
            _text.SetPosition(new Vector2f(newPosition.X + Width / 2.0f, newPosition.Y + Height / 2.0f));
        }

    }
}
