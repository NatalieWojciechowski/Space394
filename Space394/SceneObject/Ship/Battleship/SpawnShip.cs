using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Space394.Scenes;
using Space394.Collision;
using Microsoft.Xna.Framework.Graphics;
using BEPUphysics.Collidables.MobileCollidables;
using BEPUphysics.Collidables;
using BEPUphysics.NarrowPhaseSystems.Pairs;
using BEPUphysics.Entities.Prefabs;

namespace Space394.SceneObjects
{
    public abstract class SpawnShip : Battleship
    {
        private Queue<AssaultFighter> availableAssaultFighters;
        private Queue<Bomber> availableBombers; 
        private Queue<Interceptor> availableInterceptors;

        private Queue<AssaultFighter> totalAvailableAssaultFighters;
        private Queue<Bomber> totalAvailableBombers;
        private Queue<Interceptor> totalAvailableInterceptors;

        protected int MAX_BOMBERS;
        protected int MAX_ASSAULT_FIGHTERS;
        protected int MAX_INTERCEPTORS;

        protected int INITIAL_BOMBERS;
        protected int INITIAL_ASSAULT_FIGHTERS;
        protected int INITIAL_INTERCEPTORS;

        protected int BOMBERS;
        protected int ASSAULT_FIGHTERS;
        protected int INTERCEPTORS;

        protected int launchedBombers;
        protected int launchedAssaultFighters;
        protected int launchedInterceptors;

        protected int WAVES_TO_LAUNCH;
        protected int wavesLaunched;

        protected Vector3[] cameraPositions;
        public Vector3[] CameraPositions
        {
            get { return cameraPositions; }
        }

        protected Vector3[] cameraViews;
        public Vector3[] CameraViews
        {
            get { return cameraViews; }
        }

        protected Vector3[] cameraUps;
        public Vector3[] CameraUps
        {
            get { return cameraUps; }
        }

        protected Vector3[] assaultFighterPositions;
        protected Quaternion assaultFighterForward;

        protected Vector3[] bomberPositions;
        protected Quaternion bomberForward;

        protected Vector3[] interceptorPositions;
        protected Quaternion interceptorForward;

        protected List<SpawnShipPiece> shipPieces;
        protected Vector3[] shipPiecePositions;
        public Vector3[] ShipPiecePositions
        {
            get { return shipPiecePositions; }
        }

        protected float DRAW_TIMER = 5.0f;

        public SpawnShip(long _uniqueId, Vector3 _position, Quaternion _rotation, Team _team)
            : base(_uniqueId, _position, _rotation, _team)
        {
            Active = true;
            shipPieces = new List<SpawnShipPiece>();
            shipPiecePositions = new Vector3[] { };

            createNodes();
        }

        #region Initialize
        public void Initialize()
        {
            if (availableAssaultFighters == null)
            {
                totalAvailableAssaultFighters = new Queue<AssaultFighter>();
                availableAssaultFighters = new Queue<AssaultFighter>();
            }
            if (availableBombers == null)
            {
                totalAvailableBombers = new Queue<Bomber>();
                availableBombers = new Queue<Bomber>();
            }
            if (availableInterceptors == null)
            {
                totalAvailableInterceptors = new Queue<Interceptor>();
                availableInterceptors = new Queue<Interceptor>();
            }
            InitalizeShips();
        }

        public void InitalizeShips()
        {
            for (int i = 0; i < MAX_ASSAULT_FIGHTERS; i++)
            {
                totalAvailableAssaultFighters.Enqueue(createAssaultFighter(Vector3.Zero, Quaternion.Identity, ShipTeam));
            }
            for (int i = 0; i < MAX_BOMBERS; i++)
            {
                totalAvailableBombers.Enqueue(createBomber(Vector3.Zero, Quaternion.Identity, ShipTeam));
            }
            for (int i = 0; i < MAX_INTERCEPTORS; i++)
            {
                totalAvailableInterceptors.Enqueue(createInterceptor(Vector3.Zero, Quaternion.Identity, ShipTeam));
            }

            for (int i = 0; i < INITIAL_ASSAULT_FIGHTERS; i++)
            {
                availableAssaultFighters.Enqueue(totalAvailableAssaultFighters.Dequeue());
            }
            for (int i = 0; i < INITIAL_BOMBERS; i++)
            {
                availableBombers.Enqueue(totalAvailableBombers.Dequeue());
            }
            for (int i = 0; i < INITIAL_INTERCEPTORS; i++)
            {
                availableInterceptors.Enqueue(totalAvailableInterceptors.Dequeue());
            } 
        }
        #endregion

