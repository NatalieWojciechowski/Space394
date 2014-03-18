using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Space394.SceneObjects;
using Space394.Scenes;
using Space394.Collision;

namespace Space394.PlayerStates
{
    class PilotingPlayerState : PlayerState
    {
        private bool camPosSet = false;

        private Vector3 facing = Vector3.Forward;
        public Vector3 Facing
        {
            get { return facing; }
            set { facing = value; }
        }

        private Fighter playerShip;

        private Vector3 playerMovement;

        private Vector3 cameraPositionOffset;
        private Vector3 cameraLookAt;

        private PlayerState nextState;

        private const int LEAVING_RADIUS = 45000;
        private CollisionSphere leavingSphere;

        private float cameraLock = 0;
        private const float CAMERA_LOCK = 1.35f;
        private Vector3 lockPosition;
        private Vector3 lockUp;

        private float bounceZ = 10.1f;

        private Vector3 bounceBackOffset;

        public static Vector3 defaultOffset = new Vector3(0, 15, -65);
        public static Vector3 defaultLookAt = new Vector3(0, 0, 1000000);

        public PilotingPlayerState(Player _player)
        {
            LogCat.updateValue("PlayerState", "Piloting");

            player = _player;
            playerShip = _player.PlayerShip;
            playerShip.DustGenerator.Active = true;

            leavingSphere = new CollisionSphere(Vector3.Zero, LEAVING_RADIUS);

            InputManager.bindMouse();

            player.PlayerHUDActive = true;

            bounceBackOffset = new Vector3(0, 15, -150);
            cameraPositionOffset = defaultOffset;
            cameraLookAt = defaultLookAt;

            setViewNoMove();
        }

        public override void Update(float deltaTime)
        {
            if (playerShip != null)
            {
                if (playerShip.JustReflected)
                {
                    lockCamera();
                }
                else { }

                // Init happens before positions are set in the game scene
                if (camPosSet == false)
                {
                    player.PlayerCamera.playerInit(new Vector3(0.0f, 0.0f, 0.15f), player);
                    setViewNoMove();
                    camPosSet = true;
                }

                if (!player.IsPaused)
                {
                    ProcessInput(deltaTime);
                }
                else { }

                if (playerShip.Health <= 0)
                {
                    StateComplete = true;
                    if (((GameScene)Space394Game.GameInstance.CurrentScene).CurrentState ==
                        GameScene.state.victory)
                    {
                        nextState = new ScoreboardPlayerState(player);
                    }
                    else
                    {
                        nextState = new DyingPlayerState(player);
                    }
                }
                else { }

                if (playerShip.Health > 0 && !((CollisionSphere)playerShip.CollisionBase).isCollidingSq(leavingSphere))
                {
                    StateComplete = true;
                    nextState = new TurningAroundPlayerState(player);
                }
                else { }

                cameraLock -= deltaTime;
                if (cameraLock <= 0)
                {
                    if (cameraPositionOffset != defaultOffset)
                    {
                        cameraPositionOffset = defaultOffset;
                    }
                    else { }
                }
                else 
                {
                    cameraPositionOffset.Z += bounceZ * deltaTime;
                }

                setView(deltaTime);
            }
            else { } // No ship
        }

