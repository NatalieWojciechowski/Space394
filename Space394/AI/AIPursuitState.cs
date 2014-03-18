using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Space394.Scenes;
using Space394.SceneObjects;
using Microsoft.Xna.Framework;
using Space394.Collision;

namespace Space394.AIControl
{
    public class AIPursuitState : AIPilotingState
    {
        private Random random;
        private const float DEGREE_MOD = 3.0f;

        // Distances are checked with DistanceSquared
        private const float ENGAGE_DISTANCE = 15000000f;
        private float primaryRange;
        private float secondaryRange; 

        private float fireRate;
        private float fireTimer;

        private float specialRate;
        private float specialTimer;

        private const float DIRECTION_UPDATE_RATE = 0.025f;

        public AIPursuitState(AI _ai)
            : base(_ai)
        {
            currentState = state.Pursuit;
            random = new Random(System.DateTime.Now.Millisecond + (int)ai.Ship.UniqueId);
            // recycleState(null);
        }

        public AIPursuitState(AI _ai, Ship _target, int _fleeCounter)
            : this(_ai)
        {
            // recycleState(_target, fleeCounter, ai.getShip().getInterestTime());
        }

        public override void Update(float deltaTime)
        {
            pursuitRetreatSphere.Position = ai.Ship.Position;

            if (ai.aiShared.target != null)
            {
                // the new forward vector, so the avatar faces the target
                Vector3 predictionPosition = ai.Ship.Position - (ai.aiShared.target.Position + ((Fighter)ai.aiShared.target).Velocity * 0.05f);
                targetDistSq = Vector3.DistanceSquared(ai.Ship.Position, predictionPosition);
                Vector3 newForward = Vector3.Normalize(predictionPosition); // + ((Fighter)target).getVelocity()*0.02f));        // targetPosition);

                // calc the rotation so the avatar faces the target
                ai.Ship.Rotation = ai.Ship.AdjustRotation(Vector3.Forward, newForward, Vector3.Up, deltaTime);
                ai.Ship.updateVelocity();

                fireTimer -= deltaTime;
                specialTimer -= deltaTime;

                if (specialTimer <= 0 && targetDistSq < secondaryRange)
                {
                    ai.Ship.fireSecondary();
                    specialTimer = specialRate;
                }
                else if (fireTimer <= 0 && targetDistSq < primaryRange)
                {
                    ai.Ship.firePrimary();
                    fireTimer = fireRate;
                }
                else { }
            }
            else { }

            base.Update(deltaTime);
        }

        public override AIState getNextState(AI _ai)
        {
            AI.AI_Overmind.inPursuit--;
            return (nextState); // Already recycled
        }

        public Vector3 getRandomDirection()
        {
            return Vector3.Normalize(new Vector3(getRandSign() * ((float)random.NextDouble()), getRandSign() * ((float)random.NextDouble()), getRandSign() * ((float)random.NextDouble())));
        }

        public float getRandSign()
        {
            return 0.5f - (float)random.NextDouble();
        }

        public Vector3 offsetPos(Vector3 _position, Vector3 _distDiff)
        {
            return _position + (((Fighter)ai.aiShared.target).Velocity*0.01f);
//            return _position + (_distDiff * -0.01f);
        }

        public override AIState recycleState()
        {
            if (ai.CurrentState.CurrentState != state.Pursuit)
            {
                AI.AI_Overmind.inPursuit++;
            }
            else { }

            StateComplete = false;
            nextState = null;

            if (ai.Ship.CollisionBase != null)
            {
                ai.Ship.CollisionBase.Active = true;
            }
            else { }

            primaryRange = ai.Ship.PrimaryRange;
            secondaryRange = ai.Ship.SecondaryRange;

            fireRate = ai.Ship.FireRate + 0.55f * (1 + (float)random.NextDouble());
            fireTimer = fireRate;

            specialRate = ai.Ship.SpecialRate + 4.75f * (1 + (float)random.NextDouble());
            specialTimer = specialRate;

            return this;
        }
    }
}
