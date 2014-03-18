using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Space394.Collision;
using BEPUphysics.Entities.Prefabs;

namespace Space394.SceneObjects
{
    public abstract class Bomber : Fighter
    {
        public Bomber(long _uniqueId, Vector3 _position, Quaternion _rotation, Team _team, SpawnShip _home)
            : base(_uniqueId, _position, _rotation, _team, _home)
        {
            maxZSpeed = 7500.0f;
            minZSpeed = 150.0f;
            normalZSpeed = 350.0f;
            ZSpeed = NormalZSpeed;

            MAX_HEALTH = 7;
            Health = MaxHealth;

            MAX_SHIELDS = 3;
            Shields = MaxShields;

            SHIELD_RECOVER_RATE = 0.05f;

            rollAccel = 5.5f;
            rollBreak = 4.5f;
            rollCap = 1.5f;

            pitchAccel = 4.5f;
            pitchBreak = 3.5f;
            pitchCap = 0.75f;

            yawAccel = 4.5f;
            yawBreak = 3.5f;
            yawCap = 0.75f;

            baseHeat = 0.35f;
            heat = 0;
            overheatHeat = 1.5f;
            heatingRate = 0.5f;
            coolingRate = 0.1f;
            heatDamageRate = 0.75f;
            heatWarningThreshold = 1.2f;
            heatDamageThreshold = 1.4f;

            accelerationRate = 10000.0f;

            FIRE_RATE = 0.35f;
            fireTimer = FireRate;

            SPECIAL_RATE = 1.0f;
            specialTimer = SpecialRate;

            SecondaryAmmo = MaxSecondaryAmmo;

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

            INTEREST_TIME = 50; // Default for ship is 20

            Shields = 3; // TO SHOW SCALING BARS
        }

        public override void returnToSpawnShip()
        {
            if (home != null)
            {
                home.addBomber(this);
            }
            else { }
            Active = false;
        }
    }
}
