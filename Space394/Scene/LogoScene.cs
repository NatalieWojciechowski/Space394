using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Space394.SceneObjects;
using Microsoft.Xna.Framework.Input;
using Space394;

namespace Space394.Scenes
{
    public class LogoScene : Scene
    {
        private Texture2D opening;

        private float openingDelay;
        private const float OPENING_DELAY = 0.50f;

        private float fade;
        private const float FADE_SPEED = 1.5f;

        private bool fadeOut;

        private float delay;
        private const float DELAY = 1.0f;

        private float ttl;
        private const float TTL = 2.5f;

        public LogoScene()
            : base()
        {
            //opening = new AnimatedTextureFiles(new Vector2(), 0, 1, 0);

            fade = 0;

            fadeOut = false;

            openingDelay = OPENING_DELAY;

            ttl = TTL;

            LogCat.updateValue("Scene", "Logo");
        }

        public override void Initialize()
        {
            opening = ContentLoadManager.loadTexture("Textures/Screens/grubby_paw_logo");

            base.Initialize();
        }

        public override void Update(float deltaTime) 
        {
            openingDelay -= deltaTime;

            if (openingDelay <= 0)
            {

                if (fadeOut)
                {
                    delay -= deltaTime;
                }
                else { }

                if (fadeOut && delay <= 0)
                {
                    fade = Math.Max(0.0f, fade - FADE_SPEED * deltaTime);
                }
                else
                {
                    fade += FADE_SPEED * deltaTime;
                    if (!fadeOut && fade >= 1.0f)
                    {
                        fadeOut = true;
                        delay = DELAY;
                    }
                    else { }
                    fade = Math.Min(1.0f, fade);
                }

                ttl -= deltaTime;
                if (ttl <= 0)
                {
                    readyToExit = true;
                }
                else { }
            }
            else { }

            if (InputManager.isSuperConfirmationKeyPressed())
            {
                readyToExit = true;
            }
            else { }

            base.Update(deltaTime);
        }

        public override void Draw() 
        {
            getGraphics().Clear(Color.Black);

            // opening.DrawFrame(Space394Game.GameInstance.SpriteBatch, Vector2.Zero);

            SpriteBatch spriteBatch = Space394Game.GameInstance.SpriteBatch;
            spriteBatch.Begin();
            spriteBatch.Draw(opening,
                new Rectangle(Space394Game.GameInstance.GraphicsDevice.Viewport.X, Space394Game.GameInstance.GraphicsDevice.Viewport.Y,
                    Space394Game.GameInstance.GraphicsDevice.Viewport.Width, Space394Game.GameInstance.GraphicsDevice.Viewport.Height), 
                new Color(fade, fade, fade));
            spriteBatch.End();

            Space394Game.GameInstance.GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            base.Draw();
        }

        public override Space394Game.sceneEnum GetNextScene()
        {
            return Space394Game.sceneEnum.StartScene;
        }

        public override void Exit(Scene nextScene)
        {
            SceneObjects.Clear();
            base.Exit(nextScene);
        }
    }
}
