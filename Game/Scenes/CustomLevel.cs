using JailBreaker.Data;
using JailBreaker.Destructibles;
using JailBreaker.Game.Ui;
using JailBreaker.Interactibles;
using JailBreaker.Player;
using JailBreaker.Ui;
using Raylib_cs;
using RocketEngine;
using RocketEngine.Scenemanagement;
using RocketEngine.Ui;
using RocketEngine.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using JailBreaker.Props;
using JailBreaker.Game.TextTriggers;
using JailBreaker.Game.Destructibles;
using RocketEngine.DataStore;
using JailBreaker.Game.Interactibles.WeaponPickUps;

namespace JailBreaker.Scenes 
{
    public class CustomLevel : Scene
    {

		LaniasPlayer player;
		ScreenFade fade;
	    PauseMenuPanel pauseMenu;
		public CustomLevel(string name = "CustomLevel") : base(name) { }

		/// <summary>
		/// gets called once at the beginning when the scene is created
		/// </summary>
		/// 

		public override void CreateScene()
        {
            base.CreateScene();

			#region Exit Fade

			fade = InstanceService.Instantiate(new ScreenFade());
			fade.onFadeInFinished += () =>
			{
				SaveSystem.SaveData("Level", 1);
				SceneService.LoadSceneByName("Level1");
			};
			fade.Time = 1.0f;
			fade.Alpha = 1.0f; // black screen at the beginning

			#endregion

			#region Load Tilemaps

			// local path
			Tilemap backgroundTM = InstanceService.Instantiate(new Tilemap(new JailBreaker.WallsTileSheet(), "CustomTilemapBG.json", rows: 50, columns: 100));
			Tilemap groundTMNoColision = InstanceService.Instantiate(new Tilemap(new JailBreaker.PlatformsTileSheet(), "CustomTilemapNoCollion.json", rows: 50, columns: 100));
			Tilemap groundTM = InstanceService.Instantiate(new Tilemap(new JailBreaker.PlatformsTileSheet(), "CustomTilemap.json", rows: 50, columns: 100, hasCollison: true));

			backgroundTM.TintTilemap(Color.LightGray);


			#endregion

			#region Load Prefabs

			LoadPrefabs("CustomPrefabs.json");

			#endregion

			#region Level Audio

			GameObject audioholder = InstanceService.Instantiate(new GameObject());
			AudioComponent level1music = new AudioComponent(audioholder, "Game/Assets/Audio/Music/JailbreakerBeat.mp3", true, .3f);
			level1music.Play();

			#endregion

			#region Camera Shake

			GameObject cameraShakeListenerHolder = InstanceService.Instantiate(new GameObject());
            CameraShakeListener cameraShakeListener = new CameraShakeListener(cameraShakeListenerHolder);

			#endregion

			// player needs to be at the bottom
			#region Player

			//player = InstanceService.Instantiate(new LaniasPlayer());
			player = (LaniasPlayer)(SceneService.FindFirstInstantiablesWithName("Player"));

			if (player != null)
			{
				player.walkForce = 100;
			}

			#endregion

			fade.FadeOut();

			#region Camera Settings

			mainCamera.Target = player;
			mainCamera.UseCameraBorders = false;
			mainCamera.BorderX = new Vector2(350, 1200);
			mainCamera.BorderY = new Vector2(430, 630);
			mainCamera.Offset = new Vector2(0, 10);

			#endregion
		}

		public override void UpdateScene()
		{
			base.UpdateScene();

			if(Raylib.IsKeyPressed(KeyboardKey.Escape))
			{
				if (pauseMenu == null)
				{
					pauseMenu = InstanceService.Instantiate(new PauseMenuPanel());
					pauseMenu.OnDestroyed += () => pauseMenu = null;
					GameManager.GamePaused = true;
					GameManager.ShowCursor = true;
				}
				else
				{
					GameManager.ShowCursor = false;
					GameManager.GamePaused = false;
				}
			}
		}

	}
}
