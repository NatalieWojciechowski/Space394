using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Space394.Collision;
using Space394.AIControl;
using Space394.PlayerStates;
using Space394.Particles;
using Space394.Scenes;
using BEPUphysics.Collidables.MobileCollidables;
using BEPUphysics.Collidables;
using BEPUphysics.NarrowPhaseSystems.Pairs;

namespace Space394.SceneObjects
{
    public abstract class Fighter : Ship
    {
        protected AI myAI;
        public AI MyAI
        {
            get { return myAI; }
        }

        protected Player myPlayer;
        public Player MyPlayer
        {
            get
            {
                if (myPlayer != null)
                {
                    return myPlayer;
                }
                else
                {
                    return null;
                }
            }
            set { myPlayer = value; }
        }

        protected Vector3[] laserOffsets;
        protected int laserIndex = 0;

        protected float FIRE_RATE;
        public float FireRate
        {
            get { return FIRE_RATE; }
        }
        protected float fireTimer;

        // Special is Secondary attacks
        protected float SPECIAL_RATE;
        public float SpecialRate
        {
            get { return SPECIAL_RATE; }
        }
        protected float specialTimer;

        /* Ranges are usually checked against DistanceSquared values */
        protected float PRIMARY_RANGE;
        public float PrimaryRange
        {
            get { return PRIMARY_RANGE; }
        }
        protected float SECONDARY_RANGE;
        public float SecondaryRange
        {
            get { return SECONDARY_RANGE; }
        }

        protected Vector3[] secondaryAttackPositions;
        protected Quaternion[] secondaryAttackForward;

        protected float INTEREST_TIME = 15f;
        public float InterestTime
        {
            get { return INTEREST_TIME; }
        }
        protected float interestTimer;

        private bool playerControlled = false;
        public bool PlayerControlled
        {
            get { return playerControlled; }
            set { playerControlled = value; }
        }

        private Vector3 velocity;
        public Vector3 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }

        protected float accelerationRate = 100000.0f;
        private Vector3 acceleration = Vector3.Zero;
        public Vector3 Acceleration
        {
            get { return acceleration; }
            set { acceleration = value; }
        }

        protected float jerkRate = 100000.0f;
        private Vector3 jerk = Vector3.Zero;
        public Vector3 Jerk
        {
            get { return jerk; }
            set { jerk = value; }
        }

        private float rotationSpeed = 1.0f;
        public float RotationSpeed
        {
            get { return rotationSpeed; }
        }
        // No reason for a setter

        private float zSpeed = 20.0f; // The speed the ship is moving at
        public float ZSpeed
        {
            get { return zSpeed; }
            set { zSpeed = value; }
        }

        protected float normalZSpeed = 30.0f; // The speed the ship should move at normally
        public float NormalZSpeed
        {
            get { return normalZSpeed; }
        }

        protected float maxZSpeed = 50.0f;
        public float MaxZSpeed
        {
            get { return maxZSpeed; }
        }

        protected float minZSpeed = 10.0f;
        public float MinZSpeed
        {
            get { return minZSpeed; }
        }

        protected float rollBreak;
        protected float rollSpeed = 0;
        public float RollSpeed
        {
            get { return rollSpeed; }
            // No setter
        }
        protected float rollAccel;
        protected float rollCap;

        protected float pitchBreak;
        protected float pitchSpeed = 0;
        public float PitchSpeed
        {
            get { return pitchSpeed; }
            // No setter
        }
        protected float pitchAccel;
        protected float pitchCap;

        protected float yawBreak;
        private float yawSpeed = 0;
        public float YawSpeed
        {
            get { return yawSpeed; }
            // No setter
        }
        protected float yawAccel;
        protected float yawCap;

        protected SpawnShip home;

        protected DustParticleGenerator dustGenerator;
        public DustParticleGenerator DustGenerator
        {
            get { return dustGenerator; }
        }

        protected List<EngineTrailParticleGenerator> trailGenerators;
        public List<EngineTrailParticleGenerator> TrailGenerators
        {
            get { return trailGenerators; }
        }

