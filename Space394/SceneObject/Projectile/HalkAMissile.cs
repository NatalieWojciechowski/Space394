using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Space394.Collision;
using BEPUphysics;
using BEPUphysics.Collidables.MobileCollidables;
using BEPUphysics.Collidables;
using BEPUphysics.NarrowPhaseSystems.Pairs;
using BEPUphysics.CollisionRuleManagement;
using BEPUphysics.Entities.Prefabs;

namespace Space394.SceneObjects
{
    public class HalkAMissile : Missile
    {
        private HalkAMissile[] HalkAMissiles;

        private const float PARENT_TTD = 2.5f;
        private const float CHILD_TTD = 1.25f;
        private const float GRANDCHILD_TTD = 1.25f;

        private float ttd; // Time to detonate

        private const int CHILDREN_COUNT = 5;
        private const int GRANDCHILDREN_COUNT = 5;

        private int level = 0; // 0 parent, 1 first child, 2 last child

        private bool split = false;

        private const float WANDER_INFLUENCE = 2.5f;
        private const float MISSILE_SPEED = 1500.0f;
        private const float CHILD_SPEED = 500.0f;
        private const float GRANDCHILD_SPEED = 150.0f;

        private const float RESIZE = 50.0f;

        private const float DAMAGE = 1.0f;

        public HalkAMissile(long _uniqueID, Vector3 _position, Quaternion _rotation)
            : base(_uniqueID, _position, _rotation, "Models/Ships/Halk_Assault_Secondary")
        {
            Damage = DAMAGE;

            ttd = PARENT_TTD;

            projectileSpeed = MISSILE_SPEED;

            ((Sphere)detectionSphere.getPhysicsCollider()).CollisionInformation.CollisionRules.Group = halkProjectileGroup;

            HalkAMissiles = new HalkAMissile[CHILDREN_COUNT];

            for (int i = 0; i < CHILDREN_COUNT; i++)
            {
                HalkAMissiles[i] = new HalkAMissile(_uniqueID, _position, _rotation, 1);
            }

            CollisionBase.Active = false;
            CollisionBase.Parent = this;
        }

        public HalkAMissile(long _uniqueID, Vector3 _position, Quaternion _rotation, int _level)
            : base(_uniqueID, _position, _rotation, "Models/Ships/Halk_Assault_Secondary")
        {
            level = _level;

            if (level == 1)
            {
                Damage = DAMAGE;
                ttd = CHILD_TTD;
                Scale = Scale * 5;

                HalkAMissiles = new HalkAMissile[GRANDCHILDREN_COUNT];
                for (int i = 0; i < GRANDCHILDREN_COUNT; i++)
                {
                    HalkAMissiles[i] = new HalkAMissile(_uniqueID, _position, _rotation, 2);
                }
            }
            else if (level == 2)
            {
                Damage = DAMAGE;
                ttd = GRANDCHILD_TTD;
                Scale = Scale * 10;
            }
            else 
            {
                Damage = DAMAGE;
                ttd = 0.0f;
                Scale = Scale * 2;
            }
            projectileSpeed = MISSILE_SPEED;

            CollisionBase.Active = false;
            CollisionBase.Parent = this;
        }

        public override void respawn(long _uniqueID, Vector3 _position, Quaternion _rotation, SceneObject _parent)
        {
            if (level == 1)
            {
                ttd = CHILD_TTD;
            }
            else if (level == 2)
            {
                ttd = GRANDCHILD_TTD;
            }
            else
            {
                ttd = PARENT_TTD;
            }

            split = false;

            base.respawn(_uniqueID, _position, _rotation, _parent);
        }

        public override void Update(float deltaTime)
        {
            if (Active)
            {
                ttd -= deltaTime;

                if (ttd <= 0)
                {
                    onCollide(this, 0);
                }
                else { }

                base.Update(deltaTime);
            }
            else { }
        }

        private void burst()
        {
            if (!split)
            {
                Quaternion parentRotation = Rotation;
                Vector3 parentPosition = Position;
                if (level == 0)
                {
                    for (int i = 0; i < CHILDREN_COUNT; i++)
                    {
                        HalkAMissiles[i].respawn(UniqueId,
                            parentPosition, parentRotation, Parent);
                        if (ttd <= 0) // Not on actual collides
                        {
                            Vector3 vel = getConeDirection(parentRotation, i) * CHILD_SPEED;
                            HalkAMissiles[i].Velocity = vel;
                            HalkAMissiles[i].TargetPos = vel/*getSmartTargetPosition(getPosition(), vel)*/;
                        }
                        else { }
                        Space394Game.GameInstance.CurrentScene.addSceneObject(HalkAMissiles[i]);
                    }
                }
                else if (level == 1)
                {
                    for (int i = 0; i < GRANDCHILDREN_COUNT; i++)
                    {
                        HalkAMissiles[i].respawn(UniqueId,
                            parentPosition, parentRotation, Parent);
                        if (ttd <= 0) // Not on actual collides
                        {
                            Vector3 vel = getConeDirection(parentRotation, i) * GRANDCHILD_SPEED;
                            HalkAMissiles[i].Velocity = vel;
                            HalkAMissiles[i].TargetPos = vel/*getSmartTargetPosition(getPosition(), vel)*/;
                        }
                        else { }
                        Space394Game.GameInstance.CurrentScene.addSceneObject(HalkAMissiles[i]);
                    }
                }
                else if (level == 2)
                {

                }
                else
                { }

                split = true;
            }
            else { }
        }