        public override void Draw(PlayerCamera camera)
        {
            player.PlayerHUD.DrawTray(camera);
            player.PlayerHUD.DrawShipHUD(camera);

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

        public void lockCamera()
        {
            lockPosition = Vector3.Transform(cameraPositionOffset, playerShip.PreviousRotation) + playerShip.PreviousPosition;
            lockUp = Vector3.Transform(Vector3.Up, playerShip.PreviousRotation);
            cameraPositionOffset = bounceBackOffset;
            cameraLock = CAMERA_LOCK;
        }

        public void setView(float deltaTime)
        {
            Vector3 position = Vector3.Transform(cameraPositionOffset, playerShip.Rotation) + playerShip.Position;
            Vector3 target = Vector3.Transform(cameraLookAt, playerShip.Rotation) + playerShip.Position;
            Vector3 up = Vector3.Transform(Vector3.Up, playerShip.Rotation);

            player.PlayerCamera.springToView(position, target, up, (playerShip.Position - playerShip.PreviousPosition) / deltaTime, playerShip.Rotation, deltaTime);
        }

        public void setViewNoMove()
        {
            Vector3 position = Vector3.Transform(cameraPositionOffset, playerShip.Rotation) + playerShip.Position;
            Vector3 target = Vector3.Transform(cameraLookAt, playerShip.Rotation) + playerShip.Position;
            Vector3 up = Vector3.Transform(Vector3.Up, playerShip.Rotation);

            player.PlayerCamera.unsetVelSet();

            player.PlayerCamera.setViewMatrix(position, target, up);
        }

        public override PlayerState getNextState(Player _player)
        {
            OnExit();
            return (nextState);
        }

        public override void ProcessInput() { } // Use the deltaTime one instead

        public void ProcessInput(float deltaTime)
        {
            playerMovement = Vector3.Zero;

            if (InputManager.isCombinedLeftBumperDown(player.PlayerNumber)
                || InputManager.isCombinedLeftRightStickDown(player.PlayerNumber))
            {
                rollShip(InputManager.RightStickXDegree(player.PlayerNumber), deltaTime);
            }
            else if (InputManager.isCombinedRightBumperDown(player.PlayerNumber)
                || InputManager.isCombinedRightRightStickDown(player.PlayerNumber))
            {
                rollShip(InputManager.RightStickXDegree(player.PlayerNumber), deltaTime);
            }

            // Inverted
            if (InputManager.isCombinedUpLeftStickDown(player.PlayerNumber))
            {
                pitchShip(InputManager.LeftStickYDegree(player.PlayerNumber), deltaTime);
            }
            else if (InputManager.isCombinedDownLeftStickDown(player.PlayerNumber))
            {
                pitchShip(InputManager.LeftStickYDegree(player.PlayerNumber), deltaTime);
            }

            // Also inverted to be more predictable
            if (InputManager.isCombinedRightLeftStickDown(player.PlayerNumber))
            {
                yawShip(-InputManager.LeftStickXDegree(player.PlayerNumber), deltaTime);
            }
            else if (InputManager.isCombinedLeftLeftStickDown(player.PlayerNumber))
            {
                yawShip(-InputManager.LeftStickXDegree(player.PlayerNumber), deltaTime);
            }

            if (InputManager.isCombinedPrimaryFireDown(player.PlayerNumber))
            {
                playerShip.firePrimary();
            }
            else { }

            if (InputManager.isCombinedSecondaryFireDown(player.PlayerNumber))
            {
                playerShip.fireSecondary();
            }
            else { }

            if (InputManager.isCombinedBrakeDown(player.PlayerNumber))
            {
                //playerShip.setZSpeed(playerShip.getMinZSpeed());
                playerShip.decelerate(deltaTime);
            }
            else if (InputManager.isCombinedBoostDown(player.PlayerNumber))
            {
                //playerShip.setZSpeed(playerShip.getMaxZSpeed());
                playerShip.accelerate(deltaTime);
            }
            else
            {
                //playerShip.setZSpeed(playerShip.getNormalZSpeed());
            }

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

            InputManager.bindMouse();
        }

        private void rollShip(float degree, float deltaTime)
        {
            playerShip.rollShip(degree, deltaTime);
        }

        private void pitchShip(float degree, float deltaTime)
        {
            playerShip.pitchShip(degree, deltaTime);
        }
        
        private void yawShip(float degree, float deltaTime)
        {
            playerShip.yawShip(degree, deltaTime);
        }

        public override void OnExit()
        {
            player.PlayerHUDActive = false;
            playerShip.CollisionBase.Active = false;
        }
    }
}
