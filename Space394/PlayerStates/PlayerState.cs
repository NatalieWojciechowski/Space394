using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Space394.PlayerStates
{
    public abstract class PlayerState
    {
        public enum state
        {
            menu,
            login,
            team_select,
            spawn_select,
            spawning,
            piloting,
            spectator,
            dying
        };

        protected Player player;

        private bool paused = false;
        public bool Paused
        {
            get { return paused; }
            set { paused = value; }
        }

        private bool stateComplete = false;
        public bool StateComplete
        {
            get { return stateComplete; }
            set { stateComplete = value; }
        }

        protected state currentState;
        public state CurrentState
        {
            get { return currentState; }
        }

        public abstract PlayerState getNextState(Player _player);

        public abstract void OnExit();

        public abstract void ProcessInput();

        public abstract void Update(float deltaTime);

        public abstract void Draw(PlayerCamera camera);
    }
}
