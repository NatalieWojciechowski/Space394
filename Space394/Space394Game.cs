//#define LIVE

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Space394.Scenes;
using Space394.Packaging;
using Space394.SceneObjects;

namespace Space394
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Space394Game : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager graphics;

        private Viewport defaultViewPort;
        public Viewport DefaultViewPort
        {
            get { return defaultViewPort; }
        }

        private SpriteBatch spriteBatch;
        public SpriteBatch SpriteBatch
        {
            get { return spriteBatch; }
        }

        private float aspectRatio;
        public float AspectRatio
        {
            get { return aspectRatio; }
        }

        private static Space394Game game;
        public static Space394Game GameInstance
        {
            get { return Space394Game.game; }
        }

        private static NetworkSession session;
        public static NetworkSession Session
        {
            get { return Space394Game.session; }
        }
        public static void clearNetworkSession() { session = null; }
        private PacketWriter packetWriter;
        private PacketReader packetReader;

        private Stack<Package> packageList;
        public void addPackage(Package package)
        {
            packageList.Push(package);
        }
        private void runPackageList()
        {
            foreach (Package package in packageList)
            {
                package.run();
            }
            packageList.Clear();
        }

        private Scene currentScene;
        public Scene CurrentScene
        {
            get { return currentScene; }
        }

        private List<Player> localPlayers;
        public List<Player> Players
        {
            get { return localPlayers; }
        }

        private const int MAX_CONTROLLERS = 5;
        private Player[] controllers;
        public Player[] Controllers
        {
            get { return controllers; }
        }

        private const int MAX_PLAYERS = 4;
        private int numberOfPlayers = 0;
        public int NumberOfPlayers
        {
            get { return numberOfPlayers; }
        }

        public void clearPlayers() { numberOfPlayers = 0; localPlayers.Clear(); }

        public int increasePlayers(int number)
        {
            if (numberOfPlayers < MAX_PLAYERS && currentScene != null
                && !localPlayers.Contains(controllers[number]))
            {
                localPlayers.Add(controllers[number]);
                currentScene.addPlayer(controllers[number]);
                localPlayers[numberOfPlayers].setPlayerCamera(currentScene);
                localPlayers[numberOfPlayers].IsActive = true;
                localPlayers[numberOfPlayers].ScreenIndex = numberOfPlayers;
                numberOfPlayers++;
                updatePlayerCameras();
            }
            else { }
            return numberOfPlayers;
        }
        public int decreasePlayers(int number)
        {
            if (numberOfPlayers > 0 && currentScene != null
                && localPlayers.Contains(controllers[number]))
            {
                numberOfPlayers--;
                localPlayers[numberOfPlayers].IsActive = false;
                localPlayers.Remove(controllers[number]);
                currentScene.removePlayer(controllers[number]);
                updatePlayerCameras();
            }
            else { }
            return numberOfPlayers;
        }

        public void updatePlayerCameras()
        {
            if (currentScene != null)
            {
                GameCamera camera = currentScene.Camera;
                if (camera.SplitScreenEnabled)
                {
                    for (int i = 0; i < localPlayers.Count; i++)
                    {
                        if (localPlayers[i] != null)
                        {
                            if (localPlayers[i].PlayerCamera != null)
                            {
                                localPlayers[i].PlayerCamera.setRegion(i, numberOfPlayers);
                            }
                            else { }
                        }
                        else { }
                    }
                    camera.TRegions = numberOfPlayers;
                }
                else { }
            }
            else { }
        }

        public const float TimeScale = 1000; // 1/TimeScale milliseconds per update

        public enum sceneEnum
        {
            CreditsScene,
            GameScene,
            LoadingScene,
            LobbyScene,
            LogoScene,
            MainMenuScene,
            MissionSelectScene,
            OptionsScene,
            PlayerSelectScene,
            StartScene
        }; // Currently 10 scenes

        private sceneEnum currentSceneEnum;
        private sceneEnum nextSceneEnum;

        private enum state
        {
            enteringScene,
            inScene,
            leavingScene
        };

        private state currentState;

        public static GameTime lastGameTime;

        public Space394Game()
        {
            game = this;

            // Components.Add(new GamerServicesComponent(this));

            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            // Initialize();

#if !DEBUG
            graphics.PreferredBackBufferWidth = 1920;
            graphics.PreferredBackBufferHeight = 1080;
            graphics.PreferMultiSampling = false;
            graphics.ApplyChanges();
            graphics.IsFullScreen = true;

            AutoTexture2D.WidthConversion = graphics.PreferredBackBufferWidth / 800.0f;
            AutoTexture2D.HeightConversion = graphics.PreferredBackBufferHeight / 480.0f;
#else
            AutoTexture2D.WidthConversion = 1;
            AutoTexture2D.HeightConversion = 1;
#endif
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            currentState = state.enteringScene;
            nextSceneEnum = sceneEnum.LogoScene;
            currentSceneEnum = sceneEnum.LogoScene;

            packageList = new Stack<Package>();

            LuaManager.Initialize();

            InputManager.Initialize();

            LogCat.Initialize();

            ContentLoadManager.Initialize();

            ProjectileManager.Initialize();

            SoundManager.Initialize();

            packetWriter = new PacketWriter();
            packetReader = new PacketReader();

            localPlayers = new List<Player>();

            controllers = new Player[MAX_CONTROLLERS];

            controllers[0] = new Player(Player.ControllerIndex.One);
            controllers[1] = new Player(Player.ControllerIndex.Two);
            controllers[2] = new Player(Player.ControllerIndex.Three);
            controllers[3] = new Player(Player.ControllerIndex.Four);
            controllers[4] = new Player(Player.ControllerIndex.Keyboard);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            defaultViewPort = graphics.GraphicsDevice.Viewport;

            aspectRatio = graphics.GraphicsDevice.Viewport.AspectRatio;
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            SoundManager.UnInitialize();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            LogCat.beginProfiling("Update");

            lastGameTime = gameTime;

            // Let's start out looking at our list
            runPackageList();

            HandleInput();

            // This method will make sure everyone has an XBox id
            CheckPlayerCount();

            LogCat.updateValue("Players", "" + numberOfPlayers);

            // This should only happen in the beginning
            if (currentScene == null)
            {
                LogCat.addValue("Scene", "Null");
                switchScene();
            }
            else { }

            if (currentState == state.enteringScene || currentState == state.inScene)
            {
                // All things in milliseconds
                currentScene.Update(gameTime.ElapsedGameTime.Milliseconds / TimeScale);
                currentState = state.inScene;

                if (currentScene.ReadyToExit)
                {
                    currentState = state.leavingScene;
                }
                else { }
            }
            else if (currentState == state.leavingScene)
            {
                // Set up the next scene based on the events of the previous screen
                nextSceneEnum = currentScene.GetNextScene();
                switchScene();
            }
            else { }

            InputManager.FlushInput();
            base.Update(gameTime);
            LogCat.endProfiling("Update");
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            currentScene.Draw();

            LogCat.DrawLog();

            base.Draw(gameTime);
        }

        protected void CheckPlayerCount()
        {
#if LIVE
            if (Gamer.SignedInGamers.Count > numberOfPlayers)
            {
                increasePlayers(Player.playerNumber.one);
                localPlayersActive[numberOfPlayers - 1] = true;
            }
            else { }
#endif
        }

        protected void HandleInput()
        {
            // Allows the game to exit
            /*if (InputManager.isQuitKeyPressed())
            {
                if (currentScene != null)
                {
                    currentScene.AbortThreads();
                }
                else { }
                this.Exit();
            }
            else { }*/

            #region Packages
            /*
            for (int i = 0; i < MAX_PLAYERS; i++)
            {
                if (InputManager.isStartPressed((PlayerIndex)i)
                    || InputManager.isKeyLeftShiftPressed())
                {
#if LIVE
                if (!Guide.IsVisible)
                {
                    // If there are no profiles signed in, we cannot proceed.
                    // Show the Guide so the user can sign in.
                    Guide.ShowSignIn(MAX_PLAYERS, false);
                    if (currentScene != null)
                    {
                        currentScene.Pause();
                    }
                    else { }
                }
                else { }
#else
                    PackageFactory.createPlayerJoinPackage((PlayerIndex)i);
                    break;
#endif
                }
                else { }
            }

            for (int i = 0; i < MAX_PLAYERS; i++)
            {
                if (InputManager.isXPressed((PlayerIndex)i)
                    || InputManager.isKeyLeftCtrlPressed())
                {
                    PackageFactory.createPlayerDropPackage((PlayerIndex)i);
                    break;
                }
                else { }
            }
            */
            #endregion
        }

        public void screenComplete()
        {
            currentState = state.leavingScene;
        }

        private void switchScene()
        {
            Scene nextScene = null;
            switch (nextSceneEnum)
            {
                case sceneEnum.CreditsScene:
                    nextScene = new CreditsScene();
                    break;
                case sceneEnum.GameScene:
                    nextScene = new GameScene();
                    break;
                case sceneEnum.LoadingScene:
                    nextScene = new LoadingScene();
                    break;
                case sceneEnum.LobbyScene:
                    nextScene = new LobbyScene();
                    break;
                case sceneEnum.LogoScene:
                    nextScene = new LogoScene();
                    break;
                case sceneEnum.MainMenuScene:
                    nextScene = new MainMenuScene();
                    break;
                case sceneEnum.MissionSelectScene:
                    nextScene = new MissionSelectScene();
                    break;
                case sceneEnum.OptionsScene:
                    nextScene = new OptionsScene();
                    break;
                case sceneEnum.PlayerSelectScene:
                    nextScene = new PlayerSelectScene();
                    break;
                case sceneEnum.StartScene:
                    nextScene = new StartScene();
                    break;
            }
            if (currentScene != null)
            {
                currentScene.Exit(nextScene);
            }
            else { }
            currentScene = nextScene;
            currentSceneEnum = nextSceneEnum;
            currentScene.Initialize();
            currentState = state.enteringScene;
            foreach (Player player in localPlayers)
            {
                player.setPlayerCamera(currentScene);
                updatePlayerCameras();
            }
        }
    }
}
