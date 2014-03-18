using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Space394.SceneObjects
{
    public class SceneObjectFactory
    {
        private static int seed = 0;
        private static int seedIncrement = 7;

        public static AssaultFighter createNewAssaultFighter(Vector3 position, Quaternion rotation, Ship.Team _team, SpawnShip _home)
        {
            AssaultFighter rFighter = null;
            switch (_team)
            {
                case Ship.Team.Esxolus:
                    rFighter = new EsxolusAssaultFighter(seed += seedIncrement, position, rotation, _home);
                    break;
                case Ship.Team.Halk:
                    rFighter = new HalkAssaultFighter(seed += seedIncrement, position, rotation, _home);
                    break;
            }
            return ((AssaultFighter)rFighter); 
        }

        public static Interceptor createNewInterceptor(Vector3 position, Quaternion rotation, Ship.Team _team, SpawnShip _home)
        {
            Interceptor rFighter = null;
            switch (_team)
            {
                case Ship.Team.Esxolus:
                    rFighter = new EsxolusInterceptor(seed += seedIncrement, position, rotation, _home);
                    break;
                case Ship.Team.Halk:
                    rFighter = new HalkInterceptor(seed += seedIncrement, position, rotation, _home);
                    break;
            }
            return ((Interceptor)rFighter); 
        }

        public static Bomber createNewBomber(Vector3 position, Quaternion rotation, Ship.Team _team, SpawnShip _home)
        {
            Bomber rFighter = null;
            switch (_team)
            {
                case Ship.Team.Esxolus:
                    rFighter = new EsxolusBomber(seed += seedIncrement, position, rotation, _home);
                    break;
                case Ship.Team.Halk:
                    rFighter = new HalkBomber(seed += seedIncrement, position, rotation, _home);
                    break;
            }
            return ((Bomber)rFighter); 
        }

        public static CapitalShip createNewCapitalShip(Vector3 position, Quaternion rotation, Ship.Team _team)
        {
            CapitalShip rBattleship = null;
            switch (_team)
            {
                case Ship.Team.Esxolus:
                    rBattleship = new EsxolusCapitalShip(seed += seedIncrement, position, rotation);
                    break;
                case Ship.Team.Halk:
                    rBattleship = new HalkCapitalShip(seed += seedIncrement, position, rotation);
                    break;
            }
            return ((CapitalShip)Space394Game.GameInstance.CurrentScene.addSceneObject(rBattleship)); 
        }

        public static StarBox createNewStarBox()
        { return ((StarBox)Space394Game.GameInstance.CurrentScene.addSceneObject(new StarBox(seed += seedIncrement))); }

        public static Laser createNewLaser(Vector3 position, Quaternion rotation, Ship parent)
        { return ((Laser)ProjectileManager.spawnLaser(seed += seedIncrement, position, rotation, parent)); }

        public static Laser createNewLaser(Vector3 position, Quaternion rotation, Turret parent)
        { return ((Laser)ProjectileManager.spawnLaser(seed += seedIncrement, position, rotation, parent)); }

        public static EsxolusAMissile createNewEsxolusAMissile(Vector3 position, Quaternion rotation, Ship parent)
        { return ((EsxolusAMissile)ProjectileManager.spawnEsxolusAMissile(seed += seedIncrement, position, rotation, parent)); }

        public static EsxolusBMissile createNewEsxolusBMissile(Vector3 position, Quaternion rotation, Ship parent)
        { return ((EsxolusBMissile)ProjectileManager.spawnEsxolusBMissile(seed += seedIncrement, position, rotation, parent)); }

        public static EsxolusIMissile createNewEsxolusIMissile(Vector3 position, Quaternion rotation, Ship parent)
        { return ((EsxolusIMissile)ProjectileManager.spawnEsxolusIMissile(seed += seedIncrement, position, rotation, parent)); }

        public static HalkAMissile createNewHalkAMissile(Vector3 position, Quaternion rotation, Ship parent)
        { return ((HalkAMissile)ProjectileManager.spawnHalkAMissile(seed += seedIncrement, position, rotation, parent)); }

        public static HalkBMissile createNewHalkBMissile(Vector3 position, Quaternion rotation, Ship parent)
        { return ((HalkBMissile)ProjectileManager.spawnHalkBMissile(seed += seedIncrement, position, rotation, parent)); }

        public static HalkIMissile createNewHalkIMissile(Vector3 position, Quaternion rotation, Ship parent)
        { return ((HalkIMissile)ProjectileManager.spawnHalkIMissile(seed += seedIncrement, position, rotation, parent)); }

        public static Explosion createExplosion(Vector3 position, Quaternion rotation)
        { return ((Explosion)Space394Game.GameInstance.CurrentScene.addSceneObject(new Explosion(seed += seedIncrement, position, rotation))); }

        public static MassiveExplosion createMassiveExplosion(Vector3 position, Quaternion rotation)
        { return ((MassiveExplosion)Space394Game.GameInstance.CurrentScene.addSceneObject(new MassiveExplosion(seed += seedIncrement, position, rotation))); }

        public static EsxolusTurret createEsxolusTurret(Vector3 position, Quaternion rotation, Vector3 fireConeNormal, float fireConeAngle, Battleship parent)
        { return ((EsxolusTurret)Space394Game.GameInstance.CurrentScene.addSceneObject(new EsxolusTurret(seed += seedIncrement, position, rotation, fireConeNormal, fireConeAngle, parent))); }

        public static HalkTurret createHalkTurret(Vector3 position, Quaternion rotation, Vector3 fireConeNormal, float fireConeAngle, Battleship parent)
        { return ((HalkTurret)Space394Game.GameInstance.CurrentScene.addSceneObject(new HalkTurret(seed += seedIncrement, position, rotation, fireConeNormal, fireConeAngle, parent))); }
    }
}
