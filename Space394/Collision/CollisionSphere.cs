using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BEPUphysics.CollisionShapes;
using BEPUphysics.Entities.Prefabs;
using BEPUphysics.Collidables.MobileCollidables;
using BEPUphysics.Collidables;
using BEPUphysics.NarrowPhaseSystems.Pairs;
using BEPUphysics.Collidables.Events;
using BEPUphysics.EntityStateManagement;
using BEPUphysics.PositionUpdating;

namespace Space394.Collision
{
    // This will be the basic collision model for fighters
    public class CollisionSphere : CollisionBase
    {
        private int radius;
        public int Radius
        {
            get { return radius; }
            set 
            {
                radius = value;
                radiusSq = radius * radius;
                boundingSphere.Radius = radius;
                sphere.Radius = radius;
            }
        }
        private int radiusSq;
        public int RadiusSq
        {
            get { return radiusSq; }
        }

        private BoundingSphere boundingSphere;
        public BoundingSphere BoundingSphere
        {
            get { return boundingSphere; }
        }
        // No setter 

        private Sphere sphere;

        private const int RESOLUTION = 20;

        public CollisionSphere(Vector3 _position, Model _model)
        {
            boundingSphere = _model.Meshes.First().BoundingSphere;
            base.Position = _position;
            radius = (int)(boundingSphere.Radius);
            radiusSq = radius * radius;
#if DEBUG
            if (!BoundingSphereRenderer.isInitialized())
            {
                BoundingSphereRenderer.InitializeGraphics(Space394Game.GameInstance.GraphicsDevice, RESOLUTION);
            }
            else { }
#endif
            sphere = new Sphere(Position, radius, 1);
            //sphere.CollisionInformation.Events.InitialCollisionDetected += new InitialCollisionDetectedEventHandler<EntityCollidable>(onCollide);
            sphere.CollisionInformation.Tag = this;
            sphere.PositionUpdateMode = PositionUpdateMode.Passive;
        }

        public CollisionSphere(Vector3 _position, int _radius)
        {
            boundingSphere = new BoundingSphere(_position, _radius);
            base.Position = _position;
            radius = _radius;
            radiusSq = radius * radius;
#if DEBUG
            if (!BoundingSphereRenderer.isInitialized())
            {
                BoundingSphereRenderer.InitializeGraphics(Space394Game.GameInstance.GraphicsDevice, RESOLUTION);
            }
            else { }
#endif
            sphere = new Sphere(Position, radius, 1);
            //sphere.CollisionInformation.Events.InitialCollisionDetected += new InitialCollisionDetectedEventHandler<EntityCollidable>(onCollide);
            sphere.CollisionInformation.Tag = this;
            sphere.PositionUpdateMode = PositionUpdateMode.Passive;
        }

        public override BEPUphysics.ISpaceObject getPhysicsCollider()
        {
            return sphere;
        }

        public override Vector3 Position
        {
            set
            {
                base.Position = value;
                boundingSphere.Center = value;
                sphere.Position = value;
            }
        }

