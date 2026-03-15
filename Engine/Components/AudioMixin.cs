using Raylib_cs;
using RocketEngine.TimeManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RocketEngine.AudioSystem.Mixin
{
    public abstract class AudioMixin //to do: summay for all methods !!!
    {
        #region Audio attributes
        protected string filePath;
        public string FilePath
        {
            get { return filePath; }
            set
            {
                filePath = value;
			}
        }
        protected float timePlaying;
        private float pitch = 0;
        public float Pitch
        {
            get { return pitch; }
            set { pitch = value; }
        }
        private float volume = 1;
        public float Volume
        {
            get { return volume; }
            set { volume = value; }
        }
        public bool loop;
        protected bool paused;

        public Vector2 distanceRange;
        public bool updateDistanceAtRuntime;
        private bool useDistanceBasedAudio;
        public bool UseDistanceBasedAudio
        {
            get { return useDistanceBasedAudio; }
            set { useDistanceBasedAudio = value; }  
        }

        public Action onSoundFinished;
        public bool SoundPlaying
        {
            get;
            protected set;
        }
        private bool use3DAudio;

        public bool Use3DAudio
        {
            get { return use3DAudio; }
            set { use3DAudio = value; }
        }

        public ComponentBase Parent { get; set; }

        #endregion

        #region Audio Methods
        public AudioMixin(ComponentBase parent) { Parent = parent; } // to do: anschauen

        public virtual void UpdateAudioMixin(UpdateMode updateMode) { }
        public virtual void Play() { }
        public virtual void Stop() { }
        public virtual void Pause(bool pause = true) { }
        public virtual void Resume() { }
        public virtual void PlayOneShot(float volume = 1, float pitch = 0) { }
        //public virtual void DistanceBasedSound() { }
        //public virtual void Calculate3DAudio() { }
        #endregion
    }
}
