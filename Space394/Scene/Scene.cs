using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections;
using Space394.SceneObjects;
using Space394.Particles;

namespace Space394.Scenes
{
    public abstract class Scene
    {
        private GameCamera camera;
        public GameCamera Camera
        {
            get { return camera; }
        }
        // Setter needs work

        protected bool readyToExit = false;
        public bool ReadyToExit
        {
            get { return readyToExit; }
        }

        protected const int MAX_PLAYERS = 4;

        private bool isPaused = false;
        public bool IsPaused
        {
            get { return isPaused; }
            set { isPaused = value; }
        }
        public bool Pause() { isPaused = true; return isPaused; }
        public bool UnPause() { isPaused = false; return isPaused; }

        private Dictionary<int, SceneObject> sceneObjects;
        public Dictionary<int, SceneObject> SceneObjects
        {
            get { return sceneObjects; }
        }
        public virtual SceneObject addSceneObject(SceneObject add) { queuedForAdd.Add(add); return add; }
        public virtual void removeSceneObject(SceneObject remove) 
        { 
            if (sceneObjects.ContainsKey(remove.GetHashCode()))
            {
                queuedForRemoval.Add(remove);
            }
            else { }
        }

        public void removeSceneObject(int removeKey) 
        {
            if (sceneObjects.ContainsKey(removeKey))
            {
                queuedForRemoval.Add(sceneObjects[removeKey]);
            }
            else { } 
        }

        public SceneObject getSceneObject(int key) 
        { 
            if (sceneObjects.ContainsKey(key)) 
            { 
                return sceneObjects[key]; 
            }
            else
            {
                return null;
            }
        }

        private List<SceneObject> queuedForAdd;
        private List<SceneObject> queuedForRemoval;

        protected Scene()
        {
            camera = new GameCamera(this, new Vector3(0.0f, 50.0f, 0.0f), new Quaternion(), Space394Game.GameInstance.AspectRatio);
            sceneObjects = new Dictionary<int, SceneObject>();
            queuedForAdd = new List<SceneObject>();
            queuedForRemoval = new List<SceneObject>();
        }

        public virtual void Initialize()
        {
            
        }

        public virtual void Update(float deltaTime)
        {
            // if (!isPaused)
            //{
            LogCat.updateValue("AddObjects", "" + queuedForAdd.Count);

            foreach (SceneObject item in queuedForAdd)
            {
                if (!sceneObjects.ContainsKey(item.GetHashCode()))
                {
                    sceneObjects.Add(item.GetHashCode(), item);
                    item.onAddToScene(this);
                }
                else { }
            }
            queuedForAdd.Clear();

            LogCat.updateValue("SceneObjects", "" + sceneObjects.Count);

            foreach (SceneObject item in sceneObjects.Values)
            {
                if (item.Active)
                {
                    item.Update(deltaTime);
                }
                else { }
                if (item.QueuedRemoval)
                {
                    queuedForRemoval.Add(item);
                }
                else { }
            }

            LogCat.updateValue("RemoveObjects", "" + queuedForRemoval.Count);

            foreach (SceneObject item in queuedForRemoval)
            {
                sceneObjects.Remove(item.GetHashCode());
            }
            queuedForRemoval.Clear();

            foreach (Player player in Space394Game.GameInstance.Players)
            {
                if (player != null)
                {
                    player.Update(deltaTime);
                }
                else { }
            }
            // }
            // else { }
        }

        public virtual void Draw()
        {
            foreach (Player player in Space394Game.GameInstance.Players)
            {
                if (player != null)
                {
                        player.Draw(sceneObjects);
                } else {}
            }
            camera.Draw(null);
            if (isPaused)
            {
                camera.DrawPaused();
            }
            else { }
        }

        public virtual void Exit(Scene nextScene)
        {
            ContentLoadManager.clear();
        }

        protected GraphicsDevice getGraphics()
        {
            return Space394Game.GameInstance.GraphicsDevice;
        }

        public abstract Space394Game.sceneEnum GetNextScene();

        public void handSceneObjects(Dictionary<int, SceneObject> newList)
        {
            sceneObjects = newList;
            foreach (SceneObject sceneObject in sceneObjects.Values)
            {
                sceneObject.onAddToScene(this);
            }
        }

        public virtual void ForceReadyToExit()
        {
            AbortThreads();
            readyToExit = true;
        }

        public virtual void AbortThreads()
        {

        }

        public virtual void addPlayer(Player player)
        {

        }

        public virtual void removePlayer(Player player)
        {

        }
    }
}
