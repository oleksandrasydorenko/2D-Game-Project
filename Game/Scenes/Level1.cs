using JailBreaker.Data;
using JailBreaker.Destructibles;
using JailBreaker.Game.Classes.Weapons;
using JailBreaker.Game.Interactibles.WeaponPickUps;
using JailBreaker.Game.TextTriggers;
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
namespace JailBreaker.Scenes;

public class Level1 : Scene //LANIAS LEVEL
{

    LaniasPlayer player;
    ScreenFade fade;
	PauseMenuPanel pauseMenu;

	public Level1(string name = "Level1") : base(name) { }

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
            SaveSystem.SaveData("Level1", 1);
            if (player != null) {
                
                SaveSystem.SaveData("CurrentWeapon",(int)player.weapons.currentWeapon.type);
                SaveSystem.SaveData("SecondWeapon", (int)player.weapons.secondaryWeapon);

            }
            SceneService.LoadSceneByName("Level2");
        };
        fade.Time = 1.0f;
        fade.Alpha = 1.0f; // black screen at the beginning
        #endregion

        #region Load Tilemaps
        Tilemap backgroundTM = InstanceService.Instantiate(new Tilemap(new JailBreaker.WallsTileSheet(), "Game/Assets/Tilemaps/Level1/Level1TilemapBG.json", rows: 65, columns: 100));
        Tilemap backgrounfTMNoCollision = InstanceService.Instantiate(new Tilemap(new JailBreaker.PlatformsTileSheet(), "Game/Assets/Tilemaps/Level1/Level1TilemapNoCollision.json", rows: 80, columns: 100, hasCollison: false));
        Tilemap groundTM = InstanceService.Instantiate(new Tilemap(new JailBreaker.PlatformsTileSheet(), "Game/Assets/Tilemaps/Level1/Level1Tilemap.json", rows: 85, columns: 100, hasCollison: true));
        //Tilemap backgroundTM = InstanceService.Instantiate(new Tilemap(new JailBreaker.WallsTileSheet(), "Game/Assets/Tilemaps/Level1TilemapBG.json", rows: 65, columns: 100));
        //Tilemap backgrounfTMNoCollision = InstanceService.Instantiate(new Tilemap(new JailBreaker.PlatformsTileSheet(), "Game/Assets/Tilemaps/Level1TilemapNoCollision.json", rows: 65, columns: 100, hasCollison: false));
        //Tilemap groundTM = InstanceService.Instantiate(new Tilemap(new JailBreaker.PlatformsTileSheet(), "Game/Assets/Tilemaps/Level1Tilemap.json", rows: 65, columns: 100, hasCollison: true));
        backgroundTM.TintTilemap(Color.LightGray);
        #endregion

        #region Load Prefbas
        //prefabFilePath = "Game/Assets/Prefabs/Level1/Level1Prefabs.json";
        //LoadPrefabs("Level1Prefabs.json"); 
        LoadPrefabs("Game/Assets/Prefabs/Level1/Level1Prefabs.json");
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

		#region CameraShake
		GameObject cameraShakeListenerHolder = InstanceService.Instantiate(new GameObject());
        CameraShakeListener cameraShakeListener = new CameraShakeListener(cameraShakeListenerHolder);
        #endregion

        #region Exit Door
        //DoorWithKey door = InstanceService.Instantiate(new DoorWithKey());
        DoorWithKey door = InstanceService.InstantiateWithPosition(new DoorWithKey(), new Vector2(479, 416));
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
            player.weapons.EquipWeapon((WeaponType)(SaveSystem.LoadData().CurrentWeapon),player);
            player.weapons.EquipWeapon((WeaponType)(SaveSystem.LoadData().SecondWeapon), player);
            player.walkForce = 100;
		}
		#endregion

		Key key = InstanceService.InstantiateWithPosition(new Key(), new Vector2(599, 535));

        fade.FadeOut();

        #region camera settings
        mainCamera.Target = player;
        mainCamera.UseCameraBorders = true;
        mainCamera.BorderX = new Vector2(350, 1200);
        mainCamera.BorderY = new Vector2(150,800);
        mainCamera.Offset = new Vector2(0, 10);
        #endregion
    }
    public override void UpdateScene()
    {
        base.UpdateScene();

        if (Raylib.IsKeyPressed((KeyboardKey.Four))) player.SetPosition(599, 485);

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
            
       