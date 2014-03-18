using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Space394.Collision;
using Space394.Scenes;
/*using BEPUphysics.Collidables.MobileCollidables;
using BEPUphysics.Collidables;
using BEPUphysics.NarrowPhaseSystems.Pairs; */

namespace Space394.SceneObjects
{
    public class HalkCapitalShip : CapitalShip
    {
        public HalkCapitalShip(long _uniqueId, Vector3 _position, Quaternion _rotation)
            : base(_uniqueId, _position, _rotation, Team.Halk)
        {
            #region Camera Positions
            cameraPositions = new Vector3[]
                {
                    new Vector3(0, 50000, 0),
                    new Vector3(0, -140, 10680),
                    new Vector3(-8510, 1425, -450),
                    new Vector3(8510, 1425, -450)
                };

            cameraViews = new Vector3[]
                {
                    new Vector3(0, -1, 0),
                    new Vector3(0, -40, 6680),
                    new Vector3(-4510, 1525, -450),
                    new Vector3(4510, 1525, -450)
                };

            cameraUps = new Vector3[]
                {
                    new Vector3(0, 0, 1),
                    new Vector3(0, 1, 0),
                    new Vector3(0, 1, 0),
                    new Vector3(0, 1, 0)
                };
            #endregion

            screenModel = ContentLoadManager.loadModel("Models/Ships/CapitalShipSections/H_L_Doors");

            // Center positions of the pieces, mostly for drawing recticles
            shipPiecePositions = new Vector3[]
            {
                new Vector3(5445.229f, -307.698f, 4933.062f),
                new Vector3(5445.229f, -307.698f, 416.721f),
                new Vector3(3357.501f, 1666.45f, -333.489f),
                new Vector3(5445.229f, -307.698f, -3335.115f),
                new Vector3(5445.229f, -307.698f, -6154.579f), 
            
                new Vector3(0.0f, -40.992f, 6505.973f), 
                new Vector3(0.0f, -40.992f, 6505.973f), 
                new Vector3(0.0f, 2860.938f, 1518.291f), 
                new Vector3(0.0f, 634.938f, -3443.972f),            
                new Vector3(0.0f, 108.806f, -5768.761f), 

                new Vector3(-5445.229f, -307.698f, -5567.618f),
                new Vector3(-5080.176f, -307.698f, -3067.395f),
                new Vector3(-3357.501f, 1666.45f, -333.489f),
                new Vector3(-5080.176f, -307.698f, 793.405f),
                new Vector3(-5080.176f, -307.698f, 4593.555f),
 
                new Vector3(0.0f, 2051.71f, -137.093f),
            };

            #region ShipPositions & Spawns
            #region Assault Fighters
            assaultFighterPositions = new Vector3[]
                {
                    new Vector3(0, -40, 6680),
                    new Vector3(-90, -40, 6805),
                    new Vector3(90, -40, 6805),

                    new Vector3(0, -90, 6805),
                    new Vector3(-90, -90, 6930),
                    new Vector3(90, -90, 6930),

                    new Vector3(0, 10, 6805),
                    new Vector3(-90, 10, 6930),
                    new Vector3(90, 10, 6930),
                };
            assaultFighterForward = Quaternion.CreateFromAxisAngle(Vector3.Backward, 0);

            for (int i = 0; i < INITIAL_ASSAULT_FIGHTERS; i++)
            {
                assaultFighterPositions[i] = Vector3.Transform(assaultFighterPositions[i], Rotation) + Position;
            }
            assaultFighterForward = Rotation * assaultFighterForward;
            #endregion

            #region Bombers
            bomberPositions = new Vector3[]
                {
                    new Vector3(-4510, 1525, -450),
                    new Vector3(-4635, 1525, -540),
                    new Vector3(-4635, 1525, -360),

                    new Vector3(-4635, 1475, -450),
                    new Vector3(-4760, 1475, -540),
                    new Vector3(-4760, 1475, -360),

                    new Vector3(-4635, 1575, -450),
                    new Vector3(-4760, 1575, -540),
                    new Vector3(-4760, 1575, -360),
                };

            bomberForward = Quaternion.CreateFromAxisAngle(Vector3.Right, 0);
            bomberForward *= Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-90), 0, 0);

            for (int i = 0; i < INITIAL_BOMBERS; i++)
            {
                bomberPositions[i] = Vector3.Transform(bomberPositions[i], Rotation) + Position;
            }
            bomberForward = Rotation * bomberForward;
            #endregion

