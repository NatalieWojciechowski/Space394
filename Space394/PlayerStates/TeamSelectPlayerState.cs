using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Space394.SceneObjects;
using Space394.Scenes;

namespace Space394.PlayerStates
{
    public class TeamSelectPlayerState : PlayerState
    {
        private enum Team
        {
            Esxolus,
            Halk,
            Auto,
            Spectate
        };

        private Team selectedTeam;

        private AutoTexture2D box;

        private AutoTexture2D esxolus;
        private AutoTexture2D halk;
        private AutoTexture2D auto;
        private AutoTexture2D spectate;

        private AutoTexture2D esxolusLit;
        private AutoTexture2D halkLit;
        private AutoTexture2D autoLit;
        private AutoTexture2D spectateLit;

        private Vector3 cameraPosition;
        private Vector3 cameraView;
        private Vector3 cameraUp;

        private PlayerState nextState;

        public TeamSelectPlayerState(Player _player)
        {
            selectedTeam = Team.Auto;

            currentState = state.team_select;

            nextState = null;

            player = _player;
            player.TeamNotSet = true;

            cameraPosition = new Vector3(0, 75000, 0);
            cameraView = new Vector3(0, 0, 0);
            cameraUp = new Vector3(0, 0, 1);

            player.PlayerCamera.setViewMatrix(cameraPosition, cameraView, cameraUp);

            player.PlayerHUD.InitializeGraphics();

            player.PlayerShip = null;

            switch ( Space394Game.GameInstance.NumberOfPlayers)
            {
                case 1:
                    box = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/team_select"), new Vector2(142, 418));
                    break;
                case 2:
                    box = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/team_select"), new Vector2(142, 418));
                    break;
                case 3:
                    box = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/team_select"), new Vector2(142, 418));
                    break;
                case 4:
                    box = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/team_select"), new Vector2(142, 418));
                    break;

            }

            esxolus = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/team_select_esx"), new Vector2(142, 281));
            halk = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/team_select_halk"), new Vector2(538, 280));
            auto = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/team_select_auto"), new Vector2(352, 272));
            spectate = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/team_select_spectate"), new Vector2(343, 348));

            esxolusLit = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/team_select_esx_lit"), new Vector2(142, 281));
            halkLit = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/team_select_halk_lit"), new Vector2(538, 280));
            autoLit = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/team_select_auto_lit"), new Vector2(352, 272));
            spectateLit = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/team_select_spectate_lit"), new Vector2(343, 348));

            LogCat.updateValue("PlayerState", "TeamSelect");
        }

        public override void Update(float deltaTime)
        {
            player.PlayerCamera.setViewMatrix(cameraPosition, cameraView, cameraUp);
        }

        public override void Draw(PlayerCamera camera)
        {
            Space394Game.GameInstance.GraphicsDevice.Viewport = camera.ViewPort;
            
            SpriteBatch spriteBatch = Space394Game.GameInstance.SpriteBatch;
            spriteBatch.Begin();

            box.DrawAlreadyBegunMaintainRatio(camera);

            switch (selectedTeam)
            {
                case Team.Auto:
                    esxolus.DrawAlreadyBegunMaintainRatio(camera);
                    halk.DrawAlreadyBegunMaintainRatio(camera);
                    autoLit.DrawAlreadyBegunMaintainRatio(camera);
                    spectate.DrawAlreadyBegunMaintainRatio(camera);
                    box.DrawAlreadyBegunMaintainRatio(camera);
                    break;
                case Team.Esxolus:
                    esxolusLit.DrawAlreadyBegunMaintainRatio(camera);
                    halk.DrawAlreadyBegunMaintainRatio(camera);
                    auto.DrawAlreadyBegunMaintainRatio(camera);
                    spectate.DrawAlreadyBegunMaintainRatio(camera);
                    box.DrawAlreadyBegunMaintainRatio(camera);
                    break;
                case Team.Halk:
                    esxolus.DrawAlreadyBegunMaintainRatio(camera);
                    halkLit.DrawAlreadyBegunMaintainRatio(camera);
                    auto.DrawAlreadyBegunMaintainRatio(camera);
                    spectate.DrawAlreadyBegunMaintainRatio(camera);
                    box.DrawAlreadyBegunMaintainRatio(camera);
                    break;
                case Team.Spectate:
                    esxolus.DrawAlreadyBegunMaintainRatio(camera);
                    halk.DrawAlreadyBegunMaintainRatio(camera);
                    auto.DrawAlreadyBegunMaintainRatio(camera);
                    spectateLit.DrawAlreadyBegunMaintainRatio(camera);
                    box.DrawAlreadyBegunMaintainRatio(camera);
                    break;
            }

            spriteBatch.End();

            player.PlayerHUD.DrawTray(camera);

            Space394Game.GameInstance.GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            Space394Game.GameInstance.GraphicsDevice.Viewport = Space394Game.GameInstance.DefaultViewPort;
        }

