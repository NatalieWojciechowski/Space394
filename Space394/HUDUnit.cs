using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Space394.SceneObjects;

namespace Space394
{
    public class HUDUnit
    {
        private TextureQuad quad;
        VertexDeclaration vertexDeclaration;

        private Ship ship;
        private Vector3 position;
        public Vector3 Position
        {
            get { return position; }
            set { position = value; }
        }
        private Quaternion rotation;
        private Vector3 up;

        private Texture2D shieldBarTex;
        private Texture2D healthBarTex;
        private Texture2D barBackgroundTex;
        private Texture2D enemyMarkerTex;
        private Texture2D friendlyMarkerTex;
        private Texture2D objectiveMarkerTex;

        private int barMaxWidth;

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

        protected VertexPositionColor[] points;
        protected short[] lineListIndices;

        protected VertexPositionColor[] pointList;
        VertexPositionColorTexture[] vertices;
        protected VertexBuffer vertexBuffer;
        AlphaTestEffect alphaEffect;

        protected struct barGraphics
        {
            public static bool initialized = false;
            public static BasicEffect basicEffect;
            public static RasterizerState rasterizerState;
            public static VertexDeclaration vertexDeclaration;
        };

        private const int CORNERS = 18;
        private const int EDGES = 18;

        private const float RETICLE_RESIZE = 2.0f;

        private const float SCALE_FACTOR = 0.001f;
        private const float MIN_SCALE = 1.0f;
        private const float MAX_SCALE = 5000000000.0f;

        private int modelLevel = 0;
        public int ModelLevel
        {
            get { return modelLevel; }
            set { modelLevel = value; }
        }

        public HUDUnit(Ship _ship)
        {
            ship = _ship;
        }

        public HUDUnit(Ship _ship, int _width, int _height, int _depth)
        {
            if (!barGraphics.initialized)
            {
                barGraphics.rasterizerState = new RasterizerState();
                barGraphics.rasterizerState.FillMode = FillMode.WireFrame;
                barGraphics.rasterizerState.CullMode = CullMode.None;

                barGraphics.vertexDeclaration = new VertexDeclaration(new VertexElement[]
                    {
                        new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
                        new VertexElement(12, VertexElementFormat.Color, VertexElementUsage.Color, 0)
                    }
                );

                barGraphics.basicEffect = new BasicEffect(Space394Game.GameInstance.GraphicsDevice);
                barGraphics.basicEffect.VertexColorEnabled = true;

                barGraphics.initialized = true;
            }
            else { }

            ship = _ship;

            width = _width;
            height = _height;
            depth = _depth;

            shieldBarTex = ContentLoadManager.loadTexture("Textures/ShieldBar");
            healthBarTex = ContentLoadManager.loadTexture("Textures/HealthBar");
            barBackgroundTex = ContentLoadManager.loadTexture("Textures/BackBar");
            barMaxWidth = barBackgroundTex.Width;

            enemyMarkerTex = ContentLoadManager.loadTexture("Textures/enemy_marker");
            friendlyMarkerTex = ContentLoadManager.loadTexture("Textures/friendly_marker");
            objectiveMarkerTex = ContentLoadManager.loadTexture("Textures/objective_marker");

            alphaEffect = new AlphaTestEffect(Space394Game.GameInstance.GraphicsDevice);

            alphaEffect.AlphaFunction = CompareFunction.Greater;
            alphaEffect.ReferenceAlpha = 128;

            // Preallocate an array of four vertices.
            vertices = new VertexPositionColorTexture[4];

            double totalScale = 0.075;
            int hudScaleX = ((int)(20 * totalScale));
            int hudScaleY = ((int)(4 * totalScale));
            vertices[0].Position = new Vector3(hudScaleX, hudScaleY, 0);
            vertices[1].Position = new Vector3(-hudScaleX, hudScaleY, 0);
            vertices[2].Position = new Vector3(hudScaleX, -hudScaleY, 0);
            vertices[3].Position = new Vector3(-hudScaleX, -hudScaleY, 0);

            rotation = ship.Rotation * Quaternion.CreateFromYawPitchRoll(0, MathHelper.ToRadians(90f), 0);
            up = Vector3.Transform(Vector3.Up, rotation);

            quad = new TextureQuad(Vector3.Zero, Vector3.Backward, Vector3.Up, 1, 1);
        }

