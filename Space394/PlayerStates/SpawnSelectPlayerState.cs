using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Space394.SceneObjects;
using Space394.Scenes;
using Microsoft.Xna.Framework.Graphics;

namespace Space394.PlayerStates
{
    public class SpawnSelectPlayerState : PlayerState
    {
        private int index;

        private enum ShipType
        {
            None = 0,
            AssaultFighter = 1,
            Interceptor = 2,
            Bomber = 3
        };
        private ShipType currentShip;

        private bool selectingShip = true;
        private bool ready = false;

        private AutoTexture2D assaultFighter;
        private AutoTexture2D interceptor;
        private AutoTexture2D bomber;

        private AutoTexture2D spawnSelectTexture;
        private AutoTexture2D spawnSelectPressA;

        //private AutoTexture2D preparingToLaunch;

        private PlayerState nextState;

        private AutoTexture2D preparedToLaunch;
        private AutoTexture2D launchIn;
        private AutoTexture2D one;
        private AutoTexture2D two;
        private AutoTexture2D three;
        private AutoTexture2D four;
        private AutoTexture2D five;
        private AutoTexture2D back;

        private const float ROTATION_SPEED = 0.1f;
        private float modelRotation;

        private const float MODEL_SCALE = 0.025f;

        private Vector3 modelPosition;

        private int rememberedSpawnShipNumber;

        public SpawnSelectPlayerState(Player _player)
        {
            index = 0;
            currentShip = ShipType.None;

            modelPosition = new Vector3(0.1f, 0.0f, 1.0f);

            player = _player;

            player.PlayerShip = null;

            if (player.CurrentTeam == Ship.Team.Esxolus)
            {
                assaultFighter = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/esxolus_assault"), Vector2.Zero);
                interceptor = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/esxolus_interceptor"), Vector2.Zero);
                bomber = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/esxolus_bomber"), Vector2.Zero);
                spawnSelectTexture = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/spawn_esx"), new Vector2(142, 418));
                //preparingToLaunch = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/
            }
            else
            {
                assaultFighter = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/halk_assault"), Vector2.Zero);
                interceptor = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/halk_interceptor"), Vector2.Zero);
                bomber = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/halk_bomber"), Vector2.Zero);
                spawnSelectTexture = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/spawn_halk"), new Vector2(142, 418));
            }
            spawnSelectPressA = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/spawn_press_a"), new Vector2(249, 388));
            modelRotation = 0;

            rememberedSpawnShipNumber = getSpawnShips().Count;

            preparedToLaunch = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/spawn_countdown_prep"), new Vector2(278, 113));
            launchIn = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/spawn_countdown_launch"), new Vector2(343, 113));
            one = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/spawn_countdown_1"), new Vector2(361, 167));
            two = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/spawn_countdown_2"), new Vector2(361, 167));
            three = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/spawn_countdown_3"), new Vector2(361, 167));
            four = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/spawn_countdown_4"), new Vector2(361, 167));
            five = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/spawn_countdown_5"), new Vector2(361, 167));
            back = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/back"), new Vector2(25, 435));

            while (index < getSpawnShips().Count
                && getSpawnShips()[index].ShipTeam != player.CurrentTeam) // They must have a ship, but just in case
            {
                index++;
            }

            LogCat.updateValue("PlayerState", "SpawnSelect");
        }

        public override void Update(float deltaTime)
        {
            if (!ready)
            {
                Vector3 position = Vector3.Transform(getSpawnShips()[index].CameraPositions[(int)currentShip], getSpawnShips()[index].Rotation) + getSpawnShips()[index].Position;
                Vector3 target = Vector3.Transform(getSpawnShips()[index].CameraViews[(int)currentShip], getSpawnShips()[index].Rotation) + getSpawnShips()[index].Position;
                Vector3 up = Vector3.Transform(getSpawnShips()[index].CameraUps[(int)currentShip], getSpawnShips()[index].Rotation);

                player.PlayerCamera.setViewMatrix(position, target, up);
            }
            else { }


            modelRotation += ROTATION_SPEED * deltaTime;

            if (getSpawnShips()[index].Health <= 0 || rememberedSpawnShipNumber != getSpawnShips().Count)
            {
                ready = false;
                selectingShip = true;
                index = 0;
                while (index < getSpawnShips().Count
                && getSpawnShips()[index].ShipTeam != player.CurrentTeam) // They must have a ship, but just in case
                {
                    index++;
                }
                if (index == getSpawnShips().Count)
                {
                    // Nothing found
                    nextState = new ScoreboardPlayerState(player);
                    StateComplete = true;
                }
                else { }
            }
            else { }

            if (ready && ((GameScene)Space394Game.GameInstance.CurrentScene).WaveReleased)
            {
                nextState = new SpawningPlayerState(player);
                StateComplete = true;
            }
            else { }
        }

