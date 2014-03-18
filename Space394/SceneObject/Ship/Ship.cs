using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BEPUphysics.Collidables.MobileCollidables;
using BEPUphysics.Collidables;
using BEPUphysics.NarrowPhaseSystems.Pairs;
using Space394.Collision;
using BEPUphysics.CollisionRuleManagement;

namespace Space394.SceneObjects
{
    public abstract class Ship : SceneObject
    {
        protected float MAX_HEALTH = 0;
        public float MaxHealth
        {
            get { return MAX_HEALTH; }
        }

        private float shields = 0;
        public float Shields
        {
            get { return shields; }
            set { shields = value; }
        }

        protected float MAX_SHIELDS = 0;
        public float MaxShields
        {
            get { return MAX_SHIELDS; }
        }

        protected float SHIELD_RECOVER_RATE = 0.2f;

        public enum Team
        {
            Esxolus = 0,
            Halk = 1
        };

        protected short MAX_ATTACKERS = 3;
        public short MaxAttackers
        {
            get { return MAX_ATTACKERS; }
        }

        private short attackerCount = 0;
        public short AttackerCount
        {
            get { return attackerCount; }
            set { attackerCount += value; updateAttackable(); }
        }
        public short resetAttackerCount() { attackerCount = 0; return attackerCount; }

        private bool attackable = false;
        public bool Attackable
        {
            get { return attackable; }
            set { attackable = value; }
        }

        protected int MAX_SECONDARY_AMMO = 0;
        public int MaxSecondaryAmmo
        {
            get { return MAX_SECONDARY_AMMO; }
        }

        private int secondaryAmmo = 0;
        public int SecondaryAmmo
        {
            get { return secondaryAmmo; }
            set { secondaryAmmo = value; }
        }

        protected Team team;
        public Team ShipTeam
        {
            get { return team; }
        }

        protected Model closeModel;
        protected Model farModel;

        protected HUDUnit hudUnit;
        public HUDUnit HUDUnit
        {
            get { return hudUnit; }
        }

        private const float SIMPLE_DRAW_DIST = 10000000f;
        public float SimpleDrawDist
        {
            get { return SIMPLE_DRAW_DIST; }
        } 

        private static CollisionGroup esxolusShipGroup;
        public static CollisionGroup EsxolusShipGroup
        {
            get 
            {
                if (esxolusShipGroup == null)
                {
                    esxolusShipGroup = new CollisionGroup();
                }
                else { }
                return esxolusShipGroup; 
            }
        }

        private static CollisionGroup halkShipGroup;
        public static CollisionGroup HalkShipGroup
        {
            get 
            {
                if (halkShipGroup == null)
                {
                    halkShipGroup = new CollisionGroup();
                }
                else { }
                return halkShipGroup; 
            }
        }

        private static CollisionGroup esxolusMeshShipGroup;
        public static CollisionGroup EsxolusMeshShipGroup
        {
            get
            {
                if (esxolusMeshShipGroup == null)
                {
                    esxolusMeshShipGroup = new CollisionGroup();
                }
                else { }
                return esxolusMeshShipGroup;
            }
        }

        private static CollisionGroup halkMeshShipGroup;
        public static CollisionGroup HalkMeshShipGroup
        {
            get
            {
                if (halkMeshShipGroup == null)
                {
                    halkMeshShipGroup = new CollisionGroup();
                }
                else { }
                return halkMeshShipGroup;
            }
        }

        private static bool groupsSet = false;

        public Ship(long _uniqueId, Vector3 _position, Quaternion _rotation, Team _team)
            : base(_uniqueId, _position, _rotation, "Models/box")
        {
            Damage = 1.0f; // Instant destroy
            team = _team;

            int hudScale = 2;
            hudUnit = new HUDUnit(this, 10*hudScale,4*hudScale,1*hudScale);

            if (!groupsSet)
            {
                CollisionGroupPair pairEE = new CollisionGroupPair(esxolusMeshShipGroup, esxolusMeshShipGroup);
                CollisionRules.CollisionGroupRules.Add(pairEE, CollisionRule.NoBroadPhase);

                CollisionGroupPair pairHH = new CollisionGroupPair(halkMeshShipGroup, halkMeshShipGroup);
                CollisionRules.CollisionGroupRules.Add(pairHH, CollisionRule.NoBroadPhase);

                CollisionGroupPair pairEH = new CollisionGroupPair(esxolusMeshShipGroup, halkMeshShipGroup);
                CollisionRules.CollisionGroupRules.Add(pairEH, CollisionRule.NoBroadPhase);
                groupsSet = true;
            }
        }

        protected Model setCloseModelByString(String modelName)
        {
            closeModel = ContentLoadManager.loadModel(modelName);
            return farModel;
        }

        protected Model setFarModelByString(String modelName)
        {
            farModel = ContentLoadManager.loadModel(modelName);
            return farModel;
        }

        public override void Update(float deltaTime)
        {
            shields += SHIELD_RECOVER_RATE * deltaTime;
            if (shields > MAX_SHIELDS)
            {
                shields = MAX_SHIELDS;
            }
            else { }

            hudUnit.updateFromShip();
            
            base.Update(deltaTime);
        }

        public override void onCollide(SceneObject caller, float damage)
        {
            if (Health > 0 && caller != null && caller != this)
            {
                if (caller is Projectile && ((Projectile)caller).Parent is Fighter && ((Fighter)((Projectile)caller).Parent).PlayerControlled) // Projectile hit
                {
                    ((Fighter)((Projectile)caller).Parent).MyPlayer.PlayerHUD.DrawConfirmed = true;
                }
                else if (caller is Fighter && ((Fighter)caller).PlayerControlled) // Straight collision
                {
                    ((Fighter)caller).MyPlayer.PlayerHUD.DrawConfirmed = true;
                }
                else { }
            }
            else { }
            shields -= damage;
            if (shields < 0)
            {
                float lastHealth = Health;
                Health += shields;
                shields = 0;
            }
            else { }
        }

        public bool updateAttackable()
        {
            if (attackerCount < MAX_ATTACKERS)
            {
                if (!attackable)
                {
                    Attackable = true;
                }
                else { }
            }
            else
            {
                if (attackable)
                {
                    Attackable = false;
                }
                else { }
            }

            return attackable;
        }

        public abstract void DrawReticule(GameCamera camera);
    }
}
