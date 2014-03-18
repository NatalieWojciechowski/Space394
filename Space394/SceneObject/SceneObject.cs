using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Space394.Collision;
using Space394.Scenes;
using BEPUphysics;
using BEPUphysics.Collidables;
using BEPUphysics.Collidables.MobileCollidables;
using BEPUphysics.NarrowPhaseSystems.Pairs;
using Space394.Particles;

namespace Space394.SceneObjects
{
    public abstract class SceneObject
    {
        private long uniqueId;
        public long UniqueId
        {
            get { return uniqueId; }
            set { uniqueId = value; }
        }

        private Model model; // Only accessible from this class
        public Model Model
        {
            get { return model; }
            set { model = value; }
        }
        protected Model setModelByString(String modelName)
        {
            model = ContentLoadManager.loadModel(modelName);
            return model;
        }
        public Model getModelByString(String modelName)
        {
            return ContentLoadManager.loadModel(modelName);
        }

        private float health = 0;
        public float Health
        {
            get { return health; }
            set { health = value; }
        }

        private float damage = 0;
        public float Damage
        {
            get { return damage; }
            set { damage = value; }
        }

        public abstract void onCollide(SceneObject caller, float damage);

        private bool queuedRemoval = false;
        public bool QueuedRemoval
        {
            get { return queuedRemoval; }
            set { queuedRemoval = value; }
        }

        private bool active = true;
        public virtual bool Active
        {
            get { return active; }
            set
            {
                Scene cScene = Space394Game.GameInstance.CurrentScene;
                if (cScene is GameScene)
                {
                    if (value && !active && collisionBase != null)
                    {
                        ((GameScene)cScene).CollisionManager.addToCollisionList(collisionBase);
                    }
                    else if (!value && active && collisionBase != null)
                    {
                        ((GameScene)cScene).CollisionManager.removeFromCollisionList(collisionBase);
                    }
                    else { }
                }
                else { }
                active = value; 
            }
        }

        private Vector3 previousPosition;
        public Vector3 PreviousPosition
        {
            get { return previousPosition; }
            set { previousPosition = value; }
        }
        private Vector3 position; // Only accessible from this class
        public virtual Vector3 Position
        {
            get { return position; }
            set 
            {
                position = value;
                if (collisionBase != null)
                {
                    collisionBase.Position = position;
                }
                else { }
            }
        }

        public float MAX_SLERP = 2.25f;

        private Quaternion previousRotation;
        public Quaternion PreviousRotation
        {
            get { return previousRotation; }
            set { previousRotation = value; }
        }

        private Quaternion rotation; // Only accessible from this class
        public Quaternion Rotation
        {
            get { return rotation; }
            set 
            { rotation = value; }
        }

        public float getPitch()
        {
            return (float)Math.Atan2(2*(rotation.Y*rotation.Z + rotation.W*rotation.X), rotation.W*rotation.W - rotation.X*rotation.X - rotation.Y*rotation.Y + rotation.Z*rotation.Z);
        }

        public float getYaw()
        {
            return (float)Math.Asin(-2*(rotation.X*rotation.Z - rotation.W*rotation.Y));
        }

        public float getRoll()
        {
            return (float)Math.Atan2(2*(rotation.X*rotation.Y + rotation.W*rotation.Z), rotation.W*rotation.W + rotation.X*rotation.X - rotation.Y*rotation.Y - rotation.Z*rotation.Z);
        }

        private float scale;
        public float Scale
        {
            get { return scale; }
            set { scale = value; }
        }

        private Vector3 scales;
        public Vector3 Scales
        {
            get { return scales; }
            set { scales = value; }
        }

        private CollisionBase collisionBase;
        public CollisionBase CollisionBase
        {
            get { return collisionBase; }
            set { collisionBase = value; }
        }

        protected Quaternion jitterRotationVelocity;
        protected Quaternion jitterRotation;
        // No setters or getters (at least for now)

        protected Vector3 jitterPositionVelocity;
        protected Vector3 jitterPosition; 
        // No setters or getters (at least for now)

        public Vector3 getPositionWithJitter() { return (position + jitterPosition); }

        protected SceneObject(long _uniqueID, Vector3 _position, Quaternion _rotation, String modelName)
        {
            uniqueId = _uniqueID;
            position = _position;
            rotation = _rotation;
            scale = 1;
            model = ContentLoadManager.loadModel(modelName);
            jitterPosition = Vector3.Zero;
            jitterPositionVelocity = Vector3.Zero;
            jitterRotation = Quaternion.Identity;
            jitterRotationVelocity = Quaternion.Identity;
            scales = Vector3.One;
        }

        public virtual void Update(float deltaTime)
        {
            if (collisionBase != null)
            {
                collisionBase.Position = position;
            }
            else { }
        }

