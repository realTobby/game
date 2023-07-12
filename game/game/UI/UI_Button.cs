using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game.UI
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

        public UI_Button(Vector2f pos, string buttonText, int textSize, int width, int height, Sprite buttonSprite, View cameraView) : base(pos, cameraView)
        {
            _text = new UI_Text(buttonText, 16, pos, cameraView);
            _textSize = textSize;
            _width = width;
            _height = height;
            _buttonText = buttonText;
            _buttonSprite = buttonSprite;
            _buttonSprite.Position = pos;
            _buttonSprite.Color = normalColor;
        }

        public override void Draw()
        {
            base.StartUIDraw();

            // Update the position relative to the camera view
            Vector2f offsetPosition = Position + cameraView.Center - cameraView.Size / 2f;

            _buttonSprite.Position = offsetPosition;

            Game.Instance.GetRenderWindow().Draw(_buttonSprite);

            if (!string.IsNullOrEmpty(_buttonText)) _text.Draw();

            base.EndUIDraw();

        }

        public override void Update()
        {
            var mousePos = Mouse.GetPosition(Game.Instance.GetRenderWindow());
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

            _text.Update();
        }
    }
}
