using JailBreaker.Data;
using JailBreaker.DeathArea;
using JailBreaker.Destructibles;
using JailBreaker.Enemy;
using JailBreaker.Game;
using JailBreaker.Game.Classes.Weapons;
using JailBreaker.Game.Interactibles.WeaponPickUps;
using JailBreaker.Game.Ui;
using JailBreaker.Interactibles;
using JailBreaker.Player;
using JailBreaker.Props;
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

namespace JailBreaker.Scenes 
{
    public class Level2NewScene : Scene
    {

		LaniasPlayer player;
        ScreenFade fade;
		PauseMenuPanel pauseMenu;
		public Level2NewScene(string name = "Level2") : base(name) { }

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
                SaveSystem.SaveData("Level", 3);
                SceneService.LoadSceneByName("Level3");
                if (player != null)
                {

                    SaveSystem.SaveData("CurrentWeapon", (int)player.weapons.currentWeapon.type);
                    SaveSystem.SaveData("SecondWeapon", (int)player.weapons.secondaryWeapon);

                }
            };
            fade.Time = 1.0f;
            fade.Alpha = 1.0f; // black screen at the beginning
            #endregion

            #region Tilemaps
            //real path
            Tilemap backgroundTM = InstanceService.Instantiate(new Tilemap(new JailBreaker.WallsTileSheet(), "Game/Assets/Tilemaps/Level2/Level2TilemapBG.json", rows: 200, columns: 170));
            Tilemap groundTMNoCollsion = new Tilemap(new JailBreaker.PlatformsTileSheet(), "Game/Assets/Tilemaps/Level2/Level2TilemapNoCollision.json", rows: 200, columns: 170);
            Tilemap groundTM = InstanceService.Instantiate(new Tilemap(new JailBreaker.PlatformsTileSheet(), "Game/Assets/Tilemaps/Level2/Level2Tilemap.json", rows: 200, columns: 170, hasCollison: true));
            //local Path
            //Tilemap backgroundTM = InstanceService.Instantiate(new Tilemap(new JailBreaker.WallsTileSheet(), "Game/Assets/Tilemaps/Level2TilemapBG.json", rows: 200, columns: 170));
            //Tilemap groundTMNoCollsion = InstanceService.Instantiate(new Tilemap(new JailBreaker.PlatformsTileSheet(), "Game/Assets/Tilemaps/Level2TilemapNoCollision.json", rows: 200, columns: 170));
            //Tilemap groundTM = InstanceService.Instantiate(new Tilemap(new JailBreaker.PlatformsTileSheet(), "Game/Assets/Tilemaps/Level2Tilemap.json", rows: 200, columns: 170, hasCollison: true));
            backgroundTM.TintTilemap(Color.LightGray);
            #endregion

            #region Load Prefabs
            LoadPrefabs("Game/Assets/Prefabs/Level2/Level2Prefab.json");
			//LoadPrefabs("Level2Prefab.json");
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
            
            #region Platform
            Platform platform1 =InstanceService.InstantiateWithPosition(new Platform(),new Vector2(1124,1508));
            platform1.Range = new Vector2(platform1.GetPositionX()-(100), platform1.GetPositionX()+50);
            platform1.Direction *= -1;

            Platform platform2 = InstanceService.InstantiateWithPosition(new Platform(), new Vector2(1325, 1508));
            platform2.Range = new Vector2(platform2.GetPositionX() - 50, platform2.GetPositionX() + 100);

            Platform platform3 = InstanceService.InstantiateWithPosition(new Platform(), new Vector2(1595, 1463));
            platform3.Range = new Vector2(platform3.GetPositionX() - 100, platform3.GetPositionX() + 50);
            platform3.Direction *= -1;

            Platform platform4 = InstanceService.InstantiateWithPosition(new Platform(), new Vector2(1140, 820));
            platform4.Range = new Vector2(platform4.GetPositionX() - 100, platform4.GetPositionX() + 60);
            platform4.Direction *= -1;

            Platform platform5 = InstanceService.InstantiateWithPosition(new Platform(), new Vector2(880, 870));
            platform5.Range = new Vector2(platform5.GetPositionX() - 60, platform5.GetPositionX() + 100);
            #endregion

            #region Enemies
            PoliceRoboter police = InstanceService.InstantiateWithPosition(new PoliceRoboter(), new Vector2(1635,440));
            police.patrolRange = new Vector2(police.GetPositionX() - 15, police.GetPositionX() + 15);
            police.enemyLoot = Enemy.Enemy.EnemyLootOptions.Key;
            PoliceRoboter police2 = InstanceService.InstantiateWithPosition(new PoliceRoboter(), new Vector2(415, 854));
            police2.patrolRange = new Vector2(police2.GetPositionX() - 40, police2.GetPositionX() + 40);
            police2.enemyLoot = Enemy.Enemy.EnemyLootOptions.HealthPack;
            #endregion

            #region Exit Door
            DoorWithKey door = InstanceService.Instantiate(new DoorWithKey());
			door.SetPosition(new Vector2(1710, 16 * 27));
            door.onDoorTriggerEntered += () =>
            {
                player.SetInputState(InputState.Deactivated);
                fade.FadeIn();
            };
			#endregion

			#region Player
			player = (LaniasPlayer)(SceneService.FindFirstInstantiablesWithName("Player"));

			if (player != null)
			{
				player.walkForce = 100;
                player.weapons.EquipWeapon((WeaponType)(SaveSystem.LoadData().CurrentWeapon), player);
                player.weapons.EquipWeapon((WeaponType)(SaveSystem.LoadData().SecondWeapon), player);
            }
			#endregion

            #region Camera Setup
            mainCamera.Target = player;
            mainCamera.UseCameraBorders = true;
            mainCamera.BorderX = new Vector2(400, 2280);
            mainCamera.BorderY = new Vector2(400, 1520);
            mainCamera.Offset = new Vector2(0, 10);
            #endregion

        }
        public override void UpdateScene()
        {
            base.UpdateScene();
            if (Raylib.IsKeyPressed((KeyboardKey.Four))) player.SetPosition(1072, 441);

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
