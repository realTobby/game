using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game.UI
{
    public abstract class UIComponent
    {
        private Vector2f _position;

        public Vector2f Position
        {
            get { return _position; }
            set
            {
                _position = value;
                OnPositionChanged();
            }
        }

        public UIComponent(Vector2f position)
        {
            _position = position;
        }

        public abstract void Update();
        public abstract void Draw();

        protected virtual void OnPositionChanged()
        {
            // Override this method in derived classes if needed
        }
    }
}
