using Raylib_cs;
using RocketEngine;
using static RocketEngine.InstanceService;
using static RocketEngine.DrawService;
using static RocketEngine.UpdateService;
using static RocketEngine.TextureService;
using static RocketEngine.AudioService;
using static RocketEngine.Scenemanagement.SceneService;
using System.Numerics;
using static Raylib_cs.Raylib;
using System.ComponentModel;
using RocketEngine.Settings;
using JailBreaker.Player;
using RocketEngine.Scenemanagement;



class Program
{
	class Game
	{
		private bool requestGameClose; // CloseWindow() doesnt work, cause it kills the game loop throwing errors
		public Game() { Construct(); } 

		/// <summary>
		/// Gets called before the game starts 
		/// use this to set up the window and initialization 
		/// </summary>
		public void Construct()
		{

			InitWindow(DisplaySettings.WINDOW_WIDTH, DisplaySettings.WINDOW_HEIGHT, DisplaySettings.GAME_NAME);

			Raylib.SetExitKey(GameSettings.ExitKey);

			EngineSerivce.onGameClosing += () => requestGameClose = true;

			SetTargetFPS(60);

			InitAudioDevice();

			TextureService.LoadErrorTexture();
		}

	




		/// <summary>
		/// This gets called after the Game Instance is Created
		/// </summary>
		public void Start()
		{
			SceneService.LoadSceneByIndex(0);

			Run();
		}


		/// <summary>
		/// Gets Called every Frame after Input and Update
		/// </summary>

		public void Run()
		{
			while (!WindowShouldClose() && !requestGameClose) 
			{
				if (ActiveScene != null)
				{
					SceneService.ActiveScene.RunScene();
				}
			}

			DeInitialize();
		}

		/// <summary>
		/// Gets Called when the Game shuts down
		/// Use this for Closing Devices and Unloading assets
		/// </summary>
		public void DeInitialize()
		{
			// gameobjects.CleanUp(); // ich stell mir ne base class for wo man das bei allen called

			Console.WriteLine("DeInitialize");

			// unload sounds and textures here
			//...
			//
			UnloadAllTextures();

			UnloadAllAudioFiles();

			CloseAudioDevice();

			CloseWindow();
		}
	}


	public static void Main()
	{
        Game game = new Game();
		game.Start();
	}

}