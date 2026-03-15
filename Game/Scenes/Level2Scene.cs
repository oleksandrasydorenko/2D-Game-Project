using JailBreaker.Destructibles;
using JailBreaker.Enemy;
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

namespace JailBreaker.Scenes
{
    public class Level2Scene : Scene
    {

		LaniasPlayer player;

		public Level2Scene(string name = "AlphaLevel2") : base(name) { }

		/// <summary>
		/// gets called once at the beginning when the scene is created
		/// </summary>
		public override void CreateScene()
        {
            base.CreateScene();

			Tilemap backgroundTM = InstanceService.Instantiate(new Tilemap(new JailBreaker.DefaultTileSheet(), "Game/Assets/Tilemaps/AlphaTilemapLevel1BG.json", rows: 20, columns: 100));
			backgroundTM.TintTilemap(Color.Gray);
			Tilemap groundTM = InstanceService.Instantiate(new Tilemap(new JailBreaker.DefaultTileSheet(), "Game/Assets/Tilemaps/AlphaTilemapLevel1.json", rows: 20, columns: 100, hasCollison: true));

			GameObject cameraShakeListenerHolder = InstanceService.Instantiate(new GameObject());
			CameraShakeListener cameraShakeListener = new CameraShakeListener(cameraShakeListenerHolder);


			player = InstanceService.Instantiate(new LaniasPlayer());
			player.walkForce = 100;
			player.SetPosition(100, 100); ;

			mainCamera.Offset = mainCamera.Offset + new Vector2(0, 50);


			Drone drone1= InstanceService.Instantiate(new Drone());
			drone1.Name = "Drone";
			drone1.SetPosition(1100, 100);
			drone1.patrolRange = new Vector2(1030, 1270);

			DogRoboter robo  = InstanceService.Instantiate(new DogRoboter());
			robo.Name = "Robo";
			robo.SetPosition(1100, 100);
			robo.patrolRange = new Vector2(700, 900);

			// leaver
			Leaver doorLever = InstanceService.Instantiate(new Leaver());
			doorLever.SetPosition(new Vector2(16 * 93, 16 * 6f));
			// door
			RemoteDoor door = InstanceService.Instantiate(new RemoteDoor());
			door.SetPosition(new Vector2(16 * 50, 16 * 11));
			door.onDoorTriggerEntered += () => SceneService.LoadSceneByName("Credits");

			// connecting door and leaver
			door.sender = doorLever;
			doorLever.reactor = door;

			ExplosiveBarrel barrel = InstanceService.Instantiate(new ExplosiveBarrel());
			barrel.SetPosition(new Vector2(16 * 91, 16 * 6f));

			barrel = InstanceService.Instantiate(new ExplosiveBarrel());
			barrel.SetPosition(new Vector2(16 * 93.5f, 16 * 6f));

			Box box = InstanceService.Instantiate(new Box());
			box.SetPosition(new Vector2(16 * 85, 16 * 6.5f));
			box.chestLoot = Box.ChestLootOptions.HealthPack;

			box = InstanceService.Instantiate(new Box());
			box.SetPosition(new Vector2(16 * 87, 16 * 6.5f));


			JumpPad jp  = InstanceService.Instantiate(new JumpPad());
			jp.SetPosition(new Vector2(16 * 10, 16 * 12.5f));

			GameText enemyText = InstanceService.Instantiate(new GameText(Color.White, "Careful", 14));
			enemyText.SetPosition(16 * 36, 16 * 7);

			GameObject go = PrefabService.GetPrefabFromIndex(0) as GameObject;
			go.SetPosition(100, 180);


			mainCamera.Target = player;
		}



    }
}

