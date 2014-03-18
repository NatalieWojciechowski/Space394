using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Space394.SceneObjects;
using Microsoft.Xna.Framework;
using Space394.Collision;
using Microsoft.Xna.Framework.Graphics;
using Space394.PlayerStates;
using BEPUphysics;
using Space394.Events;
using Space394.AIControl;
using System.Threading;
using Space394.Particles;

namespace Space394.Scenes
{
    public class GameScene : Scene
    {
        private CollisionManager collisionManager;
        public CollisionManager CollisionManager
        {
            get { return collisionManager; }
        }

        public enum state
        {
            loading,
            game,
            victory
        };

        private state currentState;
        public state CurrentState
        {
            get { return currentState; }
            set { currentState = value; }
        }

        private Ship.Team victor;
        public Ship.Team Victor
        {
            get { return victor; }
        }

        #region DEBUG
#if DEBUG
        private bool murder = false;
        private float HYPERBEAM = 1000000.0f;
        private void killEveryone()
        {
            murder = true;
        }

        private bool waveNow = false;
        private void releaseTheWave()
        {
            waveNow = true;
        }

        private bool invincibility = false;
        private float STAR_POWER = 1000000.0f;
        private void star()
        {
            invincibility = true;
        }
#endif
        #endregion

        #region Ship Members
        private int esxolusSpawnPointsRemaining = 0;
        public int EsxolusSpawnPointsRemaining
        {
            get { return esxolusSpawnPointsRemaining; }
        }
        private int halkSpawnPointsRemaining = 0;
        public int HalkSpawnPointsRemaining
        {
            get { return halkSpawnPointsRemaining; }
        }

        public int getSpawnPointsRemaining(Ship.Team team)
        {
            if (team == Ship.Team.Esxolus)
            {
                return esxolusSpawnPointsRemaining;
            }
            else
            {
                return halkSpawnPointsRemaining;
            }
        }

        public int spawnPointIncrease(Ship.Team team)
        {
            if (team == Ship.Team.Esxolus)
            {
                return esxolusSpawnPointsRemaining++;
            }
            else
            {
                return halkSpawnPointsRemaining++;
            }
        } // For use by Lua

        public int spawnPointDecrease(Ship.Team team)
        {
            if (team == Ship.Team.Esxolus)
            {
                return esxolusSpawnPointsRemaining--;
            }
            else
            {
                return halkSpawnPointsRemaining--;
            }
        } // For use by the ships

        private List<Fighter> esxolusFighters;
        public List<Fighter> EsxolusFighters
        {
            get { return esxolusFighters; }
        }
        public Fighter removeEsxolusFighter(Fighter ship) { esxolusFighters.Remove(ship); return ship; }
        public Fighter addEsxolusFighter(Fighter ship) { esxolusFighters.Add(ship); return ship; }

        private List<Fighter> halkFighters;
        public List<Fighter> HalkFighters
        {
            get { return halkFighters; }
        }
        public Fighter removeHalkFighter(Fighter ship) { halkFighters.Remove(ship); return ship; }
        public Fighter addHalkFighter(Fighter ship) { halkFighters.Add(ship); return ship; }

        public Fighter addFighterShip(Fighter ship)
        {
            if (ship.ShipTeam == Ship.Team.Esxolus)
            {
                return addEsxolusFighter(ship);
            }
            else
            {
                return addHalkFighter(ship);
            }
        }
        public Fighter removeFighterShip(Fighter ship)
        {
            if (ship.ShipTeam == Ship.Team.Esxolus)
            {
                return removeEsxolusFighter(ship);
            }
            else
            {
                return removeHalkFighter(ship);
            }
        }

        public List<Fighter> getEnemyFighters(Ship.Team _team)
        {
            if (_team == Ship.Team.Esxolus)
            {
                return halkFighters;
            }
            else
            {
                return esxolusFighters;
            }
        }

        public static ReaderWriterLockSlim shipLock = Locks.GetLockInstance(LockRecursionPolicy.NoRecursion); //setup the lock;

