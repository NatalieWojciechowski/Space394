using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BEPUphysics.Collidables;
using BEPUphysics.DataStructures;
using BEPUphysics.MathExtensions;
using BEPUphysics.Collidables.MobileCollidables;
using BEPUphysics.NarrowPhaseSystems.Pairs;
using BEPUphysics.Collidables.Events;
using BEPUphysics.Entities;
using BEPUphysics.CollisionShapes;
using BEPUphysics.Entities.Prefabs;
using BEPUphysics.PositionUpdating;
using BEPUphysics.CollisionRuleManagement;

namespace Space394.Collision
{
    public class CollisionMesh : CollisionBase
    {
        //private ConvexHull meshCollider;
        private MobileMesh meshCollider;
        private Model model;

        private Quaternion rotation;
        public Quaternion Rotation
        {
            get { return rotation; }
            set
            {
                rotation = value;
                meshCollider.Orientation = value;
            }
        }

        public override Vector3 Position
        {
            set
            {
                base.Position = value;
                meshCollider.Position = value;
            }
        }

        private Vector3 visibilityScale;
        private const float RESCALE = 1.000f;

        /*private static CollisionGroup meshGroup;
        public static CollisionGroup MeshGroup
        {
            get 
            {
                if (!groupSet)
                {
                    meshGroup = new CollisionGroup();
                    CollisionGroupPair pair = new CollisionGroupPair(meshGroup, meshGroup);
                    CollisionRules.CollisionGroupRules.Add(pair, CollisionRule.NoBroadPhase);
                    groupSet = true;
                }
                else { }
                return meshGroup; 
            }
        }
        private static bool groupSet = false;*/

#if DEBUG
        private static RasterizerState wireFrame = null;
#endif

        public CollisionMesh(Model _model, Vector3 _position, Quaternion _rotation)
        {
            model = _model;
            base.Position = _position;
            rotation = _rotation;
            Vector3[] vertices;
            int[] indices;
            TriangleMesh.GetVerticesAndIndicesFromModel(_model, out vertices, out indices);
            AffineTransform transform = new AffineTransform(Quaternion.Identity, Vector3.Zero);
            meshCollider = new MobileMesh(vertices, indices, transform, MobileMeshSolidity.Counterclockwise);
            //meshCollider = new ConvexHull(vertices);
            meshCollider.CollisionInformation.Tag = this;
            meshCollider.CollisionInformation.LocalPosition = meshCollider.Position;
            meshCollider.Position = _position;
            meshCollider.Orientation = _rotation;
            meshCollider.PositionUpdateMode = PositionUpdateMode.Passive;

            /*if (!groupSet)
            {
                meshGroup = new CollisionGroup();
                CollisionGroupPair pair = new CollisionGroupPair(meshGroup, meshGroup);
                CollisionRules.CollisionGroupRules.Add(pair, CollisionRule.NoBroadPhase);
                groupSet = true;
            }
            else { }*/

            // meshCollider.CollisionInformation.CollisionRules.Group = meshGroup;

#if DEBUG
            if (wireFrame == null)
            {
                wireFrame = new RasterizerState();
                wireFrame.FillMode = FillMode.WireFrame;
            }
            else { }
#endif

            visibilityScale = new Vector3(RESCALE, RESCALE, RESCALE);
        }

        public override BEPUphysics.ISpaceObject getPhysicsCollider()
        {
            return meshCollider;
        }

        public override bool isColliding(CollisionBase collider)
        {
            bool result = false;
            if (collider is CollisionSphere)
            {
                
            }
            else if (collider is CollisionBox)
            {
                
            }
            else if (collider is CollisionMesh)
            {
                
            }
            else { }

            return result;
        }

        public override void debugDraw(GameCamera camera)
        {
#if DEBUG
            Space394Game.GameInstance.GraphicsDevice.Viewport = camera.ViewPort;
            RasterizerState defaultState = Space394Game.GameInstance.GraphicsDevice.RasterizerState;

            Space394Game.GameInstance.GraphicsDevice.RasterizerState = wireFrame;

            //modelDrawer.Draw(camera.getViewMatrix(), camera.getProjectionMatrix());

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
                        * Matrix.CreateScale(visibilityScale)
                        * Matrix.CreateFromQuaternion(rotation)
                        * Matrix.CreateTranslation(Position);

                    effect.View = camera.ViewMatrix;
                    effect.Projection = camera.ProjectionMatrix;
                }
                // Draw the mesh, using the effects set above.
                mesh.Draw();
            }

            Space394Game.GameInstance.GraphicsDevice.RasterizerState = defaultState;
            Space394Game.GameInstance.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            Space394Game.GameInstance.GraphicsDevice.Viewport = Space394Game.GameInstance.DefaultViewPort;
#endif
        }

        public override void addCollisionEvent(InitialCollisionDetectedEventHandler<EntityCollidable> _function)
        {
            meshCollider.CollisionInformation.Events.InitialCollisionDetected += _function;
        }
    }
}