        public void updateFromShip()
        {
            if (!(ship is SpawnShipPiece))
            {
                position = ship.Position;
            }
            else { }
        }

        public void DrawReticleOnly(GameCamera camera, Quaternion playerShipRotation)
        {
            if (ship.Active && ship != ((PlayerCamera)camera).PlayerShip)
            {
                float scaleSize = getScaleByDistance(camera);

                Space394Game.GameInstance.GraphicsDevice.Viewport = camera.ViewPort;

                Space394Game.GameInstance.GraphicsDevice.DepthStencilState = DepthStencilState.None;

                barGraphics.basicEffect.World = Matrix.CreateFromQuaternion(rotation) *
                                Matrix.CreateTranslation(position);
                barGraphics.basicEffect.View = camera.ViewMatrix;
                barGraphics.basicEffect.Projection = camera.ProjectionMatrix;

                foreach (EffectPass pass in barGraphics.basicEffect.CurrentTechnique.Passes)
                {
                    pass.Apply();
                    barGraphics.basicEffect.World = 
                        Matrix.CreateScale(scaleSize) * 
                        Matrix.CreateFromQuaternion(rotation) *
                        Matrix.CreateTranslation(position + offsetFromShip(2, playerShipRotation));
                    DrawQuad(getMarkerTex(camera), barGraphics.basicEffect.World, barGraphics.basicEffect.View, barGraphics.basicEffect.Projection, 1);
                    break;
                }

                Space394Game.GameInstance.GraphicsDevice.Viewport = Space394Game.GameInstance.DefaultViewPort;

                Space394Game.GameInstance.GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            }
        }

        public void DrawRescaling(GameCamera camera, Quaternion playerShipRotation)
        {
            if (ship.Active && ship != ((PlayerCamera)camera).PlayerShip)
            {
                Space394Game.GameInstance.GraphicsDevice.Viewport = camera.ViewPort;

                Space394Game.GameInstance.GraphicsDevice.DepthStencilState = DepthStencilState.None;

                float scale = Math.Max(MIN_SCALE, Math.Min(Vector3.Distance(ship.Position, camera.Position) * SCALE_FACTOR, MAX_SCALE));

                barGraphics.basicEffect.View = camera.ViewMatrix;
                barGraphics.basicEffect.Projection = camera.ProjectionMatrix;

                foreach (EffectPass pass in barGraphics.basicEffect.CurrentTechnique.Passes)
                {
                    pass.Apply();
                    barGraphics.basicEffect.World = Matrix.CreateScale(scale) *
                        Matrix.CreateFromQuaternion(rotation) *
                        Matrix.CreateTranslation(position + offsetFromShip(2, playerShipRotation));
                    DrawQuad(getMarkerTex(camera), barGraphics.basicEffect.World, barGraphics.basicEffect.View, barGraphics.basicEffect.Projection, 1);
                }

                Space394Game.GameInstance.GraphicsDevice.Viewport = Space394Game.GameInstance.DefaultViewPort;

                Space394Game.GameInstance.GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            }
        }

