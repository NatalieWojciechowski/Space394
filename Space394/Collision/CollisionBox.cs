using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BEPUphysics.Entities.Prefabs;
using BEPUphysics.Collidables.MobileCollidables;
using BEPUphysics.Collidables;
using BEPUphysics.Collidables.Events;
using BEPUphysics.EntityStateManagement;
using BEPUphysics.PositionUpdating;

namespace Space394.Collision
{
    // This will be the basic collision model for battleships
    public class CollisionBox : CollisionBase
    {
        private int width;
        public int Width
        {
            get { return width; }
            set { width = value; }
        }

        private int height;
        public int Height
        {
            get { return height; }
            set { height = value; }
        }

        private int depth;
        public int Depth
        {
            get { return depth; }
            set { depth = value; }
        }

        public override Vector3 Position
        {
            set
            {
                base.Position = value;
                boundingBox.Min = new Vector3(Position.X - width / 2, Position.Y - height / 2, Position.Z - depth / 2);
                boundingBox.Max = new Vector3(Position.X + width / 2, Position.Y + height / 2, Position.Z + depth / 2);
                box.Position = value;
            }
        }

        private BoundingBox boundingBox;
        public BoundingBox BoundingBox
        {
            get { return boundingBox; }
        }
        // No setter

        private Box box;

        public override bool Active
        {
            set
            {
#if DEBUG
            if (value)
            {
                recolorCube(Color.Gray);
            }
            else
            {
                recolorCube(Color.Blue);
            }
#endif
            }
        }

#if DEBUG
        private const int CORNERS = 18;
        private const int EDGES = 18;
#endif

        public CollisionBox(Vector3 _position, int _width, int _height, int _depth)
        {
            base.Position = _position;
            width = _width;
            height = _height;
            depth = _depth;

            boundingBox = new BoundingBox(new Vector3(Position.X - width/2, Position.Y - height/2, Position.Z - depth/2),
                                                new Vector3(Position.X + width/2, Position.Y + height/2, Position.Z + depth/2));

            box = new Box(Position, width, height, depth, 1);
            //box.CollisionInformation.Events.InitialCollisionDetected += new InitialCollisionDetectedEventHandler<EntityCollidable>(onCollide);
            box.CollisionInformation.Tag = this;

            box.PositionUpdateMode = PositionUpdateMode.Passive;

            constructCube();
        }

