using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Space394.SceneObjects;
using Microsoft.Xna.Framework;
using Space394.Scenes;

namespace Space394
{
    public static class ProjectileManager
    {
        private static Queue<Laser> availableLasers;
        private static Queue<EsxolusAMissile> availableEsxolusAMissiles;
        private static Queue<EsxolusBMissile> availableExsolusBMissiles;
        private static Queue<EsxolusIMissile> availableEsxolusIMissiles;
        private static Queue<HalkAMissile> availableHalkAMissiles;
        private static Queue<HalkBMissile> availableHalkBMissiles;
        private static Queue<HalkIMissile> availableHalkIMissiles;

        #region Initialize
        public static void Initialize()
        {
            if (availableLasers == null)
            {
                availableLasers = new Queue<Laser>();
            }
            else { }

            if (availableEsxolusAMissiles == null)
            {
                availableEsxolusAMissiles = new Queue<EsxolusAMissile>();
            }
            else { }
            if (availableEsxolusIMissiles == null)
            {
                availableEsxolusIMissiles = new Queue<EsxolusIMissile>();
            }
            else { }
            if (availableHalkBMissiles == null)
            {
                availableHalkBMissiles = new Queue<HalkBMissile>();
            }
            else { }
            if (availableHalkIMissiles == null)
            {
                availableHalkIMissiles = new Queue<HalkIMissile>();
            }
            else { }

            if (availableHalkAMissiles == null)
            {
                availableHalkAMissiles = new Queue<HalkAMissile>();
            }
            else { }

            if (availableExsolusBMissiles == null)
            {
                availableExsolusBMissiles = new Queue<EsxolusBMissile>();
            }
            else { }
        }

        public static void InitalizeLasers(int lasers)
        {
            int difference = lasers;
            if (availableLasers != null)
            {
                difference = lasers - availableLasers.Count();
            }

            for (int i = 0; i < difference; i++)
            {
                availableLasers.Enqueue(new Laser(0, Vector3.Zero, Quaternion.Identity));
            }
        }

        public static void InitalizeEsxolusAMissiles(int missiles)
        {
            int difference = missiles;
            if (availableEsxolusAMissiles != null)
            {
                difference = missiles - availableEsxolusAMissiles.Count();
            }

            for (int i = 0; i < difference; i++)
            {
                availableEsxolusAMissiles.Enqueue(new EsxolusAMissile(0, Vector3.Zero, Quaternion.Identity));
            }
        }

        public static void InitalizeEsxolusIMissiles(int missiles)
        {
            int difference = missiles;
            if (availableEsxolusIMissiles != null)
            {
                difference = missiles - availableEsxolusIMissiles.Count();
            }

            for (int i = 0; i < difference; i++)
            {
                availableEsxolusIMissiles.Enqueue(new EsxolusIMissile(0, Vector3.Zero, Quaternion.Identity));
            }
        }

        public static void InitalizeHalkBMissiles(int missiles)
        {
            int difference = missiles;
            if (availableHalkBMissiles != null)
            {
                difference = missiles - availableHalkBMissiles.Count();
            }

            for (int i = 0; i < difference; i++)
            {
                availableHalkBMissiles.Enqueue(new HalkBMissile(0, Vector3.Zero, Quaternion.Identity));
            }
        }

        public static void InitalizeHalkIMissiles(int missiles)
        {
            int difference = missiles;
            if (availableHalkIMissiles != null)
            {
                difference = missiles - availableHalkIMissiles.Count();
            }

            for (int i = 0; i < difference; i++)
            {
                availableHalkIMissiles.Enqueue(new HalkIMissile(0, Vector3.Zero, Quaternion.Identity));
            }
        }

        public static void InitalizeHalkAMissiles(int missiles)
        {
            int difference = missiles;
            if (availableHalkAMissiles != null)
            {
                difference = missiles - availableHalkAMissiles.Count();
            }

            for (int i = 0; i < difference; i++)
            {
                availableHalkAMissiles.Enqueue(new HalkAMissile(0, Vector3.Zero, Quaternion.Identity));
            }
        }

        public static void InitalizeEsxolusBMissiles(int bombs)
        {
            int difference = bombs;
            if (availableExsolusBMissiles != null)
            {
                difference = bombs - availableExsolusBMissiles.Count();
            }

            for (int i = 0; i < difference; i++)
            {
                availableExsolusBMissiles.Enqueue(new EsxolusBMissile(0, Vector3.Zero, Quaternion.Identity));
            }
        }
        #endregion 

        #region Spawn
        public static Laser spawnLaser(long seed, Vector3 position, Quaternion rotation, SceneObject parent)
        {
            Laser rLaser = null;
            if (availableLasers.Count == 0)
            {
                rLaser = new Laser(seed, position, rotation);
                rLaser.respawn(seed, position, rotation, parent);
            }
            else
            {
                rLaser = availableLasers.Dequeue();
                rLaser.respawn(seed, position, rotation, parent);
            }

            return ((Laser)Space394Game.GameInstance.CurrentScene.addSceneObject(rLaser));
        }

