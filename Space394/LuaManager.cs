using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using LuaInterface;
using Space394.Packaging;
using Space394.SceneObjects;
using Microsoft.Xna.Framework;
using Space394.Scenes;

namespace Space394
{
    public static class LuaManager
    {
        private static Lua lua;
        public static Lua getLua() { return lua; }

        private static luaInstance Instance;

        private const String header = "Content/Lua/";
        private const String footer = ".lua";

        public static void Initialize()
        {
            lua = new Lua();
            Instance = new luaInstance();
            // lua.RegisterFunction(Lua_String, Instance, Instange.GetType().GetMethod(Method_String));
            lua.RegisterFunction("Vect3", Instance, Instance.GetType().GetMethod("Vect3"));
            lua.RegisterFunction("Quater", Instance, Instance.GetType().GetMethod("Quater"));
            lua.RegisterFunction("SpawnInterceptor", Instance, Instance.GetType().GetMethod("SpawnInterceptor"));
            lua.RegisterFunction("SpawnAssaultFighter", Instance, Instance.GetType().GetMethod("SpawnAssaultFighter"));
            lua.RegisterFunction("SpawnBomber", Instance, Instance.GetType().GetMethod("SpawnBomber"));
            lua.RegisterFunction("SpawnCapitalShip", Instance, Instance.GetType().GetMethod("SpawnCapitalShip"));
        }

        public static void doFile(String file)
        {
            try
            {
                lua.DoFile(header + file + footer);
            }
            catch (LuaException e)
            {
                // Missing file
                Console.WriteLine(e.Message);
            }
        }

        private class luaInstance
        {
            // 0 = Exsolus
            // 1 = Halk

            public Vector3 Vect3(float x, float y, float z) { return (new Vector3(x, y, z)); }

            public Quaternion Quater(float x, float y, float z) { return Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(x), MathHelper.ToRadians(y), MathHelper.ToRadians(z)); }

            public void SpawnInterceptor(int team, Vector3 position, Quaternion rotation)
            {
                SceneObjectFactory.createNewAssaultFighter(position, rotation, ((Ship.Team)team), null);
            }

            public void SpawnAssaultFighter(int team, Vector3 position, Quaternion rotation)
            {
                SceneObjectFactory.createNewInterceptor(position, rotation, ((Ship.Team)team), null);
            }

            public void SpawnBomber(int team, Vector3 position, Quaternion rotation)
            {
                SceneObjectFactory.createNewBomber(position, rotation, ((Ship.Team)team), null);
            }

            public void SpawnCapitalShip(int team, Vector3 position, Quaternion rotation)
            {
                SceneObjectFactory.createNewCapitalShip(position, rotation, ((Ship.Team)team));
            }
        }
    }
}
