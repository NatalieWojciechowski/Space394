using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Space394.Particles
{
    public abstract class Particle
    {
        private Vector3 position;
        public Vector3 Position
        {
            get { return position; }
            set { position = value; }
        }

        private Quaternion rotation;
        public Quaternion Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }

        private Vector3 up;
        public Vector3 Up
        {
            get { return up; }
            set { up = value; }
        }

        protected Texture2D texture;

        private float startWidth;
        public float StartWidth
        {
            get { return startWidth; }
            set { startWidth = value; }
        }

        private float startHeight;
        public float StartHeight
        {
            get { return startHeight; }
            set { startHeight = value; }
        }

        private float startDepth;
        public float StartDepth
        {
            get { return startDepth; }
            set { startDepth = value; }
        }

        private float width;
        public float Width
        {
            get { return width; }
            set { width = value; }
        }

        private float height;
        public float Height
        {
            get { return height; }
            set { height = value; }
        }

        private float depth;
        public float Depth
        {
            get { return depth; }
            set { depth = value; }
        }

        public void resetScale()
        {
            width = startWidth;
            height = startHeight;
            depth = startDepth;
            vertices[0].Position = new Vector3(+width, +height, 0);
            vertices[1].Position = new Vector3(-width, +height, 0);
            vertices[2].Position = new Vector3(+width, -height, 0);
            vertices[3].Position = new Vector3(-width, -height, 0);
        }

        public void setDefaultScale(float scale)
        {
            width = startWidth = (width * scale);
            height = startHeight = (height * scale);
            depth = startDepth = (depth * scale);
            vertices[0].Position = new Vector3(+width, +height, 0);
            vertices[1].Position = new Vector3(-width, +height, 0);
            vertices[2].Position = new Vector3(+width, -height, 0);
            vertices[3].Position = new Vector3(-width, -height, 0);
        }

        private float scale = 1;
        public float Scale
        {
            get { return scale; }
            set
            {
                scale = value;
                width = (width * scale);
                height = (height * scale);
                depth = (depth * scale);
                vertices[0].Position = new Vector3(+width, +height, 0);
                vertices[1].Position = new Vector3(-width, +height, 0);
                vertices[2].Position = new Vector3(+width, -height, 0);
                vertices[3].Position = new Vector3(-width, -height, 0);
            }
        }

        protected float alpha = 1.0f;

        protected VertexPositionColor[] points;
        protected short[] lineListIndices;

        protected VertexPositionColor[] pointList;
        protected VertexPositionColorTexture[] vertices;
        protected VertexBuffer vertexBuffer;

        private bool queuedRemoval = false;
        public bool QueuedRemoval
        {
            get { return queuedRemoval; }
            set { queuedRemoval = value; }
        }

        public struct textureGraphics
        {
            public static bool initialized = false;
            public static BasicEffect basicEffect;
            public static RasterizerState rasterizerState;
            public static VertexDeclaration vertexDeclaration;
        };

        protected const int CORNERS = 18;
        protected const int EDGES = 18;

        public Particle(String _texture, Vector3 _position)
        {
            texture = ContentLoadManager.loadTexture(_texture);
            position = _position;

            width = texture.Width;
            height = texture.Height;
            depth = 0;

            startWidth = width;
            startHeight = height;

            if (!textureGraphics.initialized)
            {
                textureGraphics.rasterizerState = new RasterizerState();
                textureGraphics.rasterizerState.FillMode = FillMode.WireFrame;
                textureGraphics.rasterizerState.CullMode = CullMode.None;

                textureGraphics.vertexDeclaration = new VertexDeclaration(new VertexElement[]
                    {
                        new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
                        new VertexElement(12, VertexElementFormat.Color, VertexElementUsage.Color, 0)
                    }
                );

                textureGraphics.basicEffect = new BasicEffect(Space394Game.GameInstance.GraphicsDevice);
                textureGraphics.basicEffect.VertexColorEnabled = true;

                textureGraphics.basicEffect.VertexColorEnabled = false;
                textureGraphics.basicEffect.LightingEnabled = false;
                textureGraphics.basicEffect.TextureEnabled = true;

                textureGraphics.initialized = true;
            }
            else { }

            // Preallocate an array of four vertices.
            vertices = new VertexPositionColorTexture[4];

            // Update our vertex array to use the specified number of texture repeats.
            vertices[0].TextureCoordinate = new Vector2(0, 0);
            vertices[1].TextureCoordinate = new Vector2(1, 0);
            vertices[2].TextureCoordinate = new Vector2(0, 1);
            vertices[3].TextureCoordinate = new Vector2(1, 1);

            vertices[0].Position = new Vector3(+width, +height, 0);
            vertices[1].Position = new Vector3(-width, +height, 0);
            vertices[2].Position = new Vector3(+width, -height, 0);
            vertices[3].Position = new Vector3(-width, -height, 0);

            rotation = Quaternion.Identity;
            up = Vector3.Up;
        }         

        public abstract void Update(float deltaTime);

        public virtual void Draw(GameCamera camera)
        {
            // Set our effect to use the specified texture and camera matrices.
            textureGraphics.basicEffect.Texture = texture;
            textureGraphics.basicEffect.Alpha = alpha;

            textureGraphics.basicEffect.World = Matrix.CreateBillboard(position, camera.Position, Vector3.Cross(camera.Up, camera.Target), camera.Target);
            textureGraphics.basicEffect.View = camera.ViewMatrix;
            textureGraphics.basicEffect.Projection = camera.ProjectionMatrix;

            foreach (EffectPass pass in textureGraphics.basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();

                Space394Game.GameInstance.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, vertices, 0, 2);
            }
        }

        public virtual void Draw(GameCamera camera, Matrix world)
        {
            // Set our effect to use the specified texture and camera matrices.
            Space394Game.GameInstance.GraphicsDevice.Viewport = camera.ViewPort;

            textureGraphics.basicEffect.Texture = texture;
            textureGraphics.basicEffect.Alpha = alpha;

            textureGraphics.basicEffect.World = world * Matrix.CreateTranslation(position);
            textureGraphics.basicEffect.View = camera.ViewMatrix;
            textureGraphics.basicEffect.Projection = camera.ProjectionMatrix;

            foreach (EffectPass pass in textureGraphics.basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                Space394Game.GameInstance.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, vertices, 0, 2);
            }
        }
    }
}
