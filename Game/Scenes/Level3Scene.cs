using JailBreaker.Data;
using JailBreaker.Destructibles;
using JailBreaker.Enemy;
using JailBreaker.Game.Classes.Weapons;
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
    public class Level3Scene : Scene
    {
        LaniasPlayer player;
        Drone drone;
        PauseMenuPanel pauseMenu;

        GameText game1;
        GameText game2;
        GameText game3;
        GameText game4;

        ScreenFade fade;
        public Level3Scene(string name = "Level3") : base(name) { }

        /// <summary>
        /// gets called once at the beginning when the scene is created
        /// </summary>
        /// 
        public override void CreateScene()
        {
            base.CreateScene();
            #region Fade
            fade = InstanceService.Instantiate(new ScreenFade());
            fade.onFadeInFinished += () =>
            {
                SaveSystem.SaveData("Level4", 4);
                SceneService.LoadSceneByName("Credits");
                if (player != null)
                {

                    SaveSystem.SaveData("CurrentWeapon", (int)player.weapons.currentWeapon.type);
                    SaveSystem.SaveData("SecondWeapon", (int)player.weapons.secondaryWeapon);

                }
            };
            fade.Time = 1.0f;
            fade.Alpha = 1.0f; // black screen at the beginning
            #endregion

            #region Load Tilemaps
            //Tilemap backgroundTM = InstanceService.Instantiate(new Tilemap(new JailBreaker.WallsTileSheet(), "Game/Assets/Tilemaps/Level3/Level3TilemapBG.json", rows: 50, columns: 60));
            //Tilemap groundTMNoColission = InstanceService.Instantiate(new Tilemap(new JailBreaker.PlatformsTileSheet(), "Game/Assets/Tilemaps/Level3/Level3TilemapNoCollision.json", rows: 50, columns: 60));
            //Tilemap groundTM = InstanceService.Instantiate(new Tilemap(new JailBreaker.PlatformsTileSheet(), "Game/Assets/Tilemaps/Level3/Level3Tilemap.json", rows: 50, columns: 60, hasCollison: true));

            Tilemap backgroundTM = InstanceService.Instantiate(new Tilemap(new JailBreaker.WallsTileSheet(), "Game/Assets/Tilemaps/Level3TilemapBG.json", rows: 50, columns: 60));
            Tilemap groundTMNoColission = InstanceService.Instantiate(new Tilemap(new JailBreaker.PlatformsTileSheet(), "Game/Assets/Tilemaps/Level3TilemapNoCollision.json", rows: 50, columns: 60));
            Tilemap groundTM = InstanceService.Instantiate(new Tilemap(new JailBreaker.PlatformsTileSheet(), "Game/Assets/Tilemaps/Level3Tilemap.json", rows: 50, columns: 60, hasCollison: true));

            backgroundTM.TintTilemap(Color.LightGray);
            #endregion

            #region Numbers
            game1 = InstanceService.InstantiateWithPosition(new GameText(new Color(0, 0, 0, 0), "5", 20), new Vector2(435, 180));
            game2 = InstanceService.InstantiateWithPosition(new GameText(new Color(64, 224, 208), "3", 20), new Vector2(925, 108));
            game3 = InstanceService.InstantiateWithPosition(new GameText(new Color(64, 224, 208), "6", 20), new Vector2(185, 685));
            game4 = InstanceService.InstantiateWithPosition(new GameText(new Color(64, 224, 208), "7", 20), new Vector2(144, 430));
            #endregion

            #region Load Prefabs
            LoadPrefabs("Game/Assets/Prefabs/Level3/Level3ScenePrefabs.json");
            //LoadPrefabs("Level3ScenePrefabs.json");
            #endregion

            #region Level Audio
            GameObject audioholder = InstanceService.Instantiate(new GameObject());
            AudioComponent level1music = new AudioComponent(audioholder, "Game/Assets/Audio/Music/JailbreakerBeat.mp3", true);
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
            door.SetPosition(new Vector2(16 * 53, 16 * 25));
            door.onDoorTriggerEntered += () =>
            {
                player.SetInputState(InputState.Deactivated);
                fade.FadeIn();
            };
            KeyPad keypad = InstanceService.Instantiate(new KeyPad());
            keypad.SetPosition(16 * 50, 16 * 25);
            keypad.reactor = door;
            door.sender = keypad;

            #endregion

            drone = InstanceService.InstantiateWithPosition(new Drone(), new Vector2(435, 170));
            drone.onHealthZero += () => { game1.Color = new Color(64, 224, 208); };
            #region Player
            player = (LaniasPlayer)(SceneService.FindFirstInstantiablesWithName("Player"));

			if (player != null)
			{
				player.walkForce = 100;
                player.weapons.EquipWeapon((WeaponType)(SaveSystem.LoadData().CurrentWeapon), player);
                player.weapons.EquipWeapon((WeaponType)(SaveSystem.LoadData().SecondWeapon), player);
            }
			#endregion

            #region Camera Settings
            fade.FadeOut();

            mainCamera.Target = player;
            mainCamera.UseCameraBorders = true;
            mainCamera.BorderX = new Vector2(200, 765);
            mainCamera.BorderY = new Vector2(115, 690);
            mainCamera.Offset = new Vector2(0, 10);
            #endregion
        }

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

            //cheating

            if (Raylib.IsKeyPressed(KeyboardKey.One))
            {
                player.SetPosition(925, 80);
            }
            if (Raylib.IsKeyPressed(KeyboardKey.Two))
            {
                player.SetPosition(225, 685);
            }
            if (Raylib.IsKeyPressed(KeyboardKey.Three))
            {
                player.SetPosition(16 * 50, 16 * 25);
            }
        }
    }
}
