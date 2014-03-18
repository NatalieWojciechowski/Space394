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
    public abstract class AIPilotingState : AIState
    {
        private Random random;

        protected float targetDistSq;   // targetDistanceSquared

        protected CollisionSphere targetSphere;
        private const int TARGET_SPHERE_SIZE = 8000;

        // Distances used for valid target checks
        private const float MAX_CAPITAL_DIST = 50000000000f;
        private const float MAX_FIGHTER_DIST = 500000000f;
        private const float MIN_CAPITAL_DIST = 1000000f;
        private const float MIN_FIGHTER_DIST = 1500f;

        protected CollisionSphere pursuitRetreatSphere;
        private const int P_RETREAT_SPHERE_SIZE = 300;
        protected CollisionSphere engageRetreatSphere;
        private const int E_RETREAT_SPHERE_SIZE = 10000;

        private CollisionSphere boostSphere;
        private const int BOOST_SPHERE_SIZE = 1500;
        private CollisionSphere brakeSphere;
        private const int BRAKE_SPHERE_SIZE = 600;

        protected Vector3 newForward;

        protected float FIRE_RATE;
        protected float SPECIAL_RATE;

        private float targetCheckTimer;
        private const float TARGET_CHECK_TIMER = 2.5f;

        private const float SPEED_UPDATE_RATE = 1.5f;
        private float speedUpdateTimer;
        private const float RETREAT_UPDATE_RATE = 1f;
        private float retreatUpdateTimer;

        public float getNewInterestTimer() { return ai.aiShared.INTEREST_TIME + (float)random.NextDouble(); }

        private bool engageStateHalt = false; /* Locks out engageBattleShipState when to temporarily prevent */
        public bool EngageStateHalt
        {
            get { return engageStateHalt; }
            set { engageStateHalt = value; }
        }

        public AIPilotingState(AI _ai)
            : base(_ai)
        {
            StateComplete = false;

            random = new Random(System.DateTime.Now.Millisecond + (int)ai.Ship.UniqueId);

            targetCheckTimer = TARGET_CHECK_TIMER + (float)(random.Next(1,3) * random.NextDouble());

            targetSphere = new CollisionSphere(ai.Ship.Position, TARGET_SPHERE_SIZE);
            targetSphere.Active = true;

            boostSphere = new CollisionSphere(ai.Ship.Position, BOOST_SPHERE_SIZE);
            brakeSphere = new CollisionSphere(ai.Ship.Position, BRAKE_SPHERE_SIZE);

            pursuitRetreatSphere = new CollisionSphere(ai.Ship.Position, P_RETREAT_SPHERE_SIZE);
            engageRetreatSphere = new CollisionSphere(ai.Ship.Position, E_RETREAT_SPHERE_SIZE);

            FIRE_RATE = ai.Ship.FireRate;
            SPECIAL_RATE = ai.Ship.SpecialRate;

            speedUpdateTimer = SPEED_UPDATE_RATE;
            retreatUpdateTimer = RETREAT_UPDATE_RATE;

            ai.aiShared.INTEREST_TIME = ai.Ship.InterestTime + (float)random.NextDouble();
            ai.aiShared.interestTimer = ai.aiShared.INTEREST_TIME;
            ai.aiShared.fleeCounter = 0;
        }

        public override void Update(float deltaTime)
        {
            if (ai.Ship.Health <= 0)
            {
                StateComplete = true;
                nextState = ((AIDyingState)ai.MyDyingState).recycleState();
                if (ai.aiShared.target != null)
                {
                    dropTarget();
                }
                else { }
            }
            else
            {
                if (ai.aiShared.target != null)
                {
                    if (!ai.aiShared.target.Active)
                    {
                        dropTarget();      
                        StateComplete = true;
                        nextState = ((AIWanderState)ai.MyWanderState).recycleState();
                    }
                    else
                    {
                        // the new forward vector, so the avatar faces the ai.aiShared.target
                        Vector3 newForward = Vector3.Normalize(ai.Ship.Position - (ai.aiShared.target.Position));// scalePosition(targetPosition, 600f)));

                        // calc the rotation so the avatar faces the ai.aiShared.target
                        ai.Ship.Rotation = ai.Ship.AdjustRotation(Vector3.Forward, newForward, Vector3.Up, deltaTime);
                        ai.Ship.updateVelocity();

                        // Retreat Check
                        checkRetreatTimer(deltaTime);

                        /* Check each step from now on
                         * to see if the state was 
                         * completed in previous */

                        // Speed Spheres Check
                        if (!StateComplete)
                        {
                            checkSpeedTimer(deltaTime);
                        }
                        else { }
                        // Interest Check
                        if (!StateComplete)
                        {
                            checkInterestTimer(deltaTime);
                        }
                        else { }
                    }
                } 

                // ai.aiShared.target Check
                if (!StateComplete)
                {
                    checkTargetTimer(deltaTime);
                }
                else { }
            }
        }

        public Ship findRandomTarget()
        {
            Ship tmpShip = null;
            int listDet = 0; 
            if (ai.Ship is AssaultFighter)
            {
                listDet = random.Next(1, 4);
            }
            else if (ai.Ship is Bomber)
            {
                listDet = random.Next(1, 2);
            }
            else if (ai.Ship is Interceptor)
            {
                listDet = random.Next(1, 6);
            }
            else { }

            bool found = false;
            if (listDet == 1)
            {
                List<SpawnShip> eShips = ((GameScene)Space394Game.GameInstance.CurrentScene).getSpawnShips();
                int count = eShips.Count;
                const int MAX_TRIES = 5;
                for (int i = 0; i < MAX_TRIES; i++)
                {
                    int randomClamped = random.Next(count);
                    if (eShips[randomClamped].ShipTeam != ai.Ship.ShipTeam && eShips[randomClamped].Attackable)
                    {
                        tmpShip = eShips[randomClamped];
                        found = true;
                        break;
                    }
                    else
                    {
                        randomClamped = random.Next(count);
                    }
                }
            }

            if (!found)
            {
                /* Before grabbing random target, is the ai.Ship close to a player? */
                int pTargetBias = random.Next(1,2); // Chances of checking if player is near
                if (pTargetBias == 1)
                {
                    List<Player> pList = Space394Game.GameInstance.Players;
                    for (int i = 0; i < pList.Count; i++)
                    {
                        if (pList[i].PlayerShip == null)                        /* Least frequent but needed first */
                        {
                            break;
                        }
                        else if (pList[i].PlayerShip.Attackable &&              /* Most common break out */
                            pList[i].PlayerShip.ShipTeam != ai.Ship.ShipTeam)   /* Half chance if this far */
                        {
                            if (targetSphere.isCollidingSq((CollisionSphere)pList[i].PlayerShip.CollisionBase))
                            {
                                found = true;
                                tmpShip = pList[i].PlayerShip;
                                break;
                            }
                            else { }
                        }
                        else { }
                    }
                }
                else { }
                
                if(!found)
                {
                    List<Fighter> eShips = ((GameScene)Space394Game.GameInstance.CurrentScene).getEnemyFighters(ai.Ship.ShipTeam);
                    for (int j = 0; j < 5/*MAX_TRIES*/; j++)
                    {
                        int randomClamped = random.Next(eShips.Count);
                        /* limit to one access of the list itself */
                        if (eShips[randomClamped].Attackable)
                        {
                            tmpShip = eShips[randomClamped];
                            break;
                        }
                        else
                        {
                            randomClamped = random.Next(eShips.Count);
                        }
                    }
                }
            }
            
            return tmpShip;
        }

        public Ship findCollideTarget()
        {
            Ship target = null;
            float closestDist = float.MaxValue;
            float engageValue = 0.0f;
            // Dont need engage modifier as its only called 1-3 times
            float pursuitValue = 0.0f;
            float pursuitModifier = ai.PursuitModifier;
                
            GameScene gScene = ((GameScene)Space394Game.GameInstance.CurrentScene);// .getSpawnShips();
            Ship.Team eTeam;
            if (ai.Ship.ShipTeam == Ship.Team.Esxolus)
            {
                eTeam = Ship.Team.Halk;
            }
            else
            {
                eTeam = Ship.Team.Esxolus;
            }

            Vector3 aiPosition = ai.Ship.Position;
            float capitalDistanceDiff = MAX_CAPITAL_DIST - MIN_CAPITAL_DIST;
            float capitalMidDist = capitalDistanceDiff / 2f;

            if (!engageStateHalt)
            {
                List<SpawnShip> spawnShips = gScene.getSpawnShips();
                int count = spawnShips.Count;
                if (count > 0)
                {
                    for (int i = 0; i < count; i++)
                    {
                        Ship tmpShip = spawnShips[i];
                        if (tmpShip.ShipTeam == eTeam)
                        {
                            if (tmpShip.Attackable)
                            {
                                float tmpDist = Vector3.DistanceSquared(aiPosition, tmpShip.Position);
                                if (tmpDist < MAX_CAPITAL_DIST && tmpDist > MIN_CAPITAL_DIST)
                                {
                                    if (tmpDist < closestDist)
                                    {
                                        closestDist = tmpDist;
                                        target = spawnShips[i];
                                        engageValue = (Math.Abs((capitalMidDist - tmpDist)) / capitalDistanceDiff) * ai.EngageModifier;
                                    }
                                    else
                                    { }
                                }
                                else
                                { }
                            }
                            else { }
                        }
                        else { }
                    }
                }
            }
            else
            {
                engageStateHalt = false;
            }
            float fighterDistanceDiff = MAX_FIGHTER_DIST - MIN_FIGHTER_DIST;
            float fighterMidDist = fighterDistanceDiff / 2f;
            List<Fighter> eShips = gScene.getEnemyFighters(ai.Ship.ShipTeam);
            int eShipsCount = eShips.Count;
            for (int i = 0; i < eShipsCount; i++)
            {
                Ship tmpShip = eShips[i];   /* limit to one access of the list itself */
                if (tmpShip.Attackable)
                {
                    float tmpDist = Vector3.DistanceSquared(aiPosition, tmpShip.Position);
                    if (tmpDist < MAX_FIGHTER_DIST && tmpDist > MIN_FIGHTER_DIST)
                    {
                        ((Fighter)tmpShip).MyAI.addPotentialFighter(ai.Ship);

                        if (tmpDist < closestDist)
                        {
                            closestDist = tmpDist;
                            pursuitValue = ((fighterMidDist - tmpDist) / fighterDistanceDiff) * pursuitModifier;
                            if (pursuitValue > engageValue)
                            {
                                target = tmpShip;
                            }
                            else
                            { }
                        }
                        else
                        { }
                    }
                }
                else { }
            }

            return target;
        }

        public Ship findListTarget()
        {
            Ship target = null;
            float closestDist = float.MaxValue;
            float engageValue = 0.0f;
            // Dont need engage modifier as its only called 1-3 times
            float pursuitValue = 0.0f;
            float pursuitModifier = ai.PursuitModifier;

            GameScene gScene = ((GameScene)Space394Game.GameInstance.CurrentScene);
            Ship.Team eTeam;
            if (ai.Ship.ShipTeam == Ship.Team.Esxolus)
            {
                eTeam = Ship.Team.Halk;
            }
            else
            {
                eTeam = Ship.Team.Esxolus;
            }

            Vector3 aiPosition = ai.Ship.Position;
            float capitalDistanceDiff = MAX_CAPITAL_DIST - MIN_CAPITAL_DIST;
            float capitalMidDist = capitalDistanceDiff / 2f;

            if (!engageStateHalt)
            {
                List<SpawnShip> spawnShips = gScene.getSpawnShips();
                int count = spawnShips.Count;
                for (int i = 0; i < count; i++)
                {
                    Ship tmpShip = spawnShips[i];
                    if (tmpShip.ShipTeam == eTeam)
                    {
                        if (tmpShip.Attackable)
                        {
                            float tmpDist = Vector3.DistanceSquared(aiPosition, tmpShip.Position);
                            if (tmpDist < MAX_CAPITAL_DIST && tmpDist > MIN_CAPITAL_DIST)
                            {
                                if (tmpDist < closestDist)
                                {
                                    closestDist = tmpDist;
                                    target = spawnShips[i];
                                    engageValue = (Math.Abs((capitalMidDist - tmpDist)) / capitalDistanceDiff) * ai.EngageModifier;
                                }
                                else
                                { }
                            }
                            else
                            { }
                        }
                        else { }
                    }
                    else { }
                }
            }
            else
            { 
                engageStateHalt = false; 
            }
            float fighterDistanceDiff = MAX_FIGHTER_DIST - MIN_FIGHTER_DIST;
            float fighterMidDist = fighterDistanceDiff / 2f;

            List<Fighter> eShips = ai.PotentialFighters;
            int eShipsCount = eShips.Count;
            if (eShipsCount > 0)
            {
                for (int i = 0; i < eShipsCount; i++)
                {
                    Ship tmpShip = eShips[i];   /* limit to one access of the list itself */
                    if (tmpShip.Attackable)
                    {
                        float tmpDist = Vector3.DistanceSquared(aiPosition, tmpShip.Position);
                        if (tmpDist < MAX_FIGHTER_DIST && tmpDist > MIN_FIGHTER_DIST)
                        {
                            if (tmpDist < closestDist)
                            {
                                closestDist = tmpDist;
                                pursuitValue = ((fighterMidDist - tmpDist) / fighterDistanceDiff) * pursuitModifier;
                                if (pursuitValue > engageValue)
                                {
                                    target = tmpShip;
                                }
                                else
                                { }
                            }
                            else
                            { }
                        }
                        else
                        { }
                    }
                    else { }
                }
            }
            else { }

            ai.PotentialFighters.Clear();
            return target;
        }

        /// <summary>
        /// Intermediate function to determine which
        /// method will be used to find targets.
        /// </summary>
        /// <returns></returns>
        public Ship findPotentialTargets()
        {
            return findRandomTarget();
            //switch (ai.Ship.ShipTeam)
            //{
            //    case Ship.Team.Esxolus:
            //        return findCollideTarget();
            //    case Ship.Team.Halk:
            //        return findListTarget();
            //}
            // else
            // return null;
        }

        /// <summary>
        /// Used when leaving a state with a target. Does NOT set state complete.
        /// </summary>
        public void dropTarget()
        {
            ai.aiShared.target.AttackerCount = -1;
            ai.aiShared.target = null;
            ai.aiShared.target = null;
            ai.sharedRefresh();
        }

        public void checkRetreatTimer(float _deltaTime)
        {
            retreatUpdateTimer -= _deltaTime;
            if (retreatUpdateTimer < 0)
            {
                if (ai.aiShared.target is Fighter)
                {
                    if (pursuitRetreatSphere.isCollidingSq((CollisionSphere)ai.aiShared.target.CollisionBase))
                    {
                        // Retreat only updates base once it would kick out
                        ai.aiShared.fleeCounter++;
                        StateComplete = true; 
                        nextState = ((AIRetreatState)ai.MyRetreatState).recycleState();
                    }
                    else
                    { }
                }
                else if (ai.aiShared.target is SpawnShip)
                {
                    if (engageRetreatSphere.isCollidingSq((CollisionSphere)((SpawnShip)ai.aiShared.target).RadiusSphere))
                    {
                        // Retreat only updates base once it would kick out
                        ai.aiShared.fleeCounter++;
                        StateComplete = true;
                        nextState = ((AIRetreatState)ai.MyRetreatState).recycleState();
                    }
                    else { }
                }
                else
                { }

                retreatUpdateTimer = RETREAT_UPDATE_RATE;
            }
            else
            { }
        }

        public void checkSpeedTimer(float _deltaTime)
        {
            // Speed Control
            speedUpdateTimer -= _deltaTime;
            if (speedUpdateTimer < 0)
            {
                boostSphere.Position = ai.Ship.Position;
                brakeSphere.Position = ai.Ship.Position;

                if (ai.aiShared.target is Fighter)
                {
                    if (((CollisionSphere)ai.aiShared.target.CollisionBase).isCollidingSq(brakeSphere))
                    {
                        // Brake
                        ai.Ship.ZSpeed = ai.Ship.MinZSpeed;
                    }
                    else if (((CollisionSphere)ai.aiShared.target.CollisionBase).isCollidingSq(boostSphere))
                    {
                        // Normal speed
                        ai.Ship.ZSpeed = ai.Ship.NormalZSpeed;
                    }
                    else
                    {
                        // Boost
                        ai.Ship.ZSpeed = ai.Ship.MaxZSpeed;
                    }
                }
                else if (ai.aiShared.target is CapitalShip)
                {
                    if (((CollisionSphere)((SpawnShip)ai.aiShared.target).RadiusSphere).isCollidingSq(brakeSphere))
                    {
                        // Brake
                        ai.Ship.ZSpeed = ai.Ship.MinZSpeed;
                    }
                    else if (((CollisionSphere)((SpawnShip)ai.aiShared.target).RadiusSphere).isCollidingSq(boostSphere))
                    {
                        // Normal speed
                        ai.Ship.ZSpeed = ai.Ship.NormalZSpeed;
                    }
                    else
                    {
                        // Boost
                        ai.Ship.ZSpeed = ai.Ship.MaxZSpeed;
                    }
                }
                else
                { }
                speedUpdateTimer = SPEED_UPDATE_RATE;
            }
            else { }
        }

        public void checkInterestTimer(float _deltaTime)
        {
            ai.aiShared.interestTimer -= _deltaTime;
            if (ai.aiShared.interestTimer <= 0 && ai.CurrentState.CurrentState != state.Wandering)
            {
                LogCat.updateValue("Lost interest", LogCat.increaseCounter());
                dropTarget();
                StateComplete = true;
                nextState = ((AIWanderState)ai.MyWanderState).recycleState();
            }
            else { }
        }

        public void checkTargetTimer(float _deltaTime)
        {
            targetCheckTimer -= _deltaTime;
            if (targetCheckTimer <= 0)
            {
                if (ai.aiShared.target == null || targetCheckTimer < -1.5f /*[implicit]&& ai.aiShared.target != null*/)
                {
                    targetSphere.Position = ai.Ship.Position;

                    if (ai.aiShared.target != null)
                    {
                        dropTarget();
                    }
                    else { }

                    ai.aiShared.target = findPotentialTargets();
                    if (ai.aiShared.target != null)
                    {
                        ai.sharedRefresh(getNewInterestTimer());
                        StateComplete = true;

                        if (ai.aiShared.target is Fighter)
                        {
                            nextState = ((AIPursuitState)ai.MyPursuitState).recycleState();
                            ai.aiShared.target.AttackerCount = 1;
                        }
                        else // if Battleship
                        {
                            nextState = ((AIEngageBattleshipState)ai.MyEngageBattleshipState).recycleState();
                            ai.aiShared.target.AttackerCount = 1;
                        }
                    }
                    else
                    {
                        if (ai.CurrentState.CurrentState != state.Wandering)
                        {
                            ai.sharedRefresh();
                            StateComplete = true;
                            nextState = ((AIWanderState)ai.MyWanderState).recycleState();
                        }
                        else { }
                    }

                    targetCheckTimer = TARGET_CHECK_TIMER;
                }
            }
        }
    }
}


