using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Space394.Particles
{
    public class StarParticleGenerator : ParticleGenerator
    {
        private const int RADIUS_PULL = 100;

        public StarParticleGenerator(int numberOfStars, int starBoxRadius)
            : base()
        {
            starBoxRadius -= RADIUS_PULL;
            Random random = new Random(System.DateTime.Now.Millisecond);
            Vector3 position = new Vector3(starBoxRadius, 0, 0);
            for (int i = 0; i < numberOfStars; i++)
            {
                float yaw = (float)random.NextDouble() * (MathHelper.Pi * 2);
                float pitch = (float)random.NextDouble() * (MathHelper.Pi * 2);
                float roll = (float)random.NextDouble() * (MathHelper.Pi * 2);
                position = Vector3.Transform(position,
                    Quaternion.CreateFromYawPitchRoll(yaw, pitch, roll));
                particles.Add(new StarParticle(position));
            }
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
        }

        public override void Draw(GameCamera camera)
        {
            base.Draw(camera);
            Space394Game.GameInstance.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
        }
    }
}
