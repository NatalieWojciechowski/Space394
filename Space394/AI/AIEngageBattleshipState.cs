using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Space394.Scenes;
using Space394.SceneObjects;
using Microsoft.Xna.Framework;
using Space394.Collision;
using BEPUphysics;
using BEPUphysics.Collidables.MobileCollidables;
using BEPUphysics.Collidables;
using BEPUphysics.NarrowPhaseSystems.Pairs;

namespace Space394.AIControl
{
    public class AIEngageBattleshipState : AIPilotingState
    {
        private Random random;
        private const float DEGREE_MOD = 3.0f;
        private Vector3 randomOffsetVector;
        private Vector3 offsetTargetPosition;

        private float fuzzyFireRate;
        private float fireTimer;

        private float fuzzySpecialRate;
        private float specialTimer;

        public AIEngageBattleshipState(AI _ai)
            : base(_ai)
        {
            currentState = state.EngageBattleship;
            random = new Random(System.DateTime.Now.Millisecond + (int)ai.Ship.UniqueId);
        }

        public override void Update(float deltaTime)
        {
            if (ai.aiShared.target != null)
            {
                Vector3 aiPosition = ai.Ship.Position;
                engageRetreatSphere.Position = aiPosition;

                // the new forward vector, so the avatar faces the target
                Vector3 newForward = Vector3.Normalize(aiPosition - offsetTargetPosition);

                // calc the rotation so the avatar faces the target
                ai.Ship.Rotation = ai.Ship.AdjustRotation(Vector3.Forward, newForward, Vector3.Up, deltaTime);
                
                fireTimer -= deltaTime;
                specialTimer -= deltaTime;

                #region individual ship distances
                // Since BattleShips do not move,
                // Do not fire unless close enough
                //if (/*base.*/targetDistSq <= ENGAGE_DISTANCE)
                //{
                /*                    if (ai.getShip() is Bomber)
                                    {
                                        if (ai.getShip().getTeam() == Ship.Team.Esxolus)
                                        {
                                            if (specialTimer <= 0 && targetDistSq <= 1000000f)//distDiff.Length() <= 1200)
                                            {
                                                ai.getShip().fireSecondary();
                                                specialTimer = fuzzySpecialRate;
                                            }
                                            else
                                            {
                                                if (fireTimer <= 0)
                                                {
                                                    ai.getShip().firePrimary();
                                                    fireTimer = fuzzyFireRate;
                                                }
                                                else { }
                                            }
                                        }
                                        else  // if( Halk (bomber) ) 
                                        {
                                            if (specialTimer <= 0 && targetDistSq <= 800000f)//distDiff.Length() <= 1200)
                                            {
                                                ai.getShip().fireSecondary();
                                                specialTimer = fuzzySpecialRate;
                                            }
                                            else
                                            {
                                                if (fireTimer <= 0)
                                                {
                                                    ai.getShip().firePrimary();
                                                    fireTimer = fuzzyFireRate;
                                                }
                                                else { }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (targetDistSq <= 1000000f)
                                        {
                                            if (specialTimer <= 0)
                                            {
                                                ai.getShip().fireSecondary();
                                                specialTimer = fuzzySpecialRate;
                                            }
                                            else
                                            {
                                                if (fireTimer <= 0)
                                                {
                                                    ai.getShip().firePrimary();
                                                    fireTimer = fuzzyFireRate;
                                                }
                                                else { }
                                            }
                                        }
                                        else
                                        {
                                            if (fireTimer <= 0)
                                            {
                                                ai.getShip().firePrimary();
                                                fireTimer = fuzzyFireRate;
                                            }
                                            else { }
                                        }
                                    }
                                    if (fireTimer <= 0)
                                    {
                                        ai.getShip().firePrimary();
                                        fireTimer = fuzzyFireRate;
                                    }
                                    else { }
                                }
                //            }
                                */
                #endregion

                if (specialTimer <= 0)
                {
                    ai.Ship.fireSecondary();
                    specialTimer = fuzzySpecialRate;
                }
                else
                {
                    if (fireTimer <= 0)
                    {
                        ai.Ship.firePrimary();
                        fireTimer = fuzzyFireRate;
                    }
                    else { }
                }
            }

            base.Update(deltaTime);
        }

        public override AIState getNextState(AI _ai)
        {
            AI.AI_Overmind.inEngageBattleship--;
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

        public Vector3 offsetPos(Vector3 _position ) //, Vector3 _distDiff)
        {
            return _position +( (/*_distDiff +*/ randomOffsetVector) * 20f );
        }

        public override AIState recycleState()
        {
            if (ai.CurrentState.CurrentState != state.EngageBattleship)
            {
                AI.AI_Overmind.inEngageBattleship++;
            }
            else { }

            StateComplete = false;
            nextState = null;

            if (ai.Ship.CollisionBase != null)
            {
                ai.Ship.CollisionBase.Active = true;
            }
            else { }

            fuzzyFireRate = FIRE_RATE + 1.0f * (1 + (float)random.NextDouble());
            fireTimer = fuzzyFireRate;

            fuzzySpecialRate = SPECIAL_RATE + (4 + (float)random.NextDouble()) * (1 + (float)random.NextDouble());
            specialTimer = fuzzySpecialRate;

            randomOffsetVector = getRandomDirection();

            if (ai.aiShared.target != null)
            {
                offsetTargetPosition = offsetPos(ai.aiShared.target.Position);
            }
            // Should never actually be implemented mid-game, only through the constructor
            else
            {
                offsetTargetPosition = ai.Ship.Velocity * -20;
            }

            return this;
        }
    }
}
