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
            // Keep the same alpha for the outline but change other colors to match fill color
            textComp.OutlineColor = new Color(color.R, color.G, color.B, textComp.OutlineColor.A);
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
            renderTexture.Draw(textComp);
        }

        public override void Update(float deltaTime)
        {
            // Handle updates to properties if needed
        }
    }
}
