using Raylib_cs;
using RocketEngine.Scenemanagement;
using RocketEngine.TimeManagement;
using RocketEngine.Utils;
using System.Numerics;

namespace RocketEngine.AudioSystem.Mixin
{
    public class SoundMixin : AudioMixin
    {
        private Sound[] soundClones = new Sound[5]; // copies of main sound
        int soundIndex;
        public string FilePath
        {
            get { return base.FilePath; }
            set
            {
                base.FilePath = value;
                MainSound = AudioService.LoadSound(value);
                Console.ForegroundColor = ConsoleColor.Green;
				Console.WriteLine("new sound: " + value);
				Console.ForegroundColor = ConsoleColor.White;
			}
        }
        private Sound mainSound;
        public Sound MainSound
        {
            get
            {
                return mainSound;
            }
            set
            {
                mainSound = value;
                FillSoundPool(mainSound); // clone sound 5 times
            }
        }

        // variables to make sound sound natural
        public float Pitch
        {
            get
            {
                return base.Pitch;
            }
            set
            {
                base.Pitch = value;
                Raylib.SetSoundPitch(mainSound, Pitch);
            }
        }
        public float Volume // Volume of MainSound
        {
            get
            {
                return base.Volume;
            }
            set
            {
                base.Volume = value;
                Raylib.SetSoundVolume(mainSound, Volume);
            }
        }
        public bool Use3DAudio
        {
            get
            {
                return base.Use3DAudio;
            }
            set
            {
                base.Use3DAudio = value; // later set volume range
                if(Use3DAudio)
                {
                    Calculate3DAudio(mainSound);
                }
            }
        }
        public bool UseDistanceBasedAudio
        {
            get { return base.UseDistanceBasedAudio; }
            set
            {
                base.UseDistanceBasedAudio = value;
                if (UseDistanceBasedAudio)
                {
                    CalculateSoundDistance(MainSound, Volume);
                }
            }
        }

        public SoundMixin(ComponentBase parent, string filePath, bool loop = false, float volume = 0.5f, float pitch = 1.0f, bool use3DAudio = false, bool useDistanceBasedSound = true, float minDistance = 10, float maxDistance = 300, bool updateDistanceAtRuntime = false) : base(parent) // set all variables here in Construct with initial values
        {
            this.MainSound = AudioService.LoadSound(filePath);

            Volume = volume;
            Pitch = pitch;
            this.filePath = filePath;
            this.UseDistanceBasedAudio = useDistanceBasedSound;
            distanceRange = new Vector2(minDistance, maxDistance);
            this.updateDistanceAtRuntime = updateDistanceAtRuntime; 
            this.loop = loop;
            Use3DAudio = use3DAudio;

            onSoundFinished += () => { if (loop) { Play(); Console.WriteLine("LOOP"); } };
        }

        public override void UpdateAudioMixin(UpdateMode updateMode)
		{
			if (updateMode == UpdateMode.GameTime && Time.TimeScale == 0)
            {
                Raylib.PauseSound(MainSound);
				return;
			}


			// CheckSoundFinished();
			SoundPlaying = Raylib.IsSoundPlaying(mainSound);
            if (SoundPlaying)
            {
                if (UseDistanceBasedAudio && updateDistanceAtRuntime) CalculateSoundDistance(mainSound, Volume);
                if(Use3DAudio) Calculate3DAudio(mainSound);
                timePlaying += Time.DeltaTime;  
            }
            else
            {
                if(!paused && timePlaying > 0)
                {
                    onSoundFinished?.Invoke();
                }
            }
        }

        private void FillSoundPool(Raylib_cs.Sound sound) // actual cloning
        {
            for (int i = 0; i < soundClones.Length; i++)
            {
                soundClones[i] = Raylib.LoadSoundAlias(sound);
            }
        }

        public override void Play()
        {
            paused = false;
            timePlaying = 0f;
            if (UseDistanceBasedAudio)
            {
                CalculateSoundDistance(mainSound, Volume);
                Calculate3DAudio(mainSound); 
            }
          
            if (UseDistanceBasedAudio) CalculateSoundDistance(mainSound, Volume);
            if (Use3DAudio) Calculate3DAudio(mainSound);
            Raylib.PlaySound(MainSound);
        }