        protected Random random;
        private const float jitterInfluencePosition = 10.0f;
        private const float jitterInfluenceRotation = 1.0f;
        private Vector3 minJitterPosition;
        private Vector3 maxJitterPosition;
        private Quaternion minJitterRotation;
        private Quaternion maxJitterRotation;
        private Vector3 minJitterPositionVelocity;
        private Vector3 maxJitterPositionVelocity;
        private Quaternion minJitterRotationVelocity;
        private Quaternion maxJitterRotationVelocity;

        // Minimum heat level for this fighter after cooling
        protected float baseHeat;
        public float BaseHeat
        {
            get { return baseHeat; }
        }
        // Current heat level
        protected float heat;
        public float Heat
        {
            get { return heat; }
        }
        //  Maximum heat level for this fighter before damage
        protected float overheatHeat;
        public float OverheatHeat
        {
            get { return overheatHeat; }
        }
        public float getOverheatHeat() { return overheatHeat; }
        protected float heatingRate;
        protected float coolingRate;
        protected float heatDamageRate;
        protected float heatWarningThreshold;
        public float HeatWarningThreshold
        {
            get { return heatWarningThreshold; }
        }
        protected float heatDamageThreshold;
        public float HeatDamageThreshold
        {
            get { return heatDamageThreshold; }
        }

        // More damage for a sharper hit
        protected float reflectionDamageRate = 0.0f;

        public const float SUPER_SIMPLE_DRAW_DIST = 1000000000000f;

        private const float DECAY = 0.03f;

        private bool justReflected = false;
        public bool JustReflected
        {
            get { return justReflected; }
        }

        private bool rotationChanged = false;
        private bool velocityUpdated = false;
        public bool updateVelocity() { return (velocityUpdated = true); }

        public Fighter(long _uniqueId, Vector3 _position, Quaternion _rotation, Team _team, SpawnShip _home)
            : base(_uniqueId, _position, _rotation, _team)
        {
            myAI = new AI(this);

            SHIELD_RECOVER_RATE /= 5;

            Damage = 0.5f;

            MAX_HEALTH = 5;
            Health = MaxHealth;

            MAX_SHIELDS = 5;
            Shields = MaxShields;

            PRIMARY_RANGE   = 1000000f;
            SECONDARY_RANGE = 800000f;

            INTEREST_TIME = 20;
            interestTimer = INTEREST_TIME;

            maxJitterPosition = new Vector3(0.5f, 0.5f, 0.5f);
            minJitterPosition = -maxJitterPosition;

            maxJitterRotation = Quaternion.Identity;
            minJitterRotation = maxJitterRotation;

            maxJitterPositionVelocity = new Vector3(0.5f, 0.5f, 0.5f);
            minJitterPositionVelocity = -maxJitterPositionVelocity;

            maxJitterRotationVelocity = Quaternion.Identity;
            minJitterRotationVelocity = maxJitterRotationVelocity;

            home = _home;

            random = new Random((int)_uniqueId);

            dustGenerator = new DustParticleGenerator(this);
            trailGenerators = new List<EngineTrailParticleGenerator>();
            Active = false;
        }