        public override void onCollide(SceneObject caller, float damage)
        {
            if (Parent != null && caller != null && Parent != caller)
            {
                burst();

                if (level == 0)
                {
                    ProjectileManager.addHalkAMissile(this);
                }
                else { }

                base.onCollide(caller, damage);
            }
            else { }
        }

        // Meant to push back the Flak shots so they dont always sapwn on top of eachother
        public Vector3 getStaggeredPosition(Vector3 _position, Vector3 _velocity, int i)
        {
            const int SPACING = 20;
            Vector3 staggeredPosition = Vector3.Zero; 
            staggeredPosition = _position + (Vector3.Normalize(_velocity)*(SPACING * i));

            return staggeredPosition;
        }

        public Vector3 getNewTargetPosition(Vector3 _position)
        {
            Random random = new Random((int)(System.DateTime.Now.Millisecond + _position.X * 13 + _position.Y * 7 + _position.Z * 3));
            const float DISTANCE_MODIFIER = 1f;   // 200f;
            float offsetScale = 3000f;

            Vector3 randomVel;
            if (random.Next() % 2 == 0)
            {
                offsetScale *= -1;
            }
            randomVel.X = (float)random.NextDouble() * offsetScale;
            if (random.Next() % 2 == 0)
            {
                offsetScale *= -1;
            }
            randomVel.Y = (float)random.NextDouble() * offsetScale;
            if (random.Next() % 2 == 0)
            {
                offsetScale *= -1;
            }
            randomVel.Z = (float)random.NextDouble() * offsetScale;
            return (_position + (Vector3.Normalize(Velocity) + randomVel * DISTANCE_MODIFIER));
        }

        public Vector3 getConeDirection(Quaternion _rotation, int _arrayPosition)
        {
            Vector3 coneDirection;
            // Pitch up 
            if (_arrayPosition == 0)
            {
                coneDirection = Vector3.Transform(Vector3.Up, _rotation);//Quaternion.CreateFromAxisAngle(Vector3.Right, MathHelper.ToRadians(25)));
            }
            // Pitch Down 
            else if (_arrayPosition == 1)
            {
                coneDirection = Vector3.Transform(Vector3.Left, _rotation);//Quaternion.CreateFromAxisAngle(Vector3.Right, MathHelper.ToRadians(-25)));
            }
            // Yaw Left 
            else if (_arrayPosition == 2)
            {
                coneDirection = Vector3.Transform(Vector3.Right, _rotation);//Quaternion.CreateFromAxisAngle(Vector3.Up, MathHelper.ToRadians(25)));
            }
            // Yaw Right
            else if (_arrayPosition == 3)
            {
                coneDirection = Vector3.Transform(Vector3.Down, _rotation);//Quaternion.CreateFromAxisAngle(Vector3.Up, MathHelper.ToRadians(-25)));
            }
            // Continue Straight
            else
            {
                coneDirection = Vector3.Transform(Vector3.Backward, _rotation);
            }

            coneDirection = Vector3.Normalize(coneDirection);
            return coneDirection;
        }

        public Vector3 getSmartTargetPosition(Vector3 _position, Vector3 _velocity)
        {
            Random random = new Random((int)(System.DateTime.Now.Millisecond + _position.X * 13 + _position.Y * 7 + _position.Z * 3));
            const float DISTANCE_MODIFIER = 1f;   // 200f;
            float offsetScale = 1000f;

            Vector3 randomVel;
            if (random.Next() % 2 == 0)
            {
                offsetScale *= -1;
            }
            randomVel.X = (float)random.NextDouble() * offsetScale;
            if (random.Next() % 2 == 0)
            {
                offsetScale *= -1;
            }
            randomVel.Y = (float)random.NextDouble() * offsetScale;
            if (random.Next() % 2 == 0)
            {
                offsetScale *= -1;
            }
            randomVel.Z = (float)random.NextDouble() * offsetScale;
            return (_position + (Vector3.Normalize(Velocity) + randomVel * DISTANCE_MODIFIER));
        }
    }
}