        #region AddShip
        public AssaultFighter addAssaultFighter(AssaultFighter _assaultFighter)
        {
            /*if ((totalAvailableAssaultFighters.Count + launchedAssaultFighters) < MAX_ASSAULT_FIGHTERS)
            {
                totalAvailableAssaultFighters.Enqueue(_assaultFighter);
            }
            else { }*/
            return _assaultFighter;
        }

        public Bomber addBomber(Bomber _bomber)
        {
            /*if ((totalAvailableBombers.Count + launchedBombers) < MAX_BOMBERS)
            {
                totalAvailableBombers.Enqueue(_bomber);
            }
            else { }*/
            return _bomber;
        }

        public Interceptor addIntereptor(Interceptor _interceptor)
        {
            /*if ((totalAvailableInterceptors.Count + launchedInterceptors) < MAX_INTERCEPTORS)
            {
                totalAvailableInterceptors.Enqueue(_interceptor);
            }
            else { }*/
            return _interceptor;
        }
        #endregion

        #region Spawn
        public AssaultFighter spawnAssaultFighter(Vector3 position)
        {
            AssaultFighter rAssaultFighter = null;
            if (availableAssaultFighters.Count == 0)
            {
                rAssaultFighter = createAssaultFighter(position, assaultFighterForward, ShipTeam);
            }
            else
            {
                rAssaultFighter = availableAssaultFighters.Dequeue();
                rAssaultFighter.respawn(position, assaultFighterForward);
            }
            launchedAssaultFighters++;

            return ((AssaultFighter)(Space394Game.GameInstance.CurrentScene.addSceneObject(rAssaultFighter)));
        }

        public Bomber spawnBomber(Vector3 position)
        {
            Bomber rBomber = null;
            if (availableBombers.Count == 0)
            {
                rBomber = createBomber(position, bomberForward, ShipTeam); 
            }
            else
            {
                rBomber = availableBombers.Dequeue();
                rBomber.respawn(position, bomberForward);
            }
            launchedBombers++;

            return ((Bomber)(Space394Game.GameInstance.CurrentScene.addSceneObject(rBomber)));
        }

        public Interceptor spawnInterceptor(Vector3 position)
        {
            Interceptor rInterceptor = null;
            if (availableInterceptors.Count == 0)
            {
                rInterceptor = createInterceptor(position, interceptorForward, ShipTeam);
            }
            else
            {
                rInterceptor = availableInterceptors.Dequeue();
                rInterceptor.respawn(position, interceptorForward);
            }
            launchedInterceptors++;

            return ((Interceptor)(Space394Game.GameInstance.CurrentScene.addSceneObject(rInterceptor)));
        }
        #endregion

        #region Create
        public AssaultFighter createAssaultFighter(Vector3 _pos, Quaternion _rot, Team _team)
        {
            return (SceneObjectFactory.createNewAssaultFighter(_pos, _rot, _team, this));
        }

        public Bomber createBomber(Vector3 _pos, Quaternion _rot, Team _team)
        {
            return (SceneObjectFactory.createNewBomber(_pos, _rot, _team, this));
        }

        public Interceptor createInterceptor(Vector3 _pos, Quaternion _rot, Team _team)
        {
            return (SceneObjectFactory.createNewInterceptor(_pos, _rot, _team, this));
        }
        #endregion

        #region Count
        public int availableAssaultFightersCount()
        {
            return availableAssaultFighters.Count();
        }

        public int availableBombersCount()
        {
            return availableBombers.Count();
        }

        public int availableInterceptorsCount()
        {
            return availableInterceptors.Count();
        }
        #endregion

