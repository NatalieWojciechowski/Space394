// Camera.cs
// Andre Berthiaume, March 2011
//
// Provides basic fucntionality for camera control

#region Using Statements
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Space394.SceneObjects;
using Space394.Scenes;
#endregion

namespace Space394
{
    // Only one per scene
    public class GameCamera
    {
        // The camera should only exist for this scene
        protected Scene currentScene;
        public Scene CurrentScene
        {
            get { return currentScene; }
        }

        private Vector3 position;
        public Vector3 Position
        {
            get { return position; }
            set 
            {
                position = value;
                viewMatrix = Matrix.CreateLookAt(position,
                    target, up);
            }
        }

        private Quaternion rotation;
        public Quaternion Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }

        private Vector3 target;
        public Vector3 Target
        {
            get { return target; }
            set 
            {
                target = value;
                viewMatrix = Matrix.CreateLookAt(position,
                    target, up);
            }
        }

        private Vector3 up;
        public Vector3 Up
        {
            get { return up; }
            set { up = value; }
        }

        protected Matrix viewMatrix;
        public Matrix ViewMatrix
        {
            get { return viewMatrix; }
        }
        public Matrix setViewMatrix(Vector3 _position, Vector3 _target, Vector3 _up)
        {
            position = _position;
            target = _target;
            up = _up;
            viewMatrix = Matrix.CreateLookAt(_position, _target, _up);
            
            return viewMatrix;
        }

        private Matrix projectionMatrix;
        public Matrix ProjectionMatrix
        {
            get { return projectionMatrix; }
            set { projectionMatrix = value; }
        }

        protected Viewport viewPort;
        public Viewport ViewPort
        {
            get { return viewPort; }
        }

        // The fourth player port in case there are three people playing
        protected Viewport fourthPort;

        protected float aspectRatio;
        public float AspectRatio
        {
            get { return aspectRatio; }
        }

        private AutoTexture2D splitScreen2;
        private AutoTexture2D splitScreen3;
        private AutoTexture2D splitScreen4;
        private AutoTexture2D blackTexture;
        private AutoTexture2D pausedTexture;

        protected const float VIEW_DEPTH = 100000000.0f;

        private static bool topSplit = false; // Whether not to evenly divide with four or not
        public static bool TopSplit
        {
            get { return GameCamera.topSplit; }
            set { GameCamera.topSplit = value; }
        }

        private bool splitScreenEnabled = false;
        public bool SplitScreenEnabled
        {
            get { return splitScreenEnabled; }
            set 
            { 
                splitScreenEnabled = value; 
                Space394Game.GameInstance.updatePlayerCameras();
            }
        }

        // Total regions
        private int tRegions = 0;
        public int TRegions
        {
            get { return tRegions; }
            set 
            { 
                tRegions = value; 
            }
        }

        public GameCamera(Scene _currentScene, Vector3 _position, Quaternion _rotation, float _aspectRatio)
        {
            currentScene = _currentScene;
            position = _position;
            rotation = _rotation;
            aspectRatio = _aspectRatio;
            up = new Vector3(0, 1, 0);
            target = new Vector3();
            viewMatrix = Matrix.CreateLookAt(position,
                        target, up);
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(
                        MathHelper.ToRadians(45.0f), aspectRatio,
                        0.1f, VIEW_DEPTH);
            viewPort = Space394Game.GameInstance.GraphicsDevice.Viewport;
            fourthPort = new Viewport(
                        Space394Game.GameInstance.DefaultViewPort.Width / 2 + 1,
                        Space394Game.GameInstance.DefaultViewPort.Height / 2 + 1,
                        Space394Game.GameInstance.DefaultViewPort.Width / 2 - 1,
                        Space394Game.GameInstance.DefaultViewPort.Height / 2 - 1);

            splitScreen2 = new AutoTexture2D(Space394Game.GameInstance.Content.Load<Texture2D>("Textures\\splitScreen2"), Vector2.Zero);
            splitScreen3 = new AutoTexture2D(Space394Game.GameInstance.Content.Load<Texture2D>("Textures\\splitScreen3"), Vector2.Zero);
            splitScreen4 = new AutoTexture2D(Space394Game.GameInstance.Content.Load<Texture2D>("Textures\\splitScreen4"), Vector2.Zero);
            blackTexture = new AutoTexture2D(Space394Game.GameInstance.Content.Load<Texture2D>("Textures\\blackTexture"), Vector2.Zero);
            blackTexture.Width = fourthPort.Width;
            blackTexture.Height = fourthPort.Height;
            pausedTexture = new AutoTexture2D(Space394Game.GameInstance.Content.Load<Texture2D>("Textures\\pausedTexture"), Vector2.Zero);
        }