        public override PlayerState getNextState(Player _player)
        {
            OnExit();
            if (nextState == null)
            {
                if ((GameScene)Space394Game.GameInstance.CurrentScene is GameScene)
                {
                    if (((GameScene)Space394Game.GameInstance.CurrentScene).getSpawnPointsRemaining(_player.CurrentTeam) <= 0)
                    {
                        // player.getPlayerHUD().InitializeHUDTeam();
                        nextState = new ScoreboardPlayerState(_player);
                    }
                    else
                    {
                        // player.getPlayerHUD().InitializeHUDTeam();
                        nextState = new SpawnSelectPlayerState(_player);
                    }
                }
                else
                {
                    nextState = this;
                }
            }
            else { }
            return nextState;
        }

        public override void ProcessInput()
        {
            if (Space394Game.GameInstance.CurrentScene is GameScene)
            {
                if (InputManager.isCombinedUpCombinedStickPressed(player.PlayerNumber))
                {
                    selectedTeam = Team.Auto;
                }
                else if (InputManager.isCombinedDownCombinedStickPressed(player.PlayerNumber))
                {
                    selectedTeam = Team.Spectate;
                }
                else { }
                switch (selectedTeam)
                {
                    case Team.Auto:
                        if (InputManager.isCombinedLeftCombinedStickPressed(player.PlayerNumber))
                        {
                            selectedTeam = Team.Esxolus;
                            //selectorPosition = esxolusPosition;
                        }
                        else if (InputManager.isCombinedRightCombinedStickPressed(player.PlayerNumber))
                        {
                            selectedTeam = Team.Halk;
                            //selectorPosition = halkPosition;
                        }
                        else { }
                        break;

                    case Team.Esxolus:
                        if (InputManager.isCombinedLeftCombinedStickPressed(player.PlayerNumber))
                        {
                            selectedTeam = Team.Halk;
                            //selectorPosition = halkPosition;
                        }
                        else if (InputManager.isCombinedRightCombinedStickPressed(player.PlayerNumber))
                        {
                            selectedTeam = Team.Auto;
                            //selectorPosition = autoPosition;
                        }
                        else { }
                        break;

                    case Team.Halk:
                        if (InputManager.isCombinedLeftCombinedStickPressed(player.PlayerNumber))
                        {
                            selectedTeam = Team.Auto;
                            //selectorPosition = autoPosition;
                        }
                        else if (InputManager.isCombinedRightCombinedStickPressed(player.PlayerNumber))
                        {
                            selectedTeam = Team.Esxolus;
                            //selectorPosition = esxolusPosition;
                        }
                        else { }
                        break;

                    case Team.Spectate:
                        if (InputManager.isCombinedLeftCombinedStickPressed(player.PlayerNumber))
                        {
                            selectedTeam = Team.Esxolus;
                            //selectorPosition = esxolusPosition;
                        }
                        else if (InputManager.isCombinedRightCombinedStickPressed(player.PlayerNumber))
                        {
                            selectedTeam = Team.Halk;
                            //selectorPosition = halkPosition;
                        }
                        else if (InputManager.isCombinedUpCombinedStickPressed(player.PlayerNumber))
                        { }
                        break;
                }
                //selector.setPosition(selectorPosition);

                if (InputManager.isCombinedConfirmPressed(player.PlayerNumber)
                    || InputManager.isCombinedPrimaryFirePressed(player.PlayerNumber))
                {
                    switch (selectedTeam)
                    {
                        case Team.Auto:
                            if (((GameScene)Space394Game.GameInstance.CurrentScene).EsxolusPlayers <
                                ((GameScene)Space394Game.GameInstance.CurrentScene).HalkPlayers)
                            {
                                player.CurrentTeam = Ship.Team.Esxolus;
                            }
                            else if (((GameScene)Space394Game.GameInstance.CurrentScene).EsxolusPlayers >
                                ((GameScene)Space394Game.GameInstance.CurrentScene).HalkPlayers)
                            {
                                player.CurrentTeam = Ship.Team.Halk;
                            }
                            else
                            {
                                Random random = new Random(System.DateTime.Now.Millisecond);
                                switch (random.Next() % 2)
                                {
                                    case 0:
                                        player.CurrentTeam = Ship.Team.Esxolus;
                                        break;
                                    case 1:
                                        player.CurrentTeam = Ship.Team.Halk;
                                        break;
                                }
                            }
                            player.TeamNotSet = false;
                            break;
                        case Team.Esxolus:
                            player.CurrentTeam = Ship.Team.Esxolus;
                            player.TeamNotSet = false;
                            break;
                        case Team.Halk:
                            player.CurrentTeam = Ship.Team.Halk;
                            player.TeamNotSet = false;
                            break;
                        case Team.Spectate:
                            nextState = new SpectatorPlayerState(player);
                            break;
                    }
                    StateComplete = true;
                }
                else { }

                if (InputManager.isCombinedPausePressed(player.PlayerNumber))
                {
                    player.pause();
                }
                else { }
            }
            else { } // Still loading
        } // End process input

        public override void OnExit()
        {
            
        }
    }
}
