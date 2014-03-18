using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Space394.SceneObjects;
using BEPUphysics;
using BEPUphysics.Collidables.MobileCollidables;
using BEPUphysics.Collidables;
using BEPUphysics.NarrowPhaseSystems.Pairs;
using BEPUphysics.Collidables.Events;
using BEPUphysics.CollisionRuleManagement;

namespace Space394.Collision
{
    public abstract class CollisionBase
    {
        public enum collider_type
        {
            fighter = 0,
            capital = 1,
            other = 2
        };

        private bool onCollisionList = false;
        public bool OnCollisionList
        {
            get { return onCollisionList; }
            set { onCollisionList = value; }
        }

        public abstract ISpaceObject getPhysicsCollider();

        private collider_type colliderType;
        public collider_type ColliderType
        {
            get { return colliderType; }
            set { colliderType = value; }
        }

        private SceneObject parent;
        public SceneObject Parent
        {
            get { return parent; }
            set { parent = value; }
        }

        private bool active = true;
        public virtual bool Active
        {
            get { return active; }
            set { active = value; }
        }

        private Vector3 position;
        public virtual Vector3 Position
        {
            get { return position; }
            set { position = value; }
        }

        protected VertexPositionColor[] points;
        protected short[] lineListIndices;
        
        protected VertexPositionColor[] pointList;
        protected VertexBuffer vertexBuffer;

        private bool hasCollided = false;
        public bool HasCollided
        {
            get { return hasCollided; }
            set { hasCollided = value; }
        }

        private bool staticColor = false;
        public bool StaticColor
        {
            get { return staticColor; }
            set { staticColor = value; }
        }

        public struct graphics
        {
            public static bool initialized = false;
            public static BasicEffect basicEffect;
            public static RasterizerState rasterizerState;
            public static VertexDeclaration vertexDeclaration;
        };

        private static CollisionGroup esxolusGroup;
        public static CollisionGroup EsxolusGroup
        {
            get { return CollisionBase.esxolusGroup; }
        }
        private static CollisionGroup halkGroup;
        public static CollisionGroup HalkGroup
        {
            get { return CollisionBase.halkGroup; }
        }

        public CollisionBase()
        {
            if (!graphics.initialized)
            {
                graphics.rasterizerState = new RasterizerState();
                graphics.rasterizerState.FillMode = FillMode.WireFrame;
                graphics.rasterizerState.CullMode = CullMode.None;

                graphics.vertexDeclaration = new VertexDeclaration(new VertexElement[]
                    {
                        new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
                        new VertexElement(12, VertexElementFormat.Color, VertexElementUsage.Color, 0)
                    }
                );

                graphics.basicEffect = new BasicEffect(Space394Game.GameInstance.GraphicsDevice);
                graphics.basicEffect.VertexColorEnabled = true;

                graphics.initialized = true;

                esxolusGroup = new CollisionGroup();
                halkGroup = new CollisionGroup();
                CollisionGroupPair pairE = new CollisionGroupPair(esxolusGroup, esxolusGroup);
                CollisionRules.CollisionGroupRules.Add(pairE, CollisionRule.NoBroadPhase);
                CollisionGroupPair pairH = new CollisionGroupPair(halkGroup, halkGroup);
                CollisionRules.CollisionGroupRules.Add(pairH, CollisionRule.NoBroadPhase);
            }
            else { }
        }

        protected virtual void onCollide(EntityCollidable eCollidable, Collidable collidable, CollidablePairHandler pairHandler)
        {
            LogCat.updateValue("OnCollide", "Success");
        }

        public abstract bool isColliding(CollisionBase collider);

        public abstract void debugDraw(GameCamera camera);

        public abstract void addCollisionEvent(InitialCollisionDetectedEventHandler<EntityCollidable> _function);
    }
}