        // Draws on the default drawport
        public virtual void Draw(Dictionary<int, SceneObject> drawables)
        {
            Space394Game.GameInstance.GraphicsDevice.Viewport = Space394Game.GameInstance.DefaultViewPort;
            if (drawables != null)
            {
                foreach (KeyValuePair<int, SceneObject> draw in drawables)
                {
                    draw.Value.Draw(this);
                }
            }
            else { }
            if (tRegions == 2)
            {
                Space394Game.GameInstance.GraphicsDevice.Viewport = Space394Game.GameInstance.DefaultViewPort;
                SpriteBatch spriteBatch = Space394Game.GameInstance.SpriteBatch;
                spriteBatch.Begin();
                splitScreen2.DrawAlreadyBegunMaintainRatio();
                spriteBatch.End();
                Space394Game.GameInstance.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            }
            else if (tRegions == 3) // Black out the fourth port unless the option is on
            {
                if (!topSplit)
                {
                    Space394Game.GameInstance.GraphicsDevice.Viewport = Space394Game.GameInstance.DefaultViewPort;
                    SpriteBatch spriteBatch = Space394Game.GameInstance.SpriteBatch;

                    // Draw out the fourth player
                    Space394Game.GameInstance.GraphicsDevice.Viewport = fourthPort;
                    spriteBatch.Begin();
                    blackTexture.DrawAlreadyBegunMaintainRatio();
                    spriteBatch.End();
                    Space394Game.GameInstance.GraphicsDevice.Viewport = Space394Game.GameInstance.DefaultViewPort;
                    Space394Game.GameInstance.GraphicsDevice.DepthStencilState = DepthStencilState.Default;

                    spriteBatch.Begin();
                    splitScreen4.DrawAlreadyBegunMaintainRatio();
                    spriteBatch.End();
                    Space394Game.GameInstance.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
                }
                else
                {
                    Space394Game.GameInstance.GraphicsDevice.Viewport = Space394Game.GameInstance.DefaultViewPort;
                    SpriteBatch spriteBatch = Space394Game.GameInstance.SpriteBatch;

                    spriteBatch.Begin();
                    splitScreen3.DrawAlreadyBegunMaintainRatio();
                    spriteBatch.End();
                    Space394Game.GameInstance.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
                }
            }
            else if (tRegions == 4)
            {
                Space394Game.GameInstance.GraphicsDevice.Viewport = Space394Game.GameInstance.DefaultViewPort;
                SpriteBatch spriteBatch = Space394Game.GameInstance.SpriteBatch;
                spriteBatch.Begin();
                splitScreen4.DrawAlreadyBegunMaintainRatio();
                spriteBatch.End();
                Space394Game.GameInstance.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            }
            else { }
        }

        public void DrawPaused()
        {
            Space394Game.GameInstance.GraphicsDevice.Viewport = Space394Game.GameInstance.DefaultViewPort;
            SpriteBatch spriteBatch = Space394Game.GameInstance.SpriteBatch;
            spriteBatch.Begin();
            pausedTexture.DrawAlreadyBegunMaintainRatio();
            spriteBatch.End();
            Space394Game.GameInstance.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
        }
        
        public void clearScreen()
        {
            Space394Game.GameInstance.GraphicsDevice.Clear(Color.Black);
        }

        public void Update(float deltaTime)
        {
            rotation = Quaternion.CreateFromRotationMatrix(ViewMatrix);
        }
    }
}
