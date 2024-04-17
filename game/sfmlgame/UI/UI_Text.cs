using SFML.Graphics;
using SFML.System;
using sfmlgame.Assets;

namespace sfmlgame.UI
{
    public class UI_Text : UIComponent
    {
        internal Text textComp;
        private UIBinding<string> textBinding;
        private string defaultText = string.Empty;

        public float Opacity { get; set; } = 255;  // Full opacity by default
        public float Scale { get; set; } = 1.0f;   // Normal scale by default

        // Constructor with UIBinding
        public UI_Text(string text, int size, Vector2f pos, UIBinding<string> textBinding)
            : base(pos)
        {
            this.textBinding = textBinding;
            InitializeText(text, size);
            textComp.Position = pos;
        }

        // Basic constructor
        public UI_Text(string text, int size, Vector2f pos) : base(pos)
        {
            InitializeText(text, size);
            textComp.Position = pos;
        }

        private void InitializeText(string text, int size)
        {
            defaultText = text;
            textComp = new Text(text, GameAssets.Instance.pixelFont1, (uint)size);
            textComp.Color = Color.Black;
            textComp.OutlineColor = Color.White;
            textComp.OutlineThickness = 1.25f;

            UpdateDimensions(); // Initialize dimensions based on text
        }

        public override void SetPosition(Vector2f pos)
        {
            base.SetPosition(pos);
            textComp.Position = pos;
            UpdateDimensions();
        }

        private void UpdateDimensions()
        {
            FloatRect textBounds = textComp.GetLocalBounds();
            Width = Convert.ToInt32(textBounds.Width + textComp.OutlineThickness * 2); // Consider outline thickness
            Height = Convert.ToInt32(textBounds.Height + textComp.OutlineThickness * 2);
        }

        public void SetText(string newText)
        {
            textComp.DisplayedString = newText;
            defaultText = newText; // Update the default text as well
            UpdateDimensions(); // Update dimensions as the text changes
        }

        public void SetSize(uint size)
        {
            textComp.CharacterSize = size;
            UpdateDimensions(); // Update dimensions as the size changes
        }

        public void SetBold(bool isBold)
        {
            textComp.Style = isBold ? Text.Styles.Bold : Text.Styles.Regular;
        }

        public void SetColor(Color color)
        {
            textComp.FillColor = color;
        }

        public void SetColor(Color color, Color outlineColor, float thickness)
        {
            textComp.FillColor = color;
            SetOutline(outlineColor, thickness);
        }

        public void SetOutline(Color color, float thickness)
        {
            textComp.OutlineColor = color;
            textComp.OutlineThickness = thickness;
        }

        public override void Draw(RenderTexture renderTexture)
        {
            string displayText = defaultText;

            if (textBinding != null && textBinding.Value != null)
            {
                displayText += textBinding.Value;
            }

            textComp.DisplayedString = displayText;
            UpdateDimensions(); // Ensure dimensions are correct before drawing
            textComp.Color = new Color(textComp.Color.R, textComp.Color.G, textComp.Color.B, (byte)Opacity);
            textComp.Scale = new Vector2f(Scale, Scale);
            renderTexture.Draw(textComp);
        }

        public override void Update(float deltaTime)
        {
            // Handle updates to properties if needed
        }
    }
}