        #region Assign
        public Fighter assignPlayerAssaultFighter()
        {
            Fighter rFighter = null;
            Fighter[] fighters = availableAssaultFighters.ToArray();
            int aAssaultFighters = availableAssaultFighters.Count;
            for (int i = 0; i < aAssaultFighters; i++)
            {
                rFighter = fighters[i];
                rFighter.Position = assaultFighterPositions[i];
                rFighter.Rotation = assaultFighterForward;
                if (!rFighter.PlayerControlled)
                {
                    break;
                }
                else { }
            }
            if (rFighter != null)
            {
                rFighter.PlayerControlled = true;
            }
            else { }
            return rFighter;
        }

        public Fighter assignPlayerBomber()
        {
            Fighter rFighter = null;
            Fighter[] fighters = availableBombers.ToArray();
            int aBombers = availableBombers.Count;
            for (int i = 0; i < aBombers; i++)
            {
                rFighter = fighters[i];
                rFighter.Position = bomberPositions[i];
                rFighter.Rotation = bomberForward;
                if (!rFighter.PlayerControlled)
                {
                    break;
                }
                else { }
            }
            if (rFighter != null)
            {
                rFighter.PlayerControlled = true;
            }
            else { }
            return rFighter;
        }

        public Fighter assignPlayerInterceptor()
        {
            Fighter rFighter = null;
            Fighter[] fighters = availableInterceptors.ToArray();
            int aInterceptors = availableInterceptors.Count;
            for (int i = 0; i < aInterceptors; i++)
            {
                rFighter = fighters[i];
                rFighter.Position = interceptorPositions[i];
                rFighter.Rotation = interceptorForward;
                if (!rFighter.PlayerControlled)
                {
                    break;
                }
                else { }
            }
            if (rFighter != null)
            {
                rFighter.PlayerControlled = true;
            }
            else { }
            return rFighter;
        }
        #endregion

        public override void Update(float deltaTime)
        {
            foreach (SpawnShipPiece piece in shipPieces)
            {
                piece.Update(deltaTime);
            }
            base.Update(deltaTime);
        }

        public override void onCollide(SceneObject caller, float damage)
        {
            base.onCollide(caller, damage);

            if (Health <= 0)
            {
                // Check is added in for missles and when multiple 
                // shots hit the spawnship before it dissapears.
                if (Active)
                {
                    ((GameScene)Space394Game.GameInstance.CurrentScene).spawnPointDecrease(ShipTeam);
                    //((GameScene)(Space394Game.GameInstance.CurrentScene)).removeSpawnShip(this);
                    foreach (SpawnShipPiece piece in shipPieces)
                    {
                        piece.onCollide(this, piece.MaxHealth);
                    }
                }
                else { }
            }
            else { }
        }

        public void SpawnWave()
        {
            if (Health > 0)
            {
                if (wavesLaunched < WAVES_TO_LAUNCH)
                {
                    int i = 0;
                    foreach (AssaultFighter fighter in availableAssaultFighters)
                    {
                        fighter.respawn(assaultFighterPositions[i], assaultFighterForward);
                        i++;
                    }
                    while (availableAssaultFighters.Count > 0)
                    {
                        Space394Game.GameInstance.CurrentScene.addSceneObject(availableAssaultFighters.Dequeue());
                    }

                    i = 0;
                    foreach (Bomber fighter in availableBombers)
                    {
                        fighter.respawn(bomberPositions[i], bomberForward);
                        i++;
                    }
                    while (availableBombers.Count > 0)
                    {
                        Space394Game.GameInstance.CurrentScene.addSceneObject(availableBombers.Dequeue());
                    }

                    i = 0;
                    foreach (Interceptor fighter in availableInterceptors)
                    {
                        fighter.respawn(interceptorPositions[i], interceptorForward);
                        i++;
                    }
                    while (availableInterceptors.Count > 0)
                    {
                        Space394Game.GameInstance.CurrentScene.addSceneObject(availableInterceptors.Dequeue());
                    }
                    wavesLaunched++;
                    LogCat.updateValue("Waves Launched", "" + wavesLaunched);

                    int aAssaultFighters = totalAvailableAssaultFighters.Count;
                    for (int j = 0; j < ASSAULT_FIGHTERS && aAssaultFighters > 0; j++)
                    {
                        availableAssaultFighters.Enqueue(totalAvailableAssaultFighters.Dequeue());
                    }

                    int aBombers = totalAvailableBombers.Count;
                    for (int j = 0; j < BOMBERS && aBombers > 0; j++)
                    {
                        availableBombers.Enqueue(totalAvailableBombers.Dequeue());
                    }

                    int aInterceptors = totalAvailableInterceptors.Count;
                    for (int j = 0; j < INTERCEPTORS && aInterceptors > 0; j++)
                    {
                        availableInterceptors.Enqueue(totalAvailableInterceptors.Dequeue());
                    }
                }
                else { }
            }
            else { }
        }

