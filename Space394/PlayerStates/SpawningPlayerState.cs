using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Space394.Collision;
using Space394.Scenes;
using Space394.SceneObjects;

namespace Space394.PlayerStates
{
    public class SpawningPlayerState : PlayerState
    {
        private float spawnTimer;

        private Vector3 cameraPositionOffset;
        private Vector3 cameraLookAt;

        public SpawningPlayerState(Player _player)
        {
            LogCat.updateValue("PlayerState", "Spawning");

            spawnTimer = 0.0f;
            player = _player;
            player.PlayerShip.Active = true;
            player.IsActive = true;

            if (player.PlayerShip.CollisionBase != null)
            {
                player.PlayerShip.CollisionBase.Active = false;
            }
            else { }

            cameraPositionOffset = new Vector3(0, 15, -50);
            cameraLookAt = new Vector3(0, 0, 1000000);

            ((GameScene)Space394Game.GameInstance.CurrentScene).addFighterShip(player.PlayerShip);
            player.PlayerHUDActive = true;
        }

        public override void Update(float deltaTime)
        {
            if (spawnTimer <= 0.0f)
            {
                StateComplete = true;
            }
            else
            {
                // Update while spawning out of the ship
                spawnTimer -= deltaTime;
                if (player.PlayerShip.Velocity == Vector3.Zero)
                {
                    player.PlayerShip.Velocity = Vector3.Transform(Vector3.Backward * player.PlayerShip.ZSpeed, 
                        player.PlayerShip.Rotation);
                }
                player.PlayerShip.Velocity = 
                    player.PlayerShip.Velocity + 
                    (player.PlayerShip.Acceleration * deltaTime);
                player.PlayerShip.Position =
                    player.PlayerShip.Position + (
                    player.PlayerShip.Velocity * deltaTime);

                Vector3 position = Vector3.Transform(cameraPositionOffset, player.PlayerShip.Rotation) + player.PlayerShip.Position;
                Vector3 target = Vector3.Transform(cameraLookAt, player.PlayerShip.Rotation) + player.PlayerShip.Position;
                Vector3 up = Vector3.Transform(Vector3.Up, player.PlayerShip.Rotation);

                player.PlayerCamera.setViewMatrix(position, target, up);
            }
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

        public override PlayerState getNextState(Player _player)
        {
            OnExit();
            return (new PilotingPlayerState(_player));
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

        public override void OnExit()
        {
            player.PlayerHUDActive = false;
            player.PlayerShip.CollisionBase.Active = true;
            player.PlayerShip.Attackable = true;
        }
    }
}
