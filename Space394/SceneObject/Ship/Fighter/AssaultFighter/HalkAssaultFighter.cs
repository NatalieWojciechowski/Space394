using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Space394.Particles;

namespace Space394.SceneObjects
{
    public class HalkAssaultFighter : AssaultFighter
    {
        private const int BARRAGE_COUNT = 1;
        private const int SECONDARY_LOCATION_COUNT = 1;

        public HalkAssaultFighter(long _uniqueId, Vector3 _position, Quaternion _rotation, SpawnShip _home)
            : base(_uniqueId, _position, _rotation, Team.Halk, _home)
        {
            laserOffsets = new Vector3[] {
                new Vector3(-3.382f, -2.532f, 59.654f),
                new Vector3(3.382f, -2.532f, 59.654f)
            };
            SECONDARY_RANGE = 8000000f;
            MAX_SECONDARY_AMMO = 9;
            SecondaryAmmo = MaxSecondaryAmmo;
            secondaryAttackPositions = new Vector3[] {
                new Vector3(0, -4.275f, 11.3f), 
            };
            secondaryAttackForward = new Quaternion[] {
                Quaternion.CreateFromAxisAngle(Vector3.Forward, 0f) 
            };

            trailGenerators.Add(new HEngineTrailGenerator(this, new Vector3(-3.004f, 1.722f, -22.46f))); // -12.46f
            trailGenerators.Add(new HEngineTrailGenerator(this, new Vector3(-0.475f, 1.413f, -22.46f)));
            trailGenerators.Add(new HEngineTrailGenerator(this, new Vector3(-0.992f, 3.3987f, -22.46f)));
            trailGenerators.Add(new HEngineTrailGenerator(this, new Vector3(1.229f, 4.355f, -22.46f)));
            trailGenerators.Add(new HEngineTrailGenerator(this, new Vector3(2.429f, 2.204f, -22.46f)));

            setCloseModelByString("Models/Ships/Halk_Assault");
            setFarModelByString("Models/Ships/Halk_Assault");
        }

        public override void fireSecondary()
        {
            if (SecondaryAmmo > 0)
            {
                if (specialTimer <= 0)
                {
                    const float DISTANCE_MODIFIER = 500f;
                    HalkAMissile cMissle;

                    Vector3 missilePos = Vector3.Transform(secondaryAttackPositions[0] + jitterPosition, Rotation) + Position;
                    cMissle = SceneObjectFactory.createNewHalkAMissile(missilePos, Rotation * secondaryAttackForward[0], this);
                    cMissle.TargetPos = Position + jitterPosition + ((Vector3.Normalize(Velocity) * DISTANCE_MODIFIER) + getRandomUnitVector());
                    cMissle.Active = true;

                    specialTimer = SpecialRate;
                    SecondaryAmmo--;
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
