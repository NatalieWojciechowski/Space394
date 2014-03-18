using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Space394.SceneObjects;
using Space394.Scenes;
using Space394.Collision;

namespace Space394.AIControl
{
    public class AIWanderState : AIPilotingState
    {
        private Random random;
        private const float DEGREE_MOD = 3.5f;

        // How likely to pick a different goal
        // node than the closest. Adds randomness to AI.
        private float NODE_MOD;

        private List<Vector3> friendlyNodes;
        private List<Vector3> enemyNodes;
        private bool haveLists = false;

        // Used to determine target node
        private const float TOO_CLOSE           = 10000000f;
        private const float TOO_CLOSE_MID       = 15000000f;
        private const float TOO_CLOSE_FARTHER   = 20000000f;

        private const float DEFAULT_WANDER_TIMER = 1.0f;
        private float wanderTimer;
        public float getNewWanderTime() { return DEFAULT_WANDER_TIMER + 1.5f*(float)random.NextDouble(); }

        // How often do we update the rotations 
        private const float ROTATION_UPDATE_TIMER = 0.5f;
        private float rotationUpdateTimer;
        public float getNewRotationUpdateTime() { return (float)random.NextDouble(); }

        // How often do we check if we need to change goals?
        private const float GOAL_UPDATE_TIMER = 8f;
        private float goalUpdateTimer;
        public float getNewGoalUpdateTime() { return GOAL_UPDATE_TIMER + (float)random.NextDouble(); }

        private Vector3 goalPosition;

        public AIWanderState(AI _ai)
            : base(_ai)
        {
            currentState = state.Wandering;
            random = new Random(System.DateTime.Now.Millisecond + (int)ai.Ship.UniqueId);
        }

        public AIWanderState(AI _ai, float _timer)
            : this(_ai)
        {
            currentState = state.Wandering;
            random = new Random(System.DateTime.Now.Millisecond + (int)ai.Ship.UniqueId);
            rotationUpdateTimer = _timer;
        }

        public override void Update(float deltaTime)
        {
            goalUpdateTimer -= deltaTime;
            if (goalUpdateTimer < 0)
            {
                goalPosition = getNewGoalPosition();
                goalUpdateTimer = getNewGoalUpdateTime();
            }
            // Adjust the direction of the actual ship
            Vector3 newForward = Vector3.Normalize(ai.Ship.Position - goalPosition);

            ai.Ship.Rotation = ai.Ship.AdjustRotation(Vector3.Forward, newForward, Vector3.Up, deltaTime);
            ai.Ship.updateVelocity();

            rotationUpdateTimer -= deltaTime;
            if (rotationUpdateTimer <= 0)
            {
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
                rotationUpdateTimer = getNewRotationUpdateTime();
            }

            wanderTimer -= deltaTime;
            if (wanderTimer < 0)
            {
                base.Update(deltaTime);
            }
        }

        public override AIState getNextState(AI _ai)
        {
            haveLists = false; // get a new list when re-entering in case it has changed
            AI.AI_Overmind.inWander--;
            return (nextState); // Already recycled
        }

        public override AIState recycleState()
        {
            float updateTimer = getNewRotationUpdateTime();
            return recycleState(updateTimer);
        }

        public AIState recycleState(float _timer)
        {
            if (ai.CurrentState.CurrentState != state.Wandering)
            {
                AI.AI_Overmind.inWander++;

                StateComplete = false;
                nextState = null;

                if (ai.Ship.CollisionBase != null)
                {
                    ai.Ship.CollisionBase.Active = true;
                }
                else { }

                NODE_MOD = (float)random.NextDouble();

                rotationUpdateTimer = _timer;

                if (!haveLists)
                {
                    switch (ai.Ship.ShipTeam)
                    {
                        case Ship.Team.Esxolus:
                            friendlyNodes = ((GameScene)Space394Game.GameInstance.CurrentScene).EsxolusCapitalNodes;
                            enemyNodes = ((GameScene)Space394Game.GameInstance.CurrentScene).HalkCapitalNodes;
                            break;
                        case Ship.Team.Halk:
                            friendlyNodes = ((GameScene)Space394Game.GameInstance.CurrentScene).HalkCapitalNodes;
                            enemyNodes = ((GameScene)Space394Game.GameInstance.CurrentScene).EsxolusCapitalNodes;
                            break;
                    }
                    haveLists = true;
                }

                goalUpdateTimer = GOAL_UPDATE_TIMER;
                goalPosition = getNewGoalPosition();

            }
            else { }

            return this;
        }