        private List<SpawnShip> spawnShips;
        public List<SpawnShip> SpawnShips
        {
            get { return spawnShips; }
        }
        public SpawnShip removeSpawnShip(SpawnShip ship)
        {
            using (new WriteLock(shipLock))
            {
                spawnShips.Remove(ship);
            }
            return ship;
        }
        public SpawnShip addSpawnShip(SpawnShip ship)
        {
            using (new WriteLock(shipLock))
            {
                spawnShips.Add(ship);
            }
            return ship;
        }
        public List<SpawnShip> getSpawnShips() { return spawnShips; }

        private List<Vector3> halkCapitalNodes;
        public List<Vector3> HalkCapitalNodes
        {
            get { return halkCapitalNodes; }
        }
        public Vector3 addHalkCapitalNode(Vector3 _node)
        {
            using (new WriteLock(shipLock))
            {
                halkCapitalNodes.Add(_node);
            }
            return _node;
        }

        private List<Vector3> esxolusCapitalNodes;
        public List<Vector3> EsxolusCapitalNodes
        {
            get { return esxolusCapitalNodes; }
        }
        public Vector3 addEsxolusCapitalNode(Vector3 _node)
        {
            using (new WriteLock(shipLock))
            {
                esxolusCapitalNodes.Add(_node);
            }
            return _node;
        }

        public SpawnShip getSpawnShip(Ship.Team team, int index)
        {
            int i = 0;
            SpawnShip rShip = null;
            while (rShip == null || index != 0)
            {
                if (spawnShips[i].ShipTeam == team)
                {
                    rShip = spawnShips[i];
                    index--;
                }
                else { }
                i++;
                if (i == spawnShips.Count)
                {
                    break;
                }
                else { }
            }
            return rShip;
        }

        // Bomber Ships remaining
        private int esxolusBombersRemaining = 0;
        public int EsxolusBombersRemaining
        {
            get { return esxolusBombersRemaining; }
        }

        private int halkBombersRemaining = 0;
        public int HalkBombersRemaining
        {
            get { return halkBombersRemaining; }
        }

        public int getBombersRemaining(Ship.Team team)
        {
            if (team == Ship.Team.Esxolus)
            {
                return esxolusBombersRemaining;
            }
            else
            {
                return halkBombersRemaining;
            }
        }

        public int bombersIncrease(Ship.Team team)
        {
            if (team == Ship.Team.Esxolus)
            {
                return esxolusBombersRemaining++;
            }
            else
            {
                return halkBombersRemaining++;
            }
        }

        public int bombersDecrease(Ship.Team team)
        {
            if (team == Ship.Team.Esxolus)
            {
                return esxolusBombersRemaining--;
            }
            else
            {
                return halkBombersRemaining--;
            }
        }

        // Assault Fighter Ships remaining
        private int esxolusAssaultFightersRemaining = 0;
        public int EsxolusAssaultFightersRemaining
        {
            get { return esxolusAssaultFightersRemaining; }
        }

        private int halkAssaultFightersRemaining = 0;
        public int HalkAssaultFightersRemaining
        {
            get { return halkAssaultFightersRemaining; }
        }

        public int getAssaultFightersRemaining(Ship.Team team)
        {
            if (team == Ship.Team.Esxolus)
            {
                return esxolusAssaultFightersRemaining;
            }
            else
            {
                return halkAssaultFightersRemaining;
            }
        }

        public int assaultFightersIncrease(Ship.Team team)
        {
            if (team == Ship.Team.Esxolus)
            {
                return esxolusAssaultFightersRemaining++;
            }
            else
            {
                return halkAssaultFightersRemaining++;
            }
        }

        public int assaultFightersDecrease(Ship.Team team)
        {
            if (team == Ship.Team.Esxolus)
            {
                return esxolusAssaultFightersRemaining--;
            }
            else
            {
                return halkAssaultFightersRemaining--;
            }
        }

        // Interceptor Ships remaining
        private int esxolusInterceptorsRemaining = 0;
        public int EsxolusInterceptorsRemaining
        {
            get { return esxolusInterceptorsRemaining; }
        }

        private int halkInterceptorsRemaining = 0;
        public int HalkInterceptorsRemaining
        {
            get { return halkInterceptorsRemaining; }
        }

        public int getInterceptorsRemaining(Ship.Team team)
        {
            if (team == Ship.Team.Esxolus)
            {
                return esxolusInterceptorsRemaining;
            }
            else
            {
                return halkInterceptorsRemaining;
            }
        }

