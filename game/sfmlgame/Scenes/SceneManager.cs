using sfmglame.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sfmlgame.Scenes
{
    public class SceneManager
    {
        private Stack<Scene> scenes;

        public SceneManager()
        {
            scenes = new Stack<Scene>();
        }

        public void Clear()
        {
            foreach(var scene in scenes)
            {
                scene.UnloadContent();
            }
        }

        public void PushScene(Scene scene)
        {
            scenes.Push(scene);
            scene.LoadContent();
        }

        public void PopScene()
        {
            if(scenes.Count > 0)
            {
                Scene scene = scenes.Peek();
                scene.UnloadContent();
                scenes.Pop();
            }
            UniversalLog.LogInfo("No Scene to Unload!");
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


        public void CallGameSceneForOpenPlayerInfo()
        {
            GameScene gameScene = scenes.Peek() as GameScene;
            gameScene.PlayerInfo.IsOpen = !gameScene.PlayerInfo.IsOpen;
        }

    }
}
