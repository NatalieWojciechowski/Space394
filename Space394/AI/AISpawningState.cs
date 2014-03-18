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
    public class AISpawningState : AIState
    {
        private const float SPAWN_TIMER = 7.5f;
        private float spawnTimer;

        private Random random;

        private const float WANDER_RADIUS = 25.0f;
        private const float JITTER_RADIUS = 5.0f;
        private const float MAX_SPEED = 30.0f;

        public AISpawningState(AI _ai)
            : base(_ai)
        {
            currentState = state.Spawning;
            StateComplete = false;
        }

        public override void Update(float deltaTime)
        {
            /*
             * Different from the Player in that the player does not reach 
             * SpawingPlayerState until after the wave is released.
             */
            if (ai.Ship.CollisionBase.Active)
            {
                ai.Ship.CollisionBase.Active = false;
            }

            if (spawnTimer <= 0.0f)
            {
                StateComplete = true;
                ai.Ship.CollisionBase.Active = true;
                ai.Ship.Attackable = true;
                nextState = ((AIWanderState)ai.MyWanderState).recycleState();
            }
            else
            {
                // Update while spawning out of the ship
                spawnTimer -= deltaTime;
            }

            if (ai.Ship.Health <= 0)
            {
                StateComplete = true;
                nextState = ((AIDyingState)ai.MyDyingState).recycleState();
            }
            else { }
        }

        public override AIState getNextState(AI _ai)
        {
            AI.AI_Overmind.inSpawning--;
            return nextState;
        }

        public override AIState recycleState()
        {
            if (ai.CurrentState.CurrentState != state.Spawning)
            {
                AI.AI_Overmind.inSpawning++;

                StateComplete = false;
                ai.Ship.Active = true;
                ai.Ship.Attackable = false;
                ai.Ship.resetAttackerCount();

                random = new Random(System.DateTime.Now.Millisecond + (int)ai.Ship.UniqueId);

                spawnTimer = SPAWN_TIMER + (float)random.NextDouble() + (float)random.NextDouble();

                if (ai.Ship.CollisionBase != null)
                {
                    ai.Ship.CollisionBase.Active = true;
                }
                else { }

                if (ai.Ship != null)
                {
                    ((GameScene)Space394Game.GameInstance.CurrentScene).addFighterShip(ai.Ship);
                }
                else { }
            }
            else { }

            return this;
        }
    }
}
