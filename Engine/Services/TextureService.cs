using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raylib_cs;

namespace RocketEngine
{

	/// <summary>
	/// Texture Service is used to load and unload textures once and not all the time
	/// </summary>

	public static class TextureService
	{
		static Dictionary<string, Texture2D> loadedTextures = new Dictionary<string, Texture2D>();

		private static Texture2D fallBackTexture;

		public static Texture2D LoadTexture(string filePath)
		{
			Texture2D loadedTexture;

			if (loadedTextures.TryGetValue(filePath, out loadedTexture))
			{
				Console.WriteLine("Texture already loaded");
				return loadedTexture;
			}
			else
			{
				loadedTexture = Raylib.LoadTexture(filePath);

				bool textureValid = Raylib.IsTextureValid(loadedTexture);

				if (!textureValid) // if the load texure method fails the Width will be 0 
				{
					loadedTexture = fallBackTexture;
				}

				Raylib.SetTextureFilter(loadedTexture, TextureFilter.Point);
				Console.WriteLine("Texture 2d loaded");
				loadedTextures.Add(filePath, loadedTexture);
				return loadedTexture;
			}
		}

		public static void UnloadAllTextures()
		{
			foreach (Texture2D texture in loadedTextures.Values)
			{
				Raylib.UnloadTexture(texture);	
			}

			loadedTextures.Clear();	
		}

		public static void LoadErrorTexture()
		{
			fallBackTexture = Raylib.LoadTexture("Game/Assets/Textures/Error.png");
		}
	}
}
