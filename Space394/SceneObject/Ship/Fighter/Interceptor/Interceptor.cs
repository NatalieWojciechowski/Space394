using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Space394.AIControl;
using Microsoft.Xna.Framework.Graphics;
using Space394.Collision;
using BEPUphysics.Collidables.MobileCollidables;
using BEPUphysics.Collidables;
using BEPUphysics.NarrowPhaseSystems.Pairs;
using BEPUphysics.Entities.Prefabs;

namespace Space394.SceneObjects
{
    public abstract class Interceptor : Fighter
    {
        public Interceptor(long _uniqueId, Vector3 _position, Quaternion _rotation, Team _team, SpawnShip _home)
            : base(_uniqueId, _position, _rotation, _team, _home)
        {
            maxZSpeed = 10000.0f;
            minZSpeed = 300.0f;
            normalZSpeed = 500.0f;
            ZSpeed = NormalZSpeed;

            MAX_HEALTH = 3;
            Health = MaxHealth;

            MAX_SHIELDS = 1;
            Shields = MaxShields;

            SHIELD_RECOVER_RATE = 0.2f;

            rollAccel = 10.0f;
            rollBreak = 5.0f;
            rollCap = 2.5f;

            pitchAccel = 7.5f;
            pitchBreak = 5.0f;
            pitchCap = 1.5f;

            yawAccel = 7.5f;
            yawBreak = 5.0f;
            yawCap = 1.5f;

            baseHeat = 0.55f;
            heat = 0;
            overheatHeat = 1.5f;
            heatingRate = 1.5f;
            coolingRate = 0.25f;
            heatDamageRate = 0.1f;
            heatWarningThreshold = 1.2f;
            heatDamageThreshold = 1.4f;

            accelerationRate = 25000.0f;

            FIRE_RATE = 0.1f;
            fireTimer = FireRate;

            SPECIAL_RATE = 0.5f;
            specialTimer = SpecialRate;

            PRIMARY_RANGE = 100000000f;

            CollisionBase = new CollisionSphere(_position, 10);
            CollisionBase.Parent = this;
            CollisionBase.addCollisionEvent(collisionEvent);

            if (ShipTeam == Team.Esxolus)
            {
                ((Sphere)CollisionBase.getPhysicsCollider()).CollisionInformation.CollisionRules.Group = EsxolusShipGroup;
            }
            else
            {
                ((Sphere)CollisionBase.getPhysicsCollider()).CollisionInformation.CollisionRules.Group = HalkShipGroup;
            }
        }

        public override void returnToSpawnShip()
        {
            if (home != null)
            {
                home.addIntereptor(this);
            }
            else { }
            Active = false;
        }
    }
}
