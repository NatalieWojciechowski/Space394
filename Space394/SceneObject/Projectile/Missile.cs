using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Space394.Collision;
using BEPUphysics.CollisionRuleManagement;
using Space394.Scenes;
using BEPUphysics.Entities.Prefabs;
using Space394.Particles;

namespace Space394.SceneObjects
{
    public class Missile : Explosive
    {
        private const float WANDER_INFLUENCE = 2.5f;
        private const float MISSILE_SPEED = 4500.0f;

        protected CollisionSphere detectionSphere;
        protected int detectionRange = 100;

        protected MissileTrailParticleGenerator trailGenerator;

        protected CollisionSphere firstTrailingSphere; // Ensure collisions
        protected CollisionSphere secondTrailingSphere; // Ensure collisions
        protected const float TRAILING_DIST = 100;

        public Missile(long _uniqueID, Vector3 _position, Quaternion _rotation, String _model)
            : base(_uniqueID, _position, _rotation, _model)
        {
            Damage = 55.0f;

            projectileSpeed = MISSILE_SPEED;

            trailGenerator = new MissileTrailParticleGenerator(this);
            trailGenerator.Active = false;

            detectionSphere = new CollisionSphere(_position, detectionRange);
            detectionSphere.addCollisionEvent(collisionEvent);

            detectionSphere.Active = false;
            detectionSphere.Parent = this;

            CollisionBase.Active = false;
            CollisionBase.Parent = this;

            firstTrailingSphere = new CollisionSphere(_position, BASE_SIZE);
            firstTrailingSphere.addCollisionEvent(collisionEvent);
            firstTrailingSphere.Active = false;
            firstTrailingSphere.Parent = this;
            ((Sphere)firstTrailingSphere.getPhysicsCollider()).CollisionInformation.CollisionRules.Group = projectileGroup;

            secondTrailingSphere = new CollisionSphere(_position, BASE_SIZE);
            secondTrailingSphere.addCollisionEvent(collisionEvent);
            secondTrailingSphere.Active = false;
            secondTrailingSphere.Parent = this;
            ((Sphere)secondTrailingSphere.getPhysicsCollider()).CollisionInformation.CollisionRules.Group = projectileGroup;
        }

        public override void Update(float deltaTime)
        {
            if (Active)
            {
                base.Update(deltaTime);
                trailGenerator.Update(deltaTime);
                detectionSphere.Position = Position;
                if (Velocity.LengthSquared() != 0)
                {
                    firstTrailingSphere.Position = Position - (Vector3.Normalize(Velocity) * TRAILING_DIST);
                    secondTrailingSphere.Position = Position - (Vector3.Normalize(Velocity) * 2 * TRAILING_DIST);
                }
                else
                {
                    firstTrailingSphere.Position = Position;
                    secondTrailingSphere.Position = Position;
                }
            }
            else 
            {
                LogCat.updateValue("Missile Error", "Inactive!");
            }
        }

        public override void Draw(GameCamera camera)
        {
#if DEBUG
            detectionSphere.debugDraw(camera);
            firstTrailingSphere.debugDraw(camera);
            secondTrailingSphere.debugDraw(camera);
#endif
            trailGenerator.Draw(camera);
            base.Draw(camera);
        }

        public override void onAddToScene(Scene scene)
        {
            trailGenerator.clearLists();
            base.onAddToScene(scene);
        }

        public override void respawn(long _uniqueID, Vector3 _position, Quaternion _rotation, SceneObject _parent)
        {
            detectionSphere.Active = true;
            ((GameScene)Space394Game.GameInstance.CurrentScene).CollisionManager.addToCollisionList(detectionSphere);
            firstTrailingSphere.Active = true;
            ((GameScene)Space394Game.GameInstance.CurrentScene).CollisionManager.addToCollisionList(firstTrailingSphere);
            secondTrailingSphere.Active = true;
            ((GameScene)Space394Game.GameInstance.CurrentScene).CollisionManager.addToCollisionList(secondTrailingSphere);
            trailGenerator.Active = true;
            base.respawn(_uniqueID, _position, _rotation, _parent);
        }

        public override void onCollide(SceneObject caller, float damage)
        {
            detectionSphere.Active = false;
            ((GameScene)Space394Game.GameInstance.CurrentScene).CollisionManager.removeFromCollisionList(detectionSphere);
            firstTrailingSphere.Active = false;
            ((GameScene)Space394Game.GameInstance.CurrentScene).CollisionManager.removeFromCollisionList(firstTrailingSphere);
            secondTrailingSphere.Active = false;
            ((GameScene)Space394Game.GameInstance.CurrentScene).CollisionManager.removeFromCollisionList(secondTrailingSphere);
            trailGenerator.Active = false;
            trailGenerator.clearLists();
            base.onCollide(caller, damage);
        }
    }
}
