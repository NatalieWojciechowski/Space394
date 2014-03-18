using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Space394
{
    public class PanelTexture2D
    {
        private Vector3 position;
        public Vector3 Position
        {
            get { return position; }
            set { position = value; }
        }

        private Rectangle result;
        private Texture2D texture;

        #region /* For TriangleStrip */
        private int width;
        public int getWidth() { return width; }
        public int setWidth(int _width) { width = _width; return width; }

        private int height;
        public int getHeight() { return height; }
        public int setHeight(int _height) { height = _height; return height; }

        private int depth;
        public int getDepth() { return depth; }
        public int setDepth(int _depth) { depth = _depth; return depth; }

        protected VertexPositionColor[] points;
        protected short[] triangleStripIndices;
        protected BasicEffect basicEffect;
        protected VertexDeclaration vertexDeclaration;
        protected VertexPositionColor[] pointList;
        protected VertexBuffer vertexBuffer;
        protected RasterizerState rasterizerState;

        private const int CORNERS = 4;
        private const int EDGES = 4;
        #endregion


        public PanelTexture2D(Texture2D _texture, Vector3 _position)
        {
            texture = _texture;
            position = _position;

            width = 200;
            height = 100;
            depth = 1;

/*            basicEffect = new BasicEffect(Space394Game.GameInstance.GraphicsDevice);
            basicEffect.VertexColorEnabled = true;
            */
            result = new Rectangle();
        }

        public void Draw(GameCamera camera)
        {
            result.X = (int)(position.X); // / (Space394Game.GameInstance.DefaultViewPort.Width / camera.getViewPort().Width));
            result.Y = (int)(position.Y); // / (Space394Game.GameInstance.DefaultViewPort.Height / camera.getViewPort().Height));
            result.Width = texture.Width / (Space394Game.GameInstance.DefaultViewPort.Width / camera.ViewPort.Width);
            result.Height = texture.Height / (Space394Game.GameInstance.DefaultViewPort.Height / camera.ViewPort.Height);

            Space394Game.GameInstance.GraphicsDevice.Viewport = camera.ViewPort;

            SpriteBatch spriteBatch = Space394Game.GameInstance.SpriteBatch;
            spriteBatch.Begin();

            spriteBatch.Draw(texture, result, Color.White);

            spriteBatch.End();

            constructStrip();

/*            foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();

                Space394Game.GameInstance.GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionColor>(
                    PrimitiveType.TriangleStrip,
                    points,
                    0,  // vertex buffer offset to add to each element of the index buffer
                    CORNERS,  // number of vertices in pointList
                    triangleStripIndices,  // the index buffer
                    0,  // first index element to read
                    EDGES - 1   // number of primitives to draw
                    );

                break;
            }
 */
            Space394Game.GameInstance.GraphicsDevice.DepthStencilState = DepthStencilState.Default;

//             Space394Game.GameInstance.GraphicsDevice.Viewport = Space394Game.GameInstance.DefaultViewPort;
        }

        private void constructStrip()
        {
            points = new VertexPositionColor[CORNERS];

            points[0] = new VertexPositionColor(new Vector3(-(width / 2), -(height / 2), -(depth / 2)), Color.Blue); // - - - 0
            points[1] = new VertexPositionColor(new Vector3(-(width / 2), +(height / 2), -(depth / 2)), Color.Blue); // - + - 2
            points[2] = new VertexPositionColor(new Vector3(-(width / 2), -(height / 2), -(depth / 2)), Color.Blue); // - - - 0
            points[3] = new VertexPositionColor(new Vector3(+(width / 2), -(height / 2), -(depth / 2)), Color.Blue); // + - - 4
        }
    }
}
