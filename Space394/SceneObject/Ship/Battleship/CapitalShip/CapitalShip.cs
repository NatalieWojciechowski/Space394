using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Space394.Collision;
using Microsoft.Xna.Framework.Graphics;
using Space394.Scenes;

namespace Space394.SceneObjects
{
    public abstract class CapitalShip : SpawnShip
    {
        protected Model collisionMesh;

        protected Model screenModel;

        public CapitalShip(long _uniqueId, Vector3 _position, Quaternion _rotation, Team _team)
            : base(_uniqueId, _position, _rotation, _team)
        {
            BOMBERS = 3; // 3;
            ASSAULT_FIGHTERS = 3; // 3;
            INTERCEPTORS = 3; // 3;

            INITIAL_BOMBERS = 9; // 9;
            INITIAL_ASSAULT_FIGHTERS = 9; // 9;
            INITIAL_INTERCEPTORS = 9; // 9;

            MAX_BOMBERS = 96;
            MAX_ASSAULT_FIGHTERS = 96;
            MAX_INTERCEPTORS = 96;

            launchedBombers = 0;
            launchedAssaultFighters = 0;
            launchedInterceptors = 0;

            WAVES_TO_LAUNCH = 30;
            wavesLaunched = 0;

            MAX_HEALTH = 1000;
            Health = MaxHealth;

            MAX_SHIELDS = 1000;
            Shields = MaxShields;

            MAX_ATTACKERS = 5;

            Attackable = true;
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
        }

        public override void Draw(GameCamera camera)
        {
            base.Draw(camera);
            drawExtraModel(camera, screenModel);
        }
    }
}
