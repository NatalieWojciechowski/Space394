using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Space394.Collision;
using Space394.Scenes;
using Microsoft.Xna.Framework.Graphics;
using BEPUphysics.Entities.Prefabs;

namespace Space394.SceneObjects
{
    public abstract class Turret : SceneObject
    {
        public float MAX_HEALTH = 5;

        protected Vector3 fireConeNormalVector;
        protected float fireConeAngle;

        protected CollisionSphere detectionSphere;

        protected Ship.Team team;

        protected Fighter target = null;

        protected float fireTimer;
        protected const float FIRE_TIMER = 0.25f;

        protected const float AIM_MULTI = 3;

        protected const int FIRE_RANGE = 7500;
        protected const int FIRE_RANGE_SQ = FIRE_RANGE * FIRE_RANGE;

        private Battleship parent;
        public Battleship Parent
        {
            get { return parent; }
            set { parent = value; }
        }

        protected const float TIMER = 300.0f;     
        protected static float timer = TIMER;

        protected float SLERP_SPEED = 25.0f;

        public enum activationPhase
        {
            first,
            second,
            third,
        };
        private static activationPhase currentPhase = activationPhase.first;
        private static activationPhase nextAssignedPhase = activationPhase.first;

        private activationPhase assignedPhase;

        public Turret(long _uniqueId, Vector3 _position, Quaternion _rotation, String _modelFile, Vector3 _fireConeNormal, float _fireConeAngle, Battleship _parent)
            : base(_uniqueId, _position, _rotation, _modelFile)
        {
            Health = MAX_HEALTH;

            fireConeNormalVector = _fireConeNormal;
            fireConeNormalVector.Normalize(); // Just in case
            fireConeAngle = MathHelper.ToRadians(_fireConeAngle);

            parent = _parent;

            assignedPhase = nextAssignedPhase;
            switch (nextAssignedPhase)
            {
                case activationPhase.first: nextAssignedPhase = activationPhase.second; break;
                case activationPhase.second: nextAssignedPhase = activationPhase.third; break;
                case activationPhase.third: nextAssignedPhase = activationPhase.first; break;
            }

            MAX_SLERP = SLERP_SPEED;

            Rotation = AdjustRotationNoLimit(Vector3.Backward, fireConeNormalVector, Vector3.Up);

            fireTimer = FIRE_TIMER;

            detectionSphere = new CollisionSphere(_position, FIRE_RANGE);
            detectionSphere.Active = true;
        }

        public override void Update(float deltaTime)
        {
            if (Space394Game.GameInstance.CurrentScene is GameScene)
            {
                if (assignedPhase == currentPhase)
                {
                    fireTimer -= deltaTime;
                    if (target == null || target.Health <= 0 || !(Vector3.DistanceSquared(Position, target.Position) <= FIRE_RANGE_SQ))
                    {
                        using (new ReadLock(GameScene.shipLock))
                        {
                            List<Fighter> eShips = ((GameScene)Space394Game.GameInstance.CurrentScene).getEnemyFighters(team);
                            foreach (Fighter enemy in eShips)
                            {
                                if (Vector3.DistanceSquared(Position, enemy.Position) <= FIRE_RANGE_SQ)
                                {
                                    target = enemy;
                                    break;
                                }
                                else { }
                            }
                        }
                    }
                    else
                    {
                        Vector3 newForward = Vector3.Normalize(Position - (target.Position + (target.Velocity * deltaTime * AIM_MULTI)));
                        // Quaternion desiredRotation = AdjustRotation(Vector3.Forward, newForward, Vector3.Up, deltaTime);
                        Quaternion desiredRotation = AdjustRotationNoLimit(Vector3.Forward, newForward, Vector3.Up);
                        if (Vector3.Dot(newForward, fireConeNormalVector) <= (1 - fireConeAngle))
                        {
                            Rotation = desiredRotation;
                            fire();
                        }
                        else { }
                    }
                }
                else { }

                base.Update(deltaTime);
            }
            else { } // Not game scene
        }

        public static void updateGlobalTurretTimer(float deltaTime)
        {
            timer -= deltaTime;
            //if (timer <= 0)
            {
                switch (currentPhase)
                {
                    case activationPhase.first: currentPhase = activationPhase.second; break;
                    case activationPhase.second: currentPhase = activationPhase.third; break;
                    case activationPhase.third: currentPhase = activationPhase.first; break;
                }
            }
            //else { }
        }

        public override void Draw(GameCamera camera)
        {
#if DEBUG
            Space394Game.GameInstance.GraphicsDevice.Viewport = camera.ViewPort;

            CollisionBase.graphics.basicEffect.World = Matrix.CreateTranslation(Position);
            CollisionBase.graphics.basicEffect.View = camera.ViewMatrix;
            CollisionBase.graphics.basicEffect.Projection = camera.ProjectionMatrix;

            foreach (EffectPass pass in CollisionBase.graphics.basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();

                Space394Game.GameInstance.GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionColor>(
                    PrimitiveType.LineStrip,
                    new VertexPositionColor[] { new VertexPositionColor(Vector3.Zero, Color.Yellow), // Pretransformed
                                                new VertexPositionColor((fireConeNormalVector * FIRE_RANGE), Color.Red) },
                    0,  // vertex buffer offset to add to each element of the index buffer
                    2,  // number of vertices in pointList
                    new short[] {0, 1},  // the index buffer
                    0,  // first index element to read
                    1   // number of primitives to draw
                    );

                break;
            }

            foreach (EffectPass pass in CollisionBase.graphics.basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();

                Space394Game.GameInstance.GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionColor>(
                    PrimitiveType.LineStrip,
                    new VertexPositionColor[] { new VertexPositionColor(Vector3.Zero, Color.Blue), // Pretransformed
                                                new VertexPositionColor((Vector3.Transform(Vector3.Backward, Rotation) * FIRE_RANGE), Color.Green) },
                    0,  // vertex buffer offset to add to each element of the index buffer
                    2,  // number of vertices in pointList
                    new short[] { 0, 1 },  // the index buffer
                    0,  // first index element to read
                    1   // number of primitives to draw
                    );

                break;
            }

            Space394Game.GameInstance.GraphicsDevice.Viewport = Space394Game.GameInstance.DefaultViewPort;

            Space394Game.GameInstance.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
#endif
            if (Vector3.DistanceSquared(camera.Position, Position) < Fighter.SUPER_SIMPLE_DRAW_DIST)
            {
                base.Draw(camera);
            }
            else { }
        }

        public virtual void fire()
        {
            if (fireTimer <= 0)
            {
                Vector3 laserPosition;
                // Vector3 extend = Vector3.Transform(Vector3.Up, getRotation()) * 30;
                laserPosition = Position + jitterPosition;
                Laser rLaser = SceneObjectFactory.createNewLaser(laserPosition, Rotation, parent);
                if (team == Ship.Team.Esxolus)
                {
                    rLaser.laserType = Laser.laser_type.esxolus_turret;
                }
                else // Halk
                {
                    rLaser.laserType = Laser.laser_type.halk_turret;
                }
                fireTimer = FIRE_TIMER;
            }
            else { }
        }

        public override void onCollide(SceneObject caller, float damage)
        {
            Health -= damage;
            if (Health <= 0)
            {
                SceneObjectFactory.createExplosion(Position, Rotation);
                QueuedRemoval = true;
            }
            else { }
        }
    }
}
