using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Space394.Particles;
using Microsoft.Xna.Framework.Graphics;

namespace Space394.SceneObjects
{
    public class MassiveExplosion : SceneObject
    {
        private ExplosionParticleGenerator generator;

        private Model explosion;

        private const float MAX_SCALE_RESCALE = 1500f;
        private float MAX_SCALE = 1000f; //150f; //50f;

        private const float SCALE_RATE_RESCALE = 750f;
        private float SCALE_RATE = 1500f; //70;
        
        private const float OUTER_EXTRA_SIZE = 100f;

        private const float spinRate = 100.0f;
        private const float outerspinRate = 150.0f;

        private const float outerAlpha = 0.5f;

        private Vector3 spinDirection;
        private Vector3 outerSpinDirection;

        private float transparency = 1;
        private const float TRANSPARENCY_SCALE = 3.0f;
        private const float TRANSPARENCY_TIP = 0.75f;

        private bool skipUpdate = true;

        public MassiveExplosion(long _uniqueId, Vector3 _position, Quaternion _rotation)
            : base(_uniqueId, _position, _rotation, "Models/explosion")
        {
            Scale = 1;

            Active = true;

            explosion = ContentLoadManager.loadModel("Models/explosion");

            Random random = new Random((int)(_position.X + _position.Y + _position.Z));
            float x = random.Next() * spinRate; if (random.Next() % 2 == 0) { x *= -1; } else { }
            float y = random.Next() * spinRate; if (random.Next() % 2 == 0) { y *= -1; } else { }
            float z = random.Next() * spinRate; if (random.Next() % 2 == 0) { z *= -1; } else { }

            spinDirection = new Vector3(x, y, z);

            x = random.Next() * outerspinRate; if (random.Next() % 2 == 0) { x *= -1; } else { }
            y = random.Next() * outerspinRate; if (random.Next() % 2 == 0) { y *= -1; } else { }
            z = random.Next() * outerspinRate; if (random.Next() % 2 == 0) { z *= -1; } else { }

            outerSpinDirection = new Vector3(x, y, z);

            float r = (float)random.NextDouble() * MAX_SCALE_RESCALE; if (random.Next() % 2 == 0) { x *= -1; } else { }
            MAX_SCALE += r;

            r = (float)random.NextDouble() * SCALE_RATE_RESCALE; if (random.Next() % 2 == 0) { x *= -1; } else { }
            SCALE_RATE += r;

            generator = new ExplosionParticleGenerator(_position);
        }

        public override void Draw(GameCamera camera)
        {
            generator.Draw(camera);

            foreach (ModelMesh mesh in Model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.Alpha = transparency;
                }
            }

            base.Draw(camera);

            Scale = Scale + OUTER_EXTRA_SIZE;
            foreach (ModelMesh mesh in explosion.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.Alpha = MathHelper.Max(outerAlpha - transparency, 0);
                }
            }
            drawExtraModel(camera, explosion);
            //foreach (ModelMesh mesh in explosion.Meshes)
            //{
            //    foreach (BasicEffect effect in mesh.Effects)
            //    {
            //        effect.Alpha = 1f;
            //    }
            //}
            Scale = Scale - OUTER_EXTRA_SIZE;
        }

        public override void onCollide(SceneObject caller, float damage)
        {
            // DO NOTHING
            return;
        }

        public override void Update(float deltaTime)
        {
            if (skipUpdate)
            {
                if (Scale > MAX_SCALE * TRANSPARENCY_TIP)
                {
                    transparency = MathHelper.Max(transparency - TRANSPARENCY_SCALE * deltaTime, 0);
                }
                else { }

                if (Scale <= MAX_SCALE)
                {
                    Scale = Scale + deltaTime * SCALE_RATE;
                }
                else
                {
                    Active = false;
                    QueuedRemoval = true;
                }

                Rotation = AdjustRotation(Vector3.One, spinDirection, Vector3.Up, deltaTime);

                generator.Update(deltaTime);

                base.Update(deltaTime);
            }
            else { skipUpdate = false; }
        }
    }
}