        public override void Stop()
        {
            Raylib.StopSound(MainSound);
        }

        public override void Pause(bool pause = true)
        {
            if (pause)
            {
                Raylib.PauseSound(MainSound);
                paused = true;
            }
            else
            { 
                paused = false;
                Resume();
                Console.WriteLine("continue");
            }
            Console.WriteLine("Stopped sound");
        }
        public override void Resume()
        {
            if (!paused) return;
            if (UseDistanceBasedAudio)
            {
                CalculateSoundDistance(mainSound, Volume);
                Calculate3DAudio(mainSound); // new
            }
            if (UseDistanceBasedAudio) CalculateSoundDistance(mainSound, Volume);
            if (Use3DAudio) Calculate3DAudio(mainSound);
            Raylib.ResumeSound(MainSound);
        }

        public override void PlayOneShot(float volume = 1, float pitch = 0) //play short sounds, clean up method in Audioservice
        {
            Sound soundToPlay = soundClones[soundIndex];

            Raylib.SetSoundPitch(soundToPlay, pitch);

            if (UseDistanceBasedAudio)
            {
                CalculateSoundDistance(soundToPlay, volume);
                Calculate3DAudio(soundToPlay); // new
            }
            else
            {
                Raylib.SetSoundVolume(soundToPlay, volume);
            }
            if (Use3DAudio) Calculate3DAudio(soundToPlay);
            Raylib.PlaySound(soundToPlay);
            soundIndex++;

            if (soundIndex >= soundClones.Length)
            {
                soundIndex = 0;
            }
        }
        /*public void CheckSoundFinished() // for mainSound and soundClones
        {
            if (Raylib.IsSoundPlaying(MainSound))
            {
                isPlaying = true;
            }
            else if (isPlaying) // only call event once when sound stops playing
            {
                isPlaying = false;
                onSoundFinished?.Invoke();
            }
        }*/

        public void CalculateSoundDistance(Sound sound, float baseVolume)
        {
			GameObject target = SceneService.ActiveScene.mainCamera.Target;
			float distance = Vector2.Distance(Parent.GetPosition(), target != null ? target.GetPosition() : Vector2.Zero);
            distance = MathF.Abs(distance);

            float multiplier = HelperFunctionsUtils.ReMap(distance, distanceRange.X, distanceRange.Y, 1, 0);
            multiplier = Math.Clamp(multiplier, 0f, 1f);

            float currentDistanceVolume = baseVolume * multiplier;
            Raylib.SetSoundVolume(sound, currentDistanceVolume);
        }

        public void Calculate3DAudio(Sound sound)
        {
			/*Raylib.SetSoundPan(mainSound, 0); //1: left ear, 0: right ear
            Vector2 cameraPosition = SceneService.ActiveScene.mainCamera.Target;

            Vector2 soundPosition = Parent.GetPosition();

            // direction from camera to sound
            Vector2 direction = soundPosition - cameraPosition;

            // viewing direction of camera, e.g. to the right = 1,0
            Vector2 cameraForward = new Vector2(1, 0);

            // Dot-Product: how strong is the direction of sound
            float dot = Vector2.Dot(Vector2.Normalize(cameraForward), Vector2.Normalize(direction));

            // Cross-Product: to the left or to the right?
            float side = cameraForward.X * direction.Y - cameraForward.Y * direction.X;

            // -1: sound from left, 1: sound from right
            float pan = dot;//Math.Clamp(side, -1f, 1f);

            Raylib.SetSoundPan(mainSound, pan);

            Console.WriteLine($"dot={dot}, pan={pan}");*/


			GameObject target = SceneService.ActiveScene.mainCamera.Target;
			Vector2 cameraPosition = target != null ? target.GetPosition() : Vector2.Zero;
            Vector2 soundPosition = Parent.GetPosition();

            Vector2 direction = cameraPosition - soundPosition;
            float distance = direction.Length();

            // sound changes based on players position on the x achsis
            float panX = 0.7f + 0.7f * (direction.X/ distanceRange.Y);

            Raylib.SetSoundPan(sound, panX);
        }
    }
}
