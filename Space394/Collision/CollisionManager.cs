using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Space394.SceneObjects;
using BEPUphysics;
using System.Threading;
using BEPUphysics.CollisionRuleManagement;

namespace Space394.Collision
{
    public class CollisionManager
    {

        private int width;
        public int Width
        {
            get { return width; }
        }
        // No setter

        private int height;
        public int Height
        {
            get { return height; }
        }
        // No setter

        private int depth;
        public int Depth
        {
            get { return depth; }
        }
        // No setter

        private int divisions;
        public int Divisions
        {
            get { return divisions; }
        }
        // No setter

/*#if DEBUG
        private List<SceneObject> drawList;
#endif*/

        private CollisionSector[] sectors;

        private Space space;
        public Space Space
        {
            get { return space; }
        }
        // No setter

        private LinkedList<CollisionBase> toAdd;
        private LinkedList<CollisionBase> toRemove;

        private static ReaderWriterLockSlim threadLock = Locks.GetLockInstance(LockRecursionPolicy.NoRecursion); //setup the lock;
        private static ReaderWriterLockSlim addLock = Locks.GetLockInstance(LockRecursionPolicy.NoRecursion); //setup the lock;
        private static ReaderWriterLockSlim removeLock = Locks.GetLockInstance(LockRecursionPolicy.NoRecursion); //setup the lock;
        
        // _divisions is the divisions in one direction / 2
        public CollisionManager(int _width, int _height, int _depth, int _divisions)
        {
            width = _width;
            height = _height;
            depth = _depth;
            divisions = _divisions * _divisions * _divisions;

            toAdd = new LinkedList<CollisionBase>();
            toRemove = new LinkedList<CollisionBase>();

/*#if DEBUG 
            drawList = new List<SceneObject>();
#endif*/

            space = new Space();
            space.Solver.AllowMultithreading = true;
            space.Solver.IterationLimit = 1;

            Vector3 offset = new Vector3(-_width / 2, -_height / 2, -_depth / 2);

            sectors = new CollisionSector[divisions]; // Though we use _divisions for calculating
            for (int i = 0; i < _divisions; i++)
            {
                for (int j = 0; j < _divisions; j++)
                {
                    for (int l = 0; l < _divisions; l++)
                    {
                        int x = ((i * width) / (2 * _divisions)) + (width / _divisions);
                        int y = ((j * height) / (2 * _divisions)) + (height / _divisions);
                        int z = ((l * depth) / (2 * _divisions)) + (depth / _divisions);
                        sectors[i*_divisions*_divisions + j*_divisions + l] = new CollisionSector(offset + new Vector3(x, y, z), width / _divisions, height / _divisions, depth / _divisions);
                    }
                }
            }
        }

        public void Update(float deltaTime)
        {
            //using (new ReadLock(removeLock))
            {
                //using (new WriteLock(threadLock))
                {
                    foreach (CollisionBase collider in toRemove)
                    {
                        try
                        {
                            space.Remove(collider.getPhysicsCollider());
                        }
                        catch (ArgumentException e) { LogCat.updateValue("To Remove Error", e.Message); }
                    }
                    //using (new WriteLock(removeLock))
                    {
                        toRemove.Clear();
                    }
                }
            }

            //using (new ReadLock(addLock))
            {
                //using (new WriteLock(threadLock))
                {
                    foreach (CollisionBase collider in toAdd)
                    {
                        try
                        {
                            space.Add(collider.getPhysicsCollider());
                        }
                        catch (ArgumentException e) { LogCat.updateValue("To Add Error", e.Message); }
                    }
                    //using (new WriteLock(addLock))
                    {
                        toAdd.Clear();
                    }
                }
            }

            //using (new ReadLock(threadLock))
            {
                space.Update(deltaTime);
            }
            LogCat.updateValue("Colliders", "" + space.Entities.Count);
        }

        public void addToCollisionList(CollisionBase item)
        {
            if (item != null && !item.OnCollisionList)
            {
                try
                {
                    //using (new WriteLock(addLock))
                    {
                        toAdd.AddLast(item);
                        item.OnCollisionList = true;
                    }
                }
                catch (ArgumentException e) { LogCat.updateValue("Error", e.Message);  }
/*#if DEBUG
                if (!drawList.Contains(item))
                {
                    drawList.Add(item);
                }
                else { }
#endif*/
            }
            else { }
            /*for (int i = 0; i < divisions; i++)
            {
                if (sectors[i].isColliding(item))
                {
                    sectors[i].addCollisionItem(item);
                }
                else { }
            }*/
            if (item != null)
            {
                item.HasCollided = false;
            }
            else { }
        }

        public void removeFromCollisionList(CollisionBase item)
        {
            if (item != null && item.OnCollisionList)
            {
                try
                {
                    //using (new WriteLock(removeLock))
                    {
                        toRemove.AddLast(item);
                        item.OnCollisionList = false;
                    }
                }
                catch (ArgumentException e) { LogCat.updateValue("Error", e.Message); }
/*#if DEBUG
                if (drawList.Contains(item))
                {
                    drawList.Remove(item);
                }
                else { }
#endif*/
            }
            else { }
            /*for (int i = 0; i < divisions; i++)
            {
                if (sectors[i].isColliding(item))
                {
                    sectors[i].addCollisionItem(item);
                }
                else { }
            }*/
            if (item != null)
            {
                item.HasCollided = false;
            }
            else { }
        }

        public void clearCollisions()
        {
            space = new Space();
            for (int i = 0; i < divisions; i++)
            {
                sectors[i].clearList();
            }
        }

        public void debugDraw(GameCamera camera)
        {
#if DEBUG
            for (int i = 0; i < divisions; i++)
            {
                sectors[i].debugDraw(camera);
            }
#endif
        }
    }

    public class CollisionSector
    {
        private List<SceneObject> collisionItems;
            
        private CollisionBox box;

        public CollisionSector(Vector3 position, int width, int height, int depth)
        {
            box = new CollisionBox(position, width, height, depth);
            box.StaticColor = true;
            collisionItems = new List<SceneObject>();
        }

        public bool isColliding(SceneObject item)
        {
            return (box.isColliding(item.CollisionBase));
        }

        public void addCollisionItem(SceneObject item)
        {
            collisionItems.Add(item);
        }

        public void clearList()
        {
            collisionItems.Clear();
        }

        public void debugDraw(GameCamera camera)
        {
#if DEBUG
            box.debugDraw(camera);
#endif
        }
    }
}