        public virtual void Draw(GameCamera camera)
        {
            if (active == true && model != null)
            {
                Space394Game.GameInstance.GraphicsDevice.Viewport = camera.ViewPort;
                //model.Draw(world, view, projection);
                // Copy any parent transforms.
                Matrix[] transforms = new Matrix[model.Bones.Count];
                model.CopyAbsoluteBoneTransformsTo(transforms);

                // Draw the model. A model can have multiple meshes, so loop.
                foreach (ModelMesh mesh in model.Meshes)
                {
                    // This is where the mesh orientation is set, as well 
                    // as our camera and projection.
                    foreach (BasicEffect effect in mesh.Effects)
                    {
                        effect.EnableDefaultLighting();
                        effect.World = transforms[mesh.ParentBone.Index]
                            * Matrix.CreateScale(scale)
                            * Matrix.CreateScale(scales)
                            * Matrix.CreateFromQuaternion(rotation)
                            * Matrix.CreateFromQuaternion(jitterRotation)
                            * Matrix.CreateTranslation(position)
                            * Matrix.CreateTranslation(jitterPosition);

                        //* Matrix.CreateRotationX(MathHelper.ToRadians(rotation.X))
                        //* Matrix.CreateRotationY(MathHelper.ToRadians(rotation.Y))
                        //* Matrix.CreateRotationZ(MathHelper.ToRadians(rotation.Z));
                        effect.View = camera.ViewMatrix;
                        effect.Projection = camera.ProjectionMatrix;
                    }
                    // Draw the mesh, using the effects set above.
                    mesh.Draw();
                }

#if DEBUG
                if (collisionBase != null)
                {
                    collisionBase.debugDraw(camera);
                }
                else { }
#endif
                Space394Game.GameInstance.GraphicsDevice.DepthStencilState = DepthStencilState.Default;

                Space394Game.GameInstance.GraphicsDevice.Viewport = Space394Game.GameInstance.DefaultViewPort;
            }
            else { }
        }

        protected void drawExtraModel(GameCamera camera, Model exmodel)
        {
            if (active == true && exmodel != null)
            {
                Space394Game.GameInstance.GraphicsDevice.Viewport = camera.ViewPort;
                //model.Draw(world, view, projection);
                // Copy any parent transforms.
                Matrix[] transforms = new Matrix[exmodel.Bones.Count];
                exmodel.CopyAbsoluteBoneTransformsTo(transforms);

                // Draw the model. A model can have multiple meshes, so loop.
                foreach (ModelMesh mesh in exmodel.Meshes)
                {
                    // This is where the mesh orientation is set, as well 
                    // as our camera and projection.
                    foreach (BasicEffect effect in mesh.Effects)
                    {
                        effect.EnableDefaultLighting();
                        effect.World = transforms[mesh.ParentBone.Index]
                            * Matrix.CreateScale(scale)
                            * Matrix.CreateFromQuaternion(rotation)
                            * Matrix.CreateFromQuaternion(jitterRotation)
                            * Matrix.CreateTranslation(position)
                            * Matrix.CreateTranslation(jitterPosition);

                        //* Matrix.CreateRotationX(MathHelper.ToRadians(rotation.X))
                        //* Matrix.CreateRotationY(MathHelper.ToRadians(rotation.Y))
                        //* Matrix.CreateRotationZ(MathHelper.ToRadians(rotation.Z));
                        effect.View = camera.ViewMatrix;
                        effect.Projection = camera.ProjectionMatrix;
                    }
                    // Draw the mesh, using the effects set above.
                    mesh.Draw();
                }

                Space394Game.GameInstance.GraphicsDevice.DepthStencilState = DepthStencilState.Default;

                Space394Game.GameInstance.GraphicsDevice.Viewport = Space394Game.GameInstance.DefaultViewPort;
            }
            else { }
        }

        public virtual void onAddToScene(Scene scene)
        {
            // Do nothing
        }

        private GameCamera getCamera()
        {
            return Space394Game.GameInstance.CurrentScene.Camera;
        }

        public virtual void collisionEvent(EntityCollidable eCollidable, Collidable collidable, CollidablePairHandler pairHandler)
        {
            if (collidable.Tag != null
                && collidable.Tag is CollisionBase)
            {
                SceneObject collidee = ((CollisionBase)collidable.Tag).Parent;
                if (collidee != null)
                {
                    collidee.onCollide(this, damage);
                }
                else { }
            }
            else { }
        }

        // http://gamedev.stackexchange.com/questions/15070/orienting-a-model-to-face-a-target
        // returns a quaternion that rotates vector a to vector b
        public Quaternion AdjustRotation(Vector3 source, Vector3 dest, Vector3 up, float deltaTime)
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
            Quaternion direction = Quaternion.CreateFromAxisAngle(rotAxis, rotAngle);

            return Quaternion.Slerp(Rotation, direction, MAX_SLERP * deltaTime);
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
