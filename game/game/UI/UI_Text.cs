using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game.UI
{
    public class UI_Text : UIComponent
    {
        internal Text textComp;
        private UIBinding<string> textBinding;
        private string defaultText = string.Empty;

        public UI_Text(string text, int size, Vector2f pos, View view, UIBinding<string> textBinding) : base(pos, view)
        {
            textComp = new Text(text, new Font("Assets/Fonts/m6x11.ttf"), (uint)size);
            this.textBinding = textBinding;
            defaultText = text;

            textComp.OutlineColor = Color.White;
            textComp.OutlineThickness = 2;
        }

        public UI_Text(string text, int size, Vector2f pos, View view) : base(pos, view)
        {
            textComp = new Text(text, new Font("Assets/Fonts/m6x11.ttf"), (uint)size);
            this.textBinding = null;
            defaultText = text;

            textComp.OutlineColor = Color.White;
            textComp.OutlineThickness = 2;
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

        public override void Draw()
        {
            base.StartUIDraw();

            // Update the position relative to the camera view
            Vector2f offsetPosition = Position + cameraView.Center - cameraView.Size / 2f;
            textComp.Position = offsetPosition;

            // Update the text value
            if (textBinding != null) textComp.DisplayedString = defaultText + textBinding.Value;

            // Draw the text
            Game.Instance.GetRenderWindow().Draw(textComp);

            base.EndUIDraw();
        }

        public override void Update()
        {

        }
    }
}
