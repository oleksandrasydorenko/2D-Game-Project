using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using RocketEngine.AudioSystem.Mixin;

namespace RocketEngine
{
    public class AudioComponent : InstantiableComponent, IUpdatable
    {
        AudioMixin currentMixin;

		#region Audio attributes

		public TimeManagement.UpdateMode updateMode;

		private string filePath;
        public string FilePath
        {
            get { return filePath; }
            set
            {
                filePath = value;
                if(isMusic)
                {
					((MusicMixin)currentMixin).FilePath = filePath;
				}
                else
                {
					((SoundMixin)currentMixin).FilePath = filePath;
				}
             
            }
        }
        private float pitch = 0;
        public float Pitch
        {
            get { return pitch; }
            set { pitch = value;
                if(isMusic)
                {
					((MusicMixin)currentMixin).Pitch = pitch;
				}
                else
                {
					((SoundMixin)currentMixin).Pitch = pitch;
				}  
            }
        }
        private float volume = 1;
        public float Volume
        {
            get { return volume; }
            set { volume = value;

                if(isMusic)
                {
                    ((MusicMixin)currentMixin).Volume = volume;
                }
                else
                {
					((SoundMixin)currentMixin).Volume = volume;
				}
            }
        }
        private bool loop;
        public bool Loop
        {
            get { return loop; }
            set
            {
                loop = value;

				if (isMusic)
				{
					((MusicMixin)currentMixin).loop = loop;
				}
				else
				{
					((SoundMixin)currentMixin).loop = loop;
				}
			}
        }

        private Vector2 distanceRange;
        public Vector2 DistanceRange
        {
            get { return distanceRange; }
            set
            {
                distanceRange = value;  

                if(isMusic)
                {
                    ((MusicMixin)currentMixin).distanceRange = distanceRange;
				}
                else
                {
					((SoundMixin)currentMixin).distanceRange = distanceRange;
				}
            }
        }

        private bool updateDistanceAtRuntime;
        public bool UpdateDistanceAtRuntime
        {
            get { return updateDistanceAtRuntime; }
            set
            {
                updateDistanceAtRuntime = value;

				if (isMusic)
				{
					((MusicMixin)currentMixin).updateDistanceAtRuntime = updateDistanceAtRuntime;
				}
				else
				{
					((SoundMixin)currentMixin).updateDistanceAtRuntime = updateDistanceAtRuntime;
				}
			}
        }

        private bool useDistanceBasedAudio;
        public bool UseDistanceBasedAudio
        {
            get { return useDistanceBasedAudio; }
            set { 
                useDistanceBasedAudio = value;

				if (isMusic)
				{
					((MusicMixin)currentMixin).UseDistanceBasedAudio = useDistanceBasedAudio;
				}
				else
				{
					((SoundMixin)currentMixin).UseDistanceBasedAudio = useDistanceBasedAudio;
				}
            }
        }

        public Action onSoundFinished;
        public bool SoundPlaying
        {
            get{
                return currentMixin.SoundPlaying;
            }
        }

        private bool use3DAudio;

        public bool Use3DAudio
        {
            get { return use3DAudio; }
            set { use3DAudio = value;


				if (isMusic)
				{
					((MusicMixin)currentMixin).Use3DAudio = use3DAudio;
				}
				else
				{
					((SoundMixin)currentMixin).Use3DAudio = use3DAudio;
				}
            }
        }

        bool isMusic;

        #endregion
        public AudioComponent(ComponentBase parent, string filePath, bool loop=false, float volume = 0.5f, float pitch = 1.0f, bool use3DAudio = false, bool useDistanceBasedSound = false, float minDistance = 10, float maxDistance = 300, bool updateDistanceAtRuntime = false, string name = "AudioComponent") : base(parent, name) // set all variables here with initial values
        {
            Music music = Raylib.LoadMusicStream(filePath);
            if (Raylib.GetMusicTimeLength(music) > 10)
            {
                isMusic = true;
                currentMixin = new MusicMixin(parent, filePath, loop, volume, pitch, use3DAudio, useDistanceBasedSound, minDistance, maxDistance, updateDistanceAtRuntime);
                
            }
            else
            {
                isMusic = false;
                currentMixin = new SoundMixin(parent, filePath, loop, volume, pitch, use3DAudio, useDistanceBasedSound, minDistance, maxDistance, updateDistanceAtRuntime);
            }
            currentMixin.onSoundFinished += () => onSoundFinished?.Invoke();


			this.filePath = filePath;
        }
        public void Update()
        {
            currentMixin.UpdateAudioMixin(updateMode);
        }

        public void Play()
        {
            currentMixin.Play();
        }

        public void Stop()
        {
            currentMixin.Stop();
        }

        public void Pause(bool pause = true)
        {
            currentMixin.Pause(pause);
        }

        public void Resume() { currentMixin.Resume(); }
        public void PlayOneShot(float volume = .5f, float pitch = 0) { currentMixin.PlayOneShot(volume, pitch); }
    }
}
