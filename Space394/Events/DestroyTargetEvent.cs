using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Space394.SceneObjects;
using Microsoft.Xna.Framework;

namespace Space394.Events
{
    public class DestroyTargetEvent : Event
    {
        protected List<SceneObject> targets;

        public DestroyTargetEvent(List<SceneObject> _targets)
            : base()
        {
            targets = _targets;
            HasPosition = true;

            objectiveText = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/current_objective_box"), new Vector2(632, 33));

            Positions = new List<Vector3>();
            for (int i = 0; i < targets.Count; i++)
            {
                Positions.Add(targets[i].Position);
            }
        }

        public override void Update(float deltaTime)
        {
            for (int i = 0; i < targets.Count; )
            {
                if (targets[i].Health <= 0)
                {
                    Positions.RemoveAt(i);
                    targets.RemoveAt(i);
                }
                else { i++; }
            }
            if (targets.Count == 0)
            {
                completed = true;
            }
            else { }
            for (int i = 0; i < targets.Count; i++)
            {
                Positions[i] = targets[i].Position;
            }
        }

        public override bool IsComplete()
        {
            return completed;
        }
    }
}