        public override void onAddToScene(Scene scene)
        {
            if (scene is GameScene)
            {
                ((GameScene)scene).addSpawnShip(this);
                ((GameScene)scene).spawnPointIncrease(ShipTeam);

                int count = shipPieces.Count;
                Vector3 shipPos = Position;
                if (shipPiecePositions.Count() > 0)
                {
                    for (int i = 0; i < count; i++)
                    {
                        shipPieces[i].Active = true;
                        ((GameScene)scene).CollisionManager.addToCollisionList(shipPieces[i].CollisionBase);
                        shipPieces[i].PiecePosition = shipPos + Vector3.Transform(shipPiecePositions[i], shipPieces[i].Rotation);
                    }
                }
                else // Create them from the meshes
                {
                    SpawnShipPiece tmpPiece;
                    for (int i = 0; i < count; i++)
                    {
                        tmpPiece = shipPieces[i];
                        tmpPiece.Active = true;
                        ((GameScene)scene).CollisionManager.addToCollisionList(tmpPiece.CollisionBase);
                        tmpPiece.PiecePosition = shipPos + Vector3.Transform(tmpPiece.Position, tmpPiece.Rotation);
                    }
                }

                count = LocalNodes.Count;
                if(ShipTeam == Team.Esxolus)
                {
                    for (int i = 0; i < count; i++)
                    {
                        ((GameScene)scene).EsxolusCapitalNodes.Add(LocalNodes[i]);
                    }
                }
                else
                {
                    for (int i = 0; i < count; i++)
                    {
                        ((GameScene)scene).HalkCapitalNodes.Add(LocalNodes[i]);
                    }
                }

                for (int i = 0; i < ASSAULT_FIGHTERS; i++)
                {
                    ((GameScene)scene).assaultFightersIncrease(ShipTeam);
                }

                for (int i = 0; i < BOMBERS; i++)
                {
                    ((GameScene)scene).bombersIncrease(ShipTeam);
                }

                for (int i = 0; i < INTERCEPTORS; i++)
                {
                    ((GameScene)scene).interceptorsIncrease(ShipTeam);
                }
            }
            else { }

            base.onAddToScene(scene);
        }

        public override void Draw(GameCamera camera)
        {
            /*bool drawPieces = false;
            if (((PlayerCamera)camera).PlayerShip != null)
            {
                drawPieces = true;
            }
            else { }*/

            foreach (SpawnShipPiece piece in shipPieces)
            {
                piece.Draw(camera);
                /*if (drawPieces && piece.Health > 0)
                {
                    piece.drawPieceReticle(camera);
                }
                else { }*/
            }
        }

        public override void DrawReticule(GameCamera camera)
        {
            base.DrawReticule(camera);
            foreach (SpawnShipPiece piece in shipPieces)
            {
                if (piece.Health > 0)
                {
                    piece.DrawReticule(camera);
                }
            }
        }
    }

    public class SpawnShipPiece : Battleship
    {
        private SpawnShip parent;
        private Model part;
        private Model collisionMesh;
        private List<Turret> turrets;

        protected HUDUnit pieceHUDUnit;
        public HUDUnit PieceHUDUnit
        {
            get { return pieceHUDUnit; }
        }

        public override Vector3 Position
        {
            get { return piecePosition; }
        }