        public override BEPUphysics.ISpaceObject getPhysicsCollider()
        {
            return box;
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
                        if (boundingBox.Intersects(((CollisionBox)collider).BoundingBox)) 
                        { 
                            HasCollided = true; 
                            collider.HasCollided = true;
                        } 
                        else { }
#endif
                        return (boundingBox.Intersects(((CollisionBox)collider).BoundingBox));
                    }
                    else if (collider is CollisionSphere)
                    {
#if DEBUG
                        if (boundingBox.Intersects(((CollisionSphere)collider).BoundingSphere))
                        { 
                            HasCollided = true; 
                            collider.HasCollided = true;
                        } 
                        else { }
#endif
                        return (boundingBox.Intersects(((CollisionSphere)collider).BoundingSphere));
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

        public override void debugDraw(GameCamera camera)
        {
#if DEBUG
            Space394Game.GameInstance.GraphicsDevice.Viewport = camera.ViewPort;

            graphics.basicEffect.World = Matrix.CreateTranslation(Position);
            graphics.basicEffect.View = camera.ViewMatrix;
            graphics.basicEffect.Projection = camera.ProjectionMatrix;

            if (HasCollided && !StaticColor && Active)
            {
                recolorCube(Color.Red);
            }
            else { }

            foreach (EffectPass pass in graphics.basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();

                Space394Game.GameInstance.GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionColor>(
                    PrimitiveType.LineStrip,
                    points,
                    0,  // vertex buffer offset to add to each element of the index buffer
                    CORNERS,  // number of vertices in pointList
                    lineListIndices,  // the index buffer
                    0,  // first index element to read
                    EDGES-1   // number of primitives to draw
                    );

                break;
            }

            if (HasCollided && Active)
            {
                recolorCube(Color.Blue);
                HasCollided = false;
            }
            else { }

            Space394Game.GameInstance.GraphicsDevice.Viewport = Space394Game.GameInstance.DefaultViewPort;

            Space394Game.GameInstance.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
#endif
        }

        private void recolorCube(Color color)
        {
#if DEBUG
            for (int i = 0; i < CORNERS; i++)
            {
                points[i].Color = color;
            }
#endif
        }

        private void constructCube()
        {
#if DEBUG
            points = new VertexPositionColor[CORNERS];

            points[0] = new VertexPositionColor(new Vector3(-(width / 2), -(height / 2), -(depth / 2)), Color.Blue); // - - - 0
            points[1] = new VertexPositionColor(new Vector3(-(width / 2), +(height / 2), -(depth / 2)), Color.Blue); // - + - 2
            points[2] = new VertexPositionColor(new Vector3(-(width / 2), -(height / 2), -(depth / 2)), Color.Blue); // - - - 0
            points[3] = new VertexPositionColor(new Vector3(+(width / 2), -(height / 2), -(depth / 2)), Color.Blue); // + - - 4
            points[4] = new VertexPositionColor(new Vector3(-(width / 2), -(height / 2), -(depth / 2)), Color.Blue); // - - - 0
            points[5] = new VertexPositionColor(new Vector3(-(width / 2), -(height / 2), +(depth / 2)), Color.Blue); // - - + 1
            points[6] = new VertexPositionColor(new Vector3(-(width / 2), +(height / 2), +(depth / 2)), Color.Blue); // - + + 3
            points[7] = new VertexPositionColor(new Vector3(-(width / 2), -(height / 2), +(depth / 2)), Color.Blue); // - - + 1
            points[8] = new VertexPositionColor(new Vector3(+(width / 2), -(height / 2), +(depth / 2)), Color.Blue); // + - + 5
            points[9] = new VertexPositionColor(new Vector3(+(width / 2), +(height / 2), +(depth / 2)), Color.Blue); // + + + 7
            points[10] = new VertexPositionColor(new Vector3(+(width / 2), -(height / 2), +(depth / 2)), Color.Blue); // + - + 5
            points[11] = new VertexPositionColor(new Vector3(+(width / 2), -(height / 2), -(depth / 2)), Color.Blue); // + - - 4
            points[12] = new VertexPositionColor(new Vector3(+(width / 2), +(height / 2), -(depth / 2)), Color.Blue); // + + - 6
            points[13] = new VertexPositionColor(new Vector3(+(width / 2), +(height / 2), +(depth / 2)), Color.Blue); // + + + 7
            points[14] = new VertexPositionColor(new Vector3(+(width / 2), +(height / 2), -(depth / 2)), Color.Blue); // + + - 6
            points[15] = new VertexPositionColor(new Vector3(-(width / 2), +(height / 2), -(depth / 2)), Color.Blue); // - + - 2
            points[16] = new VertexPositionColor(new Vector3(-(width / 2), +(height / 2), +(depth / 2)), Color.Blue); // - + + 3
            points[17] = new VertexPositionColor(new Vector3(+(width / 2), +(height / 2), +(depth / 2)), Color.Blue); // + + + 7

            // Initialize an array of indices of type short.
            lineListIndices = new short[EDGES];

            // Populate the array with references to indices in the vertex buffer.
            for (int i = 0; i < EDGES; i++)
            {
                lineListIndices[i] = (short)(i);
            }
#endif
        }

        public override void addCollisionEvent(InitialCollisionDetectedEventHandler<EntityCollidable> _function)
        {
            box.CollisionInformation.Events.InitialCollisionDetected += _function;
        }
    }
}
