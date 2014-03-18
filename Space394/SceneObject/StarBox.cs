using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Space394.Particles;
using Space394.Collision;
using Microsoft.Xna.Framework.Graphics;

namespace Space394.SceneObjects
{
    public class StarBox : SceneObject
    {
        private StarParticleGenerator starForge;

        private bool dimming = true;

        private const float DIMMING_SPEED = 1.0f;

        private float alpha = 1;

        public StarBox(long _uniqueId)
            : base(_uniqueId, Vector3.Zero, Quaternion.Identity, "Models/greater_starbox_model")
        {
            Scale = 1000000;

            starForge = new StarParticleGenerator(1000, (int)(Model.Meshes.First().BoundingSphere.Radius*Scale*0.4));
        }

        public override void Update(float deltaTime)
        {
            starForge.Update(deltaTime);

            if (dimming)
            {
                alpha -= deltaTime * DIMMING_SPEED;
                if (alpha <= 0)
                {   
                    alpha = 0;
                    dimming = false;
                }
                else { }
            }
            else
            {
                alpha += deltaTime * DIMMING_SPEED;
                if (alpha >= 1)
                {
                    alpha = 1;
                    dimming = true;
                }
                else { }
            }
            
            base.Update(deltaTime);
        }

        public override void Draw(GameCamera camera)
        {
            starForge.Draw(camera);

            base.Draw(camera);
        }

        public override void onCollide(SceneObject caller, float damage)
        {
            // Do nothing
        }
    }
}
