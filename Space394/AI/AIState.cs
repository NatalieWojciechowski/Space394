using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Space394.AIControl
{
    public abstract class AIState
    {
        public enum state
        {
            WaitingToSpawn,
            Spawning,
            Wandering,
            Pursuit,
            EngageBattleship,
            Retreat,
            Dying
        }

        protected state currentState;
        public state CurrentState
        {
            get { return currentState; }
        }

        protected AI ai;

        protected bool stateComplete = false;
        public bool StateComplete
        {
            get { return stateComplete; }
            set { stateComplete = value; }
        }

        public abstract AIState getNextState(AI _ai);

        protected AIState nextState;

        public AIState(AI _ai)
        {
            stateComplete = false;
            ai = _ai;
        }

        public abstract void Update(float deltaTime);

        public abstract AIState recycleState();
    }
}