        private Vector3 piecePosition;
        public Vector3 PiecePosition
        {
            get { return piecePosition; }
            set
            {
                piecePosition = value;
                pieceHUDUnit.Position = value;
            }
        }

        private Quaternion pieceRotation;

        public SpawnShipPiece(long _uniqueId, Vector3 _position, Quaternion _rotation, Team _team, 
            Model _part, Model _destroyed, Model _collisionMesh, float _health, List<Turret> _turrets, SpawnShip _parent)
            : base(_uniqueId, _position, _rotation, _team)
        {
            MAX_HEALTH = _health;
            Health = MaxHealth;
            destroyedModel = _destroyed;
            collisionMesh = _collisionMesh;
            // collisionMesh = ContentLoadManager.loadModel("Models/box");
            part = _part;
            Model = part;
            parent = _parent;
            turrets = _turrets;
            CollisionBase = new CollisionMesh(collisionMesh, Position, Rotation);
            CollisionBase.Active = true;
            CollisionBase.addCollisionEvent(collisionEvent);
            CollisionBase.Parent = this;

            if (team == Team.Esxolus)
            {
                ((MobileMesh)CollisionBase.getPhysicsCollider()).CollisionInformation.CollisionRules.Group = EsxolusMeshShipGroup;
            }
            else
            {
                ((MobileMesh)CollisionBase.getPhysicsCollider()).CollisionInformation.CollisionRules.Group = HalkMeshShipGroup;
            }

            pieceRotation = Rotation;
            piecePosition = Position;

            int hudScale = 2;
            pieceHUDUnit = new HUDUnit(this, 10 * hudScale, 4 * hudScale, 1 * hudScale);

            foreach (Turret turret in turrets)
            {
                turret.Parent = this;
            }
        }

        public override void DrawReticule(GameCamera camera)
        {
            drawPieceReticle(camera);
        }

        public void drawPieceReticle(GameCamera camera)
        {
            pieceHUDUnit.DrawRescaling(camera, (((PlayerCamera)camera).PlayerShip.Rotation));
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
        }

        public override void onCollide(SceneObject caller, float damage)
        {
            if (Health > 0 && caller != null && caller != this)
            {
                if (caller is Projectile && ((Projectile)caller).Parent is Fighter && ((Fighter)((Projectile)caller).Parent).PlayerControlled) // Projectile hit
                {
                    ((Fighter)((Projectile)caller).Parent).MyPlayer.PlayerHUD.DrawConfirmed = true;
                }
                else if (caller is Fighter && ((Fighter)caller).PlayerControlled) // Straight collision
                {
                    ((Fighter)caller).MyPlayer.PlayerHUD.DrawConfirmed = true;
                }
                else { }
            }
            else { }
            Shields = parent.Shields = parent.Shields - damage;
            float prevHealth = Health;
            if (Shields < 0)
            {
                Health += Shields;
                parent.Shields = 0;
            }
            else { }

            if (Health <= 0 && prevHealth > 0)
            {
                onDie();
                SceneObjectFactory.createMassiveExplosion(caller.Position, Rotation);
                if (parent.Health > 0 && caller != parent)
                {
                    parent.onCollide(this, MaxHealth);
                }
                else { }
            }
            else { }
        }

        // Called when the main ship dies
        public void onDie()
        {
            //setQueuedRemoval(true);
            //setAttackable(false);
            //collisionBase.setActive(false); // No more onCollides
            foreach (Turret turret in turrets)
            {
                turret.onCollide(this, 100);
                turret.QueuedRemoval = true;
            }

            // Clean up nodes from the list
            int count = LocalNodes.Count;
            GameScene scene = (GameScene)Space394Game.GameInstance.CurrentScene;
            if (ShipTeam == Team.Esxolus)
            {
                for (int i = 0; i < count; i++)
                {
                    (scene).EsxolusCapitalNodes.Remove(LocalNodes[i]);
                }
            }
            else
            {
                for (int i = 0; i < count; i++)
                {
                    (scene).HalkCapitalNodes.Remove(LocalNodes[i]);
                }
            }
        }
    }
}