        public int interceptorsIncrease(Ship.Team team)
        {
            if (team == Ship.Team.Esxolus)
            {
                return esxolusInterceptorsRemaining++;
            }
            else
            {
                return halkInterceptorsRemaining++;
            }
        }

        public int interceptorsDecrease(Ship.Team team)
        {
            if (team == Ship.Team.Esxolus)
            {
                return esxolusInterceptorsRemaining--;
            }
            else
            {
                return halkInterceptorsRemaining--;
            }
        }
        #endregion

#if DEBUG
        protected const float INITIAL_WAVE_TIMER = 60.0f;
        protected const float WAVE_TIMER = 60.0f;
#else
        protected const float INITIAL_WAVE_TIMER = 30.0f; //45.0f;
        protected const float WAVE_TIMER = 30.0f; //30.0f;
#endif
        private float waveTimer = INITIAL_WAVE_TIMER;
        public float WaveTimer
        {
            get { return waveTimer; }
        }

        private bool waveReleased = false;
        public bool WaveReleased
        {
            get { return waveReleased; }
        }

        private int esxolusPlayers = 0;
        public int EsxolusPlayers
        {
            get { return esxolusPlayers; }
        }
        public int increaseEsxolusPlayers() { return ++esxolusPlayers; }
        public int decreaseEsxolusPlayers() { return --esxolusPlayers; }

        private int halkPlayers = 0;
        public int HalkPlayers
        {
            get { return halkPlayers; }
        }
        public int increaseHalkPlayers() { return ++halkPlayers; }
        public int decreaseHalkPlayers() { return --halkPlayers; }

        private Queue<Event> esxolusEvents;
        private Queue<Event> halkEvents;

        public Event getTopEvent(Ship.Team team)
        {
            Event rEvent = null;
            switch (team)
            {
                case Ship.Team.Esxolus:
                    rEvent = esxolusEvents.Peek();
                    break;
                case Ship.Team.Halk:
                    rEvent = halkEvents.Peek();
                    break;
            }
            return rEvent;
        }
        public Event getTopEsxolusEvent()
        {
            if (esxolusEvents.Count != 0)
            {
                return esxolusEvents.Peek();
            }
            else
            {
                return null;
            }
        }
        public Event getTopHalkEvent()
        {
            if (halkEvents.Count != 0)
            {
                return halkEvents.Peek();
            }
            else
            {
                return null;
            }
        }

        private float storedDeltaTime = 0;

        private List<AI> aiList;
        public void addAI(AI ai) { lock (aiList) { aiList.Add(ai); } }
        public void removeAI(AI ai) { lock (aiList) { aiList.Remove(ai); } }
        private List<Turret> turretList;
        public void addTurret(Turret turret) { lock (turretList) { turretList.Add(turret); } }
        public void removeTurret(Turret turret) { lock (turretList) { turretList.Remove(turret); } }
        private List<ParticleGenerator> particleGeneratorList;
        public void addParticleGenerator(ParticleGenerator particleGenerator) { lock (particleGeneratorList) { particleGeneratorList.Add(particleGenerator); } }
        public void removeParticleGenerator(ParticleGenerator particleGenerator) { lock (particleGeneratorList) { particleGeneratorList.Remove(particleGenerator); } }
        
        // private Thread aiThread;

        // private Thread collisionThread;

        public GameScene()
            : base()
        {
            LogCat.updateValue("Scene", "Game");

            halkFighters = new List<Fighter>();
            esxolusFighters = new List<Fighter>();
            spawnShips = new List<SpawnShip>();
            halkCapitalNodes = new List<Vector3>();
            esxolusCapitalNodes = new List<Vector3>();

            currentState = state.loading;

            collisionManager = new CollisionManager(1000000, 1000000, 1000000, 3);

            aiList = new List<AI>();
            turretList = new List<Turret>();
            particleGeneratorList = new List<ParticleGenerator>();
            /*aiThread = new Thread(new ThreadStart(AIThreadPoolCallback));
            aiThread.Start();
            aiThread.IsBackground = true;
            aiThread.Priority = ThreadPriority.Lowest;

            collisionThread = new Thread(new ThreadStart(CollisionThreadPoolCallback));
            collisionThread.Start();
            collisionThread.IsBackground = true;*/
        }

