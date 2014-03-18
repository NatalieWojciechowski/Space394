using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Space394.SceneObjects;
using Space394.Scenes;

namespace Space394.AIControl
{
    public class AI
    {
        private AIState currentState;
        public AIState CurrentState
        {
            get { return currentState; }
            set { currentState = value; }
        }
        public AIState nextState() { currentState = currentState.getNextState(this); return currentState; }

        private AIState mySpawningState;
        public AIState MySpawningState
        {
            get { return mySpawningState; }
        }

        private AIState myWaitingToSpawnState;
        public AIState MyWaitingToSpawnState
        {
            get { return myWaitingToSpawnState; }
        }

        private AIState myWanderState;
        public AIState MyWanderState
        {
            get { return myWanderState; }
        }

        private AIState myPursuitState;
        public AIState MyPursuitState
        {
            get { return myPursuitState; }
        }

        private AIState myEngageBattleshipState;
        public AIState MyEngageBattleshipState
        {
            get { return myEngageBattleshipState; }
        }

        private AIState myRetreatState;
        public AIState MyRetreatState
        {
            get { return myRetreatState; }
        }

        private AIState myDyingState;
        public AIState MyDyingState
        {
            get { return myDyingState; }
        }

        private Fighter ship;
        public Fighter Ship
        {
            get { return ship; }
        }

        private Random random;
        
        // Only Halk will check or remove from these, Esxolus adds to them
        private List<CapitalShip> potentialCapitals;
        public List<CapitalShip> PotentialCapitals
        {
            get { return potentialCapitals; }
        }
        public CapitalShip addPotentialCapital(CapitalShip _capitalShip) { potentialCapitals.Add(_capitalShip); return _capitalShip; }
        public CapitalShip removePotentialCapital(CapitalShip _capitalShip) { potentialCapitals.Remove(_capitalShip); return _capitalShip; }

        private List<Fighter> potentialFighters;
        public List<Fighter> PotentialFighters
        {
            get { return potentialFighters; }
        }
        public Fighter addPotentialFighter(Fighter _fighter) { potentialFighters.Add(_fighter); return _fighter; }
        public Fighter removePotentialFighter(Fighter _fighter) { potentialFighters.Remove(_fighter); return _fighter; }

        private bool isSpawningState;
        public bool IsSpawningState
        {
            get { return isSpawningState; }
            set { isSpawningState = value; }
        }
        private int stateChanges = 0;

        private float engageModifier;   // EngageBattleship
        public float EngageModifier
        {
            get { return engageModifier; }
        }

        private float pursuitModifier;  // Pursuit ( Fighters )
        public float PursuitModifier
        {
            get { return pursuitModifier; }
        }

        private float retreatModifier;  // Retreat
        public float RetreatModifier
        {
            get { return retreatModifier; }
        }

        public static void updateGlobalAITimer(float deltaTime)
        {
            switch (currentPhase)
            {
                case aiActivationPhase.first: currentPhase = aiActivationPhase.second; break;
                case aiActivationPhase.second: currentPhase = aiActivationPhase.third; break;
                case aiActivationPhase.third: currentPhase = aiActivationPhase.first; break;
            }
        }

        public enum aiActivationPhase
        {
            first,
            second,
            third,
        };
        private static aiActivationPhase currentPhase = aiActivationPhase.first;
        private static aiActivationPhase nextAssignedPhase = aiActivationPhase.first;

        private aiActivationPhase assignedPhase;

        public struct AI_Overmind
        {
            public static int inSpawning = 0;
            public static int inWaitingToSpawn = 0;
            public static int inWander = 0;
            public static int inPursuit = 0;
            public static int inEngageBattleship = 0;
            public static int inRetreat = 0;
            public static int inDying = 0;
        }

        // Shared between mostly AIPiloting type states
        public AI_Shared_Data aiShared;
        public struct AI_Shared_Data
        {
            public Ship target;
            public int fleeCounter;
            public float INTEREST_TIME;
            public float interestTimer;
        }

