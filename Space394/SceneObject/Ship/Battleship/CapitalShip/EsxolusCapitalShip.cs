using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Space394.Collision;
using Space394.Scenes;

/*using BEPUphysics.Collidables.MobileCollidables;
using BEPUphysics.Collidables;
using BEPUphysics.NarrowPhaseSystems.Pairs; */

namespace Space394.SceneObjects
{
    public class EsxolusCapitalShip : CapitalShip
    {
        public EsxolusCapitalShip(long _uniqueId, Vector3 _position, Quaternion _rotation)
            : base(_uniqueId, _position, _rotation, Team.Esxolus)
        {
            #region Camera Positions
            cameraPositions = new Vector3[]
                {
                    new Vector3(0, 50000, 0),
                    new Vector3(0, 1550, 3000), //375
                    new Vector3(0, -900, 12525), //3560
                    new Vector3(0, 2700, -375) //-750
                };

            cameraViews = new Vector3[]
                {
                    new Vector3(0, -1, 0),
                    new Vector3(0, 1550, 0),
                    new Vector3(0, -900, 9525),
                    new Vector3(0, 2700, -3375)
                };

            cameraUps = new Vector3[]
                {
                    new Vector3(0, 0, 1),
                    new Vector3(0, 1, 0),
                    new Vector3(0, 1, 0),
                    new Vector3(0, 1, 0)
                };
            #endregion 

            screenModel = ContentLoadManager.loadModel("Models/Ships/esxolus_capital_ship_screen_model");

            #region ShipPositions & Spawns
            #region Assault Fighters
            assaultFighterPositions = new Vector3[]
                {
                    new Vector3(0, 1650, 0), // 125
                    new Vector3(-90, 1650, 125),
                    new Vector3(90, 1650, 125),

                    new Vector3(0, 1600, 125),
                    new Vector3(-90, 1600, 250),
                    new Vector3(90, 1600, 250),

                    new Vector3(0, 1700, 125),
                    new Vector3(-90, 1700, 250),
                    new Vector3(90, 1700, 250),
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
                    new Vector3(0, 2800, -3375), // -3250
                    new Vector3(-90, 2800, -3250),
                    new Vector3(90, 2800, -3250),

                    new Vector3(0, 2750, -3250),
                    new Vector3(-90, 2750, -3125),
                    new Vector3(90, 2750, -3125),

                    new Vector3(0, 2850, -3250),
                    new Vector3(-90, 2850, -3125),
                    new Vector3(90, 2850, -3125),
                };
            bomberForward = Quaternion.CreateFromAxisAngle(Vector3.Backward, 0);

            for (int i = 0; i < INITIAL_BOMBERS; i++)
            {
                bomberPositions[i] = Vector3.Transform(bomberPositions[i], Rotation) + Position;
            }
            bomberForward = Rotation * bomberForward;
            #endregion 

            #region Interceptors
            interceptorPositions = new Vector3[]
                {
                    new Vector3(0, -750, 9525), // 9650 // 9680
                    new Vector3(-90, -750, 9650),
                    new Vector3(90, -750, 9650),

                    new Vector3(0, -800, 9650),
                    new Vector3(-90, -800, 9775),
                    new Vector3(90, -800, 9775),

                    new Vector3(0, -700, 9650),
                    new Vector3(-90, -700, 9775),
                    new Vector3(90, -700, 9775),
                };
            interceptorForward = Quaternion.CreateFromAxisAngle(Vector3.Backward, 0);

            for (int i = 0; i < INITIAL_INTERCEPTORS; i++)
            {
                interceptorPositions[i] = Vector3.Transform(interceptorPositions[i], Rotation) + Position;
            }
            interceptorForward = Rotation * interceptorForward;
            #endregion 
            #endregion

            #region Turrets
            // Forward Hull
            List<Turret> forwardHullTurrets = new List<Turret>();
            // Port Bow
            forwardHullTurrets.Add(SceneObjectFactory.createEsxolusTurret(Position + Vector3.Transform(new Vector3(817.5f, -298.5f, 12183.3f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(3.991f), MathHelper.ToRadians(15.35f), MathHelper.ToRadians(-9.461f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(3.991f), MathHelper.ToRadians(15.35f), MathHelper.ToRadians(-9.461f))), 30, this));
            forwardHullTurrets.Add(SceneObjectFactory.createEsxolusTurret(Position + Vector3.Transform(new Vector3(1463.7f, -407.7f, 12007.5f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(3.649f), MathHelper.ToRadians(16.136f), MathHelper.ToRadians(-9.527f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(3.649f), MathHelper.ToRadians(16.136f), MathHelper.ToRadians(-9.527f))), 30, this));
            forwardHullTurrets.Add(SceneObjectFactory.createEsxolusTurret(Position + Vector3.Transform(new Vector3(2094f, -434.4f, 10430.1f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(10.01f), MathHelper.ToRadians(68.154f), MathHelper.ToRadians(-1.754f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(10.01f), MathHelper.ToRadians(68.154f), MathHelper.ToRadians(-1.754f))), 30, this));
            forwardHullTurrets.Add(SceneObjectFactory.createEsxolusTurret(Position + Vector3.Transform(new Vector3(2723.7f, -461.7f, 8854.5f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(10.001f), MathHelper.ToRadians(68.154f), MathHelper.ToRadians(-1.754f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(10.001f), MathHelper.ToRadians(68.154f), MathHelper.ToRadians(-1.754f))), 30, this));
            forwardHullTurrets.Add(SceneObjectFactory.createEsxolusTurret(Position + Vector3.Transform(new Vector3(3357.9f, -489.3f, 7267.8f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(10.01f), MathHelper.ToRadians(68.154f), MathHelper.ToRadians(-1.754f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(10.01f), MathHelper.ToRadians(68.154f), MathHelper.ToRadians(-1.754f))), 30, this));
            forwardHullTurrets.Add(SceneObjectFactory.createEsxolusTurret(Position + Vector3.Transform(new Vector3(3980.4f, -515.7f, 5710.5f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(9.442f), MathHelper.ToRadians(68.194f), MathHelper.ToRadians(-2.376f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(9.442f), MathHelper.ToRadians(68.194f), MathHelper.ToRadians(-2.376f))), 30, this));
            forwardHullTurrets.Add(SceneObjectFactory.createEsxolusTurret(Position + Vector3.Transform(new Vector3(3204f, -289.8f, 4371f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(174.095f), MathHelper.ToRadians(58.948f), MathHelper.ToRadians(163.57f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(174.095f), MathHelper.ToRadians(58.948f), MathHelper.ToRadians(163.57f))), 30, this));
            forwardHullTurrets.Add(SceneObjectFactory.createEsxolusTurret(Position + Vector3.Transform(new Vector3(2419.5f, -45.3f, 2984.1f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(91.708f), MathHelper.ToRadians(83.308f), MathHelper.ToRadians(75.531f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(91.708f), MathHelper.ToRadians(83.308f), MathHelper.ToRadians(75.531f))), 30, this));
            forwardHullTurrets.Add(SceneObjectFactory.createEsxolusTurret(Position + Vector3.Transform(new Vector3(123.3f, 180f, 11388.3f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(50.286f), MathHelper.ToRadians(-83.61f), MathHelper.ToRadians(-87.058f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(50.286f), MathHelper.ToRadians(-83.61f), MathHelper.ToRadians(-87.058f))), 30, this));
            forwardHullTurrets.Add(SceneObjectFactory.createEsxolusTurret(Position + Vector3.Transform(new Vector3(90f, 828.3f, 5587.5f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(43.259f), MathHelper.ToRadians(-83.981f), MathHelper.ToRadians(-83.618f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(43.259f), MathHelper.ToRadians(-83.981f), MathHelper.ToRadians(-83.618f))), 30, this));
            forwardHullTurrets.Add(SceneObjectFactory.createEsxolusTurret(Position + Vector3.Transform(new Vector3(817.6f, -531.6f, 11968.8f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(178.859f), MathHelper.ToRadians(10.082f), MathHelper.ToRadians(-3.444f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(178.859f), MathHelper.ToRadians(10.082f), MathHelper.ToRadians(-3.444f))), 30, this));
            forwardHullTurrets.Add(SceneObjectFactory.createEsxolusTurret(Position + Vector3.Transform(new Vector3(1463.7f, -571.2f, 11872.2f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(178.155f), MathHelper.ToRadians(12.255f), MathHelper.ToRadians(-4.336f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(178.155f), MathHelper.ToRadians(12.255f), MathHelper.ToRadians(-4.336f))), 30, this));
            // Starboard Bow
            forwardHullTurrets.Add(SceneObjectFactory.createEsxolusTurret(Position + Vector3.Transform(new Vector3(-817.5f, -298.5f, 12183.3f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(3.991f), MathHelper.ToRadians(-15.35f), MathHelper.ToRadians(9.461f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(3.991f), MathHelper.ToRadians(-15.35f), MathHelper.ToRadians(9.461f))), 30, this));
            forwardHullTurrets.Add(SceneObjectFactory.createEsxolusTurret(Position + Vector3.Transform(new Vector3(-1463.7f, -407.7f, 12007.5f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(3.649f), MathHelper.ToRadians(-16.136f), MathHelper.ToRadians(9.527f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(3.649f), MathHelper.ToRadians(-16.136f), MathHelper.ToRadians(9.527f))), 30, this));
            forwardHullTurrets.Add(SceneObjectFactory.createEsxolusTurret(Position + Vector3.Transform(new Vector3(-2094f, -434.4f, 10430.1f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(10.01f), MathHelper.ToRadians(-68.154f), MathHelper.ToRadians(1.754f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(10.01f), MathHelper.ToRadians(-68.154f), MathHelper.ToRadians(1.754f))), 30, this));
            forwardHullTurrets.Add(SceneObjectFactory.createEsxolusTurret(Position + Vector3.Transform(new Vector3(-2723.7f, -461.7f, 8854.5f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(10.001f), MathHelper.ToRadians(-68.154f), MathHelper.ToRadians(1.754f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(10.001f), MathHelper.ToRadians(-68.154f), MathHelper.ToRadians(1.754f))), 30, this));
            forwardHullTurrets.Add(SceneObjectFactory.createEsxolusTurret(Position + Vector3.Transform(new Vector3(-3357.9f, -489.3f, 7267.8f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(10.01f), MathHelper.ToRadians(-68.154f), MathHelper.ToRadians(1.754f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(10.01f), MathHelper.ToRadians(-68.154f), MathHelper.ToRadians(1.754f))), 30, this));
            forwardHullTurrets.Add(SceneObjectFactory.createEsxolusTurret(Position + Vector3.Transform(new Vector3(-3980.4f, -515.7f, 5710.5f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(9.442f), MathHelper.ToRadians(-68.194f), MathHelper.ToRadians(2.376f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(9.442f), MathHelper.ToRadians(-68.194f), MathHelper.ToRadians(2.376f))), 30, this));
            forwardHullTurrets.Add(SceneObjectFactory.createEsxolusTurret(Position + Vector3.Transform(new Vector3(-3204f, -289.8f, 4371f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(174.095f), MathHelper.ToRadians(-58.948f), MathHelper.ToRadians(-163.57f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(174.095f), MathHelper.ToRadians(-58.948f), MathHelper.ToRadians(-163.57f))), 30, this));
            forwardHullTurrets.Add(SceneObjectFactory.createEsxolusTurret(Position + Vector3.Transform(new Vector3(-2419.5f, -45.3f, 2984.1f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(91.708f), MathHelper.ToRadians(-83.308f), MathHelper.ToRadians(-75.531f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(91.708f), MathHelper.ToRadians(-83.308f), MathHelper.ToRadians(-75.531f))), 30, this));
            forwardHullTurrets.Add(SceneObjectFactory.createEsxolusTurret(Position + Vector3.Transform(new Vector3(-123.3f, 180f, 11388.3f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(50.286f), MathHelper.ToRadians(83.61f), MathHelper.ToRadians(87.058f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(50.286f), MathHelper.ToRadians(83.61f), MathHelper.ToRadians(87.058f))), 30, this));
            forwardHullTurrets.Add(SceneObjectFactory.createEsxolusTurret(Position + Vector3.Transform(new Vector3(-90f, 828.3f, 5587.5f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(43.259f), MathHelper.ToRadians(83.981f), MathHelper.ToRadians(83.618f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(43.259f), MathHelper.ToRadians(83.981f), MathHelper.ToRadians(83.618f))), 30, this));
            forwardHullTurrets.Add(SceneObjectFactory.createEsxolusTurret(Position + Vector3.Transform(new Vector3(-817.6f, -531.6f, 11968.8f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(178.859f), MathHelper.ToRadians(-10.082f), MathHelper.ToRadians(3.444f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(178.859f), MathHelper.ToRadians(-10.082f), MathHelper.ToRadians(3.444f))), 30, this));
            forwardHullTurrets.Add(SceneObjectFactory.createEsxolusTurret(Position + Vector3.Transform(new Vector3(-1463.7f, -571.2f, 11872.2f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(178.155f), MathHelper.ToRadians(-12.255f), MathHelper.ToRadians(4.336f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(178.155f), MathHelper.ToRadians(-12.255f), MathHelper.ToRadians(4.336f))), 30, this));
            shipPieces.Add(new SpawnShipPiece(UniqueId, Position, Rotation, ShipTeam,
                ContentLoadManager.loadModel("Models/Ships/CapitalShipSections/e_forward_hull_model"),
                ContentLoadManager.loadModel("Models/Ships/CapitalShipSections/e_forward_hull_d_model"),
                ContentLoadManager.loadModel("Models/Ships/CapitalShipSections/e_forward_hull_c_model"),
                100, forwardHullTurrets, this));

            // Interceptor Hangar / Belly
            List<Turret> interceptorHangarTurrets = new List<Turret>();
            // Forward Hangar
            interceptorHangarTurrets.Add(SceneObjectFactory.createEsxolusTurret(Position + Vector3.Transform(new Vector3(0f, 406.5f, 10632.9f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(90f), MathHelper.ToRadians(0f), MathHelper.ToRadians(0f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(90f), MathHelper.ToRadians(0f), MathHelper.ToRadians(0f))), 30, this));
            interceptorHangarTurrets.Add(SceneObjectFactory.createEsxolusTurret(Position + Vector3.Transform(new Vector3(0f, -1105.5f, 10632.3f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(90f), MathHelper.ToRadians(0f), MathHelper.ToRadians(0f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(90f), MathHelper.ToRadians(0f), MathHelper.ToRadians(0f))), 30, this));
            // Port Draft (Belly)
            interceptorHangarTurrets.Add(SceneObjectFactory.createEsxolusTurret(Position + Vector3.Transform(new Vector3(1503.6f, -737.1f, 10543.2f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(90f), MathHelper.ToRadians(67.715f), MathHelper.ToRadians(0f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(90f), MathHelper.ToRadians(67.715f), MathHelper.ToRadians(0f))), 30, this));
            interceptorHangarTurrets.Add(SceneObjectFactory.createEsxolusTurret(Position + Vector3.Transform(new Vector3(2390.1f, -869.7f, 8346f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(90f), MathHelper.ToRadians(67.715f), MathHelper.ToRadians(0f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(90f), MathHelper.ToRadians(67.715f), MathHelper.ToRadians(0f))), 30, this));
            interceptorHangarTurrets.Add(SceneObjectFactory.createEsxolusTurret(Position + Vector3.Transform(new Vector3(2642.1f, -986.7f, 6083.4f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(90f), MathHelper.ToRadians(87.794f), MathHelper.ToRadians(0f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(90f), MathHelper.ToRadians(87.794f), MathHelper.ToRadians(0f))), 30, this));
            interceptorHangarTurrets.Add(SceneObjectFactory.createEsxolusTurret(Position + Vector3.Transform(new Vector3(2736.9f, -1080.6f, 3857.4f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(90f), MathHelper.ToRadians(87.794f), MathHelper.ToRadians(0f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(90f), MathHelper.ToRadians(87.794f), MathHelper.ToRadians(0f))), 30, this));
            // Starboard Draft (Belly)
            interceptorHangarTurrets.Add(SceneObjectFactory.createEsxolusTurret(Position + Vector3.Transform(new Vector3(-1503.6f, -737.1f, 10543.2f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(90f), MathHelper.ToRadians(-67.715f), MathHelper.ToRadians(0f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(90f), MathHelper.ToRadians(-67.715f), MathHelper.ToRadians(0f))), 30, this));
            interceptorHangarTurrets.Add(SceneObjectFactory.createEsxolusTurret(Position + Vector3.Transform(new Vector3(-2390.1f, -869.7f, 8346f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(90f), MathHelper.ToRadians(-67.715f), MathHelper.ToRadians(0f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(90f), MathHelper.ToRadians(-67.715f), MathHelper.ToRadians(0f))), 30, this));
            interceptorHangarTurrets.Add(SceneObjectFactory.createEsxolusTurret(Position + Vector3.Transform(new Vector3(-2642.1f, -986.7f, 6083.4f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(90f), MathHelper.ToRadians(-87.794f), MathHelper.ToRadians(0f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(90f), MathHelper.ToRadians(-87.794f), MathHelper.ToRadians(0f))), 30, this));
            interceptorHangarTurrets.Add(SceneObjectFactory.createEsxolusTurret(Position + Vector3.Transform(new Vector3(-2736.9f, -1080.6f, 3857.4f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(90f), MathHelper.ToRadians(-87.794f), MathHelper.ToRadians(0f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(90f), MathHelper.ToRadians(-87.794f), MathHelper.ToRadians(0f))), 30, this));
            shipPieces.Add(new SpawnShipPiece(UniqueId, Position, Rotation, ShipTeam,
                ContentLoadManager.loadModel("Models/Ships/CapitalShipSections/e_bow_model"),
                ContentLoadManager.loadModel("Models/Ships/CapitalShipSections/e_bow_d_model"),
                ContentLoadManager.loadModel("Models/Ships/CapitalShipSections/e_bow_c_model"),
                100, interceptorHangarTurrets, this));

            // Port Quarter
            List<Turret> portQuarterTurrets = new List<Turret>();
            portQuarterTurrets.Add(SceneObjectFactory.createEsxolusTurret(Position + Vector3.Transform(new Vector3(4497.6f, 585f, -1591.8f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(1.263f), MathHelper.ToRadians(46.646f), MathHelper.ToRadians(5.362f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(1.263f), MathHelper.ToRadians(46.646f), MathHelper.ToRadians(5.362f))), 30, this));
            portQuarterTurrets.Add(SceneObjectFactory.createEsxolusTurret(Position + Vector3.Transform(new Vector3(5898.6f, 713.7f, -3147.6f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(1.281f), MathHelper.ToRadians(47.41f), MathHelper.ToRadians(5.387f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(1.281f), MathHelper.ToRadians(47.41f), MathHelper.ToRadians(5.387f))), 30, this));
            portQuarterTurrets.Add(SceneObjectFactory.createEsxolusTurret(Position + Vector3.Transform(new Vector3(6151.5f, 660f, -3358.5f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-15.517f), MathHelper.ToRadians(29.81f), MathHelper.ToRadians(-26.742f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-15.517f), MathHelper.ToRadians(29.81f), MathHelper.ToRadians(-26.742f))), 30, this));
            portQuarterTurrets.Add(SceneObjectFactory.createEsxolusTurret(Position + Vector3.Transform(new Vector3(3079.2f, 480f, -7161.6f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-12.703f), MathHelper.ToRadians(16.23f), MathHelper.ToRadians(-16.05f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-12.703f), MathHelper.ToRadians(16.23f), MathHelper.ToRadians(-16.05f))), 30, this));
            portQuarterTurrets.Add(SceneObjectFactory.createEsxolusTurret(Position + Vector3.Transform(new Vector3(8594.1f, -526.5f, -4801.2f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-15.517f), MathHelper.ToRadians(29.81f), MathHelper.ToRadians(-26.742f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-15.517f), MathHelper.ToRadians(29.81f), MathHelper.ToRadians(-26.742f))), 30, this));
            portQuarterTurrets.Add(SceneObjectFactory.createEsxolusTurret(Position + Vector3.Transform(new Vector3(4908.9f, -18.9f, -7711.5f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-12.79f), MathHelper.ToRadians(17.494f), MathHelper.ToRadians(-16.349f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-12.79f), MathHelper.ToRadians(17.494f), MathHelper.ToRadians(-16.349f))), 30, this));
            portQuarterTurrets.Add(SceneObjectFactory.createEsxolusTurret(Position + Vector3.Transform(new Vector3(8664.6f, -653.7f, -4841.4f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(90f), MathHelper.ToRadians(140.988f), MathHelper.ToRadians(0f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(90f), MathHelper.ToRadians(140.988f), MathHelper.ToRadians(0f))), 30, this));
            portQuarterTurrets.Add(SceneObjectFactory.createEsxolusTurret(Position + Vector3.Transform(new Vector3(8664.6f, -1088.7f, -4841.4f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(90f), MathHelper.ToRadians(140.988f), MathHelper.ToRadians(0f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(90f), MathHelper.ToRadians(140.988f), MathHelper.ToRadians(0f))), 30, this));
            portQuarterTurrets.Add(SceneObjectFactory.createEsxolusTurret(Position + Vector3.Transform(new Vector3(5008.2f, -151.2f, -7790.1f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(90f), MathHelper.ToRadians(140.988f), MathHelper.ToRadians(0f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(90f), MathHelper.ToRadians(140.988f), MathHelper.ToRadians(0f))), 30, this));
            portQuarterTurrets.Add(SceneObjectFactory.createEsxolusTurret(Position + Vector3.Transform(new Vector3(5008.2f, -1578.6f, -7790.1f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(90f), MathHelper.ToRadians(140.988f), MathHelper.ToRadians(0f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(90f), MathHelper.ToRadians(140.988f), MathHelper.ToRadians(0f))), 30, this));
            portQuarterTurrets.Add(SceneObjectFactory.createEsxolusTurret(Position + Vector3.Transform(new Vector3(8596.5f, -1213.8f, -4802.7f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-162.557f), MathHelper.ToRadians(27.369f), MathHelper.ToRadians(28.643f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-162.557f), MathHelper.ToRadians(27.369f), MathHelper.ToRadians(28.643f))), 30, this));
            portQuarterTurrets.Add(SceneObjectFactory.createEsxolusTurret(Position + Vector3.Transform(new Vector3(4908.9f, -1711.8f, -7711.5f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-167.704f), MathHelper.ToRadians(13.778f), MathHelper.ToRadians(5.479f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-167.704f), MathHelper.ToRadians(13.778f), MathHelper.ToRadians(5.479f))), 30, this));
            portQuarterTurrets.Add(SceneObjectFactory.createEsxolusTurret(Position + Vector3.Transform(new Vector3(6152.1f, -2561.4f, -3359.1f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-162.172f), MathHelper.ToRadians(31.081f), MathHelper.ToRadians(29.938f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-162.172f), MathHelper.ToRadians(31.081f), MathHelper.ToRadians(29.938f))), 30, this));
            portQuarterTurrets.Add(SceneObjectFactory.createEsxolusTurret(Position + Vector3.Transform(new Vector3(3079.5f, -1912.2f, -7162.8f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-167.36f), MathHelper.ToRadians(15.166f), MathHelper.ToRadians(6.093f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-167.36f), MathHelper.ToRadians(15.166f), MathHelper.ToRadians(6.093f))), 30, this));
            portQuarterTurrets.Add(SceneObjectFactory.createEsxolusTurret(Position + Vector3.Transform(new Vector3(4497.6f, -2439.6f, -1591.8f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-179.783f), MathHelper.ToRadians(46.627f), MathHelper.ToRadians(-6.896f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-179.783f), MathHelper.ToRadians(46.627f), MathHelper.ToRadians(-6.896f))), 30, this));
            portQuarterTurrets.Add(SceneObjectFactory.createEsxolusTurret(Position + Vector3.Transform(new Vector3(5899.2f, -2605.5f, -3147.6f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(185.791f), MathHelper.ToRadians(47.948f), MathHelper.ToRadians(-2.266f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(185.791f), MathHelper.ToRadians(47.948f), MathHelper.ToRadians(-2.266f))), 30, this));
            portQuarterTurrets.Add(SceneObjectFactory.createEsxolusTurret(Position + Vector3.Transform(new Vector3(2678.4f, -1914f, -7042.8f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-176.884f), MathHelper.ToRadians(1.106f), MathHelper.ToRadians(-2.349f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-176.884f), MathHelper.ToRadians(1.106f), MathHelper.ToRadians(-2.349f))), 30, this));
            shipPieces.Add(new SpawnShipPiece(UniqueId, Position, Rotation, ShipTeam,
               ContentLoadManager.loadModel("Models/Ships/CapitalShipSections/e_port_quarter_model"),
               ContentLoadManager.loadModel("Models/Ships/CapitalShipSections/e_port_quarter_d_model"),
               ContentLoadManager.loadModel("Models/Ships/CapitalShipSections/e_port_quarter_c_model"),
               100, portQuarterTurrets, this));

            // Starboard Quarter
            List<Turret> starboardQuarterTurrets = new List<Turret>();
            starboardQuarterTurrets.Add(SceneObjectFactory.createEsxolusTurret(Position + Vector3.Transform(new Vector3(-4497.6f, 585f, -1591.8f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(1.263f), MathHelper.ToRadians(-46.646f), MathHelper.ToRadians(-5.362f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(1.263f), MathHelper.ToRadians(-46.646f), MathHelper.ToRadians(-5.362f))), 30, this));
            starboardQuarterTurrets.Add(SceneObjectFactory.createEsxolusTurret(Position + Vector3.Transform(new Vector3(-5898.6f, 713.7f, -3147.6f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(1.281f), MathHelper.ToRadians(-47.41f), MathHelper.ToRadians(-5.387f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(1.281f), MathHelper.ToRadians(-47.41f), MathHelper.ToRadians(-5.387f))), 30, this));
            starboardQuarterTurrets.Add(SceneObjectFactory.createEsxolusTurret(Position + Vector3.Transform(new Vector3(-6151.5f, 660f, -3358.5f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-15.517f), MathHelper.ToRadians(-29.81f), MathHelper.ToRadians(26.742f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-15.517f), MathHelper.ToRadians(-29.81f), MathHelper.ToRadians(26.742f))), 30, this));
            starboardQuarterTurrets.Add(SceneObjectFactory.createEsxolusTurret(Position + Vector3.Transform(new Vector3(-3079.2f, 480f, -7161.6f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-12.703f), MathHelper.ToRadians(-16.23f), MathHelper.ToRadians(16.05f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-12.703f), MathHelper.ToRadians(-16.23f), MathHelper.ToRadians(16.05f))), 30, this));
            starboardQuarterTurrets.Add(SceneObjectFactory.createEsxolusTurret(Position + Vector3.Transform(new Vector3(-8594.1f, -526.5f, -4801.2f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-15.517f), MathHelper.ToRadians(-29.81f), MathHelper.ToRadians(26.742f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-15.517f), MathHelper.ToRadians(-29.81f), MathHelper.ToRadians(26.742f))), 30, this));
            starboardQuarterTurrets.Add(SceneObjectFactory.createEsxolusTurret(Position + Vector3.Transform(new Vector3(-4908.9f, -18.9f, -7711.5f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-12.79f), MathHelper.ToRadians(-17.494f), MathHelper.ToRadians(16.349f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-12.79f), MathHelper.ToRadians(-17.494f), MathHelper.ToRadians(16.349f))), 30, this));
            starboardQuarterTurrets.Add(SceneObjectFactory.createEsxolusTurret(Position + Vector3.Transform(new Vector3(-8664.6f, -653.7f, -4841.4f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(90f), MathHelper.ToRadians(-140.988f), MathHelper.ToRadians(0f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(90f), MathHelper.ToRadians(-140.988f), MathHelper.ToRadians(0f))), 30, this));
            starboardQuarterTurrets.Add(SceneObjectFactory.createEsxolusTurret(Position + Vector3.Transform(new Vector3(-8664.6f, -1088.7f, -4841.4f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(90f), MathHelper.ToRadians(-140.988f), MathHelper.ToRadians(0f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(90f), MathHelper.ToRadians(-140.988f), MathHelper.ToRadians(0f))), 30, this));
            starboardQuarterTurrets.Add(SceneObjectFactory.createEsxolusTurret(Position + Vector3.Transform(new Vector3(-5008.2f, -151.2f, -7790.1f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(90f), MathHelper.ToRadians(-140.988f), MathHelper.ToRadians(0f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(90f), MathHelper.ToRadians(-140.988f), MathHelper.ToRadians(0f))), 30, this));
            starboardQuarterTurrets.Add(SceneObjectFactory.createEsxolusTurret(Position + Vector3.Transform(new Vector3(-5008.2f, -1578.6f, -7790.1f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(90f), MathHelper.ToRadians(-140.988f), MathHelper.ToRadians(0f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(90f), MathHelper.ToRadians(-140.988f), MathHelper.ToRadians(0f))), 30, this));
            starboardQuarterTurrets.Add(SceneObjectFactory.createEsxolusTurret(Position + Vector3.Transform(new Vector3(-8596.5f, -1213.8f, -4802.7f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-162.557f), MathHelper.ToRadians(-27.369f), MathHelper.ToRadians(-28.643f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-162.557f), MathHelper.ToRadians(-27.369f), MathHelper.ToRadians(-28.643f))), 30, this));
            starboardQuarterTurrets.Add(SceneObjectFactory.createEsxolusTurret(Position + Vector3.Transform(new Vector3(-4908.9f, -1711.8f, -7711.5f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-167.704f), MathHelper.ToRadians(-13.778f), MathHelper.ToRadians(-5.479f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-167.704f), MathHelper.ToRadians(-13.778f), MathHelper.ToRadians(-5.479f))), 30, this));
            starboardQuarterTurrets.Add(SceneObjectFactory.createEsxolusTurret(Position + Vector3.Transform(new Vector3(-6152.1f, -2561.4f, -3359.1f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-162.172f), MathHelper.ToRadians(-31.081f), MathHelper.ToRadians(-29.938f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-162.172f), MathHelper.ToRadians(-31.081f), MathHelper.ToRadians(-29.938f))), 30, this));
            starboardQuarterTurrets.Add(SceneObjectFactory.createEsxolusTurret(Position + Vector3.Transform(new Vector3(-3079.5f, -1912.2f, -7162.8f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-167.36f), MathHelper.ToRadians(-15.166f), MathHelper.ToRadians(-6.093f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-167.36f), MathHelper.ToRadians(-15.166f), MathHelper.ToRadians(-6.093f))), 30, this));
            starboardQuarterTurrets.Add(SceneObjectFactory.createEsxolusTurret(Position + Vector3.Transform(new Vector3(-4497.6f, -2439.6f, -1591.8f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-179.783f), MathHelper.ToRadians(-46.627f), MathHelper.ToRadians(6.896f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-179.783f), MathHelper.ToRadians(-46.627f), MathHelper.ToRadians(6.896f))), 30, this));
            starboardQuarterTurrets.Add(SceneObjectFactory.createEsxolusTurret(Position + Vector3.Transform(new Vector3(-5899.2f, -2605.5f, -3147.6f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(185.791f), MathHelper.ToRadians(-47.948f), MathHelper.ToRadians(2.266f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(185.791f), MathHelper.ToRadians(-47.948f), MathHelper.ToRadians(2.266f))), 30, this));
            starboardQuarterTurrets.Add(SceneObjectFactory.createEsxolusTurret(Position + Vector3.Transform(new Vector3(-2678.4f, -1914f, -7042.8f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-176.884f), MathHelper.ToRadians(-1.106f), MathHelper.ToRadians(2.349f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-176.884f), MathHelper.ToRadians(-1.106f), MathHelper.ToRadians(2.349f))), 30, this));
            shipPieces.Add(new SpawnShipPiece(UniqueId, Position, Rotation, ShipTeam,
                ContentLoadManager.loadModel("Models/Ships/CapitalShipSections/e_starboard_quarter_model"),
                ContentLoadManager.loadModel("Models/Ships/CapitalShipSections/e_starboard_quarter_d_model"),
                ContentLoadManager.loadModel("Models/Ships/CapitalShipSections/e_starboard_quarter_c_model"),
                100, starboardQuarterTurrets, this));

            //Port Overwing
            List<Turret> portOverwingTurrets = new List<Turret>();
            portOverwingTurrets.Add(SceneObjectFactory.createEsxolusTurret(Position + Vector3.Transform(new Vector3(3280.8f, -607.8f, 2982.3f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(4.123f), MathHelper.ToRadians(36.659f), MathHelper.ToRadians(-6.617f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(4.123f), MathHelper.ToRadians(36.659f), MathHelper.ToRadians(-6.617f))), 30, this));
            portOverwingTurrets.Add(SceneObjectFactory.createEsxolusTurret(Position + Vector3.Transform(new Vector3(4729.5f, -772.5f, 1941.3f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(4.12f), MathHelper.ToRadians(36.598f), MathHelper.ToRadians(-6.623f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(4.12f), MathHelper.ToRadians(36.598f), MathHelper.ToRadians(-6.623f))), 30, this));
            portOverwingTurrets.Add(SceneObjectFactory.createEsxolusTurret(Position + Vector3.Transform(new Vector3(6282f, -950.7f, 856.5f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(4.141f), MathHelper.ToRadians(36.997f), MathHelper.ToRadians(-6.587f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(4.141f), MathHelper.ToRadians(36.997f), MathHelper.ToRadians(-6.587f))), 30, this));
            portOverwingTurrets.Add(SceneObjectFactory.createEsxolusTurret(Position + Vector3.Transform(new Vector3(7803f, -1124.1f, -224.1f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(20.982f), MathHelper.ToRadians(80.386f), MathHelper.ToRadians(11.432f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(20.982f), MathHelper.ToRadians(80.386f), MathHelper.ToRadians(11.432f))), 30, this));
            portOverwingTurrets.Add(SceneObjectFactory.createEsxolusTurret(Position + Vector3.Transform(new Vector3(7871.4f, -1115.7f, -591.3f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(16.251f), MathHelper.ToRadians(80.485f), MathHelper.ToRadians(6.866f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(16.251f), MathHelper.ToRadians(80.485f), MathHelper.ToRadians(6.866f))), 30, this));
            portOverwingTurrets.Add(SceneObjectFactory.createEsxolusTurret(Position + Vector3.Transform(new Vector3(7017.9f, -930f, -1676.1f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(176.07f), MathHelper.ToRadians(51.139f), MathHelper.ToRadians(167.552f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(176.07f), MathHelper.ToRadians(51.139f), MathHelper.ToRadians(167.552f))), 30, this));
            portOverwingTurrets.Add(SceneObjectFactory.createEsxolusTurret(Position + Vector3.Transform(new Vector3(6179.4f, -747f, -2741.7f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(176.07f), MathHelper.ToRadians(51.139f), MathHelper.ToRadians(167.552f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(176.07f), MathHelper.ToRadians(51.139f), MathHelper.ToRadians(167.552f))), 30, this));
            portOverwingTurrets.Add(SceneObjectFactory.createEsxolusTurret(Position + Vector3.Transform(new Vector3(6223.5f, -1317.3f, 834.9f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-175.88f), MathHelper.ToRadians(36.997f), MathHelper.ToRadians(-6.587f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-175.88f), MathHelper.ToRadians(36.997f), MathHelper.ToRadians(-6.587f))), 30, this));
            portOverwingTurrets.Add(SceneObjectFactory.createEsxolusTurret(Position + Vector3.Transform(new Vector3(7741.5f, -1492.4f, -246.6f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-159.136f), MathHelper.ToRadians(80.397f), MathHelper.ToRadians(11.405f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-159.136f), MathHelper.ToRadians(80.397f), MathHelper.ToRadians(11.405f))), 30, this));
            portOverwingTurrets.Add(SceneObjectFactory.createEsxolusTurret(Position + Vector3.Transform(new Vector3(7810.5f, -1485.9f, -608.7f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-168.439f), MathHelper.ToRadians(80.63f), MathHelper.ToRadians(3.309f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-168.439f), MathHelper.ToRadians(80.63f), MathHelper.ToRadians(3.309f))), 30, this));
            portOverwingTurrets.Add(SceneObjectFactory.createEsxolusTurret(Position + Vector3.Transform(new Vector3(6963.3f, -1333.2f, -1703.1f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-2.308f), MathHelper.ToRadians(51.277f), MathHelper.ToRadians(169.65f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-2.308f), MathHelper.ToRadians(51.277f), MathHelper.ToRadians(169.65f))), 30, this));
            shipPieces.Add(new SpawnShipPiece(UniqueId, Position, Rotation, ShipTeam,
                ContentLoadManager.loadModel("Models/Ships/CapitalShipSections/e_port_overwing_model"),
                ContentLoadManager.loadModel("Models/Ships/CapitalShipSections/e_port_overwing_d_model"),
                ContentLoadManager.loadModel("Models/Ships/CapitalShipSections/e_port_overwing_c_model"),
                100, portOverwingTurrets, this));

            // Starboard Overwing
            List<Turret> starboardOverwingTurrets = new List<Turret>();
            starboardOverwingTurrets.Add(SceneObjectFactory.createEsxolusTurret(Position + Vector3.Transform(new Vector3(-3280.8f, -607.8f, 2982.3f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(4.123f), MathHelper.ToRadians(-36.659f), MathHelper.ToRadians(6.617f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(4.123f), MathHelper.ToRadians(-36.659f), MathHelper.ToRadians(6.617f))), 30, this));
            starboardOverwingTurrets.Add(SceneObjectFactory.createEsxolusTurret(Position + Vector3.Transform(new Vector3(-4729.5f, -772.5f, 1941.3f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(4.12f), MathHelper.ToRadians(-36.598f), MathHelper.ToRadians(6.623f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(4.12f), MathHelper.ToRadians(-36.598f), MathHelper.ToRadians(6.623f))), 30, this));
            starboardOverwingTurrets.Add(SceneObjectFactory.createEsxolusTurret(Position + Vector3.Transform(new Vector3(-6282f, -950.7f, 856.5f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(4.141f), MathHelper.ToRadians(-36.997f), MathHelper.ToRadians(6.587f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(4.141f), MathHelper.ToRadians(-36.997f), MathHelper.ToRadians(6.587f))), 30, this));
            starboardOverwingTurrets.Add(SceneObjectFactory.createEsxolusTurret(Position + Vector3.Transform(new Vector3(-7803f, -1124.1f, -224.1f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(20.982f), MathHelper.ToRadians(-80.386f), MathHelper.ToRadians(-11.432f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(20.982f), MathHelper.ToRadians(-80.386f), MathHelper.ToRadians(-11.432f))), 30, this));
            starboardOverwingTurrets.Add(SceneObjectFactory.createEsxolusTurret(Position + Vector3.Transform(new Vector3(-7871.4f, -1115.7f, -591.3f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(16.251f), MathHelper.ToRadians(-80.485f), MathHelper.ToRadians(-6.866f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(16.251f), MathHelper.ToRadians(-80.485f), MathHelper.ToRadians(-6.866f))), 30, this));
            starboardOverwingTurrets.Add(SceneObjectFactory.createEsxolusTurret(Position + Vector3.Transform(new Vector3(-7017.9f, -930f, -1676.1f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(176.07f), MathHelper.ToRadians(-51.139f), MathHelper.ToRadians(-167.552f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(176.07f), MathHelper.ToRadians(-51.139f), MathHelper.ToRadians(-167.552f))), 30, this));
            starboardOverwingTurrets.Add(SceneObjectFactory.createEsxolusTurret(Position + Vector3.Transform(new Vector3(-6179.4f, -747f, -2741.7f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(176.07f), MathHelper.ToRadians(-51.139f), MathHelper.ToRadians(-167.552f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(176.07f), MathHelper.ToRadians(-51.139f), MathHelper.ToRadians(-167.552f))), 30, this));
            starboardOverwingTurrets.Add(SceneObjectFactory.createEsxolusTurret(Position + Vector3.Transform(new Vector3(-6223.5f, -1317.3f, 834.9f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-175.88f), MathHelper.ToRadians(-36.997f), MathHelper.ToRadians(6.587f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-175.88f), MathHelper.ToRadians(-36.997f), MathHelper.ToRadians(6.587f))), 30, this));
            starboardOverwingTurrets.Add(SceneObjectFactory.createEsxolusTurret(Position + Vector3.Transform(new Vector3(-7741.5f, -1492.4f, -246.6f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-159.136f), MathHelper.ToRadians(-80.397f), MathHelper.ToRadians(-11.405f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-159.136f), MathHelper.ToRadians(-80.397f), MathHelper.ToRadians(-11.405f))), 30, this));
            starboardOverwingTurrets.Add(SceneObjectFactory.createEsxolusTurret(Position + Vector3.Transform(new Vector3(-7810.5f, -1485.9f, -608.7f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-168.439f), MathHelper.ToRadians(-80.63f), MathHelper.ToRadians(-3.309f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-168.439f), MathHelper.ToRadians(-80.63f), MathHelper.ToRadians(-3.309f))), 30, this));
            starboardOverwingTurrets.Add(SceneObjectFactory.createEsxolusTurret(Position + Vector3.Transform(new Vector3(-6963.3f, -1333.2f, -1703.1f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-2.308f), MathHelper.ToRadians(-51.277f), MathHelper.ToRadians(-169.65f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-2.308f), MathHelper.ToRadians(-51.277f), MathHelper.ToRadians(-169.65f))), 30, this));
            shipPieces.Add(new SpawnShipPiece(UniqueId, Position, Rotation, ShipTeam,
                ContentLoadManager.loadModel("Models/Ships/CapitalShipSections/e_starboard_overwing_model"),
                ContentLoadManager.loadModel("Models/Ships/CapitalShipSections/e_starboard_overwing_d_model"),
                ContentLoadManager.loadModel("Models/Ships/CapitalShipSections/e_starboard_overwing_c_model"),
                100, starboardOverwingTurrets, this));

            // Port Underwing
            List<Turret> portUnderwingTurrets = new List<Turret>();
            portUnderwingTurrets.Add(SceneObjectFactory.createEsxolusTurret(Position + Vector3.Transform(new Vector3(3462.3f, -1228.5f, 10.277f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(8.959f), MathHelper.ToRadians(14.678f), MathHelper.ToRadians(-19.962f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(8.959f), MathHelper.ToRadians(14.678f), MathHelper.ToRadians(-19.962f))), 30, this));
            portUnderwingTurrets.Add(SceneObjectFactory.createEsxolusTurret(Position + Vector3.Transform(new Vector3(5040.9f, -1801.5f, 2637f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(8.945f), MathHelper.ToRadians(14.678f), MathHelper.ToRadians(-19.962f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(8.945f), MathHelper.ToRadians(14.678f), MathHelper.ToRadians(-19.962f))), 30, this));
            portUnderwingTurrets.Add(SceneObjectFactory.createEsxolusTurret(Position + Vector3.Transform(new Vector3(6615.3f, -2371.8f, 2187.9f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(13.878f), MathHelper.ToRadians(63.639f), MathHelper.ToRadians(-9.28f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(13.878f), MathHelper.ToRadians(63.639f), MathHelper.ToRadians(-9.28f))), 30, this));
            portUnderwingTurrets.Add(SceneObjectFactory.createEsxolusTurret(Position + Vector3.Transform(new Vector3(6795f, -2399.1f, 1842.6f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(12.216f), MathHelper.ToRadians(63.146f), MathHelper.ToRadians(-9.549f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(12.216f), MathHelper.ToRadians(63.146f), MathHelper.ToRadians(-9.549f))), 30, this));
            portUnderwingTurrets.Add(SceneObjectFactory.createEsxolusTurret(Position + Vector3.Transform(new Vector3(6389.7f, -2091f, 350.1f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(160.64f), MathHelper.ToRadians(72.506f), MathHelper.ToRadians(141.147f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(160.64f), MathHelper.ToRadians(72.506f), MathHelper.ToRadians(141.147f))), 30, this));
            portUnderwingTurrets.Add(SceneObjectFactory.createEsxolusTurret(Position + Vector3.Transform(new Vector3(5870.4f, -2082.9f, -1010.7f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-16.234f), MathHelper.ToRadians(73.605f), MathHelper.ToRadians(144.224f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-16.234f), MathHelper.ToRadians(73.605f), MathHelper.ToRadians(144.224f))), 30, this));
            portUnderwingTurrets.Add(SceneObjectFactory.createEsxolusTurret(Position + Vector3.Transform(new Vector3(6141.3f, -2261.7f, -57.3f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-15.077f), MathHelper.ToRadians(73.123f), MathHelper.ToRadians(144.858f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-15.077f), MathHelper.ToRadians(73.123f), MathHelper.ToRadians(144.858f))), 30, this));
            portUnderwingTurrets.Add(SceneObjectFactory.createEsxolusTurret(Position + Vector3.Transform(new Vector3(6417.3f, -2444.4f, 903.3f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-13.738f), MathHelper.ToRadians(70.132f), MathHelper.ToRadians(146.713f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-13.738f), MathHelper.ToRadians(70.132f), MathHelper.ToRadians(146.713f))), 30, this));
            portUnderwingTurrets.Add(SceneObjectFactory.createEsxolusTurret(Position + Vector3.Transform(new Vector3(6677.7f, -2615.1f, 1803f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-169.907f), MathHelper.ToRadians(64.454f), MathHelper.ToRadians(-11.1f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-169.907f), MathHelper.ToRadians(64.454f), MathHelper.ToRadians(-11.1f))), 30, this));
            portUnderwingTurrets.Add(SceneObjectFactory.createEsxolusTurret(Position + Vector3.Transform(new Vector3(6497.7f, -2587.2f, 2145.9f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-166.783f), MathHelper.ToRadians(63.1f), MathHelper.ToRadians(-8.888f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-166.783f), MathHelper.ToRadians(63.1f), MathHelper.ToRadians(-8.888f))), 30, this));
            portUnderwingTurrets.Add(SceneObjectFactory.createEsxolusTurret(Position + Vector3.Transform(new Vector3(5672.4f, -2295.3f, 2366.1f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-171.826f), MathHelper.ToRadians(14.715f), MathHelper.ToRadians(-19.368f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-171.826f), MathHelper.ToRadians(14.715f), MathHelper.ToRadians(-19.368f))), 30, this));
            portUnderwingTurrets.Add(SceneObjectFactory.createEsxolusTurret(Position + Vector3.Transform(new Vector3(4719f, -1959f, 2631.9f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-171.743f), MathHelper.ToRadians(14.715f), MathHelper.ToRadians(-19.368f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-171.743f), MathHelper.ToRadians(14.715f), MathHelper.ToRadians(-19.368f))), 30, this));
            portUnderwingTurrets.Add(SceneObjectFactory.createEsxolusTurret(Position + Vector3.Transform(new Vector3(3771.3f, -1624.5f, 2895.9f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-171.743f), MathHelper.ToRadians(14.715f), MathHelper.ToRadians(-19.368f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-171.743f), MathHelper.ToRadians(14.715f), MathHelper.ToRadians(-19.368f))), 30, this));
            shipPieces.Add(new SpawnShipPiece(UniqueId, Position, Rotation, ShipTeam,
                ContentLoadManager.loadModel("Models/Ships/CapitalShipSections/e_port_underwing_model"),
                ContentLoadManager.loadModel("Models/Ships/CapitalShipSections/e_port_underwing_d_model"),
                ContentLoadManager.loadModel("Models/Ships/CapitalShipSections/e_port_underwing_c_model"),
                100, portUnderwingTurrets, this));

            // Starboard Underwing
            List<Turret> starboardUnderwingTurrets = new List<Turret>();
            starboardUnderwingTurrets.Add(SceneObjectFactory.createEsxolusTurret(Position + Vector3.Transform(new Vector3(-3462.3f, -1228.5f, 10.277f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(8.959f), MathHelper.ToRadians(-14.678f), MathHelper.ToRadians(19.962f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(8.959f), MathHelper.ToRadians(-14.678f), MathHelper.ToRadians(19.962f))), 30, this));
            starboardUnderwingTurrets.Add(SceneObjectFactory.createEsxolusTurret(Position + Vector3.Transform(new Vector3(-5040.9f, -1801.5f, 2637f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(8.945f), MathHelper.ToRadians(-14.678f), MathHelper.ToRadians(19.962f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(8.945f), MathHelper.ToRadians(-14.678f), MathHelper.ToRadians(19.962f))), 30, this));
            starboardUnderwingTurrets.Add(SceneObjectFactory.createEsxolusTurret(Position + Vector3.Transform(new Vector3(-6615.3f, -2371.8f, 2187.9f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(13.878f), MathHelper.ToRadians(-63.639f), MathHelper.ToRadians(9.28f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(13.878f), MathHelper.ToRadians(-63.639f), MathHelper.ToRadians(9.28f))), 30, this));
            starboardUnderwingTurrets.Add(SceneObjectFactory.createEsxolusTurret(Position + Vector3.Transform(new Vector3(-6795f, -2399.1f, 1842.6f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(12.216f), MathHelper.ToRadians(-63.146f), MathHelper.ToRadians(9.549f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(12.216f), MathHelper.ToRadians(-63.146f), MathHelper.ToRadians(9.549f))), 30, this));
            starboardUnderwingTurrets.Add(SceneObjectFactory.createEsxolusTurret(Position + Vector3.Transform(new Vector3(-6389.7f, -2091f, 350.1f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(160.64f), MathHelper.ToRadians(-72.506f), MathHelper.ToRadians(-141.147f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(160.64f), MathHelper.ToRadians(-72.506f), MathHelper.ToRadians(-141.147f))), 30, this));
            starboardUnderwingTurrets.Add(SceneObjectFactory.createEsxolusTurret(Position + Vector3.Transform(new Vector3(-5870.4f, -2082.9f, -1010.7f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-16.234f), MathHelper.ToRadians(-73.605f), MathHelper.ToRadians(-144.224f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-16.234f), MathHelper.ToRadians(-73.605f), MathHelper.ToRadians(-144.224f))), 30, this));
            starboardUnderwingTurrets.Add(SceneObjectFactory.createEsxolusTurret(Position + Vector3.Transform(new Vector3(-6141.3f, -2261.7f, -57.3f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-15.077f), MathHelper.ToRadians(-73.123f), MathHelper.ToRadians(-144.858f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-15.077f), MathHelper.ToRadians(-73.123f), MathHelper.ToRadians(-144.858f))), 30, this));
            starboardUnderwingTurrets.Add(SceneObjectFactory.createEsxolusTurret(Position + Vector3.Transform(new Vector3(-6417.3f, -2444.4f, 903.3f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-13.738f), MathHelper.ToRadians(-70.132f), MathHelper.ToRadians(-146.713f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-13.738f), MathHelper.ToRadians(-70.132f), MathHelper.ToRadians(-146.713f))), 30, this));
            starboardUnderwingTurrets.Add(SceneObjectFactory.createEsxolusTurret(Position + Vector3.Transform(new Vector3(-6677.7f, -2615.1f, 1803f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-169.907f), MathHelper.ToRadians(-64.454f), MathHelper.ToRadians(11.1f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-169.907f), MathHelper.ToRadians(-64.454f), MathHelper.ToRadians(11.1f))), 30, this));
            starboardUnderwingTurrets.Add(SceneObjectFactory.createEsxolusTurret(Position + Vector3.Transform(new Vector3(-6497.7f, -2587.2f, 2145.9f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-166.783f), MathHelper.ToRadians(-63.1f), MathHelper.ToRadians(8.888f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-166.783f), MathHelper.ToRadians(-63.1f), MathHelper.ToRadians(8.888f))), 30, this));
            starboardUnderwingTurrets.Add(SceneObjectFactory.createEsxolusTurret(Position + Vector3.Transform(new Vector3(-5672.4f, -2295.3f, 2366.1f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-171.826f), MathHelper.ToRadians(-14.715f), MathHelper.ToRadians(19.368f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-171.826f), MathHelper.ToRadians(-14.715f), MathHelper.ToRadians(19.368f))), 30, this));
            starboardUnderwingTurrets.Add(SceneObjectFactory.createEsxolusTurret(Position + Vector3.Transform(new Vector3(-4719f, -1959f, 2631.9f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-171.743f), MathHelper.ToRadians(-14.715f), MathHelper.ToRadians(19.368f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-171.743f), MathHelper.ToRadians(-14.715f), MathHelper.ToRadians(19.368f))), 30, this));
            starboardUnderwingTurrets.Add(SceneObjectFactory.createEsxolusTurret(Position + Vector3.Transform(new Vector3(-3771.3f, -1624.5f, 2895.9f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-171.743f), MathHelper.ToRadians(-14.715f), MathHelper.ToRadians(19.368f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-171.743f), MathHelper.ToRadians(-14.715f), MathHelper.ToRadians(19.368f))), 30, this));
            shipPieces.Add(new SpawnShipPiece(UniqueId, Position, Rotation, ShipTeam,
                ContentLoadManager.loadModel("Models/Ships/CapitalShipSections/e_starboard_underwing_model"),
                ContentLoadManager.loadModel("Models/Ships/CapitalShipSections/e_starboard_underwing_d_model"),
                ContentLoadManager.loadModel("Models/Ships/CapitalShipSections/e_starboard_underwing_c_model"),
                100, starboardUnderwingTurrets, this));

            // Stern
            List<Turret> sternTurrets = new List<Turret>();
            // Port Stern
            sternTurrets.Add(SceneObjectFactory.createEsxolusTurret(Position + Vector3.Transform(new Vector3(2546.1f, 546.9f, -7326.9f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(166.727f), MathHelper.ToRadians(72.767f), MathHelper.ToRadians(167.051f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(166.727f), MathHelper.ToRadians(72.767f), MathHelper.ToRadians(167.051f))), 30, this));
            sternTurrets.Add(SceneObjectFactory.createEsxolusTurret(Position + Vector3.Transform(new Vector3(2114.4f, 561f, -8949.3f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-179.081f), MathHelper.ToRadians(73.308f), MathHelper.ToRadians(179.06f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-179.081f), MathHelper.ToRadians(73.308f), MathHelper.ToRadians(179.06f))), 30, this));
            sternTurrets.Add(SceneObjectFactory.createEsxolusTurret(Position + Vector3.Transform(new Vector3(1764.3f, 571.2f, -10291.2f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-179.638f), MathHelper.ToRadians(72.027f), MathHelper.ToRadians(178.478f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-179.638f), MathHelper.ToRadians(72.027f), MathHelper.ToRadians(178.478f))), 30, this));
            sternTurrets.Add(SceneObjectFactory.createEsxolusTurret(Position + Vector3.Transform(new Vector3(1518.9f, 578.4f, -11318.4f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(179.985f), MathHelper.ToRadians(73.417f), MathHelper.ToRadians(177.891f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(179.985f), MathHelper.ToRadians(73.417f), MathHelper.ToRadians(177.891f))), 30, this));
            // Starboard Stern
            sternTurrets.Add(SceneObjectFactory.createEsxolusTurret(Position + Vector3.Transform(new Vector3(-2546.1f, 546.9f, -7326.9f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(166.727f), MathHelper.ToRadians(-72.767f), MathHelper.ToRadians(-167.051f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(166.727f), MathHelper.ToRadians(-72.767f), MathHelper.ToRadians(-167.051f))), 30, this));
            sternTurrets.Add(SceneObjectFactory.createEsxolusTurret(Position + Vector3.Transform(new Vector3(-2114.4f, 561f, -8949.3f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-179.081f), MathHelper.ToRadians(-73.308f), MathHelper.ToRadians(-179.06f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-179.081f), MathHelper.ToRadians(-73.308f), MathHelper.ToRadians(-179.06f))), 30, this));
            sternTurrets.Add(SceneObjectFactory.createEsxolusTurret(Position + Vector3.Transform(new Vector3(-1764.3f, 571.2f, -10291.2f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-179.638f), MathHelper.ToRadians(-72.027f), MathHelper.ToRadians(-178.478f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-179.638f), MathHelper.ToRadians(-72.027f), MathHelper.ToRadians(-178.478f))), 30, this));
            sternTurrets.Add(SceneObjectFactory.createEsxolusTurret(Position + Vector3.Transform(new Vector3(-1518.9f, 578.4f, -11318.4f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(179.985f), MathHelper.ToRadians(-73.417f), MathHelper.ToRadians(-177.891f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(179.985f), MathHelper.ToRadians(-73.417f), MathHelper.ToRadians(-177.891f))), 30, this));
            // Rudder
            sternTurrets.Add(SceneObjectFactory.createEsxolusTurret(Position + Vector3.Transform(new Vector3(284.4f, -2198.7f, -9577.5f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-166.521f), MathHelper.ToRadians(13.292f), MathHelper.ToRadians(79.351f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-166.521f), MathHelper.ToRadians(13.292f), MathHelper.ToRadians(79.351f))), 30, this));
            sternTurrets.Add(SceneObjectFactory.createEsxolusTurret(Position + Vector3.Transform(new Vector3(-284.4f, -2198.7f, -9577.5f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-166.521f), MathHelper.ToRadians(-13.292f), MathHelper.ToRadians(-79.351f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-166.521f), MathHelper.ToRadians(-13.292f), MathHelper.ToRadians(-79.351f))), 30, this));
            shipPieces.Add(new SpawnShipPiece(UniqueId, Position, Rotation, ShipTeam,
                ContentLoadManager.loadModel("Models/Ships/CapitalShipSections/e_stern_model"),
                ContentLoadManager.loadModel("Models/Ships/CapitalShipSections/e_stern_d_model"),
                ContentLoadManager.loadModel("Models/Ships/CapitalShipSections/e_stern_c_model"),
                100, sternTurrets, this));

            // Assault Fighter Hangar
            List<Turret> assaultFighterHangarTurrets = new List<Turret>();
            assaultFighterHangarTurrets.Add(SceneObjectFactory.createEsxolusTurret(Position + Vector3.Transform(new Vector3(730.8f, 1742.7f, 1120.5f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(90f), MathHelper.ToRadians(0f), MathHelper.ToRadians(0f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(90f), MathHelper.ToRadians(0f), MathHelper.ToRadians(0f))), 30, this));
            assaultFighterHangarTurrets.Add(SceneObjectFactory.createEsxolusTurret(Position + Vector3.Transform(new Vector3(730.8f, 1495.8f, 1019.1f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(90f), MathHelper.ToRadians(0f), MathHelper.ToRadians(0f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(90f), MathHelper.ToRadians(0f), MathHelper.ToRadians(0f))), 30, this));
            assaultFighterHangarTurrets.Add(SceneObjectFactory.createEsxolusTurret(Position + Vector3.Transform(new Vector3(-730.8f, 1742.7f, 1120.5f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(90f), MathHelper.ToRadians(0f), MathHelper.ToRadians(0f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(90f), MathHelper.ToRadians(0f), MathHelper.ToRadians(0f))), 30, this));
            assaultFighterHangarTurrets.Add(SceneObjectFactory.createEsxolusTurret(Position + Vector3.Transform(new Vector3(-730.8f, 1495.8f, 1019.1f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(90f), MathHelper.ToRadians(0f), MathHelper.ToRadians(0f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(90f), MathHelper.ToRadians(0f), MathHelper.ToRadians(0f))), 30, this));
            shipPieces.Add(new SpawnShipPiece(UniqueId, Position, Rotation, ShipTeam,
                ContentLoadManager.loadModel("Models/Ships/CapitalShipSections/e_af_hangar_model"),
                ContentLoadManager.loadModel("Models/Ships/CapitalShipSections/e_af_hangar_d_model"),
                ContentLoadManager.loadModel("Models/Ships/CapitalShipSections/e_af_hangar_c_model"),
                100, assaultFighterHangarTurrets, this));

            // Bomber Hangar
            List<Turret> bomberHangarTurrets = new List<Turret>();
            bomberHangarTurrets.Add(SceneObjectFactory.createEsxolusTurret(Position + Vector3.Transform(new Vector3(730.8f, 2882.1f, -2280f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(90f), MathHelper.ToRadians(0f), MathHelper.ToRadians(0f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(90f), MathHelper.ToRadians(0f), MathHelper.ToRadians(0f))), 30, this));
            bomberHangarTurrets.Add(SceneObjectFactory.createEsxolusTurret(Position + Vector3.Transform(new Vector3(730.8f, 2664f, -2300.7f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(90f), MathHelper.ToRadians(0f), MathHelper.ToRadians(0f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(90f), MathHelper.ToRadians(0f), MathHelper.ToRadians(0f))), 30, this));
            bomberHangarTurrets.Add(SceneObjectFactory.createEsxolusTurret(Position + Vector3.Transform(new Vector3(-730.8f, 2882.1f, -2280f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(90f), MathHelper.ToRadians(0f), MathHelper.ToRadians(0f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(90f), MathHelper.ToRadians(0f), MathHelper.ToRadians(0f))), 30, this));
            bomberHangarTurrets.Add(SceneObjectFactory.createEsxolusTurret(Position + Vector3.Transform(new Vector3(-730.8f, 2664f, -2300.7f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(90f), MathHelper.ToRadians(0f), MathHelper.ToRadians(0f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(90f), MathHelper.ToRadians(0f), MathHelper.ToRadians(0f))), 30, this));
            shipPieces.Add(new SpawnShipPiece(UniqueId, Position, Rotation, ShipTeam,
                ContentLoadManager.loadModel("Models/Ships/CapitalShipSections/e_b_hangar_model"),
                ContentLoadManager.loadModel("Models/Ships/CapitalShipSections/e_b_hangar_d_model"),
                ContentLoadManager.loadModel("Models/Ships/CapitalShipSections/e_b_hangar_c_model"),
                100, bomberHangarTurrets, this));

            // Support Hangar
            List<Turret> supportHangarTurrets = new List<Turret>();
            supportHangarTurrets.Add(SceneObjectFactory.createEsxolusTurret(Position + Vector3.Transform(new Vector3(730.8f, -3131.1f, 33.3f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(90f), MathHelper.ToRadians(0f), MathHelper.ToRadians(0f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(90f), MathHelper.ToRadians(0f), MathHelper.ToRadians(0f))), 30, this));
            supportHangarTurrets.Add(SceneObjectFactory.createEsxolusTurret(Position + Vector3.Transform(new Vector3(730.8f, -3400.8f, 124.2f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(90f), MathHelper.ToRadians(0f), MathHelper.ToRadians(0f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(90f), MathHelper.ToRadians(0f), MathHelper.ToRadians(0f))), 30, this));
            supportHangarTurrets.Add(SceneObjectFactory.createEsxolusTurret(Position + Vector3.Transform(new Vector3(-730.8f, -3131.1f, 33.3f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(90f), MathHelper.ToRadians(0f), MathHelper.ToRadians(0f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(90f), MathHelper.ToRadians(0f), MathHelper.ToRadians(0f))), 30, this));
            supportHangarTurrets.Add(SceneObjectFactory.createEsxolusTurret(Position + Vector3.Transform(new Vector3(-730.8f, -3400.8f, 124.2f), Rotation),
                  Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(90f), MathHelper.ToRadians(0f), MathHelper.ToRadians(0f)),
                  Vector3.Transform(Vector3.Up, Rotation * Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(90f), MathHelper.ToRadians(0f), MathHelper.ToRadians(0f))), 30, this));
            shipPieces.Add(new SpawnShipPiece(UniqueId, Position, Rotation, ShipTeam,
                ContentLoadManager.loadModel("Models/Ships/CapitalShipSections/e_support_hangar_model"),
                ContentLoadManager.loadModel("Models/Ships/CapitalShipSections/e_support_hangar_d_model"),
                ContentLoadManager.loadModel("Models/Ships/CapitalShipSections/e_support_hangar_c_model"), 
                100, supportHangarTurrets, this));

            shipPieces.Add(new SpawnShipPiece(UniqueId, Position, Rotation, ShipTeam,
                ContentLoadManager.loadModel("Models/Ships/CapitalShipSections/e_port_inner_engine_model"),
                ContentLoadManager.loadModel("Models/Ships/CapitalShipSections/e_port_inner_engine_d_model"), 
                ContentLoadManager.loadModel("Models/Ships/CapitalShipSections/e_port_inner_engine_c_model"), 
                100, new List<Turret>(), this));

            shipPieces.Add(new SpawnShipPiece(UniqueId, Position, Rotation, ShipTeam,
                ContentLoadManager.loadModel("Models/Ships/CapitalShipSections/e_port_outer_engine_model"),
                ContentLoadManager.loadModel("Models/Ships/CapitalShipSections/e_port_outer_engine_d_model"), 
                ContentLoadManager.loadModel("Models/Ships/CapitalShipSections/e_port_outer_engine_c_model"),
                100, new List<Turret>(), this));

            shipPieces.Add(new SpawnShipPiece(UniqueId, Position, Rotation, ShipTeam,
                ContentLoadManager.loadModel("Models/Ships/CapitalShipSections/e_starboard_inner_engine_model"),
                ContentLoadManager.loadModel("Models/Ships/CapitalShipSections/e_starboard_inner_engine_d_model"), 
                ContentLoadManager.loadModel("Models/Ships/CapitalShipSections/e_starboard_inner_engine_c_model"),
                100, new List<Turret>(), this));

            shipPieces.Add(new SpawnShipPiece(UniqueId, Position, Rotation, ShipTeam,
                ContentLoadManager.loadModel("Models/Ships/CapitalShipSections/e_starboard_outer_engine_model"),
                ContentLoadManager.loadModel("Models/Ships/CapitalShipSections/e_starboard_outer_engine_d_model"), 
                ContentLoadManager.loadModel("Models/Ships/CapitalShipSections/e_starboard_outer_engine_c_model"),
                100, new List<Turret>(), this));
            #endregion

            // Center positions of the pieces, mostly for drawing recticles
            shipPiecePositions = new Vector3[]
            {
                // 0  Bow/Int Hanger
                new Vector3(0.0f, -1655.7f, 5876.7f),
                // 1  Forward Hull
                new Vector3(0.0f, -15.0f, 6443.7f),
                // 2  Assault Hanger
                new Vector3(0.0f, 1353.0f, -514.2f),
                // 3  Bomber Hanger
                new Vector3(0.0f, 2005.5f, -4249.5f),            
                // 4  Port Quarter
                new Vector3(4689.0f, -952.5f, -2427.3f), 

                // 5  Port Overwing
                new Vector3(5253.6f, -920.7f, -481.2f), 
                // 6  Port Underwing
                new Vector3(4725.0f, -1626.9f, -652.5f), 
                // 7  Starboard Quarter
                new Vector3(-4689.0f, -952.5f, -2427.3f),            
                // 8  Starboard Overwing
                new Vector3(-5253.6f, -920.7f, -481.2f), 
                // 9  Starboard Underwing 
                new Vector3(-4725.0f, -1626.9f, -625.5f),

                // 10  Stern
                new Vector3(0.0f, -7.45f, -8273.4f),
                // 11  Port Inner Engine
                new Vector3(1359.3f, -827.1f, -9384.9f),
                // 12  Port Outer Engine
                new Vector3(3678.0f, -727.5f, -8560.2f),
                // 13  Starboard Inner Engine
                new Vector3(-1359.3f, -827.1f, -9384.9f),
                // 14  Starboard Outer Engine
                new Vector3(-3678.0f, -727.1f, -8560.2f),

                // 15  Support Hangar
                new Vector3(0.0f, -1779.9f, -1830.9f),
            };

            Initialize();

            setModelByString("Models/box");
            setCloseModelByString("Models/box");
            // collisionMesh = ContentLoadManager.loadModel("Models/Ships/esxolus_capital_ship_collision_model");

            radiusSphere = new CollisionSphere(Position, Model);

            /*collisionBase = new CollisionMesh(collisionMesh, Position, Rotation);
            collisionBase.setActive(true);
            collisionBase.addCollisionEvent(collisionEvent);
            collisionBase.setParent(this);*/
        }
    }
}
