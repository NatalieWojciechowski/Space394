using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Space394.Scenes;
using Microsoft.Xna.Framework.Graphics;
using Space394.SceneObjects;

namespace Space394.PlayerStates
{
    public class ScoreboardPlayerState : PlayerState
    {
        private Vector3 cameraPosition;
        private Vector3 cameraView;
        private Vector3 cameraUp;

        private AutoTexture2D victoryTexture;

        /*private float victoryTTL;
        private const float VICTORY_TTL = 3.0f;

        private Vector2 pressStartPosition;
        private AutoTexture2D pressStartTexture;*/

        public ScoreboardPlayerState(Player _player)
        {
            player = _player;

            player.PlayerShip = null;

            cameraPosition = new Vector3(0, 75000, 0);
            cameraView = new Vector3(0, 0, 0);
            cameraUp = new Vector3(0, 0, 1);

            if (player.TeamNotSet)
            {
                if (((GameScene)Space394Game.GameInstance.CurrentScene).getSpawnPointsRemaining(SceneObjects.Ship.Team.Halk) <= 0)
                {
                    victoryTexture = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/victory_esxolus"), Vector2.Zero);
                }
                else
                {
                    victoryTexture = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/victory_halk"), Vector2.Zero);
                }
            }
            else if (player.CurrentTeam == SceneObjects.Ship.Team.Esxolus)
            {
                if (((GameScene)Space394Game.GameInstance.CurrentScene).getSpawnPointsRemaining(_player.CurrentTeam) <= 0)
                {
                    victoryTexture = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/defeat_esxolus"), Vector2.Zero);
                }
                else
                {
                    victoryTexture = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/victory_esxolus"), Vector2.Zero);
                }
            }
            else
            {
                if (((GameScene)Space394Game.GameInstance.CurrentScene).getSpawnPointsRemaining(_player.CurrentTeam) <= 0)
                {
                    victoryTexture = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/defeat_halk"), Vector2.Zero);
                }
                else
                {
                    victoryTexture = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/victory_halk"), Vector2.Zero);
                }
            }

            player.PlayerCamera.setViewMatrix(cameraPosition, cameraView, cameraUp);

            LogCat.updateValue("PlayerState", "Scoreboard");
        }

        public override void Update(float deltaTime)
        {
            player.PlayerCamera.setViewMatrix(cameraPosition, cameraView, cameraUp);
        }

        public override void Draw(PlayerCamera camera)
        {
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

            SpriteBatch spriteBatch = Space394Game.GameInstance.SpriteBatch;
            spriteBatch.Begin();
            victoryTexture.DrawAlreadyBegunMaintainRatio(camera);
            spriteBatch.End();
        }

        public override PlayerState getNextState(Player _player)
        {
            OnExit();
            return (new TeamSelectPlayerState(_player));
        }

        public override void ProcessInput()
        {
            
        } // End process input

        public override void OnExit()
        {
            
        }
    }
}
