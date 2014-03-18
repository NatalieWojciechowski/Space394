using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Space394.Collision;
using BEPUphysics.Entities.Prefabs;

namespace Space394.SceneObjects
{
    public abstract class AssaultFighter : Fighter
    {
        public AssaultFighter(long _uniqueId, Vector3 _position, Quaternion _rotation, Team _team, SpawnShip _home)
            : base(_uniqueId, _position, _rotation, _team, _home)
        {
            maxZSpeed = 9000.0f;
            minZSpeed = 350.0f;
            normalZSpeed = 550.0f;
            ZSpeed = NormalZSpeed;

            MAX_HEALTH = 5;
            Health = MaxHealth;

            MAX_SHIELDS = 2;
            Shields = MaxShields;

            SHIELD_RECOVER_RATE = 0.1f;

            rollAccel = 7.5f;
            rollBreak = 4.5f;
            rollCap = 1.75f;

            pitchAccel = 5.5f;
            pitchBreak = 3.5f;
            pitchCap = 0.95f;

            yawAccel = 5.5f;
            yawBreak = 3.5f;
            yawCap = 0.95f;

            baseHeat = 0.45f;
            heat = 0;
            overheatHeat = 2.0f;
            heatingRate = 0.5f;
            coolingRate = 0.1f;
            heatDamageRate = 0.5f;
            heatWarningThreshold = 1.7f;
            heatDamageThreshold = 1.9f;

            accelerationRate = 15000.0f;

            FIRE_RATE = 0.25f;
            fireTimer = FireRate;

            SPECIAL_RATE = 0.7f;
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

            INTEREST_TIME = 35;
        }

        public override void returnToSpawnShip()
        {
            if (home != null)
            {
                home.addAssaultFighter(this);
            }
            else { }
            Active = false;
        }
    }
}
