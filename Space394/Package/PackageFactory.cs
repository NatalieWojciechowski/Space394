using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Space394.Packaging
{
    public static class PackageFactory
    {
        public static void Initialize()
        {
            // Do any initializing it might need
        }

        public static void createPlayerJoinPackage(PlayerIndex number)
        {
            Space394Game.GameInstance.addPackage((new PlayerJoinPackage(number)).transmit());
        }

        public static void createPlayerDropPackage(PlayerIndex number)
        {
            Space394Game.GameInstance.addPackage((new PlayerDropPackage(number)));
        }

        public static void createShipMovePackage(int idNumber, Vector3 movement, Vector3 rotation)
        {
            Space394Game.GameInstance.addPackage((new ShipMovementPackage(idNumber, movement, rotation)));
        }
    }
}
