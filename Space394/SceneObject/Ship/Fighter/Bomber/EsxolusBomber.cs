using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Space394.Particles;

namespace Space394.SceneObjects
{
    public class EsxolusBomber : Bomber
    {
        private bool leftSecondary;

        public EsxolusBomber(long _uniqueId, Vector3 _position, Quaternion _rotation, SpawnShip _home)
            : base(_uniqueId, _position, _rotation, Team.Esxolus, _home)
        {
            laserOffsets = new Vector3[] {
                new Vector3(-2.429f, -9.134f, 67.641f),
                new Vector3(2.429f, -9.134f, 67.641f)
            };
            MAX_SECONDARY_AMMO = 15;
            SecondaryAmmo = MaxSecondaryAmmo;
            SECONDARY_RANGE = 8000000f;

            leftSecondary = true;

            secondaryAttackPositions = new Vector3[] {
                new Vector3(12.325f, -2.762f, 4.149f),
                new Vector3(-12.325f, -2.762f, 4.149f),
            };
            secondaryAttackForward = new Quaternion[] {
                Quaternion.CreateFromAxisAngle(Vector3.Forward, 0f) 
            };

            trailGenerators.Add(new EEngineTrailGenerator(this, new Vector3(3.919f, 0.0f, -14.581f)));
            trailGenerators.Add(new EEngineTrailGenerator(this, new Vector3(-3.919f, 0.0f, -14.581f)));

            setCloseModelByString("Models/Ships/esxolus_bomber_model");
            setFarModelByString("Models/Ships/esxolus_bomber_LP_model");
        }

        public override void fireSecondary()
        {
            if (SecondaryAmmo > 0)
            {
                if (specialTimer <= 0)
                {
                    Quaternion shipRotation = Rotation;
                    Vector3 shipPosition = Position;
                    Vector3 shipVelocity = Velocity;

                    Vector3 SecondaryPosition;
                    if (leftSecondary)
                    {
                        SecondaryPosition = shipPosition + jitterPosition +
                            Vector3.Transform(secondaryAttackPositions[1], shipRotation);
                    }
                    else
                    {
                        SecondaryPosition = shipPosition + jitterPosition +
                            Vector3.Transform(secondaryAttackPositions[0], shipRotation);
                    }
                    leftSecondary = !leftSecondary;

                    spawnBomb(SecondaryPosition, shipRotation, shipVelocity);
                    specialTimer = SpecialRate;
                    SecondaryAmmo--;
                }
                else { }
            }
            else { }
        }

        public void spawnBomb(Vector3 _position, Quaternion _rotation, Vector3 _velocity)
        {
            EsxolusBMissile tBomb;

            tBomb = SceneObjectFactory.createNewEsxolusBMissile(
                _position + Vector3.Transform(Vector3.Forward, _rotation), _rotation, this);
            tBomb.TargetPos = _position + (Vector3.Normalize(_velocity) * 500f);
        }
    }
}
