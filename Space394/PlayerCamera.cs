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
    public class PlayerCamera : GameCamera
    {
        private Fighter playerShip;
        public Fighter PlayerShip
        {
            get { return playerShip; }
        }

        public PlayerCamera(Scene _currentScene, Vector3 _position, Quaternion _rotation, float _aspectRatio)
            : base (_currentScene, _position, _rotation, _aspectRatio)
        {
            // Positions and orients the GameObject
            viewMatrix = Matrix.CreateRotationX(0)
                                  * Matrix.CreateRotationY(0)
                                  * Matrix.CreateRotationZ(0)
                                  * Matrix.CreateTranslation(0, 3, -10);
        }

        public Vector3 playerInit(Vector3 _offset, Player _player)
        {
            if (_player.PlayerShip != null)
            {
                Position = _player.PlayerShip.Position + _offset;
                Target = _player.PlayerShip.Position;
                playerShip = _player.PlayerShip;
            }
            else
            {
                Position = Vector3.Zero + _offset;
                Target = Vector3.Zero;
                playerShip = null;
            }

            return Position;
        }

        public override void Draw(Dictionary<int, SceneObject> drawables)
        {
            Space394Game.GameInstance.GraphicsDevice.Viewport = ViewPort;

            foreach (SceneObject item in drawables.Values)
            {
                item.Draw(this);
            }

            Space394Game.GameInstance.GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            Space394Game.GameInstance.GraphicsDevice.Viewport = Space394Game.GameInstance.DefaultViewPort;
        }

        // Give the player number and it will trickle down into the right number
        public void setRegion(int region, int totalRegions)
        {
            if (GameCamera.TopSplit)
            {
                setRegionTopLarge(region, totalRegions);
            }
            else
            {
                setRegionNormal(region, totalRegions);
            }
        }

        // Regions are as follows
        // [0] for = 1 player
        //
        // [0] for = 2 player
        // [1]
        //
        // [ 0 ] for = 3 player
        // [1][2]
        //
        // [0][1] for = 4 player
        // [2][3]
        public void setRegionTopLarge(int region, int totalRegions)
        {
            TRegions = totalRegions;
            if (totalRegions == 0 || totalRegions == 1)
            {
                aspectRatio = Space394Game.GameInstance.AspectRatio;
                viewPort = Space394Game.GameInstance.DefaultViewPort;
            }
            else if (totalRegions == 2)
            {
                aspectRatio = Space394Game.GameInstance.AspectRatio * 2;
                if (region == 0)
                {
                    viewPort = new Viewport(
                        0,
                        0,
                        Space394Game.GameInstance.DefaultViewPort.Width,
                        Space394Game.GameInstance.DefaultViewPort.Height / 2 - 1);
                }
                else // region == 1
                {
                    viewPort = new Viewport(
                        0,
                        Space394Game.GameInstance.DefaultViewPort.Height / 2 + 1,
                        Space394Game.GameInstance.DefaultViewPort.Width,
                        Space394Game.GameInstance.DefaultViewPort.Height / 2 - 1);
                }
            }
            else if (totalRegions == 3)
            {
                aspectRatio = Space394Game.GameInstance.AspectRatio; // Back to the same ratio
                if (region == 0)
                {
                    aspectRatio = Space394Game.GameInstance.AspectRatio * 2; // Special
                    viewPort = new Viewport(
                        0,
                        0,
                        Space394Game.GameInstance.DefaultViewPort.Width,
                        Space394Game.GameInstance.DefaultViewPort.Height / 2 - 1);
                }
                else if (region == 1)
                {
                    viewPort = new Viewport(
                        0,
                        Space394Game.GameInstance.DefaultViewPort.Height / 2 + 1,
                        Space394Game.GameInstance.DefaultViewPort.Width / 2 - 1,
                        Space394Game.GameInstance.DefaultViewPort.Height / 2 - 1);
                }
                else // region == 2
                {
                    viewPort = new Viewport(
                        Space394Game.GameInstance.DefaultViewPort.Width / 2 + 1,
                        Space394Game.GameInstance.DefaultViewPort.Height / 2 + 1,
                        Space394Game.GameInstance.DefaultViewPort.Width / 2 - 1,
                        Space394Game.GameInstance.DefaultViewPort.Height / 2 - 1);
                }
            }
            else // total regions == 4
            {
                aspectRatio = Space394Game.GameInstance.AspectRatio; // Back to the same ratio
                if (region == 0)
                {
                    viewPort = new Viewport(
                        0,
                        0,
                        Space394Game.GameInstance.DefaultViewPort.Width / 2 - 1,
                        Space394Game.GameInstance.DefaultViewPort.Height / 2 - 1);
                }
                else if (region == 1)
                {
                    viewPort = new Viewport(
                        Space394Game.GameInstance.DefaultViewPort.Width / 2 + 1,
                        0,
                        Space394Game.GameInstance.DefaultViewPort.Width / 2 - 1,
                        Space394Game.GameInstance.DefaultViewPort.Height / 2 - 1);
                }
                else if (region == 2)
                {
                    viewPort = new Viewport(
                        0,
                        Space394Game.GameInstance.DefaultViewPort.Height / 2 + 1,
                        Space394Game.GameInstance.DefaultViewPort.Width / 2 - 1,
                        Space394Game.GameInstance.DefaultViewPort.Height / 2 - 1);
                }
                else // region == 3
                {
                    viewPort = new Viewport(
                        Space394Game.GameInstance.DefaultViewPort.Width / 2 + 1,
                        Space394Game.GameInstance.DefaultViewPort.Height / 2 + 1,
                        Space394Game.GameInstance.DefaultViewPort.Width / 2 - 1,
                        Space394Game.GameInstance.DefaultViewPort.Height / 2 - 1);
                }
            }
            ProjectionMatrix = Matrix.CreatePerspectiveFieldOfView(
                        MathHelper.ToRadians(45.0f), AspectRatio,
                        0.1f, VIEW_DEPTH);
        }

        // Regions are as follows
        // [0] for = 1 player
        //
        // [0] for = 2 player
        // [1]
        //
        // [0][1] for > 2 player
        // [2][3]
        public void setRegionNormal(int region, int totalRegions)
        {
            TRegions = totalRegions;
            if (totalRegions == 0 || totalRegions == 1)
            {
                aspectRatio = Space394Game.GameInstance.AspectRatio;
                viewPort = Space394Game.GameInstance.DefaultViewPort;
            }
            else if (totalRegions == 2)
            {
                aspectRatio = Space394Game.GameInstance.AspectRatio * 2;
                if (region == 0)
                {
                    viewPort = new Viewport(
                        0,
                        0,
                        Space394Game.GameInstance.DefaultViewPort.Width,
                        Space394Game.GameInstance.DefaultViewPort.Height / 2);
                }
                else // region == 1
                {
                    viewPort = new Viewport(
                        0,
                        Space394Game.GameInstance.DefaultViewPort.Height / 2,
                        Space394Game.GameInstance.DefaultViewPort.Width,
                        Space394Game.GameInstance.DefaultViewPort.Height / 2);
                }
            }
            else // total regions == 3 || 4
            {
                aspectRatio = Space394Game.GameInstance.AspectRatio; // Back to the same ratio
                if (region == 0)
                {
                    viewPort = new Viewport(
                        0,
                        0,
                        Space394Game.GameInstance.DefaultViewPort.Width / 2 - 1,
                        Space394Game.GameInstance.DefaultViewPort.Height / 2 - 1);
                }
                else if (region == 1)
                {
                    viewPort = new Viewport(
                        Space394Game.GameInstance.DefaultViewPort.Width / 2 + 1,
                        0,
                        Space394Game.GameInstance.DefaultViewPort.Width / 2 - 1,
                        Space394Game.GameInstance.DefaultViewPort.Height / 2 - 1);
                }
                else if (region == 2)
                {
                    viewPort = new Viewport(
                        0,
                        Space394Game.GameInstance.DefaultViewPort.Height / 2 + 1,
                        Space394Game.GameInstance.DefaultViewPort.Width / 2 - 1,
                        Space394Game.GameInstance.DefaultViewPort.Height / 2 - 1);
                }
                else // region == 3
                {
                    viewPort = new Viewport(
                        Space394Game.GameInstance.DefaultViewPort.Width / 2 + 1,
                        Space394Game.GameInstance.DefaultViewPort.Height / 2 + 1,
                        Space394Game.GameInstance.DefaultViewPort.Width / 2 - 1,
                        Space394Game.GameInstance.DefaultViewPort.Height / 2 - 1);
                }
            }
            ProjectionMatrix = Matrix.CreatePerspectiveFieldOfView(
                        MathHelper.ToRadians(45.0f), AspectRatio,
                        0.1f, VIEW_DEPTH);
        }

        private Quaternion RotationQuaternion(float degrees, Vector3 direction)
        {
            Vector4 unitAxis = Vector4.Zero;
            Vector4 axis = new Vector4(direction, 0.0f);

            // only normalize if necessary
            if ((axis.X != 0 && axis.X != 1) || (axis.Y != 0 && axis.Y != 1) ||
                (axis.Z != 0 && axis.Z != 1))
            {
                unitAxis = Vector4.Normalize(axis);
            }

            float angle = degrees * MathHelper.Pi / 180.0f;
            float sin = (float)Math.Sin(angle / 2.0f);

            // create the quaternion
            Quaternion quaternion = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);
//            Vector4 quaternion = new Vector4(0.0f, 0.0f, 0.0f, 0.0f);
            quaternion.X = axis.X * sin;
            quaternion.Y = axis.Y * sin;
            quaternion.Z = axis.Z * sin;
            quaternion.W = (float)Math.Cos(angle/2.0f);

            return Quaternion.Normalize(quaternion);
        }

        // Springy action
        private Vector3 velocity = new Vector3();
        private bool velSet = false;
        public void unsetVelSet() { velSet = false; }
        private float springK = 0.01f;
        private const float MAX_X = 7.5f;
        private Quaternion lastRotation;
        private float rollSpeed = 5.5f;

        public Matrix springToView(Vector3 _position, Vector3 _target, Vector3 _up, Vector3 _velocity, Quaternion _rotation, float deltaTime)
        {
            if (!velSet)
            {
                velocity = (Position - _position) / deltaTime;
                lastRotation = _rotation;
                velSet = true;
                setViewMatrix(_position, _target, _up);
            }
            else
            {
                Vector3 acceleration = (_velocity - velocity) / deltaTime;

                velocity = _velocity;

                acceleration *= springK;
                if (acceleration.Length() > MAX_X)
                {
                    acceleration.Normalize();
                    acceleration *= MAX_X;
                }
                else { }

                Vector3 reflect = Vector3.Transform(Vector3.Forward, _rotation);
                acceleration = Vector3.Reflect(acceleration, reflect);

                Position = _position + (acceleration);
                Target = _target - (acceleration);
                Quaternion slerp = Quaternion.Slerp(lastRotation, _rotation, rollSpeed * deltaTime);
                Up = Vector3.Transform(Vector3.Up, slerp);
                viewMatrix = Matrix.CreateLookAt(Position, Target, Up);

                lastRotation = slerp;
            }

            return ViewMatrix;
        }
    }
}
