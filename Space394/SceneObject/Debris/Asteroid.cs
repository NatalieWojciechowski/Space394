using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Space394.SceneObjects
{
    public class Asteroid : SceneObject
    {
        public float MAX_HEALTH;

        public Asteroid(long _uniqueId, Vector3 _position, Quaternion _rotation, String _modelFile)
            : base(_uniqueId, _position, _rotation, _modelFile)
        {
            MAX_HEALTH = 10.0f;
            Health = MAX_HEALTH;
        }

        public override void onCollide(SceneObject caller, float damage)
        {
            Health -= damage;
            if (Health <= 0)
            {
                QueuedRemoval = true;
            }
            else { }
        }
    }
}
