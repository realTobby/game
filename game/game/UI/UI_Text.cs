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
        private View cameraView;

        private UITextBinding textBinding;
        private string defaultText = string.Empty;

        public UI_Text(string text, int size, Vector2f pos, View view, UITextBinding textBinding) : base(pos)
        {
            textComp = new Text(text, new Font("Assets/Fonts/m6x11.ttf"), (uint)size);
            cameraView = view;
            this.textBinding = textBinding;
            defaultText = text;
        }

        public UI_Text(string text, int size, Vector2f pos, View view) : base(pos)
        {
            textComp = new Text(text, new Font("Assets/Fonts/m6x11.ttf"), (uint)size);
            cameraView = view;
            this.textBinding = null;
            defaultText = text;
        }

        public override void Draw()
        {
            RenderWindow window = Game.Instance.GetRenderWindow();

            // Store the original view
            View originalView = window.GetView();

            // Set the view of the render target to the camera's view
            window.SetView(cameraView);

            // Update the position relative to the camera view
            Vector2f offsetPosition = Position + cameraView.Center - cameraView.Size / 2f;
            textComp.Position = offsetPosition;

            // Update the text value
            if(textBinding != null) textComp.DisplayedString = defaultText + textBinding.Value;


            // Draw the text
            window.Draw(textComp);

            // Restore the original view
            window.SetView(originalView);
        }

        public override void Update()
        {

        }
    }
}
