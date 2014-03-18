using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Threading;

namespace Space394
{
    public static class LogCat
    {
        private static Dictionary<string, string> values;

#if DEBUG
        private static SpriteFont font = null;
#endif

        private const int ySpacing = 15;
        private const int xSpacing = 250;

        private static int counter = 0;

        public static Color color = Color.LightSalmon;

        private static ReaderWriterLockSlim threadLock = Locks.GetLockInstance(LockRecursionPolicy.NoRecursion); //setup the lock;

        public static void Initialize()
        {
            if (values == null)
            {
                values = new Dictionary<string, string>();
            }
            else { }
        }

        public static void DrawLog()
        {
#if DEBUG
            if (font == null)
            {
                font = Space394Game.GameInstance.Content.Load<SpriteFont>("Fonts\\DefaultFont");
            }
            else { }

            Space394Game.GameInstance.GraphicsDevice.Viewport = Space394Game.GameInstance.DefaultViewPort;

            SpriteBatch batch = Space394Game.GameInstance.SpriteBatch;
            batch.Begin();
            int i = 0;
            using (new ReadLock(threadLock))
            {
                foreach (string key in values.Keys)
                {
                    batch.DrawString(font, key + ": ", new Vector2(0, ySpacing * i), color);
                    batch.DrawString(font, values[key], new Vector2(xSpacing, ySpacing * i), color);
                    i++;
                }
            }
            batch.End();
            Space394Game.GameInstance.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
#endif
        }

        public static void addValue(String tag, String value)
        {
            using (new ReadLock(threadLock))
            {
                if (!values.ContainsKey(tag))
                {
                    using (new WriteLock(threadLock))
                    {
                        values.Add(tag, value);
                    }
                }
                else { }
            }
        }

        public static void updateValue(String index, String value)
        {
            using (new ReadLock(threadLock))
            {
                if (values.ContainsKey(index))
                {
                    values[index] = value;
                }
                else
                {
                    using (new WriteLock(threadLock))
                    {
                        values.Add(index, value);
                    }
                }
            }
        }

        public static void beginProfiling(String tag)
        {
            using (new ReadLock(threadLock))
            {
                if (values.ContainsKey(tag))
                {
                    values[tag] = "" + DateTime.Now.Millisecond;
                }
                else
                {
                    using (new WriteLock(threadLock))
                    {
                        values.Add(tag, "" + DateTime.Now.Millisecond);
                    }
                }
            }
        }

        public static void endProfiling(String tag)
        {
            using (new ReadLock(threadLock))
            {
                if (values.ContainsKey(tag))
                {
                    values[tag] = "" + (DateTime.Now.Millisecond - Convert.ToInt32(values[tag]));
                }
                else
                {
                    using (new WriteLock(threadLock))
                    {
                        values.Add(tag, "" + 0);
                    }
                }
            }
        }

        public static void resetCounter()
        {
            counter = 0;
        }

        public static string increaseCounter()
        {
            return ("" + ++counter);
        }
    }
}
