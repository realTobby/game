using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game.Scenes
{
    public abstract class Scene
    {
        public abstract void LoadContent();
        public abstract void Update(float deltaTime);
        public abstract void Draw();
        public abstract void UnloadContent();
    }
}