        public static EsxolusAMissile spawnEsxolusAMissile(long seed, Vector3 position, Quaternion rotation, SceneObject parent)
        {
            EsxolusAMissile rSecondaryAttack = null;
            if (availableEsxolusAMissiles.Count == 0)
            {
                rSecondaryAttack = new EsxolusAMissile(seed, position, rotation);
                rSecondaryAttack.respawn(seed, position, rotation, parent);
            }
            else
            {
                rSecondaryAttack = (EsxolusAMissile)availableEsxolusAMissiles.Dequeue();
                rSecondaryAttack.respawn(seed, position, rotation, parent);
            }

            return ((EsxolusAMissile)Space394Game.GameInstance.CurrentScene.addSceneObject(rSecondaryAttack));
        }

        public static EsxolusIMissile spawnEsxolusIMissile(long seed, Vector3 position, Quaternion rotation, SceneObject parent)
        {
            EsxolusIMissile rSecondaryAttack = null;
            if (availableEsxolusIMissiles.Count == 0)
            {
                rSecondaryAttack = new EsxolusIMissile(seed, position, rotation);
                rSecondaryAttack.respawn(seed, position, rotation, parent);
            }
            else
            {
                rSecondaryAttack = (EsxolusIMissile)availableEsxolusIMissiles.Dequeue();
                rSecondaryAttack.respawn(seed, position, rotation, parent);
            }

            return ((EsxolusIMissile)Space394Game.GameInstance.CurrentScene.addSceneObject(rSecondaryAttack));
        }

        public static HalkBMissile spawnHalkBMissile(long seed, Vector3 position, Quaternion rotation, SceneObject parent)
        {
            HalkBMissile rSecondaryAttack = null;
            if (availableHalkBMissiles.Count == 0)
            {
                rSecondaryAttack = new HalkBMissile(seed, position, rotation);
                rSecondaryAttack.respawn(seed, position, rotation, parent);
            }
            else
            {
                rSecondaryAttack = (HalkBMissile)availableHalkBMissiles.Dequeue();
                rSecondaryAttack.respawn(seed, position, rotation, parent);
            }

            return ((HalkBMissile)Space394Game.GameInstance.CurrentScene.addSceneObject(rSecondaryAttack));
        }

        public static HalkIMissile spawnHalkIMissile(long seed, Vector3 position, Quaternion rotation, SceneObject parent)
        {
            HalkIMissile rSecondaryAttack = null;
            if (availableHalkIMissiles.Count == 0)
            {
                rSecondaryAttack = new HalkIMissile(seed, position, rotation);
                rSecondaryAttack.respawn(seed, position, rotation, parent);
            }
            else
            {
                rSecondaryAttack = (HalkIMissile)availableHalkIMissiles.Dequeue();
                rSecondaryAttack.respawn(seed, position, rotation, parent);
            }

            return ((HalkIMissile)Space394Game.GameInstance.CurrentScene.addSceneObject(rSecondaryAttack));
        }

        public static HalkAMissile spawnHalkAMissile(long seed, Vector3 position, Quaternion rotation, SceneObject parent)
        {
            HalkAMissile rSecondaryAttack = null;
            if (availableHalkAMissiles.Count == 0)
            {
                rSecondaryAttack = new HalkAMissile(seed, position, rotation);
                rSecondaryAttack.respawn(seed, position, rotation, parent);
            }
            else
            {
                rSecondaryAttack = (HalkAMissile)availableHalkAMissiles.Dequeue();
                rSecondaryAttack.respawn(seed, position, rotation, parent);
            }

            return ((HalkAMissile)Space394Game.GameInstance.CurrentScene.addSceneObject(rSecondaryAttack));
        }

        public static EsxolusBMissile spawnEsxolusBMissile(long seed, Vector3 position, Quaternion rotation, SceneObject parent)
        {
            EsxolusBMissile rBomb = null;
            if (availableExsolusBMissiles.Count == 0)
            {
                rBomb = new EsxolusBMissile(seed, position, rotation);
                rBomb.respawn(seed, position, rotation, parent);
            }
            else
            {
                rBomb = (EsxolusBMissile)availableExsolusBMissiles.Dequeue();
                rBomb.respawn(seed, position, rotation, parent);
            }

            return ((EsxolusBMissile)Space394Game.GameInstance.CurrentScene.addSceneObject(rBomb));
        }
        #endregion

        #region Add
        public static Laser addLaser(Laser laser)
        {
            availableLasers.Enqueue(laser);
            return laser;
        }

        public static EsxolusAMissile addEsxolusAMissile(EsxolusAMissile missile)
        {
            availableEsxolusAMissiles.Enqueue(missile);
            return missile;
        }

        public static EsxolusIMissile addEsxolusIMissile(EsxolusIMissile missile)
        {
            availableEsxolusIMissiles.Enqueue(missile);
            return missile;
        }

        public static HalkBMissile addHalkBMissile(HalkBMissile missile)
        {
            availableHalkBMissiles.Enqueue(missile);
            return missile;
        }

        public static HalkIMissile addHalkIMissile(HalkIMissile missile)
        {
            availableHalkIMissiles.Enqueue(missile);
            return missile;
        }

        public static HalkAMissile addHalkAMissile(HalkAMissile clusterMissile)
        {
            availableHalkAMissiles.Enqueue(clusterMissile);
            return clusterMissile;
        }

        public static EsxolusBMissile addExsolusBMissile(EsxolusBMissile bomb)
        {
            availableExsolusBMissiles.Enqueue(bomb);
            return bomb;
        }
        #endregion
    }
}