        public Vector3 getNewGoalPosition()
        {
            Vector3 newGoalPosition = goalPosition;
            Vector3 aiPosition = ai.Ship.Position;


            float tmpDist;
            float compareDist;
            if (NODE_MOD > 0.55f)
            {
                compareDist = float.MaxValue;
                for (int i = 0; i < enemyNodes.Count; i++)
                {
                    tmpDist = Vector3.DistanceSquared(aiPosition, enemyNodes[i]);
                    if (tmpDist < compareDist)
                    {
                        newGoalPosition = enemyNodes[i];
                        compareDist = tmpDist;
                    }
                    else
                    { }
                }
            }
            else if (NODE_MOD < 0.3f)
            {
                compareDist = 0f;
                for (int i = 0; i < enemyNodes.Count; i++)
                {
                    tmpDist = Vector3.DistanceSquared(aiPosition, enemyNodes[i]);
                    if (tmpDist > compareDist)
                    {
                        newGoalPosition = enemyNodes[i];
                        compareDist = tmpDist;
                    }
                    else
                    { }
                }
            }
            else
            {
                compareDist = 0;
                for (int i = 0; i < enemyNodes.Count; i++)
                {
                    float ranSeed = (float)random.NextDouble();
                    tmpDist = Vector3.DistanceSquared(aiPosition, enemyNodes[i]);
                    if (ranSeed > 0.49f)
                    {
                        if (tmpDist > compareDist)
                        {
                            newGoalPosition = enemyNodes[i];
                            compareDist = tmpDist;
                        }
                        else
                        { }
                    }
                    else
                    {
                        if (tmpDist < compareDist)
                        {
                            newGoalPosition = enemyNodes[i];
                            compareDist = tmpDist;
                        }
                        else
                        { }
                    }
                }
            }

            // Return towards home if getting too close to the enemy ship
            if (NODE_MOD > 0.55f)   // Closest found
            {
                if (compareDist < TOO_CLOSE)
                {
                    compareDist = float.MaxValue;
                    for (int i = 0; i < friendlyNodes.Count; i++)
                    {
                        tmpDist = Vector3.DistanceSquared(aiPosition, friendlyNodes[i]);
                        if (tmpDist < compareDist)
                        {
                            newGoalPosition = friendlyNodes[i];
                            compareDist = tmpDist;
                        }
                        else
                        { }
                    }
                }
            }
            else if (NODE_MOD < 0.3f)   // farthest found
            {
                if (compareDist < TOO_CLOSE_FARTHER)
                {
                    compareDist = 0f;
                    for (int i = 0; i < friendlyNodes.Count; i++)
                    {
                        tmpDist = Vector3.DistanceSquared(aiPosition, friendlyNodes[i]);
                        if (tmpDist > compareDist)
                        {
                            newGoalPosition = friendlyNodes[i];
                            compareDist = tmpDist;
                        }
                        else
                        { }
                    }
                }
            }
            else // could be closest or farthest
            {
                if (compareDist < TOO_CLOSE_MID)
                {
                    compareDist = 0;
                    for (int i = 0; i < friendlyNodes.Count; i++)
                    {
                        float ranSeed = (float)random.NextDouble();
                        tmpDist = Vector3.DistanceSquared(aiPosition, friendlyNodes[i]);
                        if (ranSeed > 0.49f)
                        {
                            if (tmpDist > compareDist)
                            {
                                newGoalPosition = friendlyNodes[i];
                                compareDist = tmpDist;
                            }
                            else
                            { }
                        }
                        else
                        {
                            if (tmpDist < compareDist)
                            {
                                newGoalPosition = friendlyNodes[i];
                                compareDist = tmpDist;
                            }
                            else
                            { }
                        }
                    }
                }
            }

            return newGoalPosition;
        }
    }
}