        private void CollisionThreadPoolCallback()
        {
            while (true)
            {
                // collisionManager.Update(Space394Game.lastGameTime.ElapsedGameTime.Milliseconds / Space394Game.TimeScale);
                Thread.Sleep(0);
            }
        }

        // Wrapper method for use with thread pool
        private void AIThreadPoolCallback()
        {
            while (true)
            {
                float dt = storedDeltaTime; // Space394Game.lastGameTime.ElapsedGameTime.Milliseconds / Space394Game.TimeScale;
                updateAI(dt);
                updateParticles(dt);
                storedDeltaTime = 0.001f;
            }
        }

        public override void AbortThreads()
        {
            //aiThread.Abort();
            //aiThread.Join();

            base.AbortThreads();
        }

        public override void Initialize()
        {
            Camera.SplitScreenEnabled = true;

            esxolusEvents = new Queue<Event>();
            halkEvents = new Queue<Event>();

            List<SceneObject> esxolusTargets = new List<SceneObject>();
            List<SceneObject> halkTargets = new List<SceneObject>();

            foreach (SceneObject o in SceneObjects.Values)
            {
                if (o is SpawnShip)
                {
                    if (((SpawnShip)o).ShipTeam == Ship.Team.Esxolus)
                    {
                        halkTargets.Add(o);
                    }
                    else if (((SpawnShip)o).ShipTeam == Ship.Team.Halk)
                    {
                        esxolusTargets.Add(o);
                    }
                    else { }
                }
                else { }
            }

            esxolusEvents.Enqueue(new DestroyTargetEvent(esxolusTargets));
            halkEvents.Enqueue(new DestroyTargetEvent(halkTargets));

            base.Initialize();

            foreach (Player player in Space394Game.GameInstance.Players)
            {
                while (!(player.CurrentState is TeamSelectPlayerState))
                {
                    player.nextState(); // From login to team select
                }
            }

            SoundManager.StartGameMusic();
        }

        public override SceneObject addSceneObject(SceneObject add)
        {
            if (add.CollisionBase != null)
            {
                collisionManager.addToCollisionList(add.CollisionBase);
            }
            else { }
            return base.addSceneObject(add);
        }

        public override void removeSceneObject(SceneObject remove)
        {
            if (remove.CollisionBase != null)
            {
                collisionManager.removeFromCollisionList(remove.CollisionBase);
            }
            else { }
            base.removeSceneObject(remove);
        }

        public override void Update(float deltaTime)
        {
            HandleInput();

#if DEBUG
            if (murder)
            {
                foreach (SceneObject item in SceneObjects.Values)
                {
                    if (item is Fighter)
                    {
                        if (!((Fighter)item).PlayerControlled)
                        {
                            item.onCollide(null, HYPERBEAM);
                        }
                        else { }
                    }
                    else { }
                }
                murder = false;
            }
            else { }

            if (waveNow)
            {
                waveTimer = 0;
                waveNow = false;
            }
            else { }

            if (invincibility)
            {
                foreach (SceneObject item in SceneObjects.Values)
                {
                    if (item is Fighter)
                    {
                        if (((Fighter)item).PlayerControlled)
                        {
                            ((Fighter)item).Health = STAR_POWER;
                            ((Fighter)item).SecondaryAmmo = (int)STAR_POWER;
                        }
                        else { }
                    }
                    else { }
                }
                invincibility = false;
            }
            else { }
#endif

            updateAI(deltaTime);
            updateParticles(deltaTime);

            if (currentState == state.game)
            {
                waveReleased = false;
                waveTimer -= deltaTime;
                if (waveTimer <= 0)
                {
                    foreach (SpawnShip ship in spawnShips)
                    {
                        ship.SpawnWave();
                    }
                    waveTimer = WAVE_TIMER;
                    waveReleased = true;
                }
                else { }
            }
            else { }

            collisionManager.Update(deltaTime);

            Turret.updateGlobalTurretTimer(deltaTime);
            AI.updateGlobalAITimer(deltaTime);

            if (esxolusEvents.Count != 0)
            {
                esxolusEvents.Peek().Update(deltaTime);
                if (esxolusEvents.Peek().IsComplete())
                {
                    esxolusEvents.Dequeue();
                }
                else { }
            }
            else { }

            if (halkEvents.Count != 0)
            {
                halkEvents.Peek().Update(deltaTime);
                if (halkEvents.Peek().IsComplete())
                {
                    halkEvents.Dequeue();
                }
                else { }
            }

            base.Update(deltaTime);

            LogCat.updateValue("WaveTimer", "" + waveTimer);
            LogCat.updateValue("Exsolus Spawn Points", "" + esxolusSpawnPointsRemaining);
            LogCat.updateValue("Halk Spawn Points", "" + halkSpawnPointsRemaining);

            if (currentState == state.game)
            {
                //if (esxolusSpawnPointsRemaining <= 0)
                if (esxolusEvents.Count == 0)
                {
                    currentState = state.victory;
                    victor = Ship.Team.Halk;
                    foreach (Player player in Space394Game.GameInstance.Players)
                    {
                        player.CurrentState = new ScoreboardPlayerState(player);
                    }
                }
                else if (halkEvents.Count == 0) //(halkSpawnPointsRemaining <= 0)
                {
                    currentState = state.victory;
                    victor = Ship.Team.Esxolus;
                    foreach (Player player in Space394Game.GameInstance.Players)
                    {
                        player.CurrentState = new ScoreboardPlayerState(player);
                    }
                }
                else { }
            } // Let's only have one winner, huh
            else { }

            if (currentState == state.loading)
            {
                currentState = state.game;
            }
            else { }
        }

