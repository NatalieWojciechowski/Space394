using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Space394.Particles;

namespace Space394.SceneObjects
{
    public class EsxolusAssaultFighter : AssaultFighter
    {
        private const int MISSLE_SET_COUNT = 6;
        private const int SECONDARY_LOCATION_COUNT = 6;

        private Vector3[] randomUnitVectors;

        private bool firingSecondaries;
        private float secondaryTimer = 0.0f;
        private const float SECONDARY_TIMER = 0.15f;
        private int launched = 0;

        public EsxolusAssaultFighter(long _uniqueId, Vector3 _position, Quaternion _rotation, SpawnShip _home)
            : base(_uniqueId, _position, _rotation, Team.Esxolus, _home)
        {
            laserOffsets = new Vector3[] {
                new Vector3(-0.707f, 2.037f, 44.484f), //54.484f),
                new Vector3(0.707f, 2.037f, 44.484f), //54.484f),
                new Vector3(-2.307f, -1.674f, 30.986f), //40.986f),
                new Vector3(2.307f, -1.674f, 30.986f), //40.986f)
            };
            SECONDARY_RANGE = 6000000f;
            MAX_SECONDARY_AMMO = 9;
            SecondaryAmmo = MaxSecondaryAmmo;

            SPECIAL_RATE = 1.5f;

            firingSecondaries = false;

            randomUnitVectors = new Vector3[SECONDARY_LOCATION_COUNT];
            for (int i = 0; i < SECONDARY_LOCATION_COUNT; i++)
            {
                randomUnitVectors[i] = getRandomUnitVector();
            }

            #region Missle Positions
            secondaryAttackPositions = new Vector3[] { 
                // Left Top
                new Vector3(4.706f, 1.16f, 36.627f),
                // Right Top
                new Vector3(-4.706f, 1.16f, 36.627f),
                
                // Left Mid
                new Vector3(4.706f, -0.175f, 36.113f),
                // Right Mid
                new Vector3(-4.706f, -0.175f, 36.113f),

                // Left Bottom
                new Vector3(4.706f, -1.493f, 35.432f),
                // Right Bottom
                new Vector3(-4.706f, -1.493f, 35.432f),
            };
            #endregion

            secondaryAttackForward = new Quaternion[] {
                Quaternion.CreateFromAxisAngle(Vector3.Forward, 0f) 
            };
            /* Currently using HALK Angles; still waiting on the esxolus */
            secondaryAttackForward = new Quaternion[]{
                Quaternion.CreateFromAxisAngle(Vector3.Forward, 0f) 
            };

            trailGenerators.Add(new EEngineTrailGenerator(this, new Vector3(4.702f, 0.0f, -11.063f)));
            trailGenerators.Add(new EEngineTrailGenerator(this, new Vector3(-4.702f, 0.0f, -11.063f)));

            setCloseModelByString("Models/Ships/esxolus_assault_fighter_model");
            setFarModelByString("Models/Ships/esxolus_assault_fighter_LP_model");
        }

        public override void Update(float deltaTime)
        {
            if (firingSecondaries)
            {
                secondaryTimer -= deltaTime;
                if (secondaryTimer <= 0)
                {
                    Quaternion shipRotation = Rotation;
                    Vector3 shipPosition = Position;
                    Vector3 shipVelocity = Velocity;

                    const float DISTANCE_MODIFIER = 500f;
                    Vector3 targetPosOffset = (Vector3.Normalize(shipVelocity) * DISTANCE_MODIFIER);

                    Missile tMissle;
                    for (int i = launched * 2; i < 2 + (launched * 2); i++)
                    {
                        Vector3 missilePos = Vector3.Transform(secondaryAttackPositions[i] + jitterPosition, shipRotation) + shipPosition;
                        tMissle = SceneObjectFactory.createNewEsxolusAMissile(missilePos, shipRotation * secondaryAttackForward[0], this);
                        tMissle.TargetPos = shipPosition + jitterPosition + (targetPosOffset + randomUnitVectors[i]);
                    }

                    specialTimer = SpecialRate;
                    launched++;
                    if (launched >= 3)
                    {
                        firingSecondaries = false;
                        launched = 0;
                    }
                    else
                    {
                        secondaryTimer = SECONDARY_TIMER;
                    }
                }
                else { }
            }
            else { }

            base.Update(deltaTime);
        }

        public override void fireSecondary()
        {
            if (SecondaryAmmo > 0)
            {
                if (specialTimer <= 0)
                {
                    SecondaryAmmo--;
                    firingSecondaries = true;
                }
                else { }
            }
        }

        public Vector3 getRandomUnitVector()
        {
            int offsetScale = 1;
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

            return randomVel;
        }
    }
}
