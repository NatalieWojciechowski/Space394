using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using System.IO;
using Microsoft.Xna.Framework;

namespace Space394
{
    public static class SoundManager
    {
        // Records if the game is muted, its initial value - the one created when no file is found, is false - meaning sound will play
        private static bool isMuted;
        public static bool IsMuted
        {
            get { return SoundManager.isMuted; }
            set 
            {
                if (noAudio)
                {
                    value = true;
                }
                else
                {
                    // Sets muted to the passed in value and attempts to save it to file
                    try
                    {
                        byte bValue;
                        if (value) { bValue = 0; } // Converts the bool to a byte value for writing
                        else { bValue = 1; }
                        using (fs = new FileStream("Content/settings.orx", FileMode.OpenOrCreate, FileAccess.Write))
                        {
                            fs.WriteByte(bValue); // Writes the muted value
                        }
                    }
                    catch (IOException e) // If we have an error, print it
                    {
                        Console.Write(e.ToString());
                    }
                }
                isMuted = value;
            }
        }
        public static bool toggleIsMuted() { return (IsMuted = (!isMuted)); } // Short-hand for toggling

        // The sound effects
        //private static SoundEffectInstance laserFire;

        // The file for storing the mute state
        private static FileStream fs;

        private static bool noAudio = false;

        private static AudioEmitter emitter;
        private static AudioListener listener;

        private static SoundEffect menu_music;
        private static SoundEffect game_music;
        private static SoundEffectInstance musicInstance;
        private static bool playingMusic;
        public static bool PlayingMusic
        {
            get { return playingMusic; }
        }

        // Checks for the file and loads the muted state or defaults it to false
        // Loads all the sounds
        public static void Initialize()
        {
            try
            {
                using (fs = new FileStream("Content/settings.stx", FileMode.OpenOrCreate, FileAccess.Read))
                {
                    isMuted = ((fs.ReadByte()) == 0);
                }
            }
            catch (IOException e)
            {
                Console.Write(e.ToString());
                // Their loss
                isMuted = false;
            }

            try
            {
                menu_music = Space394Game.GameInstance.Content.Load<SoundEffect>("Sounds/Music/music_title");
                game_music = Space394Game.GameInstance.Content.Load<SoundEffect>("Sounds/Music/music_battle");
                /*Stream soundFile;
                SoundEffect soundEffect;
                soundFile = TitleContainer.OpenStream("Content/Sounds/laser_fire.xnb");
                soundEffect = SoundEffect.FromStream(soundFile);
                laserFire = soundEffect.CreateInstance();*/
            }
            catch (NoAudioHardwareException e)
            {
                LogCat.updateValue("Error", e.ToString());
                isMuted = true;
                noAudio = true;
            }

            playingMusic = false;

            emitter = new AudioEmitter();
            listener = new AudioListener();
        }

        public static void setListenerLocation(Vector3 position)
        {
            listener.Position = position;
        }

        // Closes the file
        public static void UnInitialize()
        {
            try
            {
                fs.Close();
                fs.Dispose();
            }
            catch (IOException) { }
        }

        public static void StopMusic()
        {
            if (musicInstance != null)
            {
                musicInstance.Stop();
                playingMusic = false;
            }
            else { }
        }

        public static void StartMenuMusic()
        {
            if (!isMuted)
            {
                musicInstance = menu_music.CreateInstance();
                musicInstance.IsLooped = true;
                musicInstance.Play();
                playingMusic = true;
            }
            else { }
        }

        public static void StartGameMusic()
        {
            if (!isMuted)
            {
                musicInstance = game_music.CreateInstance();
                musicInstance.IsLooped = true;
                musicInstance.Play();
                playingMusic = true;
            }
            else { }
        }

        // Plays the laser firing noise
        public static void playLaserFire(Vector3 position)
        {
            if (!isMuted)
            {
                /*emitter.Position = position;
                laserFire.Apply3D(listener, emitter);
                laserFire.Play();*/
            }
            else { }
        }
    }
}