        // Fighters can be player or AI controlled
        public override void Update(float deltaTime)
        {
            PreviousPosition = Position;
            PreviousRotation = Rotation;

            if (!PlayerControlled)
            {
                myAI.Update(deltaTime);
                Health -= DECAY * deltaTime;
                LogCat.updateValue("Health", "" + Health);
            }
            else
            {
                dustGenerator.Update(deltaTime);
            }

            heat -= coolingRate * deltaTime;
            heat = MathHelper.Clamp(heat, baseHeat, overheatHeat);

            if (heat >= heatDamageThreshold)
            {
                Health -= heatDamageRate * deltaTime;
            }
            else { }

            Quaternion roll;
            if (rollSpeed > 0)
            {
                rollSpeed = MathHelper.Max(rollSpeed - (rollBreak * deltaTime), 0);
            }
            else if (rollSpeed < 0)
            {
                rollSpeed = MathHelper.Min(rollSpeed + (rollBreak * deltaTime), 0);
            }
            else { }

            Quaternion pitch;
            if (pitchSpeed > 0)
            {
                pitchSpeed = MathHelper.Max(pitchSpeed - (pitchBreak * deltaTime), 0);
            }
            else if (pitchSpeed < 0)
            {
                pitchSpeed = MathHelper.Min(pitchSpeed + (pitchBreak * deltaTime), 0);
            }
            else { }

            Quaternion yaw;
            if (yawSpeed > 0)
            {
                yawSpeed = MathHelper.Max(yawSpeed - (yawBreak * deltaTime), 0);
            }
            else if (yawSpeed < 0)
            {
                yawSpeed = MathHelper.Min(yawSpeed + (yawBreak * deltaTime), 0);
            }
            else { }

            if (rollSpeed != 0 || pitchSpeed != 0 || yawSpeed != 0)
            {
                roll = Quaternion.CreateFromAxisAngle(Vector3.UnitZ, MathHelper.ToRadians(rollSpeed));
                pitch = Quaternion.CreateFromAxisAngle(Vector3.UnitX, MathHelper.ToRadians(pitchSpeed));
                yaw = Quaternion.CreateFromAxisAngle(Vector3.UnitY, MathHelper.ToRadians(yawSpeed));
                Rotation = Rotation * yaw * pitch * roll;
                rotationChanged = true;
            }
            else { }

            acceleration += jerk * deltaTime;

            if (rotationChanged || acceleration.Z != 0 || velocity.Z == 0 || velocityUpdated)
            {
                Velocity = Vector3.Transform(Vector3.Clamp(((Vector3.Backward * Velocity.Length()) + (acceleration * deltaTime)), Vector3.Backward * minZSpeed, Vector3.Backward * maxZSpeed), Rotation);
                acceleration = Vector3.Zero;
                jerk = Vector3.Zero;
            }
            else { }
            rotationChanged = false;
            velocityUpdated = false;

            //setVelocity(getVelocity() + (getAcceleration() * deltaTime));
            Position = Position + (Velocity * deltaTime);

            // Create Piloting Jitter effect for player
            if (playerControlled)
            {
                float x = (float)random.NextDouble(); if (random.Next() % 2 == 0) { x *= -1; } else { }
                float y = (float)random.NextDouble(); if (random.Next() % 2 == 0) { y *= -1; } else { }
                float z = (float)random.NextDouble(); if (random.Next() % 2 == 0) { z *= -1; } else { }
                jitterPositionVelocity += new Vector3(x, y, z) * jitterInfluencePosition * deltaTime;
                jitterPositionVelocity = Vector3.Clamp(jitterPositionVelocity, minJitterPositionVelocity, maxJitterPositionVelocity);
                jitterPosition += jitterPositionVelocity * deltaTime;
                jitterPosition = Vector3.Clamp(jitterPosition, minJitterPosition, maxJitterPosition);
                float rotx = (float)random.NextDouble(); if (random.Next() % 2 == 0) { rotx *= -1; } else { }
                float roty = (float)random.NextDouble(); if (random.Next() % 2 == 0) { roty *= -1; } else { }
                float rotz = (float)random.NextDouble(); if (random.Next() % 2 == 0) { rotz *= -1; } else { }
                jitterRotationVelocity *= Quaternion.CreateFromYawPitchRoll(rotx, roty, rotz) * jitterInfluenceRotation;
                jitterRotation *= jitterRotationVelocity * deltaTime;
            }
            else { }

            fireTimer -= deltaTime;
            specialTimer -= deltaTime;

            justReflected = false;

            base.Update(deltaTime);

            foreach (EngineTrailParticleGenerator generator in trailGenerators)
            {
                generator.Update(deltaTime);
            }
        }

        public override bool Active
        {
            set
            {
                foreach (EngineTrailParticleGenerator generator in trailGenerators)
                {
                    generator.Active = value;
                }
                base.Active = value;
            }
        }

