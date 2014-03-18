using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Space394
{
    class TextureQuad
    {
        public VertexPositionNormalTexture[] Vertices;
        public Vector3 Origin;
        public Vector3 Up;
        public Vector3 Normal;
        public Vector3 Left;
        public Vector3 UpperLeft;
        public Vector3 UpperRight;
        public Vector3 LowerLeft;
        public Vector3 LowerRight;
        public int[] Indexes;

        public TextureQuad(Vector3 origin, Vector3 normal, Vector3 up,
                 float width, float height)
        {
                Vertices = new VertexPositionNormalTexture[4];
                Indexes = new int[6];
                Origin = origin;
                Normal = normal;
                Up = up;

                // Calculate the quad corners
                Left = Vector3.Cross(normal, Up);
                Vector3 uppercenter = (Up * height / 2) + origin;
                UpperLeft = uppercenter + (Left * width / 2);
                UpperRight = uppercenter - (Left * width / 2);
                LowerLeft = UpperLeft - (Up * height);
                LowerRight = UpperRight - (Up * height);

                FillVertices();
        }

        private void FillVertices()
        {
                // Fill in texture coordinates to display full texture
                // on quad
                Vector2 textureUpperLeft = new Vector2(0.0f, 0.0f);
                Vector2 textureUpperRight = new Vector2(1.0f, 0.0f);
                Vector2 textureLowerLeft = new Vector2(0.0f, 1.0f);
                Vector2 textureLowerRight = new Vector2(1.0f, 1.0f);

                // Provide a normal for each vertex
                for (int i = 0; i < this.Vertices.Length; i++)
                {
                        this.Vertices[i].Normal = this.Normal;
                }

                // Set the position and texture coordinate for each
                // vertex
                this.Vertices[0].Position = this.LowerLeft;
                this.Vertices[0].TextureCoordinate = textureLowerLeft;
                this.Vertices[1].Position = this.UpperLeft;
                this.Vertices[1].TextureCoordinate = textureUpperLeft;
                this.Vertices[2].Position = this.LowerRight;
                this.Vertices[2].TextureCoordinate = textureLowerRight;
                this.Vertices[3].Position = this.UpperRight;
                this.Vertices[3].TextureCoordinate = textureUpperRight;

                // Set the index buffer for each vertex, using
                // clockwise winding
                this.Indexes[0] = 0;
                this.Indexes[1] = 1;
                this.Indexes[2] = 2;
                this.Indexes[3] = 2;
                this.Indexes[4] = 1;
                this.Indexes[5] = 3;
        }

    }
}
