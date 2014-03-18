using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Space394.Scenes
{
    public class OptionsScene : Scene
    {
        public OptionsScene()
            : base()
        {

        }

        public override void Update(float deltaTime) { }

        public override void Draw() { }

        public override Space394Game.sceneEnum GetNextScene()
        {
            return Space394Game.sceneEnum.MainMenuScene;
        }

        public override void Exit(Scene nextScene)
        {
            SceneObjects.Clear();
            base.Exit(nextScene);
        }
    }
}
