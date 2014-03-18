using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Space394.Scenes;
using Space394.SceneObjects;
using Space394.Collision;

namespace Space394.AIControl
{
    public class AIWaitingToSpawnState : AIState
    {
        public AIWaitingToSpawnState(AI _ai)
            : base(_ai)
        {
            currentState = state.WaitingToSpawn;
            StateComplete = false;
//            recycleState();
        }

        public override void Update(float deltaTime)
        {
            if (ai.Ship.CollisionBase.Active)
            {
                ai.Ship.CollisionBase.Active = false;
            }

            if (Space394Game.GameInstance.CurrentScene is GameScene)
            {
                if (((GameScene)Space394Game.GameInstance.CurrentScene).WaveReleased
                    && ai.Ship.Active)
                {
                    nextState = ai.MySpawningState.recycleState();
                    StateComplete = true;
                }
                else { }
            }
            else { }
        }

        public override AIState getNextState(AI _ai)
        {
            AI.AI_Overmind.inWaitingToSpawn--;
            return (nextState);
        }

        public override AIState recycleState()
        {
            if (ai.CurrentState.CurrentState != state.WaitingToSpawn)
            {
                StateComplete = false;
                AI.AI_Overmind.inWaitingToSpawn++;
                //                currentState = state.WaitingToSpawn;

                ai.Ship.resetAttackerCount();
                if (ai.Ship.CollisionBase != null)
                {
                    ai.Ship.CollisionBase.Active = false;
                }
            }
            else { }

            return this;
        }
    }
}