        public override void Draw(GameCamera camera)
        {
            // Not being checked until withint base.draw for active
            if (Active)
            {
                if (Vector3.DistanceSquared(camera.Position, Position) > SUPER_SIMPLE_DRAW_DIST)
                {
                    if (Model != null)
                    {
                        Model = null;
                        HUDUnit.ModelLevel = 0;
                    }
                    else { }
                }
                else if (Vector3.DistanceSquared(camera.Position, Position) > SimpleDrawDist)
                {
                    if (Model != farModel)
                    {
                        Model = farModel;
                        HUDUnit.ModelLevel = 2;
                    }
                    else { }
                }
                else
                {
                    if (Model != closeModel)
                    {
                        Model = closeModel;
                        HUDUnit.ModelLevel = 1;
                    }
                    else { }
                }
            }
            else { }

            base.Draw(camera);
        }

        public override void DrawReticule(GameCamera camera)
        {
            HUDUnit.DrawRescaling(camera, ((PlayerCamera)camera).PlayerShip.Rotation);
        }

        public void DrawTrails(GameCamera camera)
        {
            if (Active)
            {
                foreach (EngineTrailParticleGenerator generator in trailGenerators)
                {
                    generator.Draw(camera);
                }
                dustGenerator.Draw(camera);
            }
            else { }
        }

        public void cleanParticleLists()
        {
            foreach (EngineTrailParticleGenerator generator in trailGenerators)
            {
                generator.cleanupList();
            }
        }

        public Quaternion setRotation(float yaw, float pitch, float roll)
        {
            base.Rotation = Quaternion.CreateFromYawPitchRoll(yaw, pitch, roll);
            return Rotation;
        }

        public void respawn(Vector3 _position, Quaternion _rotation)
        {
            yawSpeed = 0;
            pitchSpeed = 0;
            rollSpeed = 0;
            Active = true;
            Position = _position;
            Rotation = _rotation;
            QueuedRemoval = false;
            Health = MaxHealth;
            Shields = MaxShields;
            SecondaryAmmo = MaxSecondaryAmmo;
        }

