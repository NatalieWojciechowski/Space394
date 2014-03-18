using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Space394.SceneObjects
{
    public abstract class FlagShip : SpawnShip
    {
        public FlagShip(long _uniqueId, Vector3 _position, Quaternion _rotation, Team _team)
            : base(_uniqueId, _position, _rotation, _team)
        {
            MAX_HEALTH = 5;
            Health = MaxHealth;

            MAX_SHIELDS = 5;
            Shields = MaxShields;

            setModelByString("Models/Ships/Halk_Capital");
            setFarModelByString("Models/box");
        }
    }
}
