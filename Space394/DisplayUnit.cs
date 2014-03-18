using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Space394
{
    class DisplayUnit
    {
//        GraphicsDeviceManager graphics = Space394Game.GameInstance.GraphicsDevice;
        BasicEffect quadEffect;
        TextureQuad quad; 

        protected RenderTarget2D displayUnit;
        protected const int displayUnitWidth = 50;
        protected const int displayUnitHeight = 50;

        protected Texture2D barsBackground;
        protected Texture2D healthTexture;
        protected Texture2D shieldTexture;

        public Vector3 Origin;
        public Vector3 UpperLeft;
        public Vector3 LowerLeft;
        public Vector3 UpperRight;
        public Vector3 LowerRight;
        public Vector3 Normal;
        public Vector3 Up;
        public Vector3 Left;

        public VertexPositionNormalTexture[] Vertices;
        public int[] Indices;

        DisplayUnit()
        {
            barsBackground = ContentLoadManager.loadTexture("Textures/BackBar");
            healthTexture = ContentLoadManager.loadTexture("Textures/HealthBar");
            shieldTexture = ContentLoadManager.loadTexture("Textures/ShieldBar");

            displayUnit = new RenderTarget2D(Space394Game.GameInstance.GraphicsDevice, displayUnitWidth, displayUnitHeight);
        

        }

        private void createQuad( Vector3 origin, Vector3 normal, Vector3 up, float width, float height )   // MSDN 's Quad
        {
            Vertices = new VertexPositionNormalTexture[4];
            Indices = new int[6];
            Origin = origin;
            Normal = normal;
            Up = up;

            // Calculate the quad corners
            Left = Vector3.Cross( normal, Up );
            Vector3 uppercenter = (Up * height / 2) + origin;
            UpperLeft = uppercenter + (Left * width / 2);
            UpperRight = uppercenter - (Left * width / 2);
            LowerLeft = UpperLeft - (Up * height);
            LowerRight = UpperRight - (Up * height);

            FillVertices();
        }

        private void FillVertices() // MSDN's Quad
        {
            // Fill in texture coordinates to display full texture
            // on quad
            Vector2 textureUpperLeft = new Vector2(0.0f, 0.0f);
            Vector2 textureUpperRight = new Vector2(1.0f, 0.0f);
            Vector2 textureLowerLeft = new Vector2(0.0f, 1.0f);
            Vector2 textureLowerRight = new Vector2(1.0f, 1.0f);

            // Provide a normal for each vertex
            for (int i = 0; i < Vertices.Length; i++)
            {
                Vertices[i].Normal = Normal;
            }

            // Set the position and texture coordinate for each
            // vertex
            Vertices[0].Position = LowerLeft;
            Vertices[0].TextureCoordinate = textureLowerLeft;
            Vertices[1].Position = UpperLeft;
            Vertices[1].TextureCoordinate = textureUpperLeft;
            Vertices[2].Position = LowerRight;
            Vertices[2].TextureCoordinate = textureLowerRight;
            Vertices[3].Position = UpperRight;
            Vertices[3].TextureCoordinate = textureUpperRight;

            // Set the index buffer for each vertex, using
            // clockwise winding
            Indices[0] = 0;
            Indices[1] = 1;
            Indices[2] = 2;
            Indices[3] = 2;
            Indices[4] = 1;
            Indices[5] = 3;
        }

        public void Draw(GameCamera camera)
        {
//            DrawQuad();
            quad = new TextureQuad(Vector3.Zero, Vector3.UnitZ, Vector3.Up, 2, 2);

            quadEffect = new BasicEffect(Space394Game.GameInstance.GraphicsDevice);
            quadEffect.AmbientLightColor = new Vector3(0.8f, 0.8f, 0.8f);
            quadEffect.LightingEnabled = true;
            quadEffect.World = Matrix.Identity;
/*            quadEffect.View = Space394Game.GameInstance.CurrentScene.getCamera().getTarget(0;
            quadEffect.Projection = this.Projection;
            quadEffect.TextureEnabled = true;
            quadEffect.Texture = someTexture; */



            SpriteBatch spriteBatch = Space394Game.GameInstance.SpriteBatch;
            spriteBatch.Begin();
            spriteBatch.Draw(barsBackground, Vector2.Zero, Color.White);
//            spriteBatch.Draw(healthTexture, GameWindow
            spriteBatch.Draw(barsBackground, Vector2.Zero, Color.White);
            spriteBatch.End();
            Space394Game.GameInstance.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
        }
        ////////////// http://msdn.microsoft.com/en-us/library/bb464051(v=xnagamestudio.10).aspx
/*        private void DrawQuad()
        {
            Space394Game.GameInstance.GraphicsDevice.VertexDeclaration = quadVertexDecl;
            quadEffect.Begin();
            foreach (EffectPass pass in quadEffect.CurrentTechnique.Passes)
            {
                pass.Begin();

                graphics.GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionNormalTexture>(
                    PrimitiveType.TriangleList, quad.Vertices, 0, 4, quad.Indices, 0, 2);

                pass.End();
            }
            quadEffect.End();
        } */
    }
}
