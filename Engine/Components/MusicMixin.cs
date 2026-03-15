using Raylib_cs;
using RocketEngine.Scenemanagement;
using RocketEngine.TimeManagement;
using RocketEngine.Utils;
using System.Numerics;

namespace RocketEngine.AudioSystem.Mixin
{
    public class MusicMixin : AudioMixin
    {
        public string FilePath
        {
            get { return base.FilePath; }
            set
            {
                base.FilePath = value;
                MainMusic = Raylib.LoadMusicStream(value);
				Console.ForegroundColor = ConsoleColor.Green;
				Console.WriteLine("new sound: " + value);
				Console.ForegroundColor = ConsoleColor.White;
			}
        }
        private List<Music> musicList = new List<Music>(); // copies of main sound
        private List<Music> musicListRemove = new List<Music>();

        private Music mainMusic;
        private Music MainMusic
        {
            get
            {
                return mainMusic;
            }
            set
            {
                mainMusic = value;
                mainMusic.Looping = false;
                Raylib.SeekMusicStream(mainMusic, 0f);
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
                Raylib.SetMusicPitch(mainMusic, Pitch);
            }
        }
        public float Volume
        {
            get
            {
                return base.Volume;
            }
            set
            {
                base.Volume = value; 
                Raylib.SetMusicVolume(mainMusic, Volume);
            }
        }

        public bool Use3DAudio
        {
            get { return base.Use3DAudio; }
            set
            {
                base.Use3DAudio = value; 
                if (Use3DAudio)
                {
                    Calculate3DMusic(MainMusic);
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
                    CalculateMusicDistance(MainMusic, Volume);
                }
            }
        }
        public MusicMixin(ComponentBase parent, string filePath, bool loop, float volume = 0.5f, float pitch = 1.0f, bool use3DAudio = false, bool useDistanceBasedSound = false, float minDistance = 10, float maxDistance = 300, bool updateDistanceAtRuntime = false) : base(parent) // set all variables here with initial values
        {
            this.MainMusic = Raylib.LoadMusicStream(filePath);

            this.loop = loop;
            this.filePath = filePath;
            Volume = volume;
            Pitch = pitch;
            this.UseDistanceBasedAudio = useDistanceBasedSound;
            distanceRange = new Vector2(minDistance, maxDistance);
            this.updateDistanceAtRuntime = updateDistanceAtRuntime;

            Use3DAudio = use3DAudio;

            onSoundFinished += () => { if (loop) { Play(); Console.WriteLine("LOOP"); } };
            // music loop happens automatically in Raylib
        }

        public override void UpdateAudioMixin(UpdateMode updateMode)
        {
            if (updateMode == UpdateMode.GameTime && Time.TimeScale == 0)
            {
                Raylib.StopMusicStream(mainMusic);

				foreach (Music track in musicList)
				{
					if (Raylib.IsMusicStreamPlaying(track))
					{
						Raylib.StopMusicStream(track);
					}
				}

				return;
            }

            SoundPlaying = Raylib.IsMusicStreamPlaying(this.MainMusic);
            if (SoundPlaying) 
            {
                if (UseDistanceBasedAudio && updateDistanceAtRuntime) CalculateMusicDistance(mainMusic, Volume);
                if (Use3DAudio) Calculate3DMusic(mainMusic);
                timePlaying += Time.DeltaTime;
                Raylib.UpdateMusicStream(mainMusic);
            }
            else
            {
                if (!paused && timePlaying > 0)
                {
                    onSoundFinished?.Invoke();
                }
            }
            foreach(Music track in musicList)
            {
                if (Raylib.IsMusicStreamPlaying(track))
                {
                    if (UseDistanceBasedAudio && updateDistanceAtRuntime) CalculateMusicDistance(track, Volume);
                    if (Use3DAudio) Calculate3DMusic(track);
                    Raylib.UpdateMusicStream(track);
                }
                else
                {
                    musicListRemove.Add(track);
                }
            }
            if(musicListRemove.Count > 0)
            {
                foreach(Music trackToRemove in musicListRemove)
                {
                    musicList.Remove(trackToRemove);
                    Raylib.UnloadMusicStream(trackToRemove); // musicListRemove
                }
                musicListRemove.Clear();
            }
        }

        public override void Play()
        {
            paused = false;
            timePlaying = 0f;
            Raylib.SeekMusicStream(mainMusic, 0f); // start from beginning
           
            if (UseDistanceBasedAudio) CalculateMusicDistance(mainMusic, Volume);
            if (Use3DAudio) Calculate3DMusic(mainMusic);

            Raylib.PlayMusicStream(mainMusic);

            Console.WriteLine("Music is playing");
        }

        public override void Stop()
        {
            Raylib.StopMusicStream(mainMusic);
        }

        public override void Pause(bool pause = true)
        {
            if (pause)
            {
                Raylib.PauseMusicStream(MainMusic);
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
            if (UseDistanceBasedAudio) CalculateMusicDistance(mainMusic, Volume);
            if (Use3DAudio) Calculate3DMusic(mainMusic);
            Raylib.ResumeMusicStream(mainMusic);
        }

        public override void PlayOneShot(float volume = 1, float pitch = 0) //play short sounds, clean up method in Audioservice
        {
            Music musicToPlay = Raylib.LoadMusicStream(filePath);

            musicToPlay.Looping = false;

            Raylib.SetMusicPitch(musicToPlay, pitch);

            if (UseDistanceBasedAudio)
            {
                CalculateMusicDistance(musicToPlay, volume);
                Calculate3DMusic(musicToPlay); // new
            }
            else
            {
                Raylib.SetMusicVolume(musicToPlay, volume);
            }
            if (Use3DAudio) Calculate3DMusic(musicToPlay);

            musicList.Add(musicToPlay);

            Raylib.PlayMusicStream(musicToPlay);
        }

        private void CalculateMusicDistance(Music music, float baseVolume)
        {
            GameObject target = SceneService.ActiveScene.mainCamera.Target;
			float distance = Vector2.Distance(Parent.GetPosition(), target != null ? target.GetPosition() : Vector2.Zero);
            distance = MathF.Abs(distance);

            float multiplier = HelperFunctionsUtils.ReMap(distance, distanceRange.X, distanceRange.Y, 1, 0);
            multiplier = Math.Clamp(multiplier, 0f, 1f);

            float currentDistanceVolume = baseVolume * multiplier;
            Raylib.SetMusicVolume(music, currentDistanceVolume);
        }

        private void Calculate3DMusic(Music music)
        {
			GameObject target = SceneService.ActiveScene.mainCamera.Target;
            Vector2 cameraPosition = target != null ? target.GetPosition() : Vector2.Zero;
            Vector2 soundPosition = Parent.GetPosition();

            Vector2 direction = cameraPosition - soundPosition;
            float distance = direction.Length();

            // sound changes based on players position on the x achsis
            float panX = 0.7f + 0.7f * (direction.X / distanceRange.Y);

            Raylib.SetMusicPan(music, panX);
        }
    }
}
