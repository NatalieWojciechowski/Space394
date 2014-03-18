using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Space394.Collision;
using Space394.Scenes;
using BEPUphysics;
using BEPUphysics.Collidables.MobileCollidables;
using BEPUphysics.Collidables;
using BEPUphysics.NarrowPhaseSystems.Pairs;
using BEPUphysics.CollisionRuleManagement;
using BEPUphysics.Entities.Prefabs;
using Space394.Particles;

namespace Space394.SceneObjects
{
    public class Explosive : Projectile
    {
        private Vector3 targetPos;
        public Vector3 TargetPos
        {
            get { return targetPos; }
            set { targetPos = value; }
        }

        protected float EXPLOSIVE_TTL = 15.0f;

        // Secondary Collider
        private CollisionBase explosionCollider;
        public CollisionBase ExplosionCollider
        {
            get { return explosionCollider; }
            set { explosionCollider = value; }
        }

        protected const int BASE_SIZE = 10;

        public Explosive(long _uniqueID, Vector3 _position, Quaternion _rotation, String _model)
            : base(_uniqueID, _position, _rotation, _model)
        {
            Damage = 45.0f;

            CollisionBase = new CollisionSphere(_position, BASE_SIZE);
            CollisionBase.Active = false;
            CollisionBase.Parent = this;
            CollisionBase.addCollisionEvent(collisionEvent);

            ((Sphere)CollisionBase.getPhysicsCollider()).CollisionInformation.CollisionRules.Group = projectileGroup;

            explosionCollider = new CollisionSphere(_position, 3);
            explosionCollider.Active = false;
            explosionCollider.Parent = this;
            explosionCollider.addCollisionEvent(collisionEvent);

            ((Sphere)explosionCollider.getPhysicsCollider()).CollisionInformation.CollisionRules.Group = projectileGroup;

            ttl = EXPLOSIVE_TTL;
        }

        public override void Update(float deltaTime)
        {
            if (explosionCollider != null)
            {
                explosionCollider.Position = Position;
            }
            else { }

            // trailGenerator.Update(deltaTime);

            // Check for zero length distances
            Vector3 store = targetPos - Position;
            if (store.LengthSquared() == 0)
            {

            }
            else
            {
                store.Normalize();
            }
                
            Velocity = Velocity + store; // Should redirect the velocity toward the target position
            base.Update(deltaTime);
        }

        public override void Draw(GameCamera camera)
        {
            base.Draw(camera);
        }

        public override void onAddToScene(Scene scene)
        {
            base.onAddToScene(scene);
        }

        public override void onCollide(SceneObject caller, float damage)
        {
            if (Parent != null && caller != null && Parent != caller)
            {
                explosionCollider.Active = false;

                SceneObjectFactory.createExplosion(Position, Rotation);

                base.onCollide(caller, damage);
            }
            else { }
        }

        public float explosionDamage()
        {
            return Damage * 0.5f;
        }

        public override void collisionEvent(EntityCollidable eCollidable, Collidable collidable, CollidablePairHandler pairHandler)
        {
            // bool valid = false;

            if (collidable.Tag != null
                && collidable.Tag is CollisionBase)
            {
                SceneObject collidee = ((CollisionBase)collidable.Tag).Parent;
                if (collidee != null && !(collidee is Projectile) && Parent != null && Parent != collidee)
                {
                    if (collidee is Ship)
                    {
                        collidee.onCollide(this, Damage);
                        /*if (collidee is Ship)
                        {
                            valid = true;
                        }*/
                    }
                    else { }
                }
                else { }
            }
            else { }

            /*if (valid == true)
            {
                SceneObjectFactory.createExplosion(Position, Rotation);
            }
            else { }*/
        }
    }
}
