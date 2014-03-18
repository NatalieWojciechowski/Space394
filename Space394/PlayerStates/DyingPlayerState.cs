using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Space394.Scenes;
using Space394.SceneObjects;

namespace Space394.PlayerStates
{
    public class DyingPlayerState : PlayerState
    {
        private Explosion explosion;

        public DyingPlayerState(Player _player)
        {
            LogCat.updateValue("PlayerState", "Dying");
            player = _player;
            if (player.PlayerShip != null)
            {
                explosion = SceneObjectFactory.createExplosion(player.PlayerShip.Position, player.PlayerShip.Rotation);
                player.PlayerShip.Active = false;
                player.PlayerShip.MyPlayer = null;
                player.PlayerShip.PlayerControlled = false;
                player.PlayerShip.cleanParticleLists();
                ((GameScene)Space394Game.GameInstance.CurrentScene).removeFighterShip(player.PlayerShip);
            }
            else { }

            //cameraPositionOffset = new Vector3(0, 15, -50);
            //cameraLookAt = new Vector3(0, 0, 1000);
        }

        public override void Update(float deltaTime)
        {
            if (explosion == null || !explosion.Active)
            {
                player.PlayerShip.returnToSpawnShip();
                StateComplete = true;
            }
            else { }
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
            return (new SpawnSelectPlayerState(_player));
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
        }
    }
}
