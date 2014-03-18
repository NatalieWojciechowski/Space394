using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BEPUphysics.Collidables;
using BEPUphysics;
using BEPUphysics.DataStructures;
using BEPUphysics.MathExtensions;
using Space394.Collision;
using Space394.Scenes;

namespace Space394.SceneObjects
{
    public abstract class Battleship : Ship
    {
        protected CollisionSphere radiusSphere;
        public CollisionSphere RadiusSphere
        {
            get { return radiusSphere; }
        }
        // No setter

        protected Model destroyedModel;

        // List of local nodes used for AI wandering
        protected List<Vector3> localNodes;
        public List<Vector3> LocalNodes
        {
            get { return localNodes; }
        }
        public Vector3 addLocalNode(Vector3 _node) { localNodes.Add(_node); return _node; }

#if DEBUG
        // used for the debug lines for node positions
        private float DRAW_LENGTH = 1000;
#endif

        protected bool warpingIn;
        protected bool begunWarp;
        protected const float WARP_SPEED = 10.0f;
        protected const float EXTEND_RATE = 50.0f;
        protected const float MAX_EXTEND = 10.0f;
        protected const float XY_EXTEND = 0.125f;

        public Battleship(long _uniqueId, Vector3 _position, Quaternion _rotation, Team _team)
            : base(_uniqueId, _position, _rotation, _team)
        {
            localNodes = new List<Vector3>();

            MAX_HEALTH = 5;
            Health = MaxHealth;

            MAX_SHIELDS = 5;
            Shields = MaxShields;

            warpingIn = true;
            begunWarp = true;

            //scales.X = XY_EXTEND;
            //scales.Y = XY_EXTEND;
        }

        protected void createNodes()
        {
            Vector3 shipPosition = Position;
            Quaternion shipRotation = Rotation;
            addLocalNode((shipPosition + (Vector3.Transform(Vector3.Backward * 10000, shipRotation))));
            addLocalNode((shipPosition + (Vector3.Transform(Vector3.Backward * -10000, shipRotation))));
            addLocalNode((shipPosition + (Vector3.Transform(Vector3.Right * 8000, shipRotation))));
            addLocalNode((shipPosition + (Vector3.Transform(Vector3.Right * -8000, shipRotation))));
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
            if (Health <= 0)
            {
                // setActive(false);
            }
            else { }
            /*if (begunWarp)
            {
                scales.Z += EXTEND_RATE *deltaTime;
                if (scales.Z >= MAX_EXTEND)
                {
                    scales.Z = MAX_EXTEND;
                    begunWarp = false;
                }
                else { }
            }
            else if (warpingIn) // Still warping though
            {
                scales.Z -= EXTEND_RATE * deltaTime;
                if (scales.Z <= 1)
                {
                    scales.X = 1;
                    scales.Y = 1;
                    scales.Z = 1;
                    warpingIn = false;
                }
                else { }
            }
            else { }*/
        }

        public override void Draw(GameCamera camera)
        {
#if DEBUG
            Space394Game.GameInstance.GraphicsDevice.Viewport = camera.ViewPort;

            CollisionBase.graphics.basicEffect.World = Matrix.Identity;
            CollisionBase.graphics.basicEffect.View = camera.ViewMatrix;
            CollisionBase.graphics.basicEffect.Projection = camera.ProjectionMatrix;

            foreach (EffectPass pass in CollisionBase.graphics.basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();

                int count = LocalNodes.Count;
                for (int i = 0; i < count; i++)
                {
                    Vector3 nodeNormal = localNodes[i]; // -Position; //  +Vector3.Transform(Vector3.Forward, shipRotation);

                    Space394Game.GameInstance.GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionColor>(
                        PrimitiveType.LineStrip,
                        new VertexPositionColor[] { new VertexPositionColor(nodeNormal, Color.Azure), // Pretransformed
                                                new VertexPositionColor(((nodeNormal + Vector3.Transform(Vector3.Forward, Rotation) * DRAW_LENGTH)), Color.Aquamarine) },
                        0,  // vertex buffer offset to add to each element of the index buffer
                        2,  // number of vertices in pointList
                        new short[] { 0, 1 },  // the index buffer
                        0,  // first index element to read
                        1   // number of primitives to draw
                        );
                }

                break;
            }

            if (CollisionBase != null)
            {
                CollisionBase.debugDraw(camera);
            }
            else { }

            Space394Game.GameInstance.GraphicsDevice.Viewport = Space394Game.GameInstance.DefaultViewPort;

            Space394Game.GameInstance.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
#endif
            if (Health > 0)
            {
                base.Draw(camera);
            }
            else
            {
                drawExtraModel(camera, destroyedModel);
            }
        }

        public override void DrawReticule(GameCamera camera)
        {
            HUDUnit.Draw(camera, ((PlayerCamera)camera).PlayerShip.Rotation);
        }

        public override void onCollide(SceneObject caller, float damage)
        {
            base.onCollide(caller, damage);

            if (Health <= 0)
            {
                SceneObjectFactory.createMassiveExplosion(Position, Rotation);
            }
            else { }
        }
    }
}
