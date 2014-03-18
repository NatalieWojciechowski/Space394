using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Space394.SceneObjects;
using System.Collections;
using Space394.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Space394.PlayerStates;
using Space394.Events;

namespace Space394
{
    public class Player
    {
        public enum ControllerIndex
        {
            One,
            Two,
            Three,
            Four,
            Keyboard
        };

        private ControllerIndex number;
        public ControllerIndex PlayerNumber
        {
            get { return number; }
        }

        private int screenIndex = -1;
        public int ScreenIndex
        {
            get { return screenIndex; }
            set { screenIndex = value; }
        }

        private bool keyboardControlled;
        public bool KeyboardControlled
        {
            get { return keyboardControlled; }
        }

        private Ship.Team currentTeam;
        public Ship.Team CurrentTeam
        {
            get { return currentTeam; }
            set { currentTeam = value; }
        }

        private PlayerState currentState;
        public PlayerState CurrentState
        {
            get { return currentState; }
            set 
            {
                currentState.OnExit();
                currentState = value; 
            }
        }
        public PlayerState nextState() { currentState = currentState.getNextState(this); return currentState; }

        private Fighter playerShip = null;
        public Fighter PlayerShip
        {
            get { return playerShip; }
            set { playerShip = value; }
        }

        private PlayerCamera playerCamera;
        public PlayerCamera PlayerCamera
        {
            get { return playerCamera; }
        }

        private PlayerHUD playerHUD;
        public PlayerHUD PlayerHUD
        {
            get { return playerHUD; }
        }

        private bool teamNotSet;
        public bool TeamNotSet
        {
            get { return teamNotSet; }
            set { teamNotSet = value; }
        }

        private bool playerHUDActive;
        public bool PlayerHUDActive
        {
            get { return playerHUDActive; }
            set { playerHUDActive = value; }
        }

        private bool objectivesDrawActive;
        public bool ObjectivesDrawActive
        {
            get { return objectivesDrawActive; }
            set { objectivesDrawActive = value; }
        }

        private bool justUnpaused = false;
        private bool isPaused;
        public bool IsPaused
        {
            get { return isPaused; }
            set 
            {
                if (!value)
                {
                    unpause();
                }
                else
                {
                    pause();
                }
            }
        }
        public bool pause() 
        {
            if (!justUnpaused)
            {
                isPaused = true;
            }
            else 
            {
                justUnpaused = false; // We can probably assume this
            }
            return (isPaused); 
        }
        public bool unpause() 
        {
            justUnpaused = true;
            return (isPaused = false); 
        }
        private PausedPlayerState pausedState;

        private bool isActive = false;
        public bool IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }

        public Player(ControllerIndex _number)
        {
            justUnpaused = false;
            isPaused = false;
            teamNotSet = true;
            number = _number;
            if (_number != ControllerIndex.Keyboard)
            {
                keyboardControlled = false;
            }
            else
            {
                keyboardControlled = true;
            }
            playerCamera = null;
            currentState = new MenuPlayerState(this);
            playerHUD = new PlayerHUD(this);
            playerHUDActive = false;
            objectivesDrawActive = true;
            pausedState = new PausedPlayerState(this);
        }

        public void setPlayerCamera(Scene scene)
        {
            playerCamera = new PlayerCamera(scene,Vector3.Zero, new Quaternion(), Space394Game.GameInstance.AspectRatio);
            PlayerCamera.playerInit(new Vector3(0.0f, 0.0f, 0.5f), this);
        }

        public void Update(float deltaTime)
        {
            if (!isPaused)
            {
                currentState.ProcessInput();
                justUnpaused = false;
            }
            else
            {
                pausedState.ProcessInput();
            }

            currentState.Update(deltaTime);
            if (isPaused)
            {
                pausedState.Update(deltaTime);
            }
            else { }

            LogCat.updateValue("Player Paused", ""+isPaused);

            if (playerCamera != null)
            {
                PlayerCamera.Update(deltaTime);
                SoundManager.setListenerLocation(PlayerCamera.Position);
            }
            else { }

            if (playerHUDActive)
            {
                playerHUD.Update(deltaTime);
            }
            else { }

            if (currentState.StateComplete)
            {
                currentState = currentState.getNextState(this);
            }
            else { }
        }

        public void Draw(Dictionary<int, SceneObject> drawables)
        {
            if (isActive && playerCamera != null)
            {
                playerCamera.Draw(drawables);
                if (playerHUDActive)
                {
                    playerHUD.Draw(playerCamera);
                }
                else { }
                currentState.Draw(playerCamera);
                if (playerShip != null)
                {
                    Space394Game.GameInstance.GraphicsDevice.DepthStencilState = DepthStencilState.None;
                    playerShip.Draw(playerCamera);
                    Space394Game.GameInstance.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
                }
                else { }
                
                foreach (SceneObject item in Space394Game.GameInstance.CurrentScene.SceneObjects.Values)
                {
                    if (item is Fighter)
                    {
                        ((Fighter)item).DrawTrails(playerCamera);
                    }
                    else { }
                }
                if (isPaused)
                {
                    pausedState.Draw(playerCamera);
                }
                else { }
            }
            else { }
        }

        protected GraphicsDevice getGraphics()
        {
            return Space394Game.GameInstance.GraphicsDevice;
        }
    }
}
