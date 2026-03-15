using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketEngine
{
	public static class AudioService // only for easier loading and unloading of sounds
	{
		static Dictionary<string, Sound> loadedSounds = new Dictionary<string, Sound>();
        //static Dictionary<string, Music> loadedMusicTracks = new Dictionary<string, Music>();

        public static Sound LoadSound(string filePath)
		{
			Sound loadedSound;

			if(loadedSounds.TryGetValue(filePath, out loadedSound)) // gets value from a dictionary without throwing an exception
			{
				loadedSound = Raylib.LoadSoundAlias(loadedSound);
				return loadedSound;
			}
			else // if not already in dictionary, add 
			{
				loadedSound = Raylib.LoadSound(filePath);
				loadedSounds.Add(filePath, loadedSound);
				loadedSound = Raylib.LoadSoundAlias(loadedSound);
				return loadedSound;
			}
		}

        /*public static Music LoadMusic(string filePath)
        {
            Music loadedMusic;

            if (loadedMusicTracks.TryGetValue(filePath, out loadedMusic))
            {
                loadedMusic = Raylib.LoadMusicStream(filePath);
                return loadedMusic;
            }
            else
            {
                loadedMusicTracks.Add(filePath, loadedMusic); 
                loadedMusic = Raylib.LoadMusicStream(filePath);
                return loadedMusic;
            }
        }*/

        private static void UnloadAllSounds()
		{
			foreach (Sound sound in loadedSounds.Values)
			{
				Raylib.UnloadSound(sound);
			}
			loadedSounds.Clear();
		}

        /*private static void UnloadAllMusic()
        {
            foreach (Music music in loadedMusicTracks.Values)
            {
                Raylib.UnloadMusicStream(music);
            }
            loadedMusicTracks.Clear();
        }*/


        public static void UnloadAllAudioFiles()
		{
			UnloadAllSounds();
            //UnloadAllMusic();
		}
    }
}
