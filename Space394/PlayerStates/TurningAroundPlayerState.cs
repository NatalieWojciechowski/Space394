using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Space394.Collision;
using Microsoft.Xna.Framework.Graphics;
using Space394.Scenes;
using Space394.SceneObjects;

namespace Space394.PlayerStates
{
    public class TurningAroundPlayerState : PlayerState
    {
        private CollisionSphere reentrySphere;

        private const int REENTRY_RADIUS = 42500;

        private Vector3 cameraPositionOffset;
        private Vector3 cameraLookAt;

        private Vector2 returningGraphicPosition;
        private AutoTexture2D returningGraphic;

        private bool alphaIn;
        private float alpha;
        private const float ALPHA_RATE = 3.0f;

        private const float SWITCH_TIMER = 1.0f;
        private float switchedTimer;

        public TurningAroundPlayerState(Player _player)
        {
            LogCat.updateValue("PlayerState", "TurningAround");

            player = _player;

            player.PlayerShip.CollisionBase.Active = true;

            player.PlayerHUDActive = true;

            alphaIn = true;
            alpha = 0.0f;

            reentrySphere = new CollisionSphere(Vector3.Zero, REENTRY_RADIUS);
            reentrySphere.Active = true;

            cameraPositionOffset = new Vector3(0, 15, -50);
            cameraLookAt = new Vector3(0, 0, 1000000);

            Texture2D graphic = ContentLoadManager.loadTexture("Textures/ReturningToBattleFieldMessage");
            returningGraphicPosition = new Vector2(400, 100);
            returningGraphicPosition.X -= graphic.Width / 2;
            returningGraphic = new AutoTexture2D(graphic, returningGraphicPosition);
        }

        public override PlayerState getNextState(Player _player)
        {
            OnExit();
            return new PilotingPlayerState(_player);
        }

        public override void OnExit()
        {
            player.PlayerHUDActive = false;
        }

        public override void ProcessInput()
        {
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

        public override void Update(float deltaTime)
        {
            switchedTimer -= deltaTime;
            if (switchedTimer <= 0)
            {
                if (alphaIn)
                {
                    alpha += ALPHA_RATE * deltaTime;
                    if (alpha >= 1.0f)
                    {
                        alphaIn = false;
                        alpha = 1.0f;
                        switchedTimer = SWITCH_TIMER;
                    }
                    else { }
                }
                else
                {
                    alpha -= ALPHA_RATE * deltaTime;
                    if (alpha <= 0.0f)
                    {
                        alphaIn = true;
                        alpha = 0.0f;
                        switchedTimer = SWITCH_TIMER;
                    }
                    else { }
                }
            }
            else { }

            player.PlayerShip.CollisionBase.Active = true;

            Vector3 targetPosition = Vector3.Zero;

            // the new forward vector, so the avatar faces the target
            Vector3 newForward = Vector3.Normalize(player.PlayerShip.Position - targetPosition);

            // if too close, offset to avoid
            if (((CollisionSphere)player.PlayerShip.CollisionBase).isCollidingSq(reentrySphere))
            {
                StateComplete = true;
                //nextState = new AIRetreatState(ai, target, fleeCounter);
            }
            else { }

            // calc the rotation so the avatar faces the target
            player.PlayerShip.Rotation = player.PlayerShip.AdjustRotation(Vector3.Forward, newForward, Vector3.Up, deltaTime);

            //player.PlayerShip.setZSpeed(player.PlayerShip.getMaxZSpeed());

            player.PlayerShip.updateVelocity();

            setView();
        }

        public void setView()
        {
            Vector3 position = Vector3.Transform(cameraPositionOffset, player.PlayerShip.Rotation) + player.PlayerShip.Position;
            Vector3 target = Vector3.Transform(cameraLookAt, player.PlayerShip.Rotation) + player.PlayerShip.Position;
            Vector3 up = Vector3.Transform(Vector3.Up, player.PlayerShip.Rotation);

            player.PlayerCamera.setViewMatrix(position, target, up);
        }

        public override void Draw(PlayerCamera camera)
        {
            player.PlayerHUD.DrawTray(camera);
            player.PlayerHUD.DrawShipHUD(camera);
            LogCat.updateValue("Alpha", ""+alpha);
            returningGraphic.Color = new Color(1.0f, 1.0f, 1.0f, alpha);
            returningGraphic.Draw(camera);
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
    }
}