#region Alternative Method - Holding for now
//           float fighterDistanceDiff = MAX_FIGHTER_DIST - MIN_FIGHTER_DIST;
//            float fighterMidDist = fighterDistanceDiff / 2f;
//            CollisionSphere aiCollider = (CollisionSphere)ai.getShip().getCollider();
//            List<Fighter> eShips = gScene.getEnemyFighters(ai.getShip().getTeam());
//            int eShipsCount = eShips.Count;
//            short fighterMaxAttackers = eShips[0].getMaxAttackers();    /* Max attackers only varries between Fighter and Capital ships at the moment */
//            for (int i = 0; i < eShipsCount; i++)
//            {
//                Ship tmpShip = eShips[i];   /* limit to one access of the list itself */
//                if (tmpShip.getAttackerCount() < fighterMaxAttackers)
//                {
//                    float tmpDist;
//                    if (aiCollider.isCollidingSq((CollisionSphere)tmpShip.getCollider(), out tmpDist))
//                    {
////                        float tmpDist = Vector3.DistanceSquared(aiPosition, tmpShip.getPosition());
//                        if (tmpDist < MAX_FIGHTER_DIST && tmpDist > MIN_FIGHTER_DIST)
//                        {
//                            if (tmpDist < closestDist)
//                            {
//                                closestDist = tmpDist;
//                                pursuitValue = (Math.Abs((fighterMidDist - tmpDist)) / fighterDistanceDiff) * pursuitModifier;
//                                if (pursuitValue > engageValue)
//                                {
//                                    target = tmpShip;
//                                }
//                                else
//                                { }
//                            }
//                            else
//                            { }
//                        }
//                        else { }
//                    }
//                }
//                else { }
//            }

#endregion