        public override bool isColliding(CollisionBase collider)
        {
            if (collider != null)
            {
                if (collider.Active && Active)
                {
                    if (collider is CollisionBox)
                    {
#if DEBUG
                        if (boundingSphere.Intersects(((CollisionBox)collider).BoundingBox)) 
                            { 
                            HasCollided = true; 
                            collider.HasCollided = true;
                        } 
                        else { }
#endif
                        return (boundingSphere.Intersects(((CollisionBox)collider).BoundingBox));
                    }
                    else if (collider is CollisionSphere)
                    {
#if DEBUG
                        if (boundingSphere.Intersects(((CollisionSphere)collider).BoundingSphere))
                        {
                            HasCollided = true;
                            collider.HasCollided = true;
                        }
                        else { }
#endif
                        return (boundingSphere.Intersects(((CollisionSphere)collider).BoundingSphere));
                    }
                    else if (collider is CollisionMesh)
                    {
                        return (collider.isColliding(this));
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public bool isCollidingSq(CollisionSphere collider)
        {
            if (collider != null)
            {
                if (collider.Active && Active)
                {
                    Vector3 vd = this.Position - collider.Position;

                    float sqrRadius = (vd.X * vd.X) + (vd.Y * vd.Y) + (vd.Z * vd.Z);

                    if (sqrRadius <= collider.radiusSq + this.radiusSq)
                    {
                        return true;
                    }

                    return false;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public bool isCollidingSq(CollisionSphere collider, out float _distanceSqr)
        {
            _distanceSqr = 0.0f;
            if (collider != null)
            {
                if (collider.Active && Active)
                {
                    Vector3 vd = this.Position - collider.Position;

                    _distanceSqr = (vd.X * vd.X) + (vd.Y * vd.Y) + (vd.Z * vd.Z);

                    if (_distanceSqr <= collider.radiusSq + this.radiusSq)
                    {
                        return true;
                    }

                    return false;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }


        public override void debugDraw(GameCamera camera)
        {
#if DEBUG
            Space394Game.GameInstance.GraphicsDevice.Viewport = camera.ViewPort;
            if (Active)
            {
                if (HasCollided && !StaticColor)
                {
                    BoundingSphereRenderer.Render(boundingSphere, Space394Game.GameInstance.GraphicsDevice, camera.ViewMatrix, camera.ProjectionMatrix, Color.Red);
                }
                else
                {
                    BoundingSphereRenderer.Render(boundingSphere, Space394Game.GameInstance.GraphicsDevice, camera.ViewMatrix, camera.ProjectionMatrix, Color.Blue);
                }
            }
            else
            {
                BoundingSphereRenderer.Render(boundingSphere, Space394Game.GameInstance.GraphicsDevice, camera.ViewMatrix, camera.ProjectionMatrix, Color.Gray);
            }
            HasCollided = false;
            Space394Game.GameInstance.GraphicsDevice.Viewport = Space394Game.GameInstance.DefaultViewPort;

            Space394Game.GameInstance.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
#endif
        }

        public void debugDrawMeshSpheres(Vector3 position, Model model, GameCamera camera)
        {
#if DEBUG
            foreach (ModelMesh mesh in model.Meshes)
            {
                BoundingSphere sphere = mesh.BoundingSphere;
                sphere.Center += position;
                BoundingSphereRenderer.Render(sphere, Space394Game.GameInstance.GraphicsDevice, camera.ViewMatrix, camera.ProjectionMatrix, Color.Blue);
            }
            Space394Game.GameInstance.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
#endif
        }

        public override void addCollisionEvent(InitialCollisionDetectedEventHandler<EntityCollidable> _function)
        {
            sphere.CollisionInformation.Events.InitialCollisionDetected += _function;
        }
    }

    #region DEBUG_DRAW

    // Courtesy of Matthew Overall of http://codingquirks.com/
    /// <summary>
    /// Provides a set of methods for rendering BoundingSpheres.
    /// </summary>
    public static class BoundingSphereRenderer
    {
        static VertexBuffer vertBuffer;
        static BasicEffect effect;
        static int sphereResolution;

        static bool initialized = false;
        public static bool isInitialized() { return initialized; }

        /// <summary>
        /// Initializes the graphics objects for rendering the spheres. If this method isn't
        /// run manually, it will be called the first time you render a sphere.
        /// </summary>
        /// <param name="graphicsDevice">The graphics device to use when rendering.</param>
        /// <param name="sphereResolution">The number of line segments
        ///     to use for each of the three circles.</param>
        public static void InitializeGraphics(GraphicsDevice graphicsDevice, int sphereResolution)
        {
            BoundingSphereRenderer.sphereResolution = sphereResolution;

            //vertDecl = new VertexDeclaration(
            effect = new BasicEffect(graphicsDevice);
            effect.LightingEnabled = false;
            effect.VertexColorEnabled = false;

            VertexPositionColor[] verts = new VertexPositionColor[(sphereResolution + 1) * 3];

            int index = 0;

            float step = MathHelper.TwoPi / (float)sphereResolution;

            //create the loop on the XY plane first
            for (float a = 0f; a <= MathHelper.TwoPi; a += step)
            {
                verts[index++] = new VertexPositionColor(
                    new Vector3((float)Math.Cos(a), (float)Math.Sin(a), 0f),
                    Color.White);
            }

            //next on the XZ plane
            for (float a = 0f; a <= MathHelper.TwoPi; a += step)
            {
                verts[index++] = new VertexPositionColor(
                    new Vector3((float)Math.Cos(a), 0f, (float)Math.Sin(a)),
                    Color.White);
            }

            //finally on the YZ plane
            for (float a = 0f; a <= MathHelper.TwoPi; a += step)
            {
                verts[index++] = new VertexPositionColor(
                    new Vector3(0f, (float)Math.Cos(a), (float)Math.Sin(a)),
                    Color.White);
            }

            vertBuffer = new VertexBuffer(graphicsDevice, typeof(VertexPositionColor), verts.Length, BufferUsage.None);
            vertBuffer.SetData(verts);

            initialized = true;
        }

        /// <summary>
        /// Renders a bounding sphere using different colors for each axis.
        /// </summary>
        /// <param name="sphere">The sphere to render.</param>
        /// <param name="graphicsDevice">The graphics device to use when rendering.</param>
        /// <param name="view">The current view matrix.</param>
        /// <param name="projection">The current projection matrix.</param>
        /// <param name="xyColor">The color for the XY circle.</param>
        /// <param name="xzColor">The color for the XZ circle.</param>
        /// <param name="yzColor">The color for the YZ circle.</param>
        public static void Render(
            BoundingSphere sphere,
            GraphicsDevice graphicsDevice,
            Matrix view,
            Matrix projection,
            Color xyColor,
            Color xzColor,
            Color yzColor)
        {
            if (vertBuffer == null)
                InitializeGraphics(graphicsDevice, 30);

            graphicsDevice.SetVertexBuffer(vertBuffer);

            effect.World =
                Matrix.CreateScale(sphere.Radius) *
                Matrix.CreateTranslation(sphere.Center);
            effect.View = view;
            effect.Projection = projection;
            effect.DiffuseColor = xyColor.ToVector3();

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();

                //render each circle individually
                graphicsDevice.DrawPrimitives(
                      PrimitiveType.LineStrip,
                      0,
                      sphereResolution);
                pass.Apply();
                effect.DiffuseColor = xzColor.ToVector3();
                graphicsDevice.DrawPrimitives(
                      PrimitiveType.LineStrip,
                      sphereResolution + 1,
                      sphereResolution);
                pass.Apply();
                effect.DiffuseColor = yzColor.ToVector3();
                graphicsDevice.DrawPrimitives(
                      PrimitiveType.LineStrip,
                      (sphereResolution + 1) * 2,
                      sphereResolution);
                pass.Apply();

            }

        }

        public static void Render(BoundingSphere[] spheres,
           GraphicsDevice graphicsDevice,
           Matrix view,
           Matrix projection,
           Color xyColor,
            Color xzColor,
            Color yzColor)
        {
            foreach (BoundingSphere sphere in spheres)
            {
                Render(sphere, graphicsDevice, view, projection, xyColor, xzColor, yzColor);
            }
        }

        public static void Render(BoundingSphere[] spheres,
            GraphicsDevice graphicsDevice,
            Matrix view,
            Matrix projection,
            Color color)
        {
            foreach (BoundingSphere sphere in spheres)
            {
                Render(sphere, graphicsDevice, view, projection, color);
            }
        }

        /// <summary>
        /// Renders a bounding sphere using a single color for all three axis.
        /// </summary>
        /// <param name="sphere">The sphere to render.</param>
        /// <param name="graphicsDevice">The graphics device to use when rendering.</param>
        /// <param name="view">The current view matrix.</param>
        /// <param name="projection">The current projection matrix.</param>
        /// <param name="color">The color to use for rendering the circles.</param>
        public static void Render(
            BoundingSphere sphere,
            GraphicsDevice graphicsDevice,
            Matrix view,
            Matrix projection,
            Color color)
        {
            if (vertBuffer == null)
                InitializeGraphics(graphicsDevice, 30);

            graphicsDevice.SetVertexBuffer(vertBuffer);

            effect.World =
                  Matrix.CreateScale(sphere.Radius) *
                  Matrix.CreateTranslation(sphere.Center);
            effect.View = view;
            effect.Projection = projection;
            effect.DiffuseColor = color.ToVector3();

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();

                //render each circle individually
                graphicsDevice.DrawPrimitives(
                      PrimitiveType.LineStrip,
                      0,
                      sphereResolution);
                graphicsDevice.DrawPrimitives(
                      PrimitiveType.LineStrip,
                      sphereResolution + 1,
                      sphereResolution);
                graphicsDevice.DrawPrimitives(
                      PrimitiveType.LineStrip,
                      (sphereResolution + 1) * 2,
                      sphereResolution);

            }
        }
    }
    
    #endregion
}