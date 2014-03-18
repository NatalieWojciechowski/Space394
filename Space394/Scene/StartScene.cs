using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Space394.PlayerStates;

namespace Space394.Scenes
{
    public class StartScene : Scene
    {
        private Texture2D stars;

        private Texture2D mainMenuTexture;

        private Vector2 pressStartPosition;
        private AutoTexture2D pressStart;

        private float fade;
        private const float FADE_SPEED = 0.5f;
        private bool fadeIn = true;
        private float fadeDelay;
        private float MIN_FADE = 0.15f;
        private float MIN_DELAY = 0.0f;
        private float MAX_FADE = 1.0f;
        private float MAX_DELAY = 0.01f;
        private bool delaySet = false;

        private bool drawStart;
        private float pressStartFadeDelay;
        private const float PRESS_START_FADE_DELAY = 2.5f;
        
        private float pressStartFade;
        private const float PRESS_START_FADE_SPEED = 0.5f;

        private bool shownStart;

        public StartScene()
            : base()
        {
            LogCat.updateValue("Scene", "Start");
        }

        public override void Initialize()
        {
            mainMenuTexture = ContentLoadManager.loadTexture("Textures/Screens/start");

            stars = ContentLoadManager.loadTexture("Textures/Screens/stars_background");

            Texture2D pressStartTexture = ContentLoadManager.loadTexture("Textures/Screens/text_press_start");
            pressStartPosition = new Vector2(400 - pressStartTexture.Width / 2, 400);
            pressStart = new AutoTexture2D(pressStartTexture, pressStartPosition);

            drawStart = false;
            pressStartFadeDelay = PRESS_START_FADE_DELAY;
            pressStartFade = 0;
            fade = 0;
            fadeDelay = MAX_DELAY;
            fadeIn = true;
            delaySet = false;

            shownStart = false;

            Space394Game.GameInstance.clearPlayers();

            SoundManager.StartMenuMusic();
            
            base.Initialize();
        }

        public override void Update(float deltaTime) 
        {
            fade = Math.Min(1.0f, fade + FADE_SPEED * deltaTime);

            pressStartFadeDelay -= deltaTime;
            if (pressStartFadeDelay <= 0)
            {
                drawStart = true;
                if (fadeIn)
                {
                    if (!delaySet)
                    {
                        pressStartFade += deltaTime * PRESS_START_FADE_SPEED;
                        if (pressStartFade >= MAX_FADE)
                        {
                            shownStart = true;
                            pressStartFade = MAX_FADE;
                            delaySet = true;
                            fadeDelay = MAX_DELAY;
                        }
                        else { }
                    }
                    else
                    {
                        fadeDelay -= deltaTime;
                        if (fadeDelay <= 0)
                        {
                            fadeIn = false;
                            delaySet = false;
                        }
                        else { }
                    }
                }
                else
                {
                    if (!delaySet)
                    {
                        pressStartFade -= deltaTime * PRESS_START_FADE_SPEED;
                        if (pressStartFade <= MIN_FADE)
                        {
                            pressStartFade = MIN_FADE;
                            delaySet = true;
                            fadeDelay = MIN_DELAY;
                        }
                        else { }
                    }
                    else
                    {
                        fadeDelay -= deltaTime;
                        if (fadeDelay <= 0)
                        {
                            fadeIn = true;
                            delaySet = false;
                        }
                        else { }
                    }
                }
            }
            else { }

            if (InputManager.isSuperConfirmationKeyPressed())
            {
                if (shownStart)
                {
                    readyToExit = true;
                }
                else
                {
                    shownStart = true;
                    drawStart = true;
                    fade = 1.0f;
                    pressStartFade = MAX_FADE;
                    delaySet = true;
                    fadeDelay = MAX_DELAY;
                }
            }
            else { }

            base.Update(deltaTime);
        }

        public override void Draw() 
        {
            SpriteBatch spriteBatch = Space394Game.GameInstance.SpriteBatch;

            spriteBatch.Begin();
            spriteBatch.Draw(stars,
                new Rectangle(Space394Game.GameInstance.GraphicsDevice.Viewport.X, Space394Game.GameInstance.GraphicsDevice.Viewport.Y,
                    Space394Game.GameInstance.GraphicsDevice.Viewport.Width, Space394Game.GameInstance.GraphicsDevice.Viewport.Height), 
                new Color(fade, fade, fade));
            spriteBatch.Draw(mainMenuTexture,
                new Rectangle(Space394Game.GameInstance.GraphicsDevice.Viewport.X, Space394Game.GameInstance.GraphicsDevice.Viewport.Y,
                    Space394Game.GameInstance.GraphicsDevice.Viewport.Width, Space394Game.GameInstance.GraphicsDevice.Viewport.Height), 
                new Color(fade, fade, fade));
            if (drawStart)
            {
                pressStart.Color = new Color(pressStartFade, pressStartFade, pressStartFade);
                pressStart.DrawAlreadyBegunMaintainRatio();
            }
            else { }
            spriteBatch.End();

            Space394Game.GameInstance.GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            base.Draw();
        }

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