        public override void Draw()
        {
            getGraphics().Clear(Color.Black);

            int numberOfPlayers = Space394Game.GameInstance.NumberOfPlayers;
            for (int i = 0; i < numberOfPlayers; i++)
            {
                if (Space394Game.GameInstance.Players[i].IsActive)
                {
                    Space394Game.GameInstance.GraphicsDevice.Viewport = Space394Game.GameInstance.Players[i].PlayerCamera.ViewPort;
                    collisionManager.debugDraw(Space394Game.GameInstance.Players[i].PlayerCamera);
                }
                else { }
            }

            base.Draw();

            Space394Game.GameInstance.GraphicsDevice.Viewport = Space394Game.GameInstance.DefaultViewPort;

            if (currentState == state.victory)
            {
                SpriteBatch spriteBatch = Space394Game.GameInstance.SpriteBatch;

                Space394Game.GameInstance.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            }
            else { }
        }

        public void updateAI(float deltaTime)
        {
            lock (aiList)
            {
                foreach (AI ai in aiList)
                {
                    ai.Update(deltaTime);
                }
            }
            Thread.Sleep(0);

            lock (turretList)
            {
                foreach (Turret turret in turretList)
                {
                    turret.Update(deltaTime);
                }
            }
            Thread.Sleep(0);
        }

        public void updateParticles(float deltaTime)
        {
            lock (particleGeneratorList)
            {
                /*foreach (ParticleGenerator generator in particleGeneratorList)
                {
                    generator.Update(deltaTime);
                }*/
            }
            //Thread.Sleep(0);
        }

        private void HandleInput()
        {
#if DEBUG
            if (InputManager.isDebugLeftPressed())
            {
                killEveryone();
            }
            else { }

            if (InputManager.isDebugUpPressed())
            {
                releaseTheWave();
            }
            else { }

            if (InputManager.isDebugDownPressed())
            {
                star();
            }
            else { }

            if (InputManager.isDebugRightPressed())
            {
                readyToExit = true;
            }
            else { }
#endif

            if (currentState == state.victory)
            {
                if (InputManager.isConfirmationKeyPressed()
                    || InputManager.isSuperConfirmationKeyPressed())
                {
                    AbortThreads();
                    readyToExit = true;
                }
                else { }
            }
            else { }
        }

        public override void Exit(Scene nextScene)
        {
            SoundManager.StopMusic();

            foreach (Player player in Space394Game.GameInstance.Controllers)
            {
                player.CurrentState = new MenuPlayerState(player);
            }
            SceneObjects.Clear();
            base.Exit(nextScene);
        }

        public override Space394Game.sceneEnum GetNextScene()
        {
            return Space394Game.sceneEnum.MainMenuScene;
        }
    }
}
