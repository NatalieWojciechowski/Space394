using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Space394.Scenes;
using Space394.SceneObjects;

namespace Space394.AIControl
{
    public class AIDyingState : AIState
    {
        private Explosion explosion;

        public AIDyingState(AI _ai)
            : base(_ai)
        {
            currentState = state.Dying;
            StateComplete = false;    
        }

        public override void Update(float deltaTime)
        {
            if (explosion == null || !explosion.Active)
            {
                ai.Ship.returnToSpawnShip();
                nextState = ai.MyWaitingToSpawnState.recycleState();
                StateComplete = true;
            }
            else { }
        }

        public override AIState getNextState(AI _ai)
        {
            AI.AI_Overmind.inDying--;
            return (nextState);
        }

        public override AIState recycleState()
        {
            if (ai.CurrentState.CurrentState != state.Dying)
            {
                StateComplete = false;
                AI.AI_Overmind.inDying++;

                if (ai.aiShared.target != null)
                {
                    ai.aiShared.target.AttackerCount = -1;
                    ai.aiShared.target = null;
                }
                else { }
                ai.Ship.resetAttackerCount();
                ai.Ship.Active = false;
                ai.Ship.cleanParticleLists();

                if (explosion == null)
                {
                    explosion = SceneObjectFactory.createExplosion(ai.Ship.Position, ai.Ship.Rotation);
                }
                else { }
                ((GameScene)Space394Game.GameInstance.CurrentScene).removeFighterShip(ai.Ship);
            }
            else { }

            return this;
        }
    }
}
