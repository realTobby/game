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
        private Text textComp;
        private UIBinding<string> textBinding;
        private string defaultText = string.Empty;

        public UI_Text(string text, int size, Vector2f pos, View view, UIBinding<string> textBinding) : base(pos, view)
        {
            textComp = new Text(text, new Font("Assets/Fonts/m6x11.ttf"), (uint)size);
            this.textBinding = textBinding;
            defaultText = text;
        }

        public UI_Text(string text, int size, Vector2f pos, View view) : base(pos, view)
        {
            textComp = new Text(text, new Font("Assets/Fonts/m6x11.ttf"), (uint)size);
            this.textBinding = null;
            defaultText = text;
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
