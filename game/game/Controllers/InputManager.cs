using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game.Controllers
{
    using SFML.System;
    using SFML.Window;
    using System.Collections.Generic;

    namespace game.Controllers
    {
        public class InputManager
        {
            private Dictionary<Keyboard.Key, bool> keyStates;
            private Vector2i lastMousePosition;

            public InputManager()
            {
                keyStates = new Dictionary<Keyboard.Key, bool>();

                // Initialize the key states dictionary with default values
                foreach (Keyboard.Key key in (Keyboard.Key[])Keyboard.Key.GetValues(typeof(Keyboard.Key)))
                {
                    keyStates[key] = false;
                }

                lastMousePosition = new Vector2i();
            }

            public void Update()
            {
                // Update the key states based on current key presses
                foreach (Keyboard.Key key in (Keyboard.Key[])Keyboard.Key.GetValues(typeof(Keyboard.Key)))
                {
                    keyStates[key] = Keyboard.IsKeyPressed(key);
                }

                // Update the mouse position
                lastMousePosition = Mouse.GetPosition();
            }

            public bool IsKeyPressed(Keyboard.Key key)
            {
                return keyStates[key];
            }

            public Vector2i GetMousePosition()
            {
                return lastMousePosition;
            }
        }
    }

}
