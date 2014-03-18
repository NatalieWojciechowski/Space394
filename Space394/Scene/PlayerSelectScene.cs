using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Space394.PlayerStates;

namespace Space394.Scenes
{
    public class PlayerSelectScene : Scene
    {
        private Texture2D stars;
        private Texture2D background;

        private SpriteFont font = null;

        private Color color = Color.White;

        private Vector2 textPosition;

        private List<bool> justJoined;
        private List<bool> readys;

        private bool everyoneReady;

        private Vector2[] joinTextPositions;
        private Vector2[] pressReadyTextPositions;
        private Vector2[] readyTextPositions;
        private Vector2[] controllerPositions;

        private AutoTexture2D pressStartToJoin;
        private AutoTexture2D pressConfirmWhenReady;

        private AutoTexture2D ready;

        private Vector2 pressStartToBeginPosition;
        private AutoTexture2D pressStartToBegin;

        private AutoTexture2D controller1;
        private AutoTexture2D controller2;
        private AutoTexture2D controller3;
        private AutoTexture2D controller4;
        private AutoTexture2D keyboard;

        private Space394Game.sceneEnum nextScene;

        private const float DELAY = 0.1f;
        private float delay;

        public PlayerSelectScene()
            : base()
        {
            readyToExit = false;
            justJoined = new List<bool>();
            readys = new List<bool>();

            everyoneReady = false;

            foreach (Player player in Space394Game.GameInstance.Players)
            {
                readys.Add(false);
            }
        }

        public override void Initialize()
        {
            textPosition = new Vector2(100, 400);

            stars = ContentLoadManager.loadTexture("Textures/Screens/stars_background_alt");
            background = ContentLoadManager.loadTexture("Textures/Screens/player_start_grid");

            Texture2D pressStartToJoinTexture = ContentLoadManager.loadTexture("Textures/Screens/text_start_to_join");
            Texture2D pressConfirmWhenReadyTexture = ContentLoadManager.loadTexture("Textures/Screens/text_a_when_ready");
            Texture2D readyTexture = ContentLoadManager.loadTexture("Textures/Screens/text_ready");
            Texture2D pressStartToBeginTexture = ContentLoadManager.loadTexture("Textures/Screens/text_start_match");

            int width = pressStartToJoinTexture.Width / 2;
            joinTextPositions = new Vector2[] {
                new Vector2(200-width, 100),
                new Vector2(600-width, 100),
                new Vector2(200-width, 340),
                new Vector2(600-width, 340) };

            width = pressConfirmWhenReadyTexture.Width / 2;
            pressReadyTextPositions = new Vector2[] {
                new Vector2(200-width, 50),
                new Vector2(600-width, 50),
                new Vector2(200-width, 290),
                new Vector2(600-width, 290) };

            width = readyTexture.Width / 2;
            readyTextPositions = new Vector2[] {
                new Vector2(200-width, 50),
                new Vector2(600-width, 50),
                new Vector2(200-width, 290),
                new Vector2(600-width, 290) };
            
            pressStartToBeginPosition = new Vector2(400 - pressStartToBeginTexture.Width / 2, 240 - pressStartToBeginTexture.Height / 2);

            pressStartToJoin = new AutoTexture2D(pressStartToJoinTexture, joinTextPositions[0]);
            pressConfirmWhenReady = new AutoTexture2D(pressConfirmWhenReadyTexture, pressReadyTextPositions[0]);
            ready = new AutoTexture2D(readyTexture, readyTextPositions[0]);

            pressStartToBegin = new AutoTexture2D(pressStartToBeginTexture, pressStartToBeginPosition);

            controller1 = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/xbox_player_1_icon"), Vector2.Zero);
            controller2 = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/xbox_player_2_icon"), Vector2.Zero);
            controller3 = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/xbox_player_3_icon"), Vector2.Zero);
            controller4 = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/xbox_player_4_icon"), Vector2.Zero);
            keyboard = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/keyboard_icon"), Vector2.Zero);

            font = Space394Game.GameInstance.Content.Load<SpriteFont>("Fonts\\DefaultFont");

            nextScene = Space394Game.sceneEnum.LoadingScene;

            controllerPositions = new Vector2[] {
                new Vector2(200, 135),
                new Vector2(600, 135),
                new Vector2(200, 375),
                new Vector2(600, 375) };
            
            base.Initialize();

            /*for (int i = 0; i < Space394Game.GameInstance.Controllers.Length-2; i++)
            {
                Space394Game.GameInstance.increasePlayers((int)Space394Game.GameInstance.Controllers[i].PlayerNumber);
                readyToExit = true;
            }*/
        }