        public void sharedRefresh()
        {
            aiShared.fleeCounter = 0;
            aiShared.interestTimer = aiShared.INTEREST_TIME;
        }

        public void sharedRefresh(float _timer)
        {
            aiShared.fleeCounter = 0;
            aiShared.interestTimer = _timer;
        }

        public void sharedRefresh(Ship _target, float _timer)
        {
            aiShared.target = _target;
            aiShared.fleeCounter = 0;
            aiShared.interestTimer = _timer;
        }

        public AI(Fighter _ship)
        {
            ship = _ship;
            createPersonality();

            random = new Random(System.DateTime.Now.Millisecond + (int)Ship.UniqueId);

            aiShared = new AI_Shared_Data();
            aiShared.INTEREST_TIME = Ship.InterestTime + (float)random.NextDouble();

            mySpawningState = new AISpawningState(this);
            myWaitingToSpawnState = new AIWaitingToSpawnState(this);
            myWanderState = new AIWanderState(this);
            myPursuitState = new AIPursuitState(this);
            myEngageBattleshipState = new AIEngageBattleshipState(this);
            myRetreatState = new AIRetreatState(this);
            myDyingState = new AIDyingState(this);

            potentialCapitals = new List<CapitalShip>();
            potentialFighters = new List<Fighter>();

            currentState = myWaitingToSpawnState;
            AI_Overmind.inWaitingToSpawn++;

            assignedPhase = nextAssignedPhase;
            switch (nextAssignedPhase)
            {
                case aiActivationPhase.first: nextAssignedPhase = aiActivationPhase.second; break;
                case aiActivationPhase.second: nextAssignedPhase = aiActivationPhase.third; break;
                case aiActivationPhase.third: nextAssignedPhase = aiActivationPhase.first; break;
            }
        }

        public void Update(float deltaTime)
        {
            if (Space394Game.GameInstance.CurrentScene is GameScene)
            {
               // if (assignedPhase == currentPhase)
                {
                    currentState.Update(deltaTime);

                    LogCat.updateValue("In Dying", "" + AI_Overmind.inDying);
                    LogCat.updateValue("In Engage Battleship", "" + AI_Overmind.inEngageBattleship);
                    LogCat.updateValue("In Pursuit", "" + AI_Overmind.inPursuit);
                    LogCat.updateValue("In Retreat", "" + AI_Overmind.inRetreat);
                    LogCat.updateValue("In Spawning", "" + AI_Overmind.inSpawning);
                    LogCat.updateValue("In Wander", "" + AI_Overmind.inWander);
                    LogCat.updateValue("In Waiting To Spawn", "" + AI_Overmind.inWaitingToSpawn);

                    if (currentState.StateComplete)
                    {
                        stateChanges++;
                        currentState = currentState.getNextState(this);
                    }
                    else { }
                }
            }
        }

        private void createPersonality()
        {
            if (ship != null)
            {
                if (ship is AssaultFighter)
                {
                    engageModifier = 0.5f;
                    pursuitModifier = 0.5f;
                    retreatModifier = 0.5f;

                    if (ship is EsxolusAssaultFighter)
                    {

                    }
                    else if (ship is HalkAssaultFighter)
                    {

                    }
                    else
                    { }

                }
                else if (ship is Bomber)
                {
                    engageModifier = 2.0f;
                    pursuitModifier = 0.25f;
                    retreatModifier = 0.15f;

                    if (ship is EsxolusBomber)
                    {

                    }
                    else if (ship is HalkBomber)
                    {

                    }
                    else
                    { }
                }

                else if (ship is Interceptor)
                {
                    engageModifier = 0.25f;
                    pursuitModifier = 2.0f;
                    retreatModifier = 0.6f;

                    if (ship is EsxolusInterceptor)
                    {

                    }
                    else if (ship is HalkInterceptor)
                    {

                    }
                    else
                    { }
                }
                else
                { }

            }
            else
            {
                engageModifier = 0.0f;
                pursuitModifier = 0.0f;
                retreatModifier = 0.0f;
            }

        }
    }
}
