using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Space394.SceneObjects;
using Space394.PlayerStates;
using BEPUphysics.NarrowPhaseSystems;

namespace Space394.Scenes
{
    public class LoadingScene : Scene
    {
        private Texture2D stars;
        private AnimatedTextureFiles LoadingTexture;

        private Texture2D controls;

        private Thread loadThread;

        private static string mapToLoad = "SampleSmall";
        public static string MapToLoad
        {
            get { return LoadingScene.mapToLoad; }
            set { LoadingScene.mapToLoad = value; }
        }

        public LoadingScene()
            : base()
        {
            LogCat.updateValue("Scene", "Loading");

            LogCat.updateValue("Loading", "Starting");
        }

        public override void Initialize()
        {
            stars = ContentLoadManager.loadTexture("Textures/Screens/stars_background");

            controls = ContentLoadManager.loadTexture("Textures/Screens/loading_controls");

            LoadingTexture = new AnimatedTextureFiles(Vector2.Zero, 0, 1, 0);
            LoadingTexture.Load("Textures/Screens/loading_", 4, 1, Vector2.Zero);

            LoadContent();
        }

        public override void Update(float deltaTime) 
        {
            LoadingTexture.UpdateFrame(deltaTime);
            if (ContentLoaded)
            {
                loadThread.Join();
                readyToExit = true;
                LogCat.updateValue("Loading", "Finished");

                base.Update(deltaTime);
            }
            else { }
        }

        public override void Draw() 
        {
            SpriteBatch spriteBatch = Space394Game.GameInstance.SpriteBatch;
            
            spriteBatch.Begin();
            spriteBatch.Draw(stars, 
                new Rectangle(Space394Game.GameInstance.GraphicsDevice.Viewport.X, Space394Game.GameInstance.GraphicsDevice.Viewport.Y,
                    Space394Game.GameInstance.GraphicsDevice.Viewport.Width, Space394Game.GameInstance.GraphicsDevice.Viewport.Height), 
                    Color.White);
            spriteBatch.End();

            LoadingTexture.DrawFrame(spriteBatch, Vector2.Zero);

            spriteBatch.Begin();
            spriteBatch.Draw(controls,
                new Rectangle(Space394Game.GameInstance.GraphicsDevice.Viewport.X, Space394Game.GameInstance.GraphicsDevice.Viewport.Y,
                    Space394Game.GameInstance.GraphicsDevice.Viewport.Width, Space394Game.GameInstance.GraphicsDevice.Viewport.Height), 
                Color.White);
            spriteBatch.End();

            Space394Game.GameInstance.GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            // base.Draw();
        }

        public override void Exit(Scene nextScene)
        {
            nextScene.handSceneObjects(SceneObjects);

            SoundManager.StopMusic();

            foreach (SceneObject so in SceneObjects.Values)
            {
                if (so.CollisionBase != null && so.Active)
                {
                    ((GameScene)nextScene).CollisionManager.addToCollisionList(so.CollisionBase);
                }
                else { }
            }
        }

        public override Space394Game.sceneEnum GetNextScene()
        {
            return Space394Game.sceneEnum.GameScene;
        }

        private volatile bool ContentLoaded = false;
        private void LoadGameContent()
        {
            LogCat.updateValue("Loading", "Started");
            
            NarrowPhaseHelper.Factories.SphereSphere.EnsureCount(750);
            NarrowPhaseHelper.Factories.MobileMeshSphere.EnsureCount(500);

            ProjectileManager.InitalizeLasers(300);
            ProjectileManager.InitalizeEsxolusAMissiles(150);
            ProjectileManager.InitalizeEsxolusBMissiles(100);
            ProjectileManager.InitalizeEsxolusIMissiles(150);
            ProjectileManager.InitalizeHalkAMissiles(55);
            ProjectileManager.InitalizeHalkBMissiles(150);
            ProjectileManager.InitalizeHalkIMissiles(150);
            SceneObjectFactory.createNewStarBox();

            LuaManager.doFile(mapToLoad);

            //load all your content here 
            ContentLoaded = true;
        }

        private void LoadContent()
        {
            loadThread = new Thread(new ThreadStart(ThreadPoolCallback));
            loadThread.Start();
            loadThread.IsBackground = true;
        }

        // Wrapper method for use with thread pool
        private void ThreadPoolCallback()
        {
            //int threadIndex = (int)threadContext;
            LoadGameContent();
        }

        public override void AbortThreads()
        {
            loadThread.Abort();
            loadThread.Join();
            
            base.AbortThreads();
        }
    }
}
