using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Space394.SceneObjects
{
    public class Flag : CollectibleItem
    {
        public Flag(long _uniqueId, Vector3 _position, Quaternion _rotation)
            : base(_uniqueId, _position, _rotation, "Models\\bigbox")
        {

        }

        public override void onCollide(SceneObject caller, float damage)
        {
            // Get picked up
        }
    }
}
