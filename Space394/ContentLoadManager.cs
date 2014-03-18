using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Threading;

namespace Space394
{
    public static class ContentLoadManager
    {
        private static Dictionary<string, Model> models;
        private static Dictionary<string, Texture2D> textures;

        private static ReaderWriterLockSlim modelLock = Locks.GetLockInstance(LockRecursionPolicy.NoRecursion); //setup the lock;
        private static ReaderWriterLockSlim textureLock = Locks.GetLockInstance(LockRecursionPolicy.NoRecursion); //setup the lock;

        public static void Initialize()
        {
            if (models == null)
            {
                models = new Dictionary<string, Model>();
            }
            else { }

            if (textures == null)
            {
                textures = new Dictionary<string, Texture2D>();
            }
            else { }
        }

        public static Model loadModel(string model)
        {
            Model rValue = null;
            using (new ReadLock(modelLock))
            {
                if (model == null)
                {
                    // Do nothing
                }
                else if (models.ContainsKey(model))
                {
                    rValue = models[model];
                }
                else
                {
                    using (new WriteLock(modelLock))
                    {
                        models.Add(model, Space394Game.GameInstance.Content.Load<Model>(model));
                    }
                    rValue = models[model];
                }
            }
            return rValue;
        }

        public static Texture2D loadTexture(string texture)
        {
            Texture2D rTexture = null;
            using (new ReadLock(textureLock))
            {
                if (texture == null)
                {
                    // Do nothing
                }
                else if (textures.ContainsKey(texture))
                {
                    rTexture = textures[texture];
                }
                else
                {
                    using (new WriteLock(textureLock))
                    {
                        textures.Add(texture, Space394Game.GameInstance.Content.Load<Texture2D>(texture));
                    }
                    rTexture = textures[texture];
                }
            }
            return rTexture;
        }

        public static void clear()
        {
            using (new WriteLock(modelLock))
            {
                models.Clear();
            }
            using (new WriteLock(textureLock))
            {
                textures.Clear();
            }
        }
    }
}