        public override void Draw(PlayerCamera camera)
        {
            SpriteBatch spriteBatch = Space394Game.GameInstance.SpriteBatch;
            spriteBatch.Begin();
            if (selectingShip)
            {
                spawnSelectTexture.DrawAlreadyBegunMaintainRatio(camera);
                spawnSelectPressA.DrawAlreadyBegunMaintainRatio(camera);
            }
            else if (!ready)
            {
                switch (currentShip)
                {
                    case ShipType.AssaultFighter:
                        assaultFighter.DrawAlreadyBegunMaintainRatio(camera);
                        break;
                    case ShipType.Interceptor:
                        interceptor.DrawAlreadyBegunMaintainRatio(camera);
                        break;
                    case ShipType.Bomber:
                        bomber.DrawAlreadyBegunMaintainRatio(camera);
                        break;
                }
            }
            else 
            {
                back.DrawAlreadyBegunMaintainRatio(camera);
                int waveTimer = (int)((GameScene)Space394Game.GameInstance.CurrentScene).WaveTimer;
                if (waveTimer < 5)
                {
                    launchIn.DrawAlreadyBegunMaintainRatio(camera);
                    switch (waveTimer)
                    {
                        case 0:
                            one.DrawAlreadyBegunMaintainRatio(camera);
                            break;
                        case 1:
                            two.DrawAlreadyBegunMaintainRatio(camera);
                            break;
                        case 2:
                            three.DrawAlreadyBegunMaintainRatio(camera);
                            break;
                        case 3:
                            four.DrawAlreadyBegunMaintainRatio(camera);
                            break;
                        case 4:
                            five.DrawAlreadyBegunMaintainRatio(camera);
                            break;
                        default:
                            five.DrawAlreadyBegunMaintainRatio(camera);
                            break;
                    }
                }
                else 
                {
                    preparedToLaunch.DrawAlreadyBegunMaintainRatio(camera);
                }
            }
            spriteBatch.End();
            player.PlayerHUD.DrawTray(camera);
            if (player.ObjectivesDrawActive)
            {
                if (player.CurrentTeam == Ship.Team.Esxolus)
                {
                    if (((GameScene)Space394Game.GameInstance.CurrentScene).getTopEsxolusEvent() != null)
                    {
                        ((GameScene)Space394Game.GameInstance.CurrentScene).getTopEsxolusEvent().Draw((PlayerCamera)camera);
                    }
                    else { }
                }
                else if (player.CurrentTeam == Ship.Team.Halk)
                {
                    if (((GameScene)Space394Game.GameInstance.CurrentScene).getTopHalkEvent() != null)
                    {
                        ((GameScene)Space394Game.GameInstance.CurrentScene).getTopHalkEvent().Draw((PlayerCamera)camera);
                    }
                }
                else { }
            }
            else { }
        }

        public override PlayerState getNextState(Player _player)
        {
            OnExit();
            // player.getPlayerHUD().InitializeHUDTeam();
            return nextState;
        }

