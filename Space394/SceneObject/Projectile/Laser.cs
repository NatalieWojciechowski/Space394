using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Space394.Collision;
using BEPUphysics.Collidables.MobileCollidables;
using BEPUphysics.Collidables;
using BEPUphysics.NarrowPhaseSystems.Pairs;
using BEPUphysics.CollisionRuleManagement;
using BEPUphysics.Entities.Prefabs;

namespace Space394.SceneObjects
{
    public class Laser : Projectile
    {
        private const float RESIZE = 0.5f;

        public enum laser_type
        {
            basic,
            esxolus,
            halk,
            esxolus_turret,
            halk_turret,
        };
        public laser_type laserType;

        private static Model basicLaserModel;
        private static Model esxolusLaserModel;
        private static Model halkLaserModel;
        private static Model esxolusTurretLaserModel;
        private static Model halkTurretLaserModel;
        private static bool modelsSet = false;

        public Laser(long _uniqueID, Vector3 _position, Quaternion _rotation)
            : base(_uniqueID, _position, _rotation, "Models/Lasers/hot_pink_laser_model")
        {
            this.Damage = 1.0f;

            projectileSpeed = 4500.0f;

            if (!modelsSet)
            {
                basicLaserModel = ContentLoadManager.loadModel("Models/Lasers/hot_pink_laser_model");
                esxolusLaserModel = ContentLoadManager.loadModel("Models/Lasers/cyan_laser_model");
                halkLaserModel = ContentLoadManager.loadModel("Models/Lasers/red_laser_model");
                esxolusTurretLaserModel = ContentLoadManager.loadModel("Models/Lasers/cyan_turret_laser_model");
                halkTurretLaserModel = ContentLoadManager.loadModel("Models/Lasers/red_turret_laser_model");
                modelsSet = true;
            }
            else { }

            CollisionBase = new CollisionSphere(Position, (int)(Model.Meshes.First().BoundingSphere.Radius * RESIZE));

            ((Sphere)CollisionBase.getPhysicsCollider()).CollisionInformation.CollisionRules.Group = projectileGroup;

            CollisionBase.Active = false;
            CollisionBase.Parent = this;
            CollisionBase.addCollisionEvent(collisionEvent);
        }

        public override void collisionEvent(EntityCollidable eCollidable, Collidable collidable, CollidablePairHandler pairHandler)
        {
            if (collidable.Tag != null
                && collidable.Tag is CollisionBase)
            {
                SceneObject collidee = ((CollisionBase)collidable.Tag).Parent;
                if (collidee != null && !(collidee is Projectile) && collidee != Parent)
                {
                    collidee.onCollide(this, Damage);
                }
                else 
                {
                    // Was another laser or parent
                    if (collidee != null && collidee is Projectile)
                    {
                        LogCat.updateValue("Collide", "With projectile!");
                    }
                    else { }
                }
            }
            else { }
        }

        public override void Draw(GameCamera camera)
        {
            switch (laserType)
            {
                case laser_type.basic:
                    drawExtraModel(camera, basicLaserModel);
                    break;
                case laser_type.esxolus:
                    drawExtraModel(camera, esxolusLaserModel);
                    break;
                case laser_type.esxolus_turret:
                    drawExtraModel(camera, esxolusTurretLaserModel);
                    break;
                case laser_type.halk:
                    drawExtraModel(camera, halkLaserModel);
                    break;
                case laser_type.halk_turret:
                    drawExtraModel(camera, halkTurretLaserModel);
                    break;
            }
        }

        public override void respawn(long _uniqueID, Vector3 _position, Quaternion _rotation, SceneObject _parent)
        {
            base.respawn(_uniqueID, _position, _rotation, _parent);
        }

        public override void onCollide(SceneObject caller, float damage)
        {
            if (caller != null && Parent != null && caller != Parent)
            {
                ProjectileManager.addLaser(this);

                base.onCollide(caller, damage);
            }
            else { }
        }
    }
}
