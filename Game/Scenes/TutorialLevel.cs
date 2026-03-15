using JailBreaker.Data;
using JailBreaker.Destructibles;
using JailBreaker.Game.Classes.Weapons;
using JailBreaker.Game.Destructibles;
using JailBreaker.Game.Interactibles.WeaponPickUps;
using JailBreaker.Game.TextTriggers;
using JailBreaker.Game.Ui;
using JailBreaker.Interactibles;
using JailBreaker.Player;
using JailBreaker.Props;
using JailBreaker.Ui;
using Raylib_cs;
using RocketEngine;
using RocketEngine.DataStore;
using RocketEngine.Scenemanagement;
using RocketEngine.Ui;
using RocketEngine.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace JailBreaker.Scenes 
{
    public class TutorialLevel : Scene
    {

		LaniasPlayer player;
		ScreenFade fade;
		PauseMenuPanel pauseMenu;
		public TutorialLevel(string name = "Tutorial") : base(name) { }

		/// <summary>
		/// gets called once at the beginning when the scene is created
		/// </summary>
		/// 

		public override void CreateScene()
        {
            base.CreateScene();

			fade = InstanceService.Instantiate(new ScreenFade());
			fade.onFadeInFinished += () =>
			{
				SaveSystem.SaveData("Level", 1);
				SceneService.LoadSceneByName("Level1");
                if (player != null)
                {

                    SaveSystem.SaveData("CurrentWeapon", (int)player.weapons.currentWeapon.type);
                    SaveSystem.SaveData("SecondWeapon", (int)player.weapons.secondaryWeapon);

                }
            };
			fade.Time = 1.0f;
			fade.Alpha = 1.0f; // black screen at the beginning
	

			#region Load Tilemaps
			// real path
			Tilemap backgroundTM = InstanceService.Instantiate(new Tilemap(new JailBreaker.WallsTileSheet(), "Game/Assets/Tilemaps/Tutorial/TutorialLevelTilemapBG.json", rows: 50, columns: 120));
			Tilemap groundTMNoColision = InstanceService.Instantiate(new Tilemap(new JailBreaker.PlatformsTileSheet(), "Game/Assets/Tilemaps/Tutorial/TutorialLevelTilemapNoCollion.json", rows: 50, columns: 120));
			Tilemap groundTM = InstanceService.Instantiate(new Tilemap(new JailBreaker.PlatformsTileSheet(), "Game/Assets/Tilemaps/Tutorial/TutorialLevelTilemap.json", rows: 50, columns: 120, hasCollison: true));

			// local path
			//Tilemap backgroundTM = InstanceService.Instantiate(new Tilemap(new JailBreaker.WallsTileSheet(), "Game/Assets/Tilemaps/TutorialLevelTilemapBG.json", rows: 50, columns: 120));
			//Tilemap groundTMNoColision = InstanceService.Instantiate(new Tilemap(new JailBreaker.PlatformsTileSheet(), "Game/Assets/Tilemaps/TutorialLevelTilemapNoCollion.json", rows: 50, columns: 120));
			//Tilemap groundTM = InstanceService.Instantiate(new Tilemap(new JailBreaker.PlatformsTileSheet(), "Game/Assets/Tilemaps/TutorialLevelTilemap.json", rows: 50, columns: 120, hasCollison: true));

			backgroundTM.TintTilemap(Color.LightGray);

			
			#endregion

			#region Load Prefabs
			LoadPrefabs("Game/Assets/Prefabs/Tutorial/TutorialPrefabs.json");
			//LoadPrefabs("TutorialPrefabs.json");
			#endregion

			#region Level Audio

			GameObject audioholder = InstanceService.Instantiate(new GameObject());
            AudioComponent level1music = new AudioComponent(audioholder, "Game/Assets/Audio/Music/Need_to_get_out....mp3", true);
			level1music.Volume = 0.3f;
			level1music.Play();

			AudioComponent alarmSound = new AudioComponent(audioholder, "Game/Assets/Audio/Music/AlarmLoopVer5.wav", true, 1.3f);
			alarmSound.Play();

			AudioComponent buzzSound = new AudioComponent(audioholder, "Game/Assets/Audio/Door/BuzzSound.wav", false);
			buzzSound.Play();

			#endregion

			#region Camera Shake
			GameObject cameraShakeListenerHolder = InstanceService.Instantiate(new GameObject());
            CameraShakeListener cameraShakeListener = new CameraShakeListener(cameraShakeListenerHolder);
			#endregion

			#region Exit Door

			RemoteDoor door = InstanceService.Instantiate(new RemoteDoor());
			door.SetPosition(new Vector2(16 * 45, 16 * 30));
            door.onDoorTriggerEntered += () =>
            {
				player.SetInputState(InputState.Deactivated);
				fade.FadeIn();
			};

            Leaver l = InstanceService.InstantiateWithPosition(new Leaver(), new Vector2(16 * 17.5f, 16 * 30.5f));

            l.reactor = door;
            door.sender = l;

			


			#endregion

			#region Tutorial

			TextTrigger walkTextTrigger = InstanceService.InstantiateWithPosition(new TextTrigger(), new Vector2(16 * 16, 16 * 38));
            walkTextTrigger.Text = "Press W A S D to walk";
            walkTextTrigger.FontSize = 6;
            walkTextTrigger.TriggerSize = new Vector2(150, 150);
            walkTextTrigger.TextColor = Color.Yellow;
			walkTextTrigger.TextPosition = walkTextTrigger.GetPosition() + new Vector2(-40,0);

			TextTrigger jumpTextTrigger = InstanceService.InstantiateWithPosition(new TextTrigger(), new Vector2(16 * 26, 16 * 38));
			jumpTextTrigger.Text = "Press SPACE to jump";
			jumpTextTrigger.FontSize = 6;
			jumpTextTrigger.TriggerSize = new Vector2(150, 150);
			jumpTextTrigger.TextColor = Color.Yellow;
			jumpTextTrigger.TextPosition = jumpTextTrigger.GetPosition() + new Vector2(-45, 0);

			TextTrigger longJumpTextTrigger = InstanceService.InstantiateWithPosition(new TextTrigger(), new Vector2(16 * 43, 16 * 38));
			longJumpTextTrigger.Text = "Hold SPACE to jump longer \nand higher";
			longJumpTextTrigger.FontSize = 6;
			longJumpTextTrigger.TriggerSize = new Vector2(200, 160);
			longJumpTextTrigger.TextColor = Color.Yellow;
            longJumpTextTrigger.TextPosition = longJumpTextTrigger.GetPosition() + new Vector2(-40,-20);

			TextTrigger shootTextTrigger = InstanceService.InstantiateWithPosition(new TextTrigger(), new Vector2(16 * 56, 16 * 38));
			shootTextTrigger.Text = "Press LEFT MOUSE BUTTON\nto destroy the box";
			shootTextTrigger.FontSize = 6;
			shootTextTrigger.TriggerSize = new Vector2(150, 150);
			shootTextTrigger.TextColor = Color.Yellow;
			shootTextTrigger.TextPosition = shootTextTrigger.GetPosition() + new Vector2(-50, -45);

			TextTrigger PauseTrigger = InstanceService.InstantiateWithPosition(new TextTrigger(), new Vector2(16 * 23, 16 * 30));
			PauseTrigger.Text = "Press ESC to Pause the Game";
			PauseTrigger.FontSize = 6;
			PauseTrigger.TriggerSize = new Vector2(100, 100);
			PauseTrigger.TextColor = Color.Yellow;
			PauseTrigger.TextPosition = PauseTrigger.GetPosition() + new Vector2(-50, -25);

			TextTrigger WeaponTutorial = InstanceService.InstantiateWithPosition(new TextTrigger(), new Vector2(16 * 73, 16 * 38));
			WeaponTutorial.Text = "Press F to swap Weapons \nand Q to throw them";
			WeaponTutorial.FontSize = 6;
			WeaponTutorial.TriggerSize = new Vector2(100, 100);
			WeaponTutorial.TextColor = Color.Yellow;
			WeaponTutorial.TextPosition = WeaponTutorial.GetPosition() + new Vector2(-50, -25);

			#endregion

			// player needs to be at the bottom
			#region Player

			player = (LaniasPlayer)(SceneService.FindFirstInstantiablesWithName("Player"));
			
			if(player != null)
			{
				player.walkForce = 100;
				player.Health = 40;
                player.weapons.EquipWeapon((WeaponType)(SaveSystem.LoadData().CurrentWeapon), player);
                player.weapons.EquipWeapon((WeaponType)(SaveSystem.LoadData().SecondWeapon), player);
            }

			#endregion

			fade.FadeOut();

			mainCamera.Target = player;
			mainCamera.UseCameraBorders = true;
			mainCamera.BorderX = new Vector2(350, 1200);
			mainCamera.BorderY = new Vector2(430, 620);
			mainCamera.Offset = new Vector2(0, 10);
		}


		/// <summary>
		/// gets called every frame use this for calucaltions that need to happen every frame
		/// like input or timers
		/// </summary>
		public override void UpdateScene()
		{
			base.UpdateScene();

			if (Raylib.IsKeyPressed(KeyboardKey.Escape))
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
