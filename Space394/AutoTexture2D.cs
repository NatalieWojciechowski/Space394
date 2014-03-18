using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Space394
{
    public class AutoTexture2D
    {
        private Vector2 position;
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        private Rectangle result;

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

        private Texture2D texture;
        public Texture2D Texture
        {
            get { return texture; }
        }

        private Color color;
        public Color Color
        {
            get { return color; }
            set { color = value; }
        }
        public byte setAlpha(byte alpha) { return color.A = alpha; }

        private static float widthConversion;
        public static float WidthConversion
        {
            get { return AutoTexture2D.widthConversion; }
            set { AutoTexture2D.widthConversion = value; }
        }

        private static float heightConversion;
        public static float HeightConversion
        {
            get { return AutoTexture2D.heightConversion; }
            set { AutoTexture2D.heightConversion = value; }
        }

        public AutoTexture2D(Texture2D _texture, Vector2 _position)
        {
            width = 1;
            height = 1;
            texture = _texture;
            position = _position;
            result = new Rectangle();
            color = Color.White;
        }

        public void Draw(GameCamera camera)
        {
            result.X = (int)(position.X / (Space394Game.GameInstance.DefaultViewPort.Width / camera.ViewPort.Width));
            result.Y = (int)(position.Y / (Space394Game.GameInstance.DefaultViewPort.Height / camera.ViewPort.Height));
            result.Width = (int)(texture.Width / (Space394Game.GameInstance.DefaultViewPort.Width / camera.ViewPort.Width) * width);
            result.Height = (int)(texture.Height / (Space394Game.GameInstance.DefaultViewPort.Height / camera.ViewPort.Height) * height);

            Space394Game.GameInstance.GraphicsDevice.Viewport = camera.ViewPort;

            SpriteBatch spriteBatch = Space394Game.GameInstance.SpriteBatch;
            spriteBatch.Begin();

            spriteBatch.Draw(texture, result, color);

            spriteBatch.End();

            Space394Game.GameInstance.GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            Space394Game.GameInstance.GraphicsDevice.Viewport = Space394Game.GameInstance.DefaultViewPort;
        }

        public void DrawAlreadyBegun(GameCamera camera)
        {
            result.X = (int)(position.X / (Space394Game.GameInstance.DefaultViewPort.Width / camera.ViewPort.Width));
            result.Y = (int)(position.Y / (Space394Game.GameInstance.DefaultViewPort.Height / camera.ViewPort.Height));
            result.Width = (int)(texture.Width / (Space394Game.GameInstance.DefaultViewPort.Width / camera.ViewPort.Width) * width);
            result.Height = (int)(texture.Height / (Space394Game.GameInstance.DefaultViewPort.Height / camera.ViewPort.Height) * height);

            SpriteBatch spriteBatch = Space394Game.GameInstance.SpriteBatch;

            spriteBatch.Draw(texture, result, color);

            Space394Game.GameInstance.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            Space394Game.GameInstance.GraphicsDevice.Viewport = camera.ViewPort;
        }

        public void DrawMaintainRatio(GameCamera camera)
        {
            int divisor = (Space394Game.GameInstance.DefaultViewPort.Height / camera.ViewPort.Height);
            result.X = (int)((position.X / (Space394Game.GameInstance.DefaultViewPort.Width / camera.ViewPort.Width)) * widthConversion);
            result.Y = (int)((position.Y / (Space394Game.GameInstance.DefaultViewPort.Height / camera.ViewPort.Height)) * heightConversion);
            result.Width = (int)((texture.Width / divisor * width) * widthConversion);
            result.Height = (int)((texture.Height / divisor * height) * heightConversion);

            Space394Game.GameInstance.GraphicsDevice.Viewport = camera.ViewPort;

            SpriteBatch spriteBatch = Space394Game.GameInstance.SpriteBatch;
            spriteBatch.Begin();

            spriteBatch.Draw(texture, result, color);

            spriteBatch.End();

            Space394Game.GameInstance.GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            Space394Game.GameInstance.GraphicsDevice.Viewport = Space394Game.GameInstance.DefaultViewPort;
        }

        public void DrawAlreadyBegunMaintainRatio(GameCamera camera)
        {
            int divisor = (Space394Game.GameInstance.DefaultViewPort.Height / camera.ViewPort.Height);
            result.X = (int)((position.X / (Space394Game.GameInstance.DefaultViewPort.Width / camera.ViewPort.Width)) * widthConversion);
            result.Y = (int)((position.Y / (Space394Game.GameInstance.DefaultViewPort.Height / camera.ViewPort.Height)) * heightConversion);
            result.Width = (int)((texture.Width / divisor * width) * widthConversion);
            result.Height = (int)((texture.Height / divisor * height) * heightConversion);

            SpriteBatch spriteBatch = Space394Game.GameInstance.SpriteBatch;

            spriteBatch.Draw(texture, result, color);

            Space394Game.GameInstance.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            Space394Game.GameInstance.GraphicsDevice.Viewport = camera.ViewPort;
        }

        public void DrawAlreadyBegunMaintainRatio()
        {
            result.X = (int)((position.X) * widthConversion);
            result.Y = (int)((position.Y) * heightConversion);
            result.Width = (int)((texture.Width) * widthConversion);
            result.Height = (int)((texture.Height) * heightConversion);

            SpriteBatch spriteBatch = Space394Game.GameInstance.SpriteBatch;

            spriteBatch.Draw(texture, result, color);

            Space394Game.GameInstance.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
        }
    }
}
