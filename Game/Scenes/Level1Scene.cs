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

namespace JailBreaker.Scenes 
{
    public class Level1Scene : Scene //////TEST SCENE
    {

		LaniasPlayer player;

		public Level1Scene(string name = "AlphaLevel1") : base(name) { }

        /// <summary>
        /// gets called once at the beginning when the scene is created
        /// </summary>
        /// 
        public override void CreateScene()
        {
            base.CreateScene();

            #region Level Audio
            GameObject audioholder = InstanceService.Instantiate(new GameObject());
            AudioComponent level1music = new AudioComponent(audioholder, "Game/Assets/Audio/Music/JailbreakerBeat.mp3", true);
            level1music.Play();
            #endregion

            GameObject cameraShakeListenerHolder = InstanceService.Instantiate(new GameObject());
            CameraShakeListener cameraShakeListener = new CameraShakeListener(cameraShakeListenerHolder);
            
            Tilemap backgroundTM = InstanceService.Instantiate(new Tilemap(new JailBreaker.DefaultTileSheet(), "Game/Assets/Tilemaps/AlphaTilemapLevel2BG.json", rows: 80, columns: 100));
			backgroundTM.TintTilemap(Color.Gray);
			Tilemap groundTM = InstanceService.Instantiate(new Tilemap(new JailBreaker.DefaultTileSheet(), "Game/Assets/Tilemaps/AlphaTilemapLevel2.json", rows: 80, columns: 100, hasCollison: true));

            player = InstanceService.Instantiate(new LaniasPlayer());
            player.walkForce = 100;
            player.SetPosition(300, 170);

			mainCamera.Offset = mainCamera.Offset + new Vector2(0, 50);

			// door
			RemoteDoor door = InstanceService.Instantiate(new RemoteDoor());
			door.SetPosition(new Vector2(16 * 53, 16 * 27));
            door.onDoorTriggerEntered += () =>
            {
                SaveSystem.SaveData("Level",2);
                SceneService.LoadSceneByName("AlphaLevel2");
            };

            Box box = InstanceService.Instantiate(new Box());
            box.SetPosition(new Vector2(16 * 39 ,16 * 16.5f));

			box = InstanceService.Instantiate(new Box());
			box.SetPosition(new Vector2(16 * 37, 16 * 16.5f));
            box.chestLoot = Box.ChestLootOptions.Key;

			box = InstanceService.Instantiate(new Box());
			box.SetPosition(new Vector2(16 * 35, 16 * 16.5f));



          /*//GameText walkText = InstanceService.Instantiate(new GameText(Color.White, "Press WASD to move", 14));
           // walkText.SetPosition(16*20, 16* 37);

			//GameText jumpText = InstanceService.Instantiate(new GameText(Color.White, "Press SPACE to jump", 14));
			//jumpText.SetPosition(16 * 32, 16 * 37);

			GameText doorText = InstanceService.Instantiate(new GameText(Color.White, "Press E to Interact", 14));
			doorText.SetPosition(16 * 47, 16 * 23);

			GameText chestText = InstanceService.Instantiate(new GameText(Color.White, "Press LEFT MOUSE \nto shoot", 14));
			chestText.SetPosition(16 * 31, 16 * 10);

			TextTrigger walkTextTrigger = InstanceService.InstantiateWithPosition(new TextTrigger(), new Vector2(16 * 20, 16 * 37));
			*/

			LoadPrefabs("prefabTest.json");

            KeyPad keypad = InstanceService.Instantiate(new KeyPad());
            keypad.SetPosition(16 * 32, 16 * 38);

            keypad.reactor = door;
            door.sender = keypad;

			mainCamera.Target = player;
		}


        /// <summary>
        /// gets called every frame use this for calucaltions that need to happen every frame
        /// like input or timers
        /// </summary>
        public override void UpdateScene()
        {
            base.UpdateScene();
            if (Raylib.IsKeyPressed((KeyboardKey.Five)))
            {
                player.SetPosition(818, 435);
            }

            if (Raylib.IsKeyPressed(KeyboardKey.Zero))
            {
                SceneService.LoadSceneByIndex(10);
            }
			else if (Raylib.IsKeyPressed(Raylib_cs.KeyboardKey.One))
			{
				SceneService.LoadSceneByIndex(12);
			}
			else if(Raylib.IsKeyPressed(KeyboardKey.Two))
			{
				SceneService.LoadSceneByIndex(14);
			}
			else if (Raylib.IsKeyPressed(KeyboardKey.Three))
			{
				SceneService.LoadSceneByIndex(16);
			}
		}
	}
}