        public void Draw(GameCamera camera, Quaternion playerShipRotation)
        {
            if (ship.Active && ship != ((PlayerCamera)camera).PlayerShip)
            {
                Space394Game.GameInstance.GraphicsDevice.Viewport = camera.ViewPort;

                Space394Game.GameInstance.GraphicsDevice.DepthStencilState = DepthStencilState.None;

                barGraphics.basicEffect.World = Matrix.CreateFromQuaternion(rotation) *
                                Matrix.CreateTranslation(position); //  + offsetFromShip(0,playerShipRotation));
                barGraphics.basicEffect.View = camera.ViewMatrix;
                barGraphics.basicEffect.Projection = camera.ProjectionMatrix;

                foreach (EffectPass pass in barGraphics.basicEffect.CurrentTechnique.Passes)
                {
                    pass.Apply();
                    #region old Code, just in case
                    /*                    Space394Game.GameInstance.GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionColor>(
                        PrimitiveType.LineStrip,
                        points,
                        0,  // vertex buffer offset to add to each element of the index buffer
                        CORNERS,  // number of vertices in pointList
                        lineListIndices,  // the index buffer
                        0,  // first index element to read
                        EDGES - 1   // number of primitives to draw
                        ); */

/*                    Space394Game.GameInstance.GraphicsDevice.DrawUserIndexedPrimitives
                        <VertexPositionNormalTexture>(
                        PrimitiveType.TriangleList,
                        quad.Vertices, 0, 4,
                        quad.Indexes, 0, 2); */


//                      shieldBarTex.setPosition(new Vector2((getPosition() + offsetFromShip()).X, (getPosition() + offsetFromShip()).Y));
//                      SpriteBatch spriteBatch = Space394Game.GameInstance.SpriteBatch;
//                      spriteBatch.Begin();
//                      shieldBarTex.DrawAlreadyBegunMaintainRatio(camera);
                    /*                    barGraphics.basicEffect.World = Matrix.CreateBillboard(position, camera.getPosition(), camera.getUpVect(), camera.getRotation());  /* Matrix.CreateFromQuaternion(rotation) *
                    //                                Matrix.CreateTranslation(position + offsetFromShip(0,playerShipRotation)); //getRotation())); */
                    #endregion 
                    /* Other-> Typically Objective Marker currently */
                    barGraphics.basicEffect.World = Matrix.CreateFromQuaternion(rotation) *
                        Matrix.CreateTranslation(position + offsetFromShip(2, playerShipRotation));
                    DrawQuad(getMarkerTex(camera), barGraphics.basicEffect.World, barGraphics.basicEffect.View, barGraphics.basicEffect.Projection, 1); 

                    // Shield Bar
                    barGraphics.basicEffect.World = Matrix.CreateFromQuaternion(rotation) *
                         Matrix.CreateTranslation(position + offsetFromShip(0, playerShipRotation));
                    DrawQuad(shieldBarTex, barGraphics.basicEffect.World, barGraphics.basicEffect.View, barGraphics.basicEffect.Projection, 1);

                    // Health Bar
                    barGraphics.basicEffect.World = Matrix.CreateFromQuaternion(rotation) *
                         Matrix.CreateTranslation(position + offsetFromShip(1, playerShipRotation));
                    DrawQuad(healthBarTex, barGraphics.basicEffect.World, barGraphics.basicEffect.View, barGraphics.basicEffect.Projection, 1);

                    break;
                }

                Space394Game.GameInstance.GraphicsDevice.Viewport = Space394Game.GameInstance.DefaultViewPort;

                Space394Game.GameInstance.GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            }
        }

        public float getScaleByDistance(GameCamera _camera)
        {
            float newScale = RETICLE_RESIZE;
            // Furtheset
            if (modelLevel == 2)
            {
                newScale *= 1.25f;
            }
            // Close
            else if (modelLevel == 1)
            {
                newScale *= 0.25f;
            }
            // Not Drawn Anyways
            //else if (modelLevel == 0)
            //{ }
            else { }

            return newScale;
        }

        public Vector3 offsetFromShip(int _set, Quaternion camQuaternion)
        {
            rotation = camQuaternion;
            up = Vector3.Transform(Vector3.Up, rotation);
            if (ship is CapitalShip || ship is SpawnShipPiece)
            {
                if (ship.ShipTeam == Ship.Team.Esxolus)
                {
                    if (_set == 0)
                    { // Shield
                        return up * 1000.0f;//4000.1f;
                    }
                    else if (_set == 1)
                    { // Health
                        return up * 900.0f;//3800.0f;
                    }
                    else
                        return Vector3.Zero;// up * 0.1f;
                }
                else    // Halk
                {
                    if (_set == 0)
                    { // Shield
                        return up * 1000.0f;//2700.1f;
                    }
                    else if (_set == 1)
                    { // Health
                        return up * 900.0f;//2500.0f;
                    }
                    else
                        return Vector3.Zero;
                }
            }
            else
            {
                if (_set == 0)
                { // Shield
                    return up * 33.0f;
                }
                else if (_set == 1)
                { // Health
                    return up * 25.0f;
                }
                else if (_set == 2) // markers (Friendly / Enemy)
                {
                    return up * 0.1f;
                }
                    return Vector3.Zero;
            }
        }

