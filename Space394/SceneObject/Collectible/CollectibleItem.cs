using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Space394.SceneObjects
{
    public abstract class CollectibleItem : SceneObject
    {
        public CollectibleItem(long _uniqueId, Vector3 _position, Quaternion _rotation, String _modelFile)
            : base(_uniqueId, _position, _rotation, _modelFile)
        {

        }
    }
}
