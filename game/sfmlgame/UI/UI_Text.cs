
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

        public UI_Text(string text, int size, Vector2f pos, UIBinding<string> textBinding) : base(pos)
        {
            textComp = new Text(text, GameAssets.Instance.pixelFont1, (uint)size);
            this.textBinding = textBinding;
            defaultText = text;

            textComp.Color = Color.Black;

            textComp.OutlineColor = Color.White;
            textComp.OutlineThickness = 3f;

            textComp.Position = pos;
        }

        public UI_Text(string text, int size, Vector2f pos) : base(pos)
        {
            textComp = new Text(text, GameAssets.Instance.pixelFont1, (uint)size);
            this.textBinding = null;
            defaultText = text;

            textComp.Color = Color.Black;

            textComp.OutlineColor = Color.White;
            textComp.OutlineThickness = 1.25f;

            textComp.Position = pos;
        }

        public void SetBold(bool isBold)
        {
            textComp.Style = Text.Styles.Regular;

            if (isBold)
                textComp.Style = Text.Styles.Bold;
        }

        public void SetSize(uint size)
        {
            textComp.CharacterSize = size;
        }

        public void SetColor(Color c)
        {
            textComp.Color = c;
            textComp.FillColor = c;
            textComp.OutlineColor = new Color(textComp.OutlineColor.R, textComp.OutlineColor.G, textComp.OutlineColor.B, c.A);
        }

        public override void Draw(RenderTexture renderTexture)
        {
            string displayText = defaultText;

            if (textBinding != null)
            {
                displayText = displayText + textBinding.Value;
            }

            textComp.DisplayedString = displayText;

            // Draw the text
            renderTexture.Draw(textComp);
        }

        public override void Update(float deltaTime)
        {

        }
    }
}