        public override void ProcessInput()
        {
            InputManager.centerMouse(); // Ignore the mouse
            if (!ready)
            {
                if (selectingShip)
                {
                    if (InputManager.isCombinedRightCombinedStickPressed(player.PlayerNumber))
                    {
                        index = (index + 1) % getSpawnShips().Count;
                        while (getSpawnShips()[index].ShipTeam != player.CurrentTeam
                        && index < getSpawnShips().Count) // They must have a ship, but just in case
                        {
                            index = ((index + 1) % getSpawnShips().Count);
                        }
                    }
                    else if (InputManager.isCombinedLeftCombinedStickPressed(player.PlayerNumber))
                    {
                        index = (getSpawnShips().Count + index - 1) % getSpawnShips().Count;
                        while (getSpawnShips()[index].ShipTeam != player.CurrentTeam
                        && index < getSpawnShips().Count) // They must have a ship, but just in case
                        {
                            index = ((getSpawnShips().Count + index - 1) % getSpawnShips().Count);
                        }
                    }
                    else { }
                }
                else
                {
                    if (InputManager.isCombinedUpCombinedStickPressed(player.PlayerNumber) ||
                        InputManager.isCombinedLeftCombinedStickPressed(player.PlayerNumber))
                    {
                        switch (currentShip)
                        {
                            case ShipType.AssaultFighter:
                                currentShip = ShipType.Interceptor;
                                break;
                            case ShipType.Interceptor:
                                currentShip = ShipType.Bomber;
                                break;
                            case ShipType.Bomber:
                                currentShip = ShipType.AssaultFighter;
                                break;
                        }
                    }
                    else if (InputManager.isCombinedDownCombinedStickPressed(player.PlayerNumber) ||
                        InputManager.isCombinedRightCombinedStickPressed(player.PlayerNumber))
                    {
                        switch (currentShip)
                        {
                            case ShipType.AssaultFighter:
                                currentShip = ShipType.Bomber;
                                break;
                            case ShipType.Interceptor:
                                currentShip = ShipType.AssaultFighter;

                                break;
                            case ShipType.Bomber:
                                currentShip = ShipType.Interceptor;
                                break;
                        }
                    }
                    else { }
                }
            }
            else { }

            if (!ready && (InputManager.isCombinedConfirmPressed(player.PlayerNumber)
                || InputManager.isCombinedPrimaryFirePressed(player.PlayerNumber)))
            {
                if (selectingShip)
                {
                    selectingShip = false;
                    currentShip = ShipType.AssaultFighter;
                }
                else
                {
                    switch (currentShip)
                    {
                        case ShipType.AssaultFighter:
                            List<SpawnShip> temp = getSpawnShips();
                            if (getSpawnShips()[index].availableAssaultFightersCount() > 0)
                            {
                                player.PlayerShip = getSpawnShips()[index].assignPlayerAssaultFighter();
                                player.PlayerShip.MyPlayer = player;

                                Vector3 position = Vector3.Transform(PilotingPlayerState.defaultOffset, player.PlayerShip.Rotation) + player.PlayerShip.Position;
                                Vector3 target = Vector3.Transform(PilotingPlayerState.defaultLookAt, player.PlayerShip.Rotation) + player.PlayerShip.Position;
                                Vector3 up = Vector3.Transform(Vector3.Up, player.PlayerShip.Rotation);

                                player.PlayerCamera.setViewMatrix(position, target, up);

                                ready = true;
                            }
                            else { }
                            break;
                        case ShipType.Interceptor:
                            if (getSpawnShips()[index].availableInterceptorsCount() > 0)
                            {
                                player.PlayerShip = getSpawnShips()[index].assignPlayerInterceptor();
                                player.PlayerShip.MyPlayer = player;

                                Vector3 position = Vector3.Transform(PilotingPlayerState.defaultOffset, player.PlayerShip.Rotation) + player.PlayerShip.Position;
                                Vector3 target = Vector3.Transform(PilotingPlayerState.defaultLookAt, player.PlayerShip.Rotation) + player.PlayerShip.Position;
                                Vector3 up = Vector3.Transform(Vector3.Up, player.PlayerShip.Rotation);

                                player.PlayerCamera.setViewMatrix(position, target, up);

                                ready = true;
                            }
                            else { }
                            break;
                        case ShipType.Bomber:
                            if (getSpawnShips()[index].availableBombersCount() > 0)
                            {
                                player.PlayerShip = getSpawnShips()[index].assignPlayerBomber();
                                player.PlayerShip.MyPlayer = player;

                                Vector3 position = Vector3.Transform(PilotingPlayerState.defaultOffset, player.PlayerShip.Rotation) + player.PlayerShip.Position;
                                Vector3 target = Vector3.Transform(PilotingPlayerState.defaultLookAt, player.PlayerShip.Rotation) + player.PlayerShip.Position;
                                Vector3 up = Vector3.Transform(Vector3.Up, player.PlayerShip.Rotation);

                                player.PlayerCamera.setViewMatrix(position, target, up);

                                ready = true;
                            }
                            else { }

                            if (player.PlayerShip != null)
                            {
                                player.PlayerShip.MyPlayer = player;
                            }
                            else { }
                            break;
                    }
                }
            }
            else { }
            
            if (InputManager.isCombinedUnconfirmPressed(player.PlayerNumber))
            {
                if (ready)
                {
                    player.PlayerShip.PlayerControlled = false;
                    player.PlayerShip = null;
                    ready = false;
                }
                else if (!selectingShip)
                {
                    selectingShip = true;
                    currentShip = 0;
                }
            }
            else { }

            if (InputManager.isCombinedToggleObjectivesPressed(player.PlayerNumber))
            {
                player.ObjectivesDrawActive = !player.ObjectivesDrawActive;
            }
            else { }

            if (InputManager.isCombinedPausePressed(player.PlayerNumber))
            {
                player.pause();
            }
            else { }
        }

        private List<SpawnShip> getSpawnShips()
        {
            return (((GameScene)Space394Game.GameInstance.CurrentScene).getSpawnShips());
        }

        public override void OnExit()
        {

        }
    }
}