            #region Interceptors
            interceptorPositions = new Vector3[]
                {
                    new Vector3(4510, 1525, -450),
                    new Vector3(4635, 1525, -540),
                    new Vector3(4635, 1525, -360),

                    new Vector3(4635, 1475, -450),
                    new Vector3(4760, 1475, -540),
                    new Vector3(4760, 1475, -360),

                    new Vector3(4635, 1575, -450),
                    new Vector3(4760, 1575, -540),
                    new Vector3(4760, 1575, -360),
                };
            interceptorForward = Quaternion.CreateFromAxisAngle(Vector3.Left, 0);
            interceptorForward *= Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(90), 0, 0);

            for (int i = 0; i < INITIAL_INTERCEPTORS; i++)
            {
                interceptorPositions[i] = Vector3.Transform(interceptorPositions[i], Rotation) + Position;
            }
            interceptorForward = Rotation * interceptorForward;
            #endregion
            #endregion

            #region Turrets
            List<Turret> turrets1 = new List<Turret>();
            List<Turret> turrets2 = new List<Turret>();
            List<Turret> turrets3 = new List<Turret>();
            List<Turret> turrets4 = new List<Turret>();
            List<Turret> turrets5 = new List<Turret>();
            List<Turret> turrets6 = new List<Turret>();
            List<Turret> turrets7 = new List<Turret>();
            List<Turret> turrets8 = new List<Turret>();
            List<Turret> turrets9 = new List<Turret>();
            List<Turret> turrets10 = new List<Turret>();
            List<Turret> turrets11 = new List<Turret>();
            List<Turret> turrets12 = new List<Turret>();
            List<Turret> turrets13 = new List<Turret>();
            List<Turret> turrets14 = new List<Turret>();
            List<Turret> turrets15 = new List<Turret>();
            List<Turret> turrets16 = new List<Turret>();

            turrets15.Add(SceneObjectFactory.createHalkTurret(Position + Vector3.Transform(new Vector3(-3953.558f, -237.003f, 6822.489f), Rotation),
                Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-1.534f), MathHelper.ToRadians(8.676f), MathHelper.ToRadians(0f)),
                Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-1.534f), MathHelper.ToRadians(8.676f), MathHelper.ToRadians(0f))), 30, this));
            turrets15.Add(SceneObjectFactory.createHalkTurret(Position + Vector3.Transform(new Vector3(-5161.55f, -237.003f, 6487.202f), Rotation),
                Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-1.618f), MathHelper.ToRadians(-20.327f), MathHelper.ToRadians(0.794f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-1.618f), MathHelper.ToRadians(-20.327f), MathHelper.ToRadians(0.794f))), 30, this));
            turrets6.Add(SceneObjectFactory.createHalkTurret(Position + Vector3.Transform(new Vector3(-1279.945f, 733.959f, 7806.728f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(0f), MathHelper.ToRadians(0f), MathHelper.ToRadians(0f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(0f), MathHelper.ToRadians(0f), MathHelper.ToRadians(0f))), 30, this));
            turrets6.Add(SceneObjectFactory.createHalkTurret(Position + Vector3.Transform(new Vector3(1279.945f, 733.959f, 7806.728f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(0f), MathHelper.ToRadians(0f), MathHelper.ToRadians(0f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(0f), MathHelper.ToRadians(0f), MathHelper.ToRadians(0f))), 30, this));
            turrets16.Add(SceneObjectFactory.createHalkTurret(Position + Vector3.Transform(new Vector3(1616.194f, -1806.898f, 1909.851f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(21.805f), MathHelper.ToRadians(0f), MathHelper.ToRadians(0f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(21.805f), MathHelper.ToRadians(0f), MathHelper.ToRadians(0f))), 30, this));
            turrets16.Add(SceneObjectFactory.createHalkTurret(Position + Vector3.Transform(new Vector3(-1616.194f, -1806.898f, 1909.851f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(21.805f), MathHelper.ToRadians(0f), MathHelper.ToRadians(0f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(21.805f), MathHelper.ToRadians(0f), MathHelper.ToRadians(0f))), 30, this));
            turrets13.Add(SceneObjectFactory.createHalkTurret(Position + Vector3.Transform(new Vector3(-5699.605f, 2176.875f, 1039.199f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(0f), MathHelper.ToRadians(-90f), MathHelper.ToRadians(0f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(0f), MathHelper.ToRadians(-90f), MathHelper.ToRadians(0f))), 30, this));
            turrets13.Add(SceneObjectFactory.createHalkTurret(Position + Vector3.Transform(new Vector3(-5699.605f, 2176.875f, -2058.457f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(0f), MathHelper.ToRadians(-90f), MathHelper.ToRadians(0f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(0f), MathHelper.ToRadians(-90f), MathHelper.ToRadians(0f))), 30, this));
            turrets3.Add(SceneObjectFactory.createHalkTurret(Position + Vector3.Transform(new Vector3(5699.605f, 2176.875f, 1039.199f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(0f), MathHelper.ToRadians(90f), MathHelper.ToRadians(0f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(0f), MathHelper.ToRadians(90f), MathHelper.ToRadians(0f))), 30, this));
            turrets3.Add(SceneObjectFactory.createHalkTurret(Position + Vector3.Transform(new Vector3(5699.605f, 2176.875f, -2058.457f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(0f), MathHelper.ToRadians(90f), MathHelper.ToRadians(0f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(0f), MathHelper.ToRadians(90f), MathHelper.ToRadians(0f))), 30, this));
            // 10
            turrets4.Add(SceneObjectFactory.createHalkTurret(Position + Vector3.Transform(new Vector3(6458.046f, 614.606f, -1719.562f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(0f), MathHelper.ToRadians(91.306f), MathHelper.ToRadians(45.363f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(0f), MathHelper.ToRadians(91.306f), MathHelper.ToRadians(45.363f))), 30, this));
            turrets4.Add(SceneObjectFactory.createHalkTurret(Position + Vector3.Transform(new Vector3(6431.109f, 591.44f, -2617.56f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(0f), MathHelper.ToRadians(91.306f), MathHelper.ToRadians(45.363f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(0f), MathHelper.ToRadians(91.306f), MathHelper.ToRadians(45.363f))), 30, this));
            turrets4.Add(SceneObjectFactory.createHalkTurret(Position + Vector3.Transform(new Vector3(6399.605f, 567.043f, -3694.177f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-0.073f), MathHelper.ToRadians(91.306f), MathHelper.ToRadians(45.363f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-0.073f), MathHelper.ToRadians(91.306f), MathHelper.ToRadians(45.363f))), 30, this));
            turrets4.Add(SceneObjectFactory.createHalkTurret(Position + Vector3.Transform(new Vector3(6384.504f, 522.777f, -4727.678f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(0f), MathHelper.ToRadians(91.306f), MathHelper.ToRadians(45.363f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(0f), MathHelper.ToRadians(91.306f), MathHelper.ToRadians(45.363f))), 30, this));
            turrets4.Add(SceneObjectFactory.createHalkTurret(Position + Vector3.Transform(new Vector3(6426.581f, -1215.904f, -4727.678f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(89.985f), MathHelper.ToRadians(91.306f), MathHelper.ToRadians(45.363f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(89.985f), MathHelper.ToRadians(91.306f), MathHelper.ToRadians(45.363f))), 30, this));
            turrets4.Add(SceneObjectFactory.createHalkTurret(Position + Vector3.Transform(new Vector3(6455.911f, -1243.601f, -3694.177f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(49.023f), MathHelper.ToRadians(93.692f), MathHelper.ToRadians(4.278f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(49.023f), MathHelper.ToRadians(93.692f), MathHelper.ToRadians(4.278f))), 30, this));
            turrets4.Add(SceneObjectFactory.createHalkTurret(Position + Vector3.Transform(new Vector3(6482.029f, -1279.068f, -2617.56f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(35.4f), MathHelper.ToRadians(92.256f), MathHelper.ToRadians(-9.243f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(35.4f), MathHelper.ToRadians(92.256f), MathHelper.ToRadians(-9.243f))), 30, this));
            turrets4.Add(SceneObjectFactory.createHalkTurret(Position + Vector3.Transform(new Vector3(6518.696f, -1289.28f, -1719.562f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(89.985f), MathHelper.ToRadians(91.306f), MathHelper.ToRadians(45.363f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(89.985f), MathHelper.ToRadians(91.306f), MathHelper.ToRadians(45.363f))), 30, this));
            turrets2.Add(SceneObjectFactory.createHalkTurret(Position + Vector3.Transform(new Vector3(6154.156f, 926.766f, 1997.336f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(0f), MathHelper.ToRadians(91.306f), MathHelper.ToRadians(45.363f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(0f), MathHelper.ToRadians(91.306f), MathHelper.ToRadians(45.363f))), 30, this));
            turrets2.Add(SceneObjectFactory.createHalkTurret(Position + Vector3.Transform(new Vector3(6162.478f, 919.087f, 1099.337f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(0f), MathHelper.ToRadians(91.306f), MathHelper.ToRadians(45.363f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(0f), MathHelper.ToRadians(91.306f), MathHelper.ToRadians(45.363f))), 30, this));
            // 20
            turrets2.Add(SceneObjectFactory.createHalkTurret(Position + Vector3.Transform(new Vector3(6352.585f, 729.876f, 22.721f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-179.562f), MathHelper.ToRadians(90.68f), MathHelper.ToRadians(134.879f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-179.562f), MathHelper.ToRadians(90.68f), MathHelper.ToRadians(134.879f))), 30, this));
            turrets2.Add(SceneObjectFactory.createHalkTurret(Position + Vector3.Transform(new Vector3(6389.78f, 693.441f, -1010.78f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(0f), MathHelper.ToRadians(91.306f), MathHelper.ToRadians(45.363f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(0f), MathHelper.ToRadians(91.306f), MathHelper.ToRadians(45.363f))), 30, this));
            turrets2.Add(SceneObjectFactory.createHalkTurret(Position + Vector3.Transform(new Vector3(6581.313f, -1238.029f, -1010.78f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(89.773f), MathHelper.ToRadians(91.306f), MathHelper.ToRadians(45.363f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(89.773f), MathHelper.ToRadians(91.306f), MathHelper.ToRadians(45.363f))), 30, this));
            turrets2.Add(SceneObjectFactory.createHalkTurret(Position + Vector3.Transform(new Vector3(6579.683f, -1240.462f, 166.853f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(92.342f), MathHelper.ToRadians(92.95f), MathHelper.ToRadians(47.43f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(92.342f), MathHelper.ToRadians(92.95f), MathHelper.ToRadians(47.43f))), 30, this));
            turrets2.Add(SceneObjectFactory.createHalkTurret(Position + Vector3.Transform(new Vector3(6567.688f, -1249.722f, 1243.469f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(35.4f), MathHelper.ToRadians(92.256f), MathHelper.ToRadians(-9.243f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(35.4f), MathHelper.ToRadians(92.256f), MathHelper.ToRadians(-9.243f))), 30, this));
            turrets2.Add(SceneObjectFactory.createHalkTurret(Position + Vector3.Transform(new Vector3(6577.115f, -1242.669f, 2141.467f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(89.985f), MathHelper.ToRadians(91.306f), MathHelper.ToRadians(45.363f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(89.985f), MathHelper.ToRadians(91.306f), MathHelper.ToRadians(45.363f))), 30, this));
            turrets1.Add(SceneObjectFactory.createHalkTurret(Position + Vector3.Transform(new Vector3(6769.449f, -538.398f, 3191.67f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(0f), MathHelper.ToRadians(86.131f), MathHelper.ToRadians(-14.113f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(0f), MathHelper.ToRadians(86.131f), MathHelper.ToRadians(-14.113f))), 30, this));
            turrets1.Add(SceneObjectFactory.createHalkTurret(Position + Vector3.Transform(new Vector3(6699.151f, -538.398f, 4245.415f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(0f), MathHelper.ToRadians(86.131f), MathHelper.ToRadians(-14.113f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(0f), MathHelper.ToRadians(86.131f), MathHelper.ToRadians(-14.113f))), 30, this));
            turrets1.Add(SceneObjectFactory.createHalkTurret(Position + Vector3.Transform(new Vector3(6663.679f, -538.398f, 5213.905f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(0f), MathHelper.ToRadians(86.131f), MathHelper.ToRadians(-14.113f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(0f), MathHelper.ToRadians(86.131f), MathHelper.ToRadians(-14.113f))), 30, this));
            turrets1.Add(SceneObjectFactory.createHalkTurret(Position + Vector3.Transform(new Vector3(6386.836f, -538.398f, 6306.999f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(0f), MathHelper.ToRadians(75.606f), MathHelper.ToRadians(-14.113f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(0f), MathHelper.ToRadians(75.606f), MathHelper.ToRadians(-14.113f))), 30, this));
            // 30
            turrets1.Add(SceneObjectFactory.createHalkTurret(Position + Vector3.Transform(new Vector3(6386.836f, -538.398f, -538.398f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(0f), MathHelper.ToRadians(75.606f), MathHelper.ToRadians(-14.113f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(0f), MathHelper.ToRadians(75.606f), MathHelper.ToRadians(-14.113f))), 30, this));
            turrets1.Add(SceneObjectFactory.createHalkTurret(Position + Vector3.Transform(new Vector3(5956.463f, -500.581f, 7528.904f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(20.235f), MathHelper.ToRadians(65.063f), MathHelper.ToRadians(7.877f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(20.235f), MathHelper.ToRadians(65.063f), MathHelper.ToRadians(7.877f))), 30, this));
            turrets1.Add(SceneObjectFactory.createHalkTurret(Position + Vector3.Transform(new Vector3(5156.184f, -500.581f, 8237.227f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(1.537f), MathHelper.ToRadians(-0.589f), MathHelper.ToRadians(0f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(1.537f), MathHelper.ToRadians(-0.589f), MathHelper.ToRadians(0f))), 30, this));
            turrets1.Add(SceneObjectFactory.createHalkTurret(Position + Vector3.Transform(new Vector3(5110.803f, -1430.69f, 6905.87f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(28.059f), MathHelper.ToRadians(63.37f), MathHelper.ToRadians(-72.894f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(28.059f), MathHelper.ToRadians(63.37f), MathHelper.ToRadians(-72.894f))), 30, this));
            turrets1.Add(SceneObjectFactory.createHalkTurret(Position + Vector3.Transform(new Vector3(5129.771f, -1847.84f, 5212.155f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(29.322f), MathHelper.ToRadians(82.158f), MathHelper.ToRadians(-73.176f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(29.322f), MathHelper.ToRadians(82.158f), MathHelper.ToRadians(-73.176f))), 30, this));
            turrets5.Add(SceneObjectFactory.createHalkTurret(Position + Vector3.Transform(new Vector3(6384.504f, 522.777f, -5565.283f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(0f), MathHelper.ToRadians(91.306f), MathHelper.ToRadians(45.363f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(0f), MathHelper.ToRadians(91.306f), MathHelper.ToRadians(45.363f))), 30, this));
            turrets5.Add(SceneObjectFactory.createHalkTurret(Position + Vector3.Transform(new Vector3(6341.374f, -1258.673f, -5565.283f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(300.773f), MathHelper.ToRadians(93.306f), MathHelper.ToRadians(253.917f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(300.773f), MathHelper.ToRadians(93.306f), MathHelper.ToRadians(253.917f))), 30, this));
            turrets5.Add(SceneObjectFactory.createHalkTurret(Position + Vector3.Transform(new Vector3(5280.929f, -1845.324f, -5594.301f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(295.795f), MathHelper.ToRadians(103.543f), MathHelper.ToRadians(221.273f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(295.795f), MathHelper.ToRadians(103.543f), MathHelper.ToRadians(221.273f))), 30, this));
            turrets4.Add(SceneObjectFactory.createHalkTurret(Position + Vector3.Transform(new Vector3(5280.929f, -1911.245f, -3837.246f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(282.79f), MathHelper.ToRadians(102.486f), MathHelper.ToRadians(207.928f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(282.79f), MathHelper.ToRadians(102.486f), MathHelper.ToRadians(207.928f))), 30, this));
            turrets4.Add(SceneObjectFactory.createHalkTurret(Position + Vector3.Transform(new Vector3(5280.929f, -1966.913f, -2445.786f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(286.453f), MathHelper.ToRadians(98.892f), MathHelper.ToRadians(211.204f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(286.453f), MathHelper.ToRadians(98.892f), MathHelper.ToRadians(211.204f))), 30, this));
            // 40
            turrets2.Add(SceneObjectFactory.createHalkTurret(Position + Vector3.Transform(new Vector3(5280.929f, -2003.682f, -637.639f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(263.578f), MathHelper.ToRadians(95.642f), MathHelper.ToRadians(189.538f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(263.578f), MathHelper.ToRadians(95.642f), MathHelper.ToRadians(189.538f))), 30, this));
            turrets2.Add(SceneObjectFactory.createHalkTurret(Position + Vector3.Transform(new Vector3(5582.091f, -1922.029f, 2737.305f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(270.539f), MathHelper.ToRadians(102.172f), MathHelper.ToRadians(195.391f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(270.539f), MathHelper.ToRadians(102.172f), MathHelper.ToRadians(195.391f))), 30, this));
            turrets5.Add(SceneObjectFactory.createHalkTurret(Position + Vector3.Transform(new Vector3(5604.601f, 1013.781f, -5544.579f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(70.664f), MathHelper.ToRadians(95.776f), MathHelper.ToRadians(148.266f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(70.664f), MathHelper.ToRadians(95.776f), MathHelper.ToRadians(148.266f))), 30, this));
            turrets6.Add(SceneObjectFactory.createHalkTurret(Position + Vector3.Transform(new Vector3(1279.945f, -861.24f, 7805.235f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(0f), MathHelper.ToRadians(0f), MathHelper.ToRadians(0f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(0f), MathHelper.ToRadians(0f), MathHelper.ToRadians(0f))), 30, this));
            turrets6.Add(SceneObjectFactory.createHalkTurret(Position + Vector3.Transform(new Vector3(-1279.945f, -861.24f, 7805.095f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(0f), MathHelper.ToRadians(0f), MathHelper.ToRadians(0f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(0f), MathHelper.ToRadians(0f), MathHelper.ToRadians(0f))), 30, this));
            turrets6.Add(SceneObjectFactory.createHalkTurret(Position + Vector3.Transform(new Vector3(-1279.945f, 1495.151f, 7568.532f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-87.804f), MathHelper.ToRadians(0.579f), MathHelper.ToRadians(-0.486f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-87.804f), MathHelper.ToRadians(0.579f), MathHelper.ToRadians(-0.486f))), 30, this));
            turrets6.Add(SceneObjectFactory.createHalkTurret(Position + Vector3.Transform(new Vector3(1279.945f, 1525.514f, 5803.048f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-88.666f), MathHelper.ToRadians(1.082f), MathHelper.ToRadians(-0.938f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-88.666f), MathHelper.ToRadians(1.082f), MathHelper.ToRadians(-0.938f))), 30, this));
            turrets6.Add(SceneObjectFactory.createHalkTurret(Position + Vector3.Transform(new Vector3(-1172.61f, 1614.518f, 5462.168f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-87.804f), MathHelper.ToRadians(0.579f), MathHelper.ToRadians(-0.486f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-87.804f), MathHelper.ToRadians(0.579f), MathHelper.ToRadians(-0.486f))), 30, this));
            turrets6.Add(SceneObjectFactory.createHalkTurret(Position + Vector3.Transform(new Vector3(1129.483f, 1532.576f, 7575.583f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-90.973f), MathHelper.ToRadians(6.65f), MathHelper.ToRadians(0.633f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-90.973f), MathHelper.ToRadians(6.65f), MathHelper.ToRadians(0.633f))), 30, this));
            turrets6.Add(SceneObjectFactory.createHalkTurret(Position + Vector3.Transform(new Vector3(1279.945f, -1596.435f, 7806.728f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(90f), MathHelper.ToRadians(0f), MathHelper.ToRadians(0f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(90f), MathHelper.ToRadians(0f), MathHelper.ToRadians(0f))), 30, this));
            // 50
            turrets6.Add(SceneObjectFactory.createHalkTurret(Position + Vector3.Transform(new Vector3(-1279.945f, -1681.07f, 7495.593f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(89.123f), MathHelper.ToRadians(-1.619f), MathHelper.ToRadians(-3.937f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(89.123f), MathHelper.ToRadians(-1.619f), MathHelper.ToRadians(-3.937f))), 30, this));
            turrets6.Add(SceneObjectFactory.createHalkTurret(Position + Vector3.Transform(new Vector3(-1279.945f, -1684.258f, 5674.688f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(84.839f), MathHelper.ToRadians(-0.37f), MathHelper.ToRadians(-5.282f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(84.839f), MathHelper.ToRadians(-0.37f), MathHelper.ToRadians(-5.282f))), 30, this));
            turrets6.Add(SceneObjectFactory.createHalkTurret(Position + Vector3.Transform(new Vector3(1241.226f, -1636.03f, 5674.688f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(88.711f), MathHelper.ToRadians(1.322f), MathHelper.ToRadians(1.259f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(88.711f), MathHelper.ToRadians(1.322f), MathHelper.ToRadians(1.259f))), 30, this));
            turrets7.Add(SceneObjectFactory.createHalkTurret(Position + Vector3.Transform(new Vector3(-1279.945f, -1685.669f, 3591.917f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(91.947f), MathHelper.ToRadians(-0.775f), MathHelper.ToRadians(-9.101f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(91.947f), MathHelper.ToRadians(-0.775f), MathHelper.ToRadians(-9.101f))), 30, this));
            turrets7.Add(SceneObjectFactory.createHalkTurret(Position + Vector3.Transform(new Vector3(1279.945f, -1650.918f, 3589.366f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(90f), MathHelper.ToRadians(0f), MathHelper.ToRadians(10.377f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(90f), MathHelper.ToRadians(0f), MathHelper.ToRadians(10.377f))), 30, this));
            turrets7.Add(SceneObjectFactory.createHalkTurret(Position + Vector3.Transform(new Vector3(1654.777f, 1450.12f, 3683.626f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-89.026f), MathHelper.ToRadians(1.415f), MathHelper.ToRadians(-17.337f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-89.026f), MathHelper.ToRadians(1.415f), MathHelper.ToRadians(-17.337f))), 30, this));
            turrets7.Add(SceneObjectFactory.createHalkTurret(Position + Vector3.Transform(new Vector3(-1609.457f, 1409.882f, 3248.44f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-90.053f), MathHelper.ToRadians(-5.811f), MathHelper.ToRadians(18.087f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-90.053f), MathHelper.ToRadians(-5.811f), MathHelper.ToRadians(18.087f))), 30, this));
            turrets8.Add(SceneObjectFactory.createHalkTurret(Position + Vector3.Transform(new Vector3(-373.938f, 2099.811f, 3448.665f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-46.038f), MathHelper.ToRadians(0.579f), MathHelper.ToRadians(-0.486f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-46.038f), MathHelper.ToRadians(0.579f), MathHelper.ToRadians(-0.486f))), 30, this));
            turrets8.Add(SceneObjectFactory.createHalkTurret(Position + Vector3.Transform(new Vector3(-983.839f, 3621.956f, 1765.883f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-90f), MathHelper.ToRadians(0f), MathHelper.ToRadians(0f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-90f), MathHelper.ToRadians(0f), MathHelper.ToRadians(0f))), 30, this));
            turrets8.Add(SceneObjectFactory.createHalkTurret(Position + Vector3.Transform(new Vector3(-1331.168f, 3856.8f, 344.307f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-90f), MathHelper.ToRadians(0f), MathHelper.ToRadians(0f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-90f), MathHelper.ToRadians(0f), MathHelper.ToRadians(0f))), 30, this));
            // 60
            turrets8.Add(SceneObjectFactory.createHalkTurret(Position + Vector3.Transform(new Vector3(1331.168f, 3856.8f, 344.307f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-90f), MathHelper.ToRadians(0f), MathHelper.ToRadians(0f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-90f), MathHelper.ToRadians(0f), MathHelper.ToRadians(0f))), 30, this));
            turrets3.Add(SceneObjectFactory.createHalkTurret(Position + Vector3.Transform(new Vector3(2083.951f, 3771.96f, -1857.823f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-162.119f), MathHelper.ToRadians(0.905f), MathHelper.ToRadians(-0.608f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-162.119f), MathHelper.ToRadians(0.905f), MathHelper.ToRadians(-0.608f))), 30, this));
            turrets13.Add(SceneObjectFactory.createHalkTurret(Position + Vector3.Transform(new Vector3(-2083.951f, 3771.96f, -1857.823f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-159.557f), MathHelper.ToRadians(1.402f), MathHelper.ToRadians(-1.34f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-159.557f), MathHelper.ToRadians(1.402f), MathHelper.ToRadians(-1.34f))), 30, this));
            turrets8.Add(SceneObjectFactory.createHalkTurret(Position + Vector3.Transform(new Vector3(-1397.262f, 2882.107f, 2360.486f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(0f), MathHelper.ToRadians(-38.146f), MathHelper.ToRadians(0f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(0f), MathHelper.ToRadians(-38.146f), MathHelper.ToRadians(0f))), 30, this));
            turrets8.Add(SceneObjectFactory.createHalkTurret(Position + Vector3.Transform(new Vector3(1397.262f, 2882.107f, 2360.486f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(0f), MathHelper.ToRadians(38.146f), MathHelper.ToRadians(0f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(0f), MathHelper.ToRadians(38.146f), MathHelper.ToRadians(0f))), 30, this));
            turrets13.Add(SceneObjectFactory.createHalkTurret(Position + Vector3.Transform(new Vector3(-5698.648f, 1313.326f, 1039.199f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(0f), MathHelper.ToRadians(-90f), MathHelper.ToRadians(0f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(0f), MathHelper.ToRadians(-90f), MathHelper.ToRadians(0f))), 30, this));
            turrets3.Add(SceneObjectFactory.createHalkTurret(Position + Vector3.Transform(new Vector3(-1279.945f, -3085.618f, 811.64f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(0f), MathHelper.ToRadians(90f), MathHelper.ToRadians(0f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(0f), MathHelper.ToRadians(90f), MathHelper.ToRadians(0f))), 30, this));
            turrets3.Add(SceneObjectFactory.createHalkTurret(Position + Vector3.Transform(new Vector3(5698.648f, 1313.326f, 1039.199f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(0f), MathHelper.ToRadians(90f), MathHelper.ToRadians(0f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(0f), MathHelper.ToRadians(90f), MathHelper.ToRadians(0f))), 30, this));
            turrets13.Add(SceneObjectFactory.createHalkTurret(Position + Vector3.Transform(new Vector3(-5698.648f, 1313.326f, -2058.457f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(0f), MathHelper.ToRadians(-90f), MathHelper.ToRadians(0f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(0f), MathHelper.ToRadians(-90f), MathHelper.ToRadians(0f))), 30, this));
            turrets16.Add(SceneObjectFactory.createHalkTurret(Position + Vector3.Transform(new Vector3(-1453.611f, -2710.361f, 811.64f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(91.969f), MathHelper.ToRadians(0.114f), MathHelper.ToRadians(-42.45f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(91.969f), MathHelper.ToRadians(0.114f), MathHelper.ToRadians(-42.45f))), 30, this));
            // 70
            turrets16.Add(SceneObjectFactory.createHalkTurret(Position + Vector3.Transform(new Vector3(1453.611f, -2710.361f, 811.64f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(91.971f), MathHelper.ToRadians(-0.072f), MathHelper.ToRadians(41.226f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(91.971f), MathHelper.ToRadians(-0.072f), MathHelper.ToRadians(41.226f))), 30, this));
            turrets16.Add(SceneObjectFactory.createHalkTurret(Position + Vector3.Transform(new Vector3(-1373.741f, -2584.897f, -1154.941f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(93.752f), MathHelper.ToRadians(0f), MathHelper.ToRadians(-39.129f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(93.752f), MathHelper.ToRadians(0f), MathHelper.ToRadians(-39.129f))), 30, this));
            turrets16.Add(SceneObjectFactory.createHalkTurret(Position + Vector3.Transform(new Vector3(1373.741f, -2584.897f, -1154.941f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(94.71f), MathHelper.ToRadians(-0.013f), MathHelper.ToRadians(38.454f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(94.71f), MathHelper.ToRadians(-0.013f), MathHelper.ToRadians(38.454f))), 30, this));
            turrets9.Add(SceneObjectFactory.createHalkTurret(Position + Vector3.Transform(new Vector3(-1453.611f, -2549.924f, -3003.217f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(90.587f), MathHelper.ToRadians(-0.17f), MathHelper.ToRadians(-38.026f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(90.587f), MathHelper.ToRadians(-0.17f), MathHelper.ToRadians(-38.026f))), 30, this));
            turrets9.Add(SceneObjectFactory.createHalkTurret(Position + Vector3.Transform(new Vector3(1279.945f, -2639.126f, -3003.217f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(96.222f), MathHelper.ToRadians(-1.386f), MathHelper.ToRadians(33.86f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(96.222f), MathHelper.ToRadians(-1.386f), MathHelper.ToRadians(33.86f))), 30, this));
            turrets10.Add(SceneObjectFactory.createHalkTurret(Position + Vector3.Transform(new Vector3(-1453.611f, -2247.169f, -5549.897f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(91.614f), MathHelper.ToRadians(0.97f), MathHelper.ToRadians(-33.133f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(91.614f), MathHelper.ToRadians(0.97f), MathHelper.ToRadians(-33.133f))), 30, this));
            turrets10.Add(SceneObjectFactory.createHalkTurret(Position + Vector3.Transform(new Vector3(1453.611f, -2247.169f, -5549.897f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(92.461f), MathHelper.ToRadians(0.73f), MathHelper.ToRadians(34.208f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(92.461f), MathHelper.ToRadians(0.73f), MathHelper.ToRadians(34.208f))), 30, this));
            turrets10.Add(SceneObjectFactory.createHalkTurret(Position + Vector3.Transform(new Vector3(-1279.945f, 3069.206f, -5549.897f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-126.355f), MathHelper.ToRadians(65.813f), MathHelper.ToRadians(-9.074f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-126.355f), MathHelper.ToRadians(65.813f), MathHelper.ToRadians(-9.074f))), 30, this));
            turrets10.Add(SceneObjectFactory.createHalkTurret(Position + Vector3.Transform(new Vector3(1279.945f, 3216.834f, -5549.897f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(101.724f), MathHelper.ToRadians(4.33f), MathHelper.ToRadians(-206.43f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(101.724f), MathHelper.ToRadians(4.33f), MathHelper.ToRadians(-206.43f))), 30, this));
            turrets9.Add(SceneObjectFactory.createHalkTurret(Position + Vector3.Transform(new Vector3(-1279.945f, 3314.872f, -3003.217f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(90f), MathHelper.ToRadians(0f), MathHelper.ToRadians(-153.801f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(90f), MathHelper.ToRadians(0f), MathHelper.ToRadians(-153.801f))), 30, this));
            // 80
            turrets9.Add(SceneObjectFactory.createHalkTurret(Position + Vector3.Transform(new Vector3(1279.945f, 3315.649f, -3003.217f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(90f), MathHelper.ToRadians(0f), MathHelper.ToRadians(154.618f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(90f), MathHelper.ToRadians(0f), MathHelper.ToRadians(154.618f))), 30, this));
            turrets3.Add(SceneObjectFactory.createHalkTurret(Position + Vector3.Transform(new Vector3(3496.364f, 2961.216f, -2139.638f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-161.1f), MathHelper.ToRadians(-0.459f), MathHelper.ToRadians(-0.842f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-161.1f), MathHelper.ToRadians(-0.459f), MathHelper.ToRadians(-0.842f))), 30, this));
            turrets3.Add(SceneObjectFactory.createHalkTurret(Position + Vector3.Transform(new Vector3(5470.439f, 2273.246f, -2377.812f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-161.1f), MathHelper.ToRadians(-0.459f), MathHelper.ToRadians(-0.842f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-161.1f), MathHelper.ToRadians(-0.459f), MathHelper.ToRadians(-0.842f))), 30, this));
            turrets13.Add(SceneObjectFactory.createHalkTurret(Position + Vector3.Transform(new Vector3(3496.364f, 2961.216f, -2139.638f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-161.1f), MathHelper.ToRadians(-0.459f), MathHelper.ToRadians(-0.842f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-161.1f), MathHelper.ToRadians(-0.459f), MathHelper.ToRadians(-0.842f))), 30, this));
            turrets13.Add(SceneObjectFactory.createHalkTurret(Position + Vector3.Transform(new Vector3(-5470.439f, 2273.246f, -2377.812f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-161.1f), MathHelper.ToRadians(-0.459f), MathHelper.ToRadians(-0.842f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-161.1f), MathHelper.ToRadians(-0.459f), MathHelper.ToRadians(-0.842f))), 30, this));
            turrets3.Add(SceneObjectFactory.createHalkTurret(Position + Vector3.Transform(new Vector3(5166.348f, 1862.074f, 1703.733f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-18.411f), MathHelper.ToRadians(-5.618f), MathHelper.ToRadians(-15.821f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-18.411f), MathHelper.ToRadians(-5.618f), MathHelper.ToRadians(-15.821f))), 30, this));
            turrets3.Add(SceneObjectFactory.createHalkTurret(Position + Vector3.Transform(new Vector3(3170.162f, 3128.398f, 1155.579f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-60.738f), MathHelper.ToRadians(-2.117f), MathHelper.ToRadians(-15.854f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-60.738f), MathHelper.ToRadians(-2.117f), MathHelper.ToRadians(-15.854f))), 30, this));
            turrets13.Add(SceneObjectFactory.createHalkTurret(Position + Vector3.Transform(new Vector3(-3045.814f, 3121.189f, 1263.399f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-19.824f), MathHelper.ToRadians(-0.066f), MathHelper.ToRadians(2.123f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-19.824f), MathHelper.ToRadians(-0.066f), MathHelper.ToRadians(2.123f))), 30, this));
            turrets13.Add(SceneObjectFactory.createHalkTurret(Position + Vector3.Transform(new Vector3(5166.348f, 1862.074f, 1703.733f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-18.411f), MathHelper.ToRadians(-5.618f), MathHelper.ToRadians(-15.821f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-18.411f), MathHelper.ToRadians(-5.618f), MathHelper.ToRadians(-15.821f))), 30, this));
            turrets13.Add(SceneObjectFactory.createHalkTurret(Position + Vector3.Transform(new Vector3(-2901.898f, 3532.982f, 344.307f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-87.747f), MathHelper.ToRadians(-0.285f), MathHelper.ToRadians(21.479f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-87.747f), MathHelper.ToRadians(-0.285f), MathHelper.ToRadians(21.479f))), 30, this));
            // 90
            turrets3.Add(SceneObjectFactory.createHalkTurret(Position + Vector3.Transform(new Vector3(4631.224f, 2859.626f, 411.096f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-88.214f), MathHelper.ToRadians(1.404f), MathHelper.ToRadians(-23.879f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-88.214f), MathHelper.ToRadians(1.404f), MathHelper.ToRadians(-23.879f))), 30, this));
            turrets13.Add(SceneObjectFactory.createHalkTurret(Position + Vector3.Transform(new Vector3(-4631.224f, 2799.065f, 344.307f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-89.406f), MathHelper.ToRadians(-0.371f), MathHelper.ToRadians(23.664f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-89.406f), MathHelper.ToRadians(-0.371f), MathHelper.ToRadians(23.664f))), 30, this));
            turrets3.Add(SceneObjectFactory.createHalkTurret(Position + Vector3.Transform(new Vector3(2901.898f, 3532.982f, 344.307f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-88.163f), MathHelper.ToRadians(1.336f), MathHelper.ToRadians(-21.728f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-88.163f), MathHelper.ToRadians(1.336f), MathHelper.ToRadians(-21.728f))), 30, this));
            turrets12.Add(SceneObjectFactory.createHalkTurret(Position + Vector3.Transform(new Vector3(-6464.686f, 614.606f, -1719.562f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-126.781f), MathHelper.ToRadians(91.465f), MathHelper.ToRadians(7.434f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-126.781f), MathHelper.ToRadians(91.465f), MathHelper.ToRadians(7.434f))), 30, this));
            turrets12.Add(SceneObjectFactory.createHalkTurret(Position + Vector3.Transform(new Vector3(-6515.85f, 591.44f, -2617.56f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-89.328f), MathHelper.ToRadians(91.306f), MathHelper.ToRadians(45.363f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-89.328f), MathHelper.ToRadians(91.306f), MathHelper.ToRadians(45.363f))), 30, this));
            turrets12.Add(SceneObjectFactory.createHalkTurret(Position + Vector3.Transform(new Vector3(-6574.89f, 567.043f, -3694.177f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(270.79f), MathHelper.ToRadians(91.306f), MathHelper.ToRadians(45.363f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(270.79f), MathHelper.ToRadians(91.306f), MathHelper.ToRadians(45.363f))), 30, this));
            turrets11.Add(SceneObjectFactory.createHalkTurret(Position + Vector3.Transform(new Vector3(-7132.872f, 522.777f, -5476.045f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(0f), MathHelper.ToRadians(-84.189f), MathHelper.ToRadians(0f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(0f), MathHelper.ToRadians(-84.189f), MathHelper.ToRadians(0f))), 30, this));
            turrets11.Add(SceneObjectFactory.createHalkTurret(Position + Vector3.Transform(new Vector3(-7132.872f, -1215.904f, -5476.045f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(0f), MathHelper.ToRadians(-84.189f), MathHelper.ToRadians(0f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(0f), MathHelper.ToRadians(-84.189f), MathHelper.ToRadians(0f))), 30, this));
            turrets12.Add(SceneObjectFactory.createHalkTurret(Position + Vector3.Transform(new Vector3(-6569.768f, -1243.601f, -3694.177f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(158.282f), MathHelper.ToRadians(95.559f), MathHelper.ToRadians(16.422f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(158.282f), MathHelper.ToRadians(95.559f), MathHelper.ToRadians(16.422f))), 30, this));
            turrets12.Add(SceneObjectFactory.createHalkTurret(Position + Vector3.Transform(new Vector3(-6448.469f, -1279.068f, -2617.56f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(161.963f), MathHelper.ToRadians(94.102f), MathHelper.ToRadians(16.734f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(161.963f), MathHelper.ToRadians(94.102f), MathHelper.ToRadians(16.734f))), 30, this));
            // 100
            turrets12.Add(SceneObjectFactory.createHalkTurret(Position + Vector3.Transform(new Vector3(-6429.133f, -1289.28f, -1719.562f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(383.351f), MathHelper.ToRadians(91.437f), MathHelper.ToRadians(244.83f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(383.351f), MathHelper.ToRadians(91.437f), MathHelper.ToRadians(244.83f))), 30, this));
            turrets14.Add(SceneObjectFactory.createHalkTurret(Position + Vector3.Transform(new Vector3(-6044.427f, 778.519f, 1997.336f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(168.108f), MathHelper.ToRadians(92.096f), MathHelper.ToRadians(-57.452f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(168.108f), MathHelper.ToRadians(92.096f), MathHelper.ToRadians(-57.452f))), 30, this));
            turrets14.Add(SceneObjectFactory.createHalkTurret(Position + Vector3.Transform(new Vector3(-6157.026f, 730.37f, 1099.337f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(202.058f), MathHelper.ToRadians(94.702f), MathHelper.ToRadians(-23.007f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(202.058f), MathHelper.ToRadians(94.702f), MathHelper.ToRadians(-23.007f))), 30, this));
            turrets14.Add(SceneObjectFactory.createHalkTurret(Position + Vector3.Transform(new Vector3(-6263.889f, 702.556f, 22.721f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(207.518f), MathHelper.ToRadians(92.67f), MathHelper.ToRadians(-17.524f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(207.518f), MathHelper.ToRadians(92.67f), MathHelper.ToRadians(-17.524f))), 30, this));
            turrets14.Add(SceneObjectFactory.createHalkTurret(Position + Vector3.Transform(new Vector3(-6384.6f, 654.82f, -1010.78f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(225.944f), MathHelper.ToRadians(93.921f), MathHelper.ToRadians(1.622f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(225.944f), MathHelper.ToRadians(93.921f), MathHelper.ToRadians(1.622f))), 30, this));
            turrets14.Add(SceneObjectFactory.createHalkTurret(Position + Vector3.Transform(new Vector3(-6407.977f, -1238.029f, -1010.78f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(229.234f), MathHelper.ToRadians(106.591f), MathHelper.ToRadians(92.511f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(229.234f), MathHelper.ToRadians(106.591f), MathHelper.ToRadians(92.511f))), 30, this));
            turrets14.Add(SceneObjectFactory.createHalkTurret(Position + Vector3.Transform(new Vector3(-6277.368f, -1164.391f, 127.842f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(181.091f), MathHelper.ToRadians(96.087f), MathHelper.ToRadians(36.225f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(181.091f), MathHelper.ToRadians(96.087f), MathHelper.ToRadians(36.225f))), 30, this));
            turrets14.Add(SceneObjectFactory.createHalkTurret(Position + Vector3.Transform(new Vector3(-6108.751f, -1249.722f, 1243.469f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(114.791f), MathHelper.ToRadians(91.571f), MathHelper.ToRadians(-20.888f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(114.791f), MathHelper.ToRadians(91.571f), MathHelper.ToRadians(-20.888f))), 30, this));
            turrets14.Add(SceneObjectFactory.createHalkTurret(Position + Vector3.Transform(new Vector3(-6087.566f, -1242.669f, 2141.467f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(182.119f), MathHelper.ToRadians(91.306f), MathHelper.ToRadians(45.363f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(182.119f), MathHelper.ToRadians(91.306f), MathHelper.ToRadians(45.363f))), 30, this));
            turrets15.Add(SceneObjectFactory.createHalkTurret(Position + Vector3.Transform(new Vector3(-6242.753f, -538.398f, 3191.67f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-370.708f), MathHelper.ToRadians(81.942f), MathHelper.ToRadians(-183.312f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-370.708f), MathHelper.ToRadians(81.942f), MathHelper.ToRadians(-183.312f))), 30, this));
            // 110
            turrets15.Add(SceneObjectFactory.createHalkTurret(Position + Vector3.Transform(new Vector3(-6233.683f, -295.971f, 4245.415f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-338.654f), MathHelper.ToRadians(79.642f), MathHelper.ToRadians(-159.705f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-338.654f), MathHelper.ToRadians(79.642f), MathHelper.ToRadians(-159.705f))), 30, this));
            turrets15.Add(SceneObjectFactory.createHalkTurret(Position + Vector3.Transform(new Vector3(-5920.788f, -538.398f, 5213.905f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-342.755f), MathHelper.ToRadians(81.265f), MathHelper.ToRadians(-148.029f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-342.755f), MathHelper.ToRadians(81.265f), MathHelper.ToRadians(-148.029f))), 30, this));
            turrets11.Add(SceneObjectFactory.createHalkTurret(Position + Vector3.Transform(new Vector3(-4597.679f, -2040.064f, -5832.757f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(90f), MathHelper.ToRadians(0f), MathHelper.ToRadians(0f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(90f), MathHelper.ToRadians(0f), MathHelper.ToRadians(0f))), 30, this));
            turrets12.Add(SceneObjectFactory.createHalkTurret(Position + Vector3.Transform(new Vector3(-5637.044f, -1938.13f, -3837.246f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(102.571f), MathHelper.ToRadians(98.709f), MathHelper.ToRadians(357.202f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(102.571f), MathHelper.ToRadians(98.709f), MathHelper.ToRadians(357.202f))), 30, this));
            turrets14.Add(SceneObjectFactory.createHalkTurret(Position + Vector3.Transform(new Vector3(-5637.044f, -1833.107f, 33.902f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(252.557f), MathHelper.ToRadians(94.216f), MathHelper.ToRadians(145.755f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(252.557f), MathHelper.ToRadians(94.216f), MathHelper.ToRadians(145.755f))), 30, this));
            turrets12.Add(SceneObjectFactory.createHalkTurret(Position + Vector3.Transform(new Vector3(-5611.833f, -1913.113f, -1833.449f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(262.413f), MathHelper.ToRadians(95.444f), MathHelper.ToRadians(157.121f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(262.413f), MathHelper.ToRadians(95.444f), MathHelper.ToRadians(157.121f))), 30, this));
            turrets14.Add(SceneObjectFactory.createHalkTurret(Position + Vector3.Transform(new Vector3(-5335.882f, -1789.109f, 2737.305f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(261.935f), MathHelper.ToRadians(100.189f), MathHelper.ToRadians(155.896f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(261.935f), MathHelper.ToRadians(100.189f), MathHelper.ToRadians(155.896f))), 30, this));
            turrets15.Add(SceneObjectFactory.createHalkTurret(Position + Vector3.Transform(new Vector3(-5034.054f, -1562.371f, 5267.118f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(31.708f), MathHelper.ToRadians(82.158f), MathHelper.ToRadians(-73.176f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(31.708f), MathHelper.ToRadians(82.158f), MathHelper.ToRadians(-73.176f))), 30, this));
            turrets9.Add(SceneObjectFactory.createHalkTurret(Position + Vector3.Transform(new Vector3(-2453.638f, 2751.554f, -3003.217f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(268.406f), MathHelper.ToRadians(-0.278f), MathHelper.ToRadians(27.042f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(268.406f), MathHelper.ToRadians(-0.278f), MathHelper.ToRadians(27.042f))), 30, this));
            turrets10.Add(SceneObjectFactory.createHalkTurret(Position + Vector3.Transform(new Vector3(2453.638f, 2470.914f, -5549.897f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(111.105f), MathHelper.ToRadians(16.185f), MathHelper.ToRadians(-206.749f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(111.105f), MathHelper.ToRadians(16.185f), MathHelper.ToRadians(-206.749f))), 30, this));
            // 120
            turrets9.Add(SceneObjectFactory.createHalkTurret(Position + Vector3.Transform(new Vector3(2453.638f, 2751.554f, -3003.217f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(269.281f), MathHelper.ToRadians(-1.449f), MathHelper.ToRadians(-26.684f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(269.281f), MathHelper.ToRadians(-1.449f), MathHelper.ToRadians(-26.684f))), 30, this));
            turrets10.Add(SceneObjectFactory.createHalkTurret(Position + Vector3.Transform(new Vector3(-2453.638f, 2469.494f, -5549.897f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(114.568f), MathHelper.ToRadians(34.107f), MathHelper.ToRadians(227.792f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(114.568f), MathHelper.ToRadians(34.107f), MathHelper.ToRadians(227.792f))), 30, this));
            turrets10.Add(SceneObjectFactory.createHalkTurret(Position + Vector3.Transform(new Vector3(0f, -2550.234f, -5549.897f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(92.416f), MathHelper.ToRadians(0f), MathHelper.ToRadians(0f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(92.416f), MathHelper.ToRadians(0f), MathHelper.ToRadians(0f))), 30, this));
            turrets9.Add(SceneObjectFactory.createHalkTurret(Position + Vector3.Transform(new Vector3(0f, -2844.579f, -3003.217f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(94.268f), MathHelper.ToRadians(0f), MathHelper.ToRadians(0f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(94.268f), MathHelper.ToRadians(0f), MathHelper.ToRadians(0f))), 30, this));
            turrets16.Add(SceneObjectFactory.createHalkTurret(Position + Vector3.Transform(new Vector3(0f, -2880.672f, -1154.941f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(96.391f), MathHelper.ToRadians(0f), MathHelper.ToRadians(0f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(96.391f), MathHelper.ToRadians(0f), MathHelper.ToRadians(0f))), 30, this));
            turrets16.Add(SceneObjectFactory.createHalkTurret(Position + Vector3.Transform(new Vector3(0f, -3085.618f, 811.64f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(95.203f), MathHelper.ToRadians(0f), MathHelper.ToRadians(0f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(95.203f), MathHelper.ToRadians(0f), MathHelper.ToRadians(0f))), 30, this));
            turrets3.Add(SceneObjectFactory.createHalkTurret(Position + Vector3.Transform(new Vector3(4631.224f, 2799.065f, -1244.237f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-89.731f), MathHelper.ToRadians(1.415f), MathHelper.ToRadians(-24.236f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-89.731f), MathHelper.ToRadians(1.415f), MathHelper.ToRadians(-24.236f))), 30, this));
            turrets3.Add(SceneObjectFactory.createHalkTurret(Position + Vector3.Transform(new Vector3(2901.898f, 3532.982f, -1244.237f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-88.187f), MathHelper.ToRadians(1.368f), MathHelper.ToRadians(-22.758f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-88.187f), MathHelper.ToRadians(1.368f), MathHelper.ToRadians(-22.758f))), 30, this));
            turrets8.Add(SceneObjectFactory.createHalkTurret(Position + Vector3.Transform(new Vector3(1331.168f, 3856.8f, -1244.237f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-90f), MathHelper.ToRadians(0f), MathHelper.ToRadians(0f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-90f), MathHelper.ToRadians(0f), MathHelper.ToRadians(0f))), 30, this));
            turrets8.Add(SceneObjectFactory.createHalkTurret(Position + Vector3.Transform(new Vector3(-1331.168f, 3856.8f, -1244.237f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-90f), MathHelper.ToRadians(0f), MathHelper.ToRadians(0f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-90f), MathHelper.ToRadians(0f), MathHelper.ToRadians(0f))), 30, this));
            // 130
            turrets13.Add(SceneObjectFactory.createHalkTurret(Position + Vector3.Transform(new Vector3(-2901.898f, 3532.982f, -1244.237f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-87.747f), MathHelper.ToRadians(-0.285f), MathHelper.ToRadians(21.479f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-87.747f), MathHelper.ToRadians(-0.285f), MathHelper.ToRadians(21.479f))), 30, this));
            turrets13.Add(SceneObjectFactory.createHalkTurret(Position + Vector3.Transform(new Vector3(-4631.224f, 2799.065f, -1244.237f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-89.406f), MathHelper.ToRadians(-0.371f), MathHelper.ToRadians(23.664f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-89.406f), MathHelper.ToRadians(-0.371f), MathHelper.ToRadians(23.664f))), 30, this));
            turrets8.Add(SceneObjectFactory.createHalkTurret(Position + Vector3.Transform(new Vector3(983.839f, 3621.956f, 1765.883f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-90f), MathHelper.ToRadians(0f), MathHelper.ToRadians(0f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-90f), MathHelper.ToRadians(0f), MathHelper.ToRadians(0f))), 30, this));
            turrets8.Add(SceneObjectFactory.createHalkTurret(Position + Vector3.Transform(new Vector3(373.938f, 2099.811f, 3448.665f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-46.038f), MathHelper.ToRadians(0.579f), MathHelper.ToRadians(-0.486f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-46.038f), MathHelper.ToRadians(0.579f), MathHelper.ToRadians(-0.486f))), 30, this));
            turrets11.Add(SceneObjectFactory.createHalkTurret(Position + Vector3.Transform(new Vector3(-6386.105f, -2040.064f, -5705.264f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(90f), MathHelper.ToRadians(0f), MathHelper.ToRadians(0f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(90f), MathHelper.ToRadians(0f), MathHelper.ToRadians(0f))), 30, this));
            turrets11.Add(SceneObjectFactory.createHalkTurret(Position + Vector3.Transform(new Vector3(-6386.105f, -2024.796f, -4986.711f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(90f), MathHelper.ToRadians(0f), MathHelper.ToRadians(0f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(90f), MathHelper.ToRadians(0f), MathHelper.ToRadians(0f))), 30, this));
            turrets11.Add(SceneObjectFactory.createHalkTurret(Position + Vector3.Transform(new Vector3(-4597.679f, 1100.64f, -5517.225f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-90f), MathHelper.ToRadians(0f), MathHelper.ToRadians(0f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-90f), MathHelper.ToRadians(0f), MathHelper.ToRadians(0f))), 30, this));
            turrets11.Add(SceneObjectFactory.createHalkTurret(Position + Vector3.Transform(new Vector3(-6386.105f, 1100.64f, -5389.732f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-90f), MathHelper.ToRadians(0f), MathHelper.ToRadians(0f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-90f), MathHelper.ToRadians(0f), MathHelper.ToRadians(0f))), 30, this));

            shipPieces.Add(new SpawnShipPiece(UniqueId, Position, Rotation, ShipTeam,
                ContentLoadManager.loadModel("Models/Ships/CapitalShipSections/HCP1"),
                ContentLoadManager.loadModel("Models/Ships/CapitalShipSections/HCD1"),
                ContentLoadManager.loadModel("Models/Ships/CapitalShipSections/HCC1"),
                100, turrets1, this));

            shipPieces.Add(new SpawnShipPiece(UniqueId, Position, Rotation, ShipTeam,
                ContentLoadManager.loadModel("Models/Ships/CapitalShipSections/HCP2"),
                ContentLoadManager.loadModel("Models/Ships/CapitalShipSections/HCD2"),
                ContentLoadManager.loadModel("Models/Ships/CapitalShipSections/HCC2"),
                100, turrets2, this));

            shipPieces.Add(new SpawnShipPiece(UniqueId, Position, Rotation, ShipTeam,
                ContentLoadManager.loadModel("Models/Ships/CapitalShipSections/HCP3"),
                ContentLoadManager.loadModel("Models/Ships/CapitalShipSections/HCD3"),
                ContentLoadManager.loadModel("Models/Ships/CapitalShipSections/HCC3"),
                100, turrets3, this));

            shipPieces.Add(new SpawnShipPiece(UniqueId, Position, Rotation, ShipTeam,
                ContentLoadManager.loadModel("Models/Ships/CapitalShipSections/HCP4"),
                ContentLoadManager.loadModel("Models/Ships/CapitalShipSections/HCD4"),
                ContentLoadManager.loadModel("Models/Ships/CapitalShipSections/HCC4"),
                100, turrets4, this));

            shipPieces.Add(new SpawnShipPiece(UniqueId, Position, Rotation, ShipTeam,
                ContentLoadManager.loadModel("Models/Ships/CapitalShipSections/HCP5"),
                ContentLoadManager.loadModel("Models/Ships/CapitalShipSections/HCD5"),
                ContentLoadManager.loadModel("Models/Ships/CapitalShipSections/HCC5"),
                100, turrets5, this));

            shipPieces.Add(new SpawnShipPiece(UniqueId, Position, Rotation, ShipTeam,
                ContentLoadManager.loadModel("Models/Ships/CapitalShipSections/HCP6"),
                ContentLoadManager.loadModel("Models/Ships/CapitalShipSections/HCD6"),
                ContentLoadManager.loadModel("Models/Ships/CapitalShipSections/HCC6"),
                100, turrets6, this));

            shipPieces.Add(new SpawnShipPiece(UniqueId, Position, Rotation, ShipTeam,
                ContentLoadManager.loadModel("Models/Ships/CapitalShipSections/HCP7"),
                ContentLoadManager.loadModel("Models/Ships/CapitalShipSections/HCD7"),
                ContentLoadManager.loadModel("Models/Ships/CapitalShipSections/HCC7"),
                100, turrets7, this));

            shipPieces.Add(new SpawnShipPiece(UniqueId, Position, Rotation, ShipTeam,
                ContentLoadManager.loadModel("Models/Ships/CapitalShipSections/HCP8"),
                ContentLoadManager.loadModel("Models/Ships/CapitalShipSections/HCD8"),
                ContentLoadManager.loadModel("Models/Ships/CapitalShipSections/HCC8"),
                100, turrets8, this));

            shipPieces.Add(new SpawnShipPiece(UniqueId, Position, Rotation, ShipTeam,
                ContentLoadManager.loadModel("Models/Ships/CapitalShipSections/HCP9"),
                ContentLoadManager.loadModel("Models/Ships/CapitalShipSections/HCD9"),
                ContentLoadManager.loadModel("Models/Ships/CapitalShipSections/HCC9"),
                100, turrets9, this));

            shipPieces.Add(new SpawnShipPiece(UniqueId, Position, Rotation, ShipTeam,
                ContentLoadManager.loadModel("Models/Ships/CapitalShipSections/HCP10"),
                ContentLoadManager.loadModel("Models/Ships/CapitalShipSections/HCD10"),
                ContentLoadManager.loadModel("Models/Ships/CapitalShipSections/HCC10"),
                100, turrets10, this));

            shipPieces.Add(new SpawnShipPiece(UniqueId, Position, Rotation, ShipTeam,
                ContentLoadManager.loadModel("Models/Ships/CapitalShipSections/HCP11"),
                ContentLoadManager.loadModel("Models/Ships/CapitalShipSections/HCD11"),
                ContentLoadManager.loadModel("Models/Ships/CapitalShipSections/HCC11"),
                100, turrets11, this));

            shipPieces.Add(new SpawnShipPiece(UniqueId, Position, Rotation, ShipTeam,
                ContentLoadManager.loadModel("Models/Ships/CapitalShipSections/HCP12"),
                ContentLoadManager.loadModel("Models/Ships/CapitalShipSections/HCD12"),
                ContentLoadManager.loadModel("Models/Ships/CapitalShipSections/HCC12"),
                100, turrets12, this));

            shipPieces.Add(new SpawnShipPiece(UniqueId, Position, Rotation, ShipTeam,
                ContentLoadManager.loadModel("Models/Ships/CapitalShipSections/HCP13"),
                ContentLoadManager.loadModel("Models/Ships/CapitalShipSections/HCD13"),
                ContentLoadManager.loadModel("Models/Ships/CapitalShipSections/HCC13"),
                100, turrets13, this));

            shipPieces.Add(new SpawnShipPiece(UniqueId, Position, Rotation, ShipTeam,
                ContentLoadManager.loadModel("Models/Ships/CapitalShipSections/HCP14"),
                ContentLoadManager.loadModel("Models/Ships/CapitalShipSections/HCD14"),
                ContentLoadManager.loadModel("Models/Ships/CapitalShipSections/HCC14"),
                100, turrets14, this));

            shipPieces.Add(new SpawnShipPiece(UniqueId, Position, Rotation, ShipTeam,
                ContentLoadManager.loadModel("Models/Ships/CapitalShipSections/HCP15"),
                ContentLoadManager.loadModel("Models/Ships/CapitalShipSections/HCD15"),
                ContentLoadManager.loadModel("Models/Ships/CapitalShipSections/HCC15"),
                100, turrets15, this));

            shipPieces.Add(new SpawnShipPiece(UniqueId, Position, Rotation, ShipTeam,
                ContentLoadManager.loadModel("Models/Ships/CapitalShipSections/HCP16"),
                ContentLoadManager.loadModel("Models/Ships/CapitalShipSections/HCD16"),
                ContentLoadManager.loadModel("Models/Ships/CapitalShipSections/HCC16"),
                100, turrets16, this));
            #endregion

            Initialize();

            setModelByString("Models/box");
            destroyedModel = Model;
            setCloseModelByString("Models/box");
            // collisionMesh = ContentLoadManager.loadModel("Models/Ships/Halk_Capital_Collision");

            radiusSphere = new CollisionSphere(Position, Model);

            /*collisionBase = new CollisionMesh(collisionMesh, Position, Rotation);
            collisionBase.setActive(true);
            collisionBase.addCollisionEvent(collisionEvent);
            collisionBase.setParent(this);*/
        }
    }
}