        public override void Update(float deltaTime) 
        {
            delay -= deltaTime;
            if (delay <= 0)
            {
                foreach (Player player in Space394Game.GameInstance.Players) // Only active players can pick
                {
                    if (everyoneReady && InputManager.isCombinedSuperConfirmPressed(player.PlayerNumber))
                    {
                        readyToExit = true;
                    }
                    else { } // Else done
                }
            }
            else { }

            // By controllers
            foreach (Player player in Space394Game.GameInstance.Controllers)
            {
                player.Update(deltaTime);
            }

            ProcessInput();
        }

        public override void Draw() 
        {
            SpriteBatch spriteBatch = Space394Game.GameInstance.SpriteBatch;
            spriteBatch.Begin();
            spriteBatch.Draw(stars,
                new Rectangle(Space394Game.GameInstance.GraphicsDevice.Viewport.X, Space394Game.GameInstance.GraphicsDevice.Viewport.Y,
                    Space394Game.GameInstance.GraphicsDevice.Viewport.Width, Space394Game.GameInstance.GraphicsDevice.Viewport.Height), 
                Color.White);
            spriteBatch.Draw(background,
                new Rectangle(Space394Game.GameInstance.GraphicsDevice.Viewport.X, Space394Game.GameInstance.GraphicsDevice.Viewport.Y,
                    Space394Game.GameInstance.GraphicsDevice.Viewport.Width, Space394Game.GameInstance.GraphicsDevice.Viewport.Height), 
                Color.White);

            for (int i = 0; i < Space394Game.GameInstance.Players.Count; i++)
            {
                Vector2 position;
                switch (Space394Game.GameInstance.Players[i].PlayerNumber)
                {
                    case Player.ControllerIndex.One:
                        position = new Vector2(controllerPositions[i].X - (controller1.Texture.Width / 2),
                            controllerPositions[i].Y - (controller1.Texture.Height / 2));
                        controller1.Position = position;
                        controller1.DrawAlreadyBegunMaintainRatio();
                        break;
                    case Player.ControllerIndex.Two:
                        position = new Vector2(controllerPositions[i].X - (controller2.Texture.Width / 2),
                            controllerPositions[i].Y - (controller2.Texture.Height / 2));
                        controller2.Position = position;
                        controller2.DrawAlreadyBegunMaintainRatio();
                        break;
                    case Player.ControllerIndex.Three:
                        position = new Vector2(controllerPositions[i].X - (controller3.Texture.Width / 2),
                            controllerPositions[i].Y - (controller3.Texture.Height / 2));
                        controller3.Position = position;
                        controller3.DrawAlreadyBegunMaintainRatio();
                        break;
                    case Player.ControllerIndex.Four:
                        position = new Vector2(controllerPositions[i].X - (controller4.Texture.Width / 2),
                            controllerPositions[i].Y - (controller4.Texture.Height / 2));
                        controller4.Position = position;
                        controller4.DrawAlreadyBegunMaintainRatio();
                        break;
                    case Player.ControllerIndex.Keyboard:
                        position = new Vector2(controllerPositions[i].X - (keyboard.Texture.Width / 2),
                            controllerPositions[i].Y - (keyboard.Texture.Height / 2));
                        keyboard.Position = position;
                        keyboard.DrawAlreadyBegunMaintainRatio();
                        break;
                }
                if (readys[i])
                {
                    ready.Position = readyTextPositions[i];
                    ready.DrawAlreadyBegunMaintainRatio();
                }
                else
                {
                    pressConfirmWhenReady.Position = pressReadyTextPositions[i];
                    pressConfirmWhenReady.DrawAlreadyBegunMaintainRatio();
                }
            }

            if (everyoneReady && delay <= 0)
            {
                pressStartToBegin.DrawAlreadyBegunMaintainRatio();
            }
            else { }

            if (Space394Game.GameInstance.Players.Count < 4)
            {
                pressStartToJoin.Position = joinTextPositions[Space394Game.GameInstance.Players.Count];
                pressStartToJoin.DrawAlreadyBegunMaintainRatio();
            }

            spriteBatch.End();

            Space394Game.GameInstance.GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            base.Draw();
        }

