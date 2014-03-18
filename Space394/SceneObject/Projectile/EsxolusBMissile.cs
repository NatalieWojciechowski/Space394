using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using BEPUphysics.Entities.Prefabs;

namespace Space394.SceneObjects
{
    public class EsxolusBMissile : Missile
    {
        private const float BASE_DAMAGE = 150.0f;

        private const float TTD = 3.5f;
        private float ttd; // Time to detonate

        public EsxolusBMissile(long _uniqueID, Vector3 _position, Quaternion _rotation)
            : base(_uniqueID, _position, _rotation, "Models/Ships/esxolus_bomber_bomb_model")
        {
            Damage = BASE_DAMAGE;

            ttd = TTD;

            ((Sphere)detectionSphere.getPhysicsCollider()).CollisionInformation.CollisionRules.Group = esxolusProjectileGroup;

            CollisionBase.Active = false;
            CollisionBase.Parent = this;
        }

        public override void Update(float deltaTime)
        {
            ttd -= deltaTime;
            if (ttd <= 0)
            {
                onCollide(this, 0);
            }
            else { }

            Velocity = Velocity + Vector3.Normalize(TargetPos - Position); // Should return the velocity toward the target position
            base.Update(deltaTime);
        }

        public override void onCollide(SceneObject caller, float damage)
        {
            if (Parent != null && caller != null && Parent != caller)
            {
                ProjectileManager.addExsolusBMissile(this);

                base.onCollide(caller, damage);
            }
            else { }
        }
    }
}
