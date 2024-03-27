using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game.Scenes
{
    public class SceneManager
    {
        private Stack<Scene> scenes;

        public SceneManager()
        {
            scenes = new Stack<Scene>();
        }

        public void PushScene(Scene scene)
        {
            scenes.Push(scene);
            scene.LoadContent();
        }

        public void PopScene()
        {
            Scene scene = scenes.Peek();
            scene.UnloadContent();
            scenes.Pop();
        }

        public void Update(float deltaTime)
        {
            Scene scene = scenes.Peek();
            scene.Update(deltaTime);
        }

        public void Draw(SFML.Graphics.RenderTexture renderTexture, float deltaTime)
        {
            Scene scene = scenes.Peek();
            scene.Draw(renderTexture, deltaTime);
        }
    }
}