        #region older offest Functions
        public Vector3 offsetFromShip(int _set, Vector3 camUp)
        {
            rotation = ship.Rotation; 
            up = Vector3.Transform(Vector3.Up, rotation);

            if (_set == 0)
            { // Shield
                return up * 12.1f;
            }
            else if (_set == 1)
            { // Health
                return up * 10.0f;
            }
            else
                return Vector3.Zero;
        }

        public Vector3 offsetFromShip(int _set, Matrix camViewMatrix)
        {
            rotation = Quaternion.CreateFromRotationMatrix(camViewMatrix);
            up = Vector3.Transform(Vector3.Up, rotation);

            if (_set == 0)
            { // Shield
                return up * 12.1f;
            }
            else if (_set == 1)
            { // Health
                return up * 10.0f;
            }
            else
                return Vector3.Zero;
        }
        #endregion 

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

        public void DrawQuad(Texture2D texture, Matrix world, Matrix view, Matrix projection, float textureRepeats)
        {
            vertexDeclaration = new VertexDeclaration(new VertexElement[]
            {
                new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
                new VertexElement(12, VertexElementFormat.Vector3, VertexElementUsage.Normal, 0),
                new VertexElement(24, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0)
            }
            );

            // Set our effect to use the specified texture and camera matrices.
            alphaEffect.Texture = texture;
            barGraphics.basicEffect.Texture = texture;

            // Update the width of the bars for shield and health
            updateTexWidth(texture);

            alphaEffect.World = world;
            alphaEffect.View = view;
            alphaEffect.Projection = projection;

            // Update our vertex array to use the specified number of texture repeats.
            vertices[0].TextureCoordinate = new Vector2(0, 0);
            vertices[1].TextureCoordinate = new Vector2(1*textureRepeats, 0);
            vertices[2].TextureCoordinate = new Vector2(0, 1*textureRepeats);
            vertices[3].TextureCoordinate = new Vector2(1*textureRepeats, 1*textureRepeats);

 
            // Draw the quad.
            barGraphics.basicEffect.CurrentTechnique.Passes[0].Apply();
            barGraphics.basicEffect.VertexColorEnabled = false;
            barGraphics.basicEffect.LightingEnabled = false;
            barGraphics.basicEffect.TextureEnabled = true;
            Space394Game.GameInstance.GraphicsDevice.DepthStencilState = DepthStencilState.None;
            Space394Game.GameInstance.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, vertices, 0, 2);
            Space394Game.GameInstance.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
        }

