#region File Description
//-----------------------------------------------------------------------------
// AnimatedTexture.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Space394
{
    public abstract class AnimatedTexture
    {
        protected AutoTexture2D myTexture;
        protected float TimePerFrame;
        protected int Frame;
        protected float TotalElapsed;
        protected bool Paused;
        protected int framecount;

        protected bool completed;
        public bool Completed
        {
            get { return completed; }
        }

        public float Rotation, Scale, Depth;
        public Vector2 Origin;

        public Texture2D getTexture() { return myTexture.Texture; }

        public AnimatedTexture(Vector2 origin, float rotation,
            float scale, float depth)
        {
            this.Origin = origin;
            this.Rotation = rotation;
            this.Scale = scale;
            this.Depth = depth;
            this.completed = false;
        }

        public virtual void Load(string asset,
            int frameCount, int framesPerSec, Vector2 position)
        {
            framecount = frameCount;
            myTexture = new AutoTexture2D(ContentLoadManager.loadTexture(asset), position);
            TimePerFrame = (float)1 / framesPerSec;
            Frame = 0;
            TotalElapsed = 0;
            Paused = false;
        }

        // class AnimatedTexture
        public virtual void UpdateFrame(float elapsed)
        {
            if (Paused)
                return;
            TotalElapsed += elapsed;
            if (TotalElapsed > TimePerFrame)
            {
                Frame++;
                if (Frame >= framecount)
                {
                    completed = true;
                }
                else { }
                // Keep the Frame between 0 and the total frames, minus one.
                Frame = Frame % framecount;
                TotalElapsed -= TimePerFrame;
            }
        }

        // class AnimatedTexture
        public void DrawFrame(SpriteBatch batch, Vector2 screenPos)
        {
            DrawFrame(batch, Frame, screenPos);
        }

        // Doesn't work
        public virtual void DrawFrame(SpriteBatch batch, int frame, Vector2 screenPos)
        {
            if (myTexture != null)
            {
                //int FrameWidth = myTexture.Texture.Width / framecount;
                //Rectangle sourcerect = new Rectangle(FrameWidth * frame, 0,
                //    FrameWidth, myTexture.Texture.Height);
                batch.Begin();
                /*batch.Draw(myTexture.Texture, screenPos, sourcerect, Color.White,
                    Rotation, Origin, Scale, SpriteEffects.None, Depth);*/
                myTexture.DrawAlreadyBegunMaintainRatio();
                batch.End();
                Space394Game.GameInstance.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            }
            else { }
        }

        public bool IsPaused
        {
            get { return Paused; }
        }

        public void Reset()
        {
            Frame = 0;
            TotalElapsed = 0f;
        }

        public void Stop()
        {
            Pause();
            Reset();
        }

        public void Play()
        {
            Paused = false;
        }

        public void Pause()
        {
            Paused = true;
        }

    }

    public class AnimatedTextureSheet : AnimatedTexture
    {
        public AnimatedTextureSheet(Vector2 origin, float rotation,
            float scale, float depth)
            : base(origin, rotation, scale, depth)
        {

        }

    }

    public class AnimatedTextureFiles : AnimatedTexture
    {
        private AutoTexture2D[] myTextures;

        public AnimatedTextureFiles(Vector2 origin, float rotation,
            float scale, float depth)
            : base(origin, rotation, scale, depth)
        {
            this.Origin = origin;
            this.Rotation = rotation;
            this.Scale = scale;
            this.Depth = depth;
        }

        public override void Load(string asset,
            int frameCount, int framesPerSec, Vector2 position)
        {
            framecount = frameCount;
            myTextures = new AutoTexture2D[frameCount];
            for (int i = 0; i < framecount; i++)
            {
                myTextures[i] = new AutoTexture2D(ContentLoadManager.loadTexture(asset+(i+1)), position);
            }
            TimePerFrame = (float)1 / framesPerSec;
            Frame = 0;
            TotalElapsed = 0;
            Paused = false;
            myTexture = myTextures[0];
        }

        // class AnimatedTexture
        public override void UpdateFrame(float elapsed)
        {
            if (Paused)
                return;
            TotalElapsed += elapsed;
            if (TotalElapsed > TimePerFrame)
            {
                Frame++;
                if (Frame >= framecount)
                {
                    completed = true;
                }
                else { }
                // Keep the Frame between 0 and the total frames, minus one.
                Frame = Frame % framecount;
                TotalElapsed -= TimePerFrame;
            }
            myTexture = myTextures[Frame];
        }

        public override void DrawFrame(SpriteBatch batch, int frame, Vector2 screenPos)
        {
            if (myTexture != null)
            {
                //int FrameWidth = myTexture.Width / framecount;
                //Rectangle sourcerect = new Rectangle(0, 0, myTexture.Width, myTexture.Height);
                batch.Begin();
                /*batch.Draw(myTexture, screenPos, sourcerect, Color.White,
                    Rotation, Origin, Scale, SpriteEffects.None, Depth);*/
                myTexture.DrawAlreadyBegunMaintainRatio();
                batch.End();
                Space394Game.GameInstance.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            }
            else { }
        }
    }
}
