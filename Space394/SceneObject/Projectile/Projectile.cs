using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using BEPUphysics.Collidables.MobileCollidables;
using BEPUphysics.Collidables;
using BEPUphysics.NarrowPhaseSystems.Pairs;
using Space394.Collision;
using BEPUphysics.CollisionRuleManagement;
using BEPUphysics.Entities.Prefabs;

namespace Space394.SceneObjects
{
    public class Projectile : SceneObject
    {
        protected const float TTL = 3.0f;
        protected float ttl;

        protected float projectileSpeed = 1500.0f; // Should be faster than any player can reach
        public float ProjectileSpeed
        {
            get { return projectileSpeed; }
        }

        private Vector3 velocity;
        public Vector3 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }

        private SceneObject parent; // Don't collide with
        public SceneObject Parent
        {
            get { return parent; }
            set { parent = value; }
        }

        protected static CollisionGroup projectileGroup;
        private static bool collisionSet = false;

        protected static CollisionGroup esxolusProjectileGroup;
        protected static CollisionGroup halkProjectileGroup;

        public Projectile(long _uniqueID, Vector3 _position, Quaternion _rotation, String _model)
            : base(_uniqueID, _position, _rotation, _model)
        {
            ttl = TTL;

            Damage = 1.0f;

            Active = false;

            if (!collisionSet)
            {
                projectileGroup = new CollisionGroup();
                CollisionGroupPair pair = new CollisionGroupPair(projectileGroup, projectileGroup);
                CollisionRules.CollisionGroupRules.Add(pair, CollisionRule.NoBroadPhase);

                esxolusProjectileGroup = new CollisionGroup();
                CollisionGroupPair pairEP = new CollisionGroupPair(esxolusProjectileGroup, projectileGroup);
                CollisionRules.CollisionGroupRules.Add(pairEP, CollisionRule.NoBroadPhase);
                CollisionGroupPair pairEE = new CollisionGroupPair(esxolusProjectileGroup, esxolusProjectileGroup);
                CollisionRules.CollisionGroupRules.Add(pairEE, CollisionRule.NoBroadPhase);
                CollisionGroupPair pairEES = new CollisionGroupPair(esxolusProjectileGroup, Ship.EsxolusShipGroup);
                CollisionRules.CollisionGroupRules.Add(pairEES, CollisionRule.NoBroadPhase);
                CollisionGroupPair pairEESM = new CollisionGroupPair(esxolusProjectileGroup, Ship.EsxolusMeshShipGroup);
                CollisionRules.CollisionGroupRules.Add(pairEESM, CollisionRule.NoBroadPhase);

                halkProjectileGroup = new CollisionGroup();
                CollisionGroupPair pairHP = new CollisionGroupPair(halkProjectileGroup, projectileGroup);
                CollisionRules.CollisionGroupRules.Add(pairHP, CollisionRule.NoBroadPhase);
                CollisionGroupPair pairHH = new CollisionGroupPair(halkProjectileGroup, halkProjectileGroup);
                CollisionRules.CollisionGroupRules.Add(pairHH, CollisionRule.NoBroadPhase);
                CollisionGroupPair pairHHS = new CollisionGroupPair(halkProjectileGroup, Ship.HalkShipGroup);
                CollisionRules.CollisionGroupRules.Add(pairHHS, CollisionRule.NoBroadPhase);
                CollisionGroupPair pairHHSM = new CollisionGroupPair(halkProjectileGroup, Ship.HalkMeshShipGroup);
                CollisionRules.CollisionGroupRules.Add(pairHHSM, CollisionRule.NoBroadPhase);

                CollisionGroupPair pairEH = new CollisionGroupPair(esxolusProjectileGroup, halkProjectileGroup);
                CollisionRules.CollisionGroupRules.Add(pairEH, CollisionRule.NoBroadPhase);

                collisionSet = true;
            }
            else { }
        }

        public override void Update(float deltaTime)
        {
            if (Active)
            {
                ttl -= deltaTime;
                if (ttl <= 0)
                {
                    onCollide(this, 1);
                }
                else { }

                //if (drawOnce)
                {
                    Position = Position + velocity * deltaTime;
                }
                //else { }

                base.Update(deltaTime);
            }
            else
            {
                LogCat.updateValue("Inactive Projectile", "Error");
            }
        }

        public virtual void respawn(long _uniqueID, Vector3 _position, Quaternion _rotation, SceneObject _parent)
        {
            Position = _position;
            Rotation = _rotation;
            UniqueId = _uniqueID;
            QueuedRemoval = false;
            CollisionBase.Active = true;
            Active = true;

            parent = _parent;

            velocity = Vector3.Backward;

            velocity = Vector3.Transform(velocity, Rotation) * projectileSpeed;

            ttl = TTL;
        }

        public override void onCollide(SceneObject caller, float damage)
        {
            if (parent != null && caller != parent)
            {
                QueuedRemoval = true;
                Active = false;
                CollisionBase.Active = false; // No more onCollides
            }
            else { }
        }

        public override void collisionEvent(EntityCollidable eCollidable, Collidable collidable, CollidablePairHandler pairHandler)
        {
            if (collidable.Tag != null
                && collidable.Tag is CollisionBase)
            {
                SceneObject collidee = ((CollisionBase)collidable.Tag).Parent;
                if (collidee != null && !(collidee is Projectile) && parent != null && parent != collidee)
                {
                    collidee.onCollide(this, Damage);
                }
                else
                {
                    // Was another projectile
                }
            }
            else { } 
        }
    }
}