        public void updateTexWidth(Texture2D _tex)
        {
            double totalScale = 1.00;
            if (ship is CapitalShip || ship is SpawnShipPiece)
            {
                totalScale = 20.0;
            }
            else { }

            #region NOTES ABOUT VERTICES / SCALE
            // Vertices build in a strange order, and form backwards
            // Back:
            //   +scale   0     -scale
            //      1-----------0
            //      3-----------2
            //
            // Front: 
            //   -scale   0   +scale
            //      0-----------1
            //      2-----------3
            #endregion

            int hudScaleX = ((int)(25 * totalScale));
            int hudScaleY = ((int)(4 * totalScale));
            float shieldWidthMod = 1.0f;
            float healthWidthMod = 1.0f;

            if (_tex == shieldBarTex)
            {
                shieldWidthMod = (ship.Shields / ship.MaxShields);
                if (ship.Shields <= 0.5)
                {
                    shieldWidthMod = 0.0f;
                }
                else { }

                shieldWidthMod = (shieldWidthMod - 0.5f) / 0.5f;
                vertices[0].Position = new Vector3(hudScaleX, hudScaleY, 0);
                vertices[1].Position = new Vector3(-hudScaleX * (shieldWidthMod), hudScaleY, 0);
                vertices[2].Position = new Vector3(hudScaleX, -hudScaleY, 0);
                vertices[3].Position = new Vector3(-hudScaleX * (shieldWidthMod), -hudScaleY, 0);
            }
            else if (_tex == healthBarTex)
            {
                healthWidthMod = ((ship.Health / ship.MaxHealth));
                if (ship.Health <= 0.5)
                {
                    healthWidthMod = 0.0f;
                }
                else { }

                healthWidthMod = (healthWidthMod - 0.5f) / 0.5f;
                vertices[0].Position = new Vector3(hudScaleX, hudScaleY, 0);
                vertices[1].Position = new Vector3(-hudScaleX * (healthWidthMod), hudScaleY, 0);
                vertices[2].Position = new Vector3(hudScaleX, -hudScaleY, 0);
                vertices[3].Position = new Vector3(-hudScaleX * (healthWidthMod), -hudScaleY, 0);
            }
            else /*if (_tex == enemyMarkerTex ||
                     _tex == friendlyMarkerTex)*/
            {
                /*if (ship is CapitalShip) //|| ship is SpawnShipPiece)
                {
                    totalScale = 40.0;
                }
                else if (ship is SpawnShipPiece)
                {
                    totalScale = 6.0f;
                }
                else
                {*/
                if (ship is Battleship)
                {
                    totalScale = 2.0f;
                }
                else
                {
                    totalScale = 1.0f;
                }
                //}

                hudScaleX = ((int)(20 * totalScale));
                hudScaleY = ((int)(20 * totalScale));

                vertices[0].Position = new Vector3(hudScaleX, hudScaleY, 0);
                vertices[1].Position = new Vector3(-hudScaleX, hudScaleY, 0);
                vertices[2].Position = new Vector3(hudScaleX, -hudScaleY, 0);
                vertices[3].Position = new Vector3(-hudScaleX, -hudScaleY, 0);
            }
            /*else if (_tex == objectiveMarkerTex)
            {
                totalScale = 20.0;

                hudScaleX = ((int)(20 * totalScale));
                hudScaleY = ((int)(20 * totalScale));

                vertices[0].Position = new Vector3(hudScaleX, hudScaleY, 0);
                vertices[1].Position = new Vector3(-hudScaleX, hudScaleY, 0);
                vertices[2].Position = new Vector3(hudScaleX, -hudScaleY, 0);
                vertices[3].Position = new Vector3(-hudScaleX, -hudScaleY, 0);
            }
            else
            {
                hudScaleX = ((int)(20 * totalScale));
                hudScaleY = ((int)(4 * totalScale));

                vertices[0].Position = new Vector3(hudScaleX, hudScaleY, 0);
                vertices[1].Position = new Vector3(-hudScaleX, hudScaleY, 0);
                vertices[2].Position = new Vector3(hudScaleX, -hudScaleY, 0);
                vertices[3].Position = new Vector3(-hudScaleX, -hudScaleY, 0);
            }*/
        }

        public Texture2D getMarkerTex(GameCamera camera)
        {
            Texture2D rTexture;

            if (((PlayerCamera)camera).PlayerShip.ShipTeam == this.ship.ShipTeam)
            {
                rTexture = friendlyMarkerTex;
            }
            else
            {
                if (((PlayerCamera)camera).PlayerShip.MyPlayer != null)
                {
                    if (ship is SpawnShipPiece && ((PlayerCamera)camera).PlayerShip.MyPlayer.ObjectivesDrawActive)
                    {
                        rTexture = objectiveMarkerTex;
                    }
                    else
                    {
                        rTexture = enemyMarkerTex;
                    }
                }
                else 
                {
                    rTexture = enemyMarkerTex;
                }
            }

            return rTexture;
        }
    }
}