        public override Space394Game.sceneEnum GetNextScene()
        {
            return nextScene;
        }

        public override void Exit(Scene nextScene)
        {
            SceneObjects.Clear();
            base.Exit(nextScene);
        }

        private void ProcessInput()
        {
            foreach (Player player in Space394Game.GameInstance.Controllers)
            {
                if (InputManager.isCombinedSuperConfirmPressed(player.PlayerNumber))
                {
                    if (!Space394Game.GameInstance.Players.Contains(player))
                    {
                        Space394Game.GameInstance.increasePlayers((int)player.PlayerNumber);
                    }
                    else { }
                }
                else if (InputManager.isCombinedUnconfirmPressed(player.PlayerNumber))
                {
                    if (Space394Game.GameInstance.Players.Contains(player))
                    {
                        if (!readys[player.ScreenIndex])
                        {
                            Space394Game.GameInstance.decreasePlayers((int)player.PlayerNumber);
                        }
                        else { }
                    }
                    else { }
                }
                else { }
            }

            foreach (Player player in Space394Game.GameInstance.Players)
            {
                if (InputManager.isCombinedConfirmPressed(player.PlayerNumber)
                    || InputManager.isCombinedSuperConfirmPressed(player.PlayerNumber)
                    || InputManager.isCombinedPrimaryFirePressed(player.PlayerNumber))
                {
                    if (!justJoined[player.ScreenIndex])
                    {
                        readys[player.ScreenIndex] = true;
                    }
                    else { }
                }
                else if (InputManager.isCombinedUnconfirmPressed(player.PlayerNumber))
                {
                    readys[player.ScreenIndex] = false;
                }
                else { }
            }

            if (!everyoneReady)
            {
                delay = DELAY;
            }
            else { }

            if (Space394Game.GameInstance.Players.Count > 0
                && Space394Game.GameInstance.Players.Count == readys.Count)
            {
                everyoneReady = true;
                foreach (bool readiness in readys)
                {
                    if (!readiness)
                    {
                        everyoneReady = false;
                        break;
                    }
                    else { }
                }
            }
            else { }

            for (int i = 0; i < justJoined.Count; i++)
            {
                justJoined[i] = false;
            }
                
            if (InputManager.isQuitKeyPressed())
            {
                readyToExit = true;
                nextScene = Space394Game.sceneEnum.MainMenuScene;
                foreach (Player player in Space394Game.GameInstance.Controllers)
                {
                    player.CurrentState = new MenuPlayerState(player);
                }
            }
            else { }
        }

        public override void addPlayer(Player player)
        {
            everyoneReady = false;
            justJoined.Add(true);
            readys.Add(false);

            delay = DELAY;
            
            base.addPlayer(player);
        }

        public override void removePlayer(Player player)
        {
            readys.RemoveAt(player.ScreenIndex);
            justJoined.RemoveAt(player.ScreenIndex);

            for (int i = 0; i < readys.Count; i++)
            {
                Space394Game.GameInstance.Players[i].ScreenIndex = i;
            }

            Space394Game.GameInstance.updatePlayerCameras();
            
            base.removePlayer(player);
        }
    }
}
