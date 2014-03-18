using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Space394.Packaging
{
    public interface Package
    {
        bool run();
        Package transmit(); // Returns this
    }

    internal class PlayerJoinPackage : Package
    {
        private PlayerIndex number;

        public PlayerJoinPackage(PlayerIndex _number) 
        {
            number = _number;
        }

        public bool run()
        {
            Space394Game.GameInstance.increasePlayers((int)number);
            return true;
        }

        public Package transmit()
        {
            // Do networking things here
            return this;
        }
    }

    internal class PlayerDropPackage : Package
    {
        private PlayerIndex number;

        public PlayerDropPackage(PlayerIndex _number)
        {
            number = _number;
        }

        public bool run()
        {
            Space394Game.GameInstance.decreasePlayers((int)number);
            return true;
        }

        public Package transmit()
        {
            // Do networking things here
            return this;
        }
    }

    internal class ShipMovementPackage : Package
    {
        private int idNumber;
        private Vector3 movement;
        private Vector3 rotation;

        public ShipMovementPackage(int _idNumber, Vector3 _movement, Vector3 _rotation)
        {
            idNumber = _idNumber;
            movement = _movement;
            rotation = _rotation;
        }

        public bool run()
        {
            
            return true;
        }

        public Package transmit()
        {
            // Do networking things here
            return this;
        }
    }
}
