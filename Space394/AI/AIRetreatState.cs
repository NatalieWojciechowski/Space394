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
    public class AIRetreatState : AIState
    {
        private Ship target;
        private Fighter aiShip;

        private Random random;
        private const float DEGREE_MOD = 3.0f;

        private float RETREAT_TIME;
        private float retreatTimer;

        private Vector3 newForward;
        private Vector3 cardinal;

        private float fireRate;
        private float fireTimer;

        private float specialRate;
        private float specialTimer;

        public AIRetreatState(AI _ai)
            : base(_ai)
        {
            currentState = state.Retreat;
            aiShip = ai.Ship;
            random = new Random(System.DateTime.Now.Millisecond + (int)aiShip.UniqueId);
            cardinal = getCardinalOffest();
        }

        public override void Update(float deltaTime)
        {

            if (aiShip.Health <= 0)
            {
                StateComplete = true;
                nextState = ((AIDyingState)ai.MyDyingState).recycleState();

                if (target != null)
                {
                    target.AttackerCount = -1;
                    target = null;
                    ai.aiShared.target = null;
                }
            }
            else
            {
                if (target != null)
                {

                    retreatTimer -= deltaTime;
                    if (retreatTimer <= 0)
                    {
                        StateComplete = true;
                        if (ai.aiShared.fleeCounter <= 3)
                        {
                            ai.aiShared.fleeCounter++;
                            if (target is Fighter)
                            {
                                nextState = ((AIPursuitState)ai.MyPursuitState).recycleState();

                            }
                            else // else BattleShip
                            {
                                // interestTimeCopy was taken in as a copy of the ships current time left 
                                nextState = ((AIEngageBattleshipState)ai.MyEngageBattleshipState).recycleState();
                            }
                        }
                        // Give up on target
                        else
                        {
                            target.AttackerCount = -1;
                            nextState = ((AIWanderState)ai.MyWanderState).recycleState();
                            if (target is CapitalShip)
                            {
                                ((AIWanderState)ai.MyWanderState).EngageStateHalt = true;
                            }
                        }
                        retreatTimer = RETREAT_TIME;
                    }
                    else
                    {
                        fireTimer -= deltaTime;
                        specialTimer -= deltaTime;

                        if (specialTimer <= 0) //  && targetDistSq < secondaryRange)
                        {
                            aiShip.fireSecondary();
                            specialTimer = specialRate;
                        }
                        else if (fireTimer <= 0) // && targetDistSq < primaryRange)
                        {
                            aiShip.firePrimary();
                            fireTimer = fireRate;
                        }
                        else { }

                        aiShip.Rotation = aiShip.AdjustRotation(Vector3.Forward, newForward, Vector3.Up, deltaTime);
                        aiShip.updateVelocity();

                        float degree = (float)random.NextDouble() * DEGREE_MOD;

                        switch (random.Next() % 3)
                        {
                            case 0:
                                ai.Ship.yawShip(degree, deltaTime);
                                break;
                            case 1:
                                ai.Ship.pitchShip(degree, deltaTime);
                                break;
                            case 2:
                                ai.Ship.rollShip(degree, deltaTime);
                                break;
                        }
                    }
                }
            }
        }

        public override AIState getNextState(AI _ai)
        {
            AI.AI_Overmind.inRetreat--;
            return (nextState); // Already recycled
        }

        public float getRandSign()
        {
            return 0.5f - (float)random.NextDouble();
        }

        // Used to give a random direction for the ship to flee from the enemy
        public Vector3 getCardinalOffest()
        {
            int selection = random.Next(4);
            if (selection == 0)
            {
                return Vector3.Up;
            }
            else if (selection == 1)
            {
                return Vector3.Down;
            }
            else if (selection == 2)
            {
                return Vector3.Right;
            }
            else if (selection == 3)
            {
                return Vector3.Left;
            }
            else
            {
                return Vector3.Up;
            }
        }

        public Vector3 offsetDirection()
        {
            //Vector3 newDirection = target.getPosition() - ai.getShip().getPosition();

            //if (target is Fighter)
            //{
            return Vector3.Transform(cardinal, aiShip.Rotation) * 10000f;
            //}
            //else if (target is SpawnShip)
            //{
            //    return Vector3.Transform(Vector3.Up, ai.getShip().getRotation());
            //}
            //else
            //{
            //    return ai.getShip().getVelocity();
            //} 
        }

        public override AIState recycleState()
        {
            return recycleState(-1);
        }

        public AIState recycleState(float _timerMod = 2.5f)
        {
            if (ai.CurrentState.CurrentState != state.Retreat)
            {
                target = ai.aiShared.target;
                StateComplete = false;
                nextState = null;
                AI.AI_Overmind.inRetreat++;

                if (aiShip.CollisionBase != null)
                {
                    aiShip.CollisionBase.Active = true;
                }
                else { }

                RETREAT_TIME = _timerMod + (float)random.NextDouble();
                retreatTimer = RETREAT_TIME;

                fireRate = aiShip.FireRate + 1.5f * (1 + (float)random.NextDouble());
                fireTimer = fireRate;

                specialRate = aiShip.SpecialRate + 13.5f * (1 + (float)random.NextDouble());
                specialTimer = specialRate;
            }
            else { }

            // the new forward vector, so the avatar faces the target
            newForward = Vector3.Normalize(aiShip.Position - offsetDirection());

            return this;
        }
    }
}
