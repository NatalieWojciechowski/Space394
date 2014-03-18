using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Space394.Scenes;
using Space394.SceneObjects;
using Microsoft.Xna.Framework;

namespace Space394.PlayerStates
{
    public class SpectatorPlayerState : PlayerState
    {
        private Vector3 cameraPosition;
        private Vector3 cameraView;
        private Vector3 cameraUp;

        private Vector3 nullPosition = new Vector3(0, 75000, 0);
        private Vector3 nullView = new Vector3(0, 0, 0);
        private Vector3 nullUp = new Vector3(0, 0, 1);

        private Vector3 cameraPositionOffset;
        private Vector3 cameraLookAt;

        private Fighter follow;
        private int index;

        private Ship.Team currentTeam;

        private AutoTexture2D controls;

        public SpectatorPlayerState(Player _player)
        {
            player = _player;

            cameraPosition = nullPosition;
            cameraView = nullView;
            cameraUp = nullUp;

            index = 0;

            controls = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/spectator_mode"), new Vector2(27, 433));

            cameraPositionOffset = new Vector3(0, 15, -50);
            cameraLookAt = new Vector3(0, 0, 1000);

            // Find a team to start on
            Random random = new Random(System.DateTime.Now.Millisecond);
            if (random.Next() % 2 == 0)
            {
                currentTeam = Ship.Team.Esxolus;
            }
            else 
            { 
                currentTeam = Ship.Team.Halk;
            }

            follow = null;
        }

        public override PlayerState getNextState(Player _player)
        {
            OnExit();
            return (new ScoreboardPlayerState(player));
        }

        public override void OnExit()
        {
            player.CurrentTeam = ((GameScene)Space394Game.GameInstance.CurrentScene).Victor;
        }

        public override void ProcessInput()
        {
            if (InputManager.isCombinedPrimaryFirePressed(player.PlayerNumber))
            {
                switch (currentTeam)
                {
                    case Ship.Team.Esxolus:
                        currentTeam = Ship.Team.Halk;
                        break;
                    case Ship.Team.Halk:
                        currentTeam = Ship.Team.Esxolus;
                        break;
                }
                follow = null;
                index = 0;
            }
            else { }

            if (InputManager.isCombinedLeftBumperPressed(player.PlayerNumber)
                || InputManager.isCombinedRightBumperPressed(player.PlayerNumber))
            {
                follow = null;
                List<Fighter> fighters = null;
                switch (currentTeam)
                {
                    case Ship.Team.Esxolus:
                        fighters = ((GameScene)Space394Game.GameInstance.CurrentScene).EsxolusFighters;
                        break;
                    case Ship.Team.Halk:
                        fighters = ((GameScene)Space394Game.GameInstance.CurrentScene).HalkFighters;
                        break;
                }
                if (InputManager.isCombinedLeftBumperPressed(player.PlayerNumber))
                {
                    if (index < 0 || index >= fighters.Count)
                    {
                        index = fighters.Count - 1;
                        follow = null;
                    }
                    else { }
                    while (fighters != null && follow == null && fighters.Count > 0 && index >= 0)
                    {
                        if (fighters[index].Active)
                        {
                            follow = fighters[index];
                        }
                        else { }
                        index--;
                    }
                    if (index < 0 || index >= fighters.Count)
                    {
                        index = fighters.Count-1;
                        follow = null;
                    }
                    else { }
                }
                else if (InputManager.isCombinedRightBumperPressed(player.PlayerNumber))
                {
                    if (index < 0 || index >= fighters.Count)
                    {
                        index = fighters.Count - 1;
                        follow = null;
                    }
                    else { }
                    while (fighters != null && follow == null && fighters.Count > 0 && index < fighters.Count)
                    {
                        if (fighters[index].Active)
                        {
                            follow = fighters[index];
                        }
                        else { }
                        index++;
                    }
                    if (index < 0 || index >= fighters.Count)
                    {
                        index = 0;
                        follow = null;
                    }
                    else { }
                } else { }
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

        public override void Update(float deltaTime)
        {
            if (follow != null)
            {
                if (!follow.Active)
                {
                    follow = null;
                    List<Fighter> fighters = null;
                    switch (currentTeam)
                    {
                        case Ship.Team.Esxolus:
                            fighters = ((GameScene)Space394Game.GameInstance.CurrentScene).EsxolusFighters;
                            break;
                        case Ship.Team.Halk:
                            fighters = ((GameScene)Space394Game.GameInstance.CurrentScene).HalkFighters;
                            break;
                    }
                    while (fighters != null && follow == null && index < fighters.Count)
                    {
                        if (fighters[index].Active)
                        {
                            follow = fighters[index];
                        }
                        else { }
                        index++;
                    }
                }
                else 
                {
                    Vector3 position = Vector3.Transform(cameraPositionOffset, follow.PreviousRotation) + follow.PreviousPosition;
                    Vector3 target = Vector3.Transform(cameraLookAt, follow.PreviousRotation) + follow.PreviousPosition;
                    Vector3 up = Vector3.Transform(Vector3.Up, follow.PreviousRotation);

                    player.PlayerCamera.setViewMatrix(position, target, up);
#if DEBUG
                    LogCat.updateValue("Ship's State", "" + follow.MyAI.CurrentState.CurrentState);
#endif 
                }
            }
            else { }
        }

        public override void Draw(PlayerCamera camera)
        {
            player.PlayerHUD.DrawTray(camera);
            controls.DrawMaintainRatio(camera);
        }
    }
}
