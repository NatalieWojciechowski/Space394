using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Space394.Events
{
    public abstract class Event
    {
        protected bool completed;

        private bool hasPosition;
        public bool HasPosition
        {
            get { return hasPosition; }
            set { hasPosition = value; }
        }

        private List<Vector3> positions;
        public List<Vector3> Positions
        {
            get { return positions; }
            set { positions = value; }
        }

        protected const float positionOffset = 15.0f;

        protected const float MIN_SCALE = 0.25f;
        protected const float MAX_SCALE = 1.0f;
        protected const float SCALE_SCALE = 500000000.0f;

        protected static Model arrow = null;

        public static bool active = true;

        protected AutoTexture2D objectiveText;

        public Event()
        {
            completed = false;
            hasPosition = false;

            if (arrow == null)
            {
                arrow = ContentLoadManager.loadModel("Models/arrow");
            }
            else { }
        }

        public abstract bool IsComplete();

        public abstract void Update(float deltaTime);

        public virtual void Draw(PlayerCamera camera)
        {
            Space394Game.GameInstance.GraphicsDevice.Viewport = camera.ViewPort;

            if (active)
            {
                if (hasPosition && camera.PlayerShip != null)
                {
                    for (int i = 0; i < positions.Count; i++)
                    {
                        Vector3 position = positions[i];

                        position -= camera.PlayerShip.Position;
                        position.Normalize();

                        Quaternion rotation = AdjustRotationNoLimit(Vector3.Up, position, Vector3.Up);

                        position *= positionOffset;
                        position += camera.PlayerShip.Position;

                        float dist = Vector3.DistanceSquared(position, positions[i]);
                        float scale = MathHelper.Clamp(Math.Abs(SCALE_SCALE / (dist)), MIN_SCALE, MAX_SCALE);

                        LogCat.updateValue("Dist", "" + dist);
                        LogCat.updateValue("InvDist", "" + (Math.Abs(SCALE_SCALE / (dist))));
                        LogCat.updateValue("Scale", "" + scale);

                        DrawArrow(position, rotation, scale, camera);
                    }
                }
                else { }
            }
            else { }

            SpriteBatch batch = Space394Game.GameInstance.SpriteBatch;
            batch.Begin();
            objectiveText.DrawAlreadyBegunMaintainRatio(camera);
            batch.End();

            Space394Game.GameInstance.GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            Space394Game.GameInstance.GraphicsDevice.Viewport = Space394Game.GameInstance.DefaultViewPort;
        }

        private void DrawArrow(Vector3 position, Quaternion rotation, float scale, GameCamera camera)
        {
            Matrix[] transforms = new Matrix[arrow.Bones.Count];
            arrow.CopyAbsoluteBoneTransformsTo(transforms);

            // Draw the model. A model can have multiple meshes, so loop.
            foreach (ModelMesh mesh in arrow.Meshes)
            {
                // This is where the mesh orientation is set, as well 
                // as our camera and projection.
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.World = transforms[mesh.ParentBone.Index]
                        * Matrix.CreateScale(scale)
                        * Matrix.CreateFromQuaternion(rotation)
                        * Matrix.CreateTranslation(position);

                    effect.View = camera.ViewMatrix;
                    effect.Projection = camera.ProjectionMatrix;
                }
                // Draw the mesh, using the effects set above.
                mesh.Draw();
            }
        }

        // http://gamedev.stackexchange.com/questions/15070/orienting-a-model-to-face-a-target
        // returns a quaternion that rotates vector a to vector b
        public Quaternion AdjustRotationNoLimit(Vector3 source, Vector3 dest, Vector3 up)
        {
            source.Normalize();
            dest.Normalize();
            float dot = Vector3.Dot(source, dest);
            // test for dot -1
            if (Math.Abs(dot - (-1.0f)) < 0.000001f)
            {
                // vector a and b point exactly in the opposite direction, 
                // so it is a 180 degrees turn around the up-axis
                return new Quaternion(up, MathHelper.ToRadians(180.0f));
            }
            // test for dot 1
            if (Math.Abs(dot - (1.0f)) < 0.000001f)
            {
                // vector a and b point exactly in the same direction
                // so we return the identity quaternion
                return new Quaternion(0.0f, 0.0f, 0.0f, 1.0f);
            }

            float rotAngle = (float)Math.Acos(dot);
            Vector3 rotAxis = Vector3.Cross(source, dest);
            rotAxis = Vector3.Normalize(rotAxis);
            return Quaternion.CreateFromAxisAngle(rotAxis, rotAngle);
        }
    }
}
