using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Space394.SceneObjects
{
    // Triggered when a player enters an area - eg, King of the Hill mode
    public class TriggerArea : SceneObject
    {
        public TriggerArea(long _uniqueId, Vector3 _position, Quaternion _rotation)
            : base(_uniqueId, _position, _rotation, "Models//box")
        {

        }

        public override void onCollide(SceneObject caller, float damage)
        {
            // Do nothing
        }
    }
}