        public override void collisionEvent(EntityCollidable eCollidable, Collidable collidable, CollidablePairHandler pairHandler)
        {
            if (((CollisionBase)collidable.Tag).Parent != null)
            {
                if (((CollisionBase)collidable.Tag).Parent is Ship)
                {
                    if (this.PlayerControlled)
                    {
                        float roll = getRoll();
                        Quaternion rollQ = Quaternion.CreateFromYawPitchRoll(0, 0, roll);
                        Quaternion rotation = Rotation / rollQ;
                        Position = pairHandler.Contacts.First().Contact.Position;
                        Rotation = ((AdjustRotationNoLimit(Vector3.Transform(Vector3.Forward, rotation),
                            Vector3.Reflect(Vector3.Forward, pairHandler.Contacts.First().Contact.Normal), //Vector3.Reflect(Vector3.Transform(Vector3.Backward, getRotation()), pairHandler.Contacts.First().Contact.Normal),
                            Vector3.Transform(Vector3.Up, rotation))) * rollQ);
                        Vector3 forward = Velocity;
                        forward.Normalize();
                        onCollide(this, Math.Abs(Quaternion.Dot(Rotation, PreviousRotation) * reflectionDamageRate)); // To ensure correct flow of damage
                        justReflected = true;
                        if (playerControlled)
                        {
                            LogCat.updateValue("Velocity", "" + Velocity);
                            LogCat.updateValue("Normal", "" + pairHandler.Contacts.First().Contact.Normal);
                            LogCat.updateValue("Reflection angle", "" + Vector3.Dot(forward, pairHandler.Contacts.First().Contact.Normal));
                        }
                        else { }
                    }
                    else
                    {
                        if (((CollisionBase)collidable.Tag).Parent is SpawnShip)
                        {
                            /* float roll = getRoll();
                            Quaternion rollQ = Quaternion.CreateFromYawPitchRoll(0, 0, roll);
                            Quaternion rotation = getRotation() / rollQ;
                            setPosition(pairHandler.Contacts.First().Contact.Position);
                            setRotation((AdjustRotationNoLimit(Vector3.Transform(Vector3.Forward, rotation),
                                Vector3.Reflect(Vector3.Forward, pairHandler.Contacts.First().Contact.Normal), //Vector3.Reflect(Vector3.Transform(Vector3.Backward, getRotation()), pairHandler.Contacts.First().Contact.Normal),
                                Vector3.Transform(Vector3.Up, rotation))) * rollQ);
                            Vector3 forward = getVelocity();
                            forward.Normalize();
                            onCollide(this, Math.Abs(Quaternion.Dot(getRotation(), getPreviousRotation()) * reflectionDamageRate)); // To ensure correct flow of damage
                            justReflected = true; */

                            onCollide(this, 10000f);
                        }
                        else // Let the other object deal damage to me 
                        {
                            // setPosition(pairHandler.Contacts.First().Contact.Position);
                            // , reflectionDamageRate);
                        }
                    }
                    Velocity = Vector3.Transform(Velocity, Rotation);
                    rotationChanged = true;
                }
                else { }
            }
            else { }
            base.collisionEvent(eCollidable, collidable, pairHandler);
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
            Shields -= damage;
            if (Shields < 0)
            {
                Health += Shields;
                Shields = 0;
            }
            else { }

            if (Health <= 0)
            {
                QueuedRemoval = true; // Let's clean up some of these ships
                CollisionBase.Active = false; // No more onCollides
                if (PlayerControlled)
                {
                    dustGenerator.Active = false;
                }
                else { }

                Attackable = false;
            }
            else 
            {
                if (PlayerControlled)
                {
                    MyPlayer.PlayerHUD.TakingHit = true;
                }
                else { }
            }
        }

        public virtual void firePrimary()
        {
            if (Active)
            {
                if (fireTimer <= 0)
                {
                    SoundManager.playLaserFire(Position);
                    Quaternion shipRotation = Rotation;
                    Vector3 shipPosition = Position;

                    Vector3 laserPosition = shipPosition + jitterPosition +
                            Vector3.Transform(laserOffsets[laserIndex], shipRotation);
                    laserIndex++;
                    if (laserIndex >= laserOffsets.Length)
                    {
                        laserIndex = 0;
                    }
                    else { }
                    Laser rLaser = SceneObjectFactory.createNewLaser(laserPosition, shipRotation, this);
                    if (ShipTeam == Team.Esxolus)
                    {
                        rLaser.laserType = Laser.laser_type.esxolus;
                    }
                    else // Halk
                    {
                        rLaser.laserType = Laser.laser_type.halk;
                    }
                    fireTimer = FireRate;
                }
                else { }
            }
            else { }
        }

        public abstract void fireSecondary();

        public abstract void returnToSpawnShip();

        public void rollShip(float degree, float deltaTime)
        {
            degree *= rollAccel * deltaTime;
            rollSpeed = MathHelper.Clamp(rollSpeed + degree, -rollCap, rollCap);
        }

        public void pitchShip(float degree, float deltaTime)
        {
            degree *= pitchAccel * deltaTime;
            pitchSpeed = MathHelper.Clamp(pitchSpeed + degree, -pitchCap, pitchCap);
        }

        public void yawShip(float degree, float deltaTime)
        {
            degree *= yawAccel * deltaTime;
            yawSpeed = MathHelper.Clamp(yawSpeed + degree, -yawCap, yawCap);
        }

        public Vector3 accelerate(float deltaTime)
        {
            acceleration.Z += accelerationRate * deltaTime;
            heat += heatingRate * deltaTime;
            return acceleration;
        }

        public Vector3 decelerate(float deltaTime) 
        { 
            acceleration.Z -= accelerationRate * deltaTime;
            heat += heatingRate * deltaTime;
            return acceleration;
        }
    }
}
