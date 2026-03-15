using JailBreaker.Destructibles;
using JailBreaker.Enemy;
using JailBreaker.Game.Classes.Weapons;
using JailBreaker.Game.Interactibles.WeaponPickUps;
using JailBreaker.Interactibles;
using JailBreaker.Player;
using Raylib_cs;
using RocketEngine;
using RocketEngine.Scenemanagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace JailBreaker.Scenes
{
	public class DanielsScene : Scene
	{
		public DanielsScene(string name = "Daniel") : base(name){ }


		Tilemap ground;
		LaniasPlayer player;
		PoliceRoboter enemy;
		DogRoboter enemy2;
		Box box1;
		GameObject testTrigger;
        Texture2D background;

		/// <summary>
		/// gets called once at the beginning when the scene is created
		/// </summary>
		public override void CreateScene()
		{
			base.CreateScene();

			player = InstanceService.Instantiate(new LaniasPlayer());
			player.SetPosition(100, -200);

            GameObject cameraShakeListenerHolder = InstanceService.Instantiate(new GameObject());
            CameraShakeListener cameraShakeListener = new CameraShakeListener(cameraShakeListenerHolder);
            enemy = InstanceService.Instantiate(new PoliceRoboter());
            enemy.SetPosition(700, 10);
            PoliceRoboter enemy2 = InstanceService.Instantiate(new PoliceRoboter());
            enemy2.SetPosition(750, 10);


            background = TextureService.LoadTexture("Game/Assets/Textures/BGTest.png");
            ground = InstanceService.Instantiate(new Tilemap(new JailBreaker.DefaultTileSheet(), "Game/Assets/Tilemaps/TestTlemap3.json", columns: 50, rows: 30, hasCollison: true));
            ground.ZIndex = -1;
			
			box1 = InstanceService.Instantiate(new Box());
			box1.SetPosition(100, -50);

			PlasmaLauncherPickUp plasmalauncher = InstanceService.Instantiate(new PlasmaLauncherPickUp());
			plasmalauncher.SetPosition(150, 100);
			PistolPickUp pistol = InstanceService.Instantiate(new PistolPickUp());
			pistol.SetPosition(200, 100);

            SniperPickUp pistol2 = InstanceService.Instantiate(new SniperPickUp());
            pistol2.SetPosition(250, 100);

            ShotGunPickUp pistol3 = InstanceService.Instantiate(new ShotGunPickUp());
            pistol3.SetPosition(300, 100);

            MachineGunPickUp pistol4 = InstanceService.Instantiate(new MachineGunPickUp());
            pistol4.SetPosition(350, 100);

			PlasmaSwordPickUp plasmasword = InstanceService.Instantiate(new PlasmaSwordPickUp());	
			plasmasword.SetPosition(400, 100);
            mainCamera.Target = player;
		}

		/// <summary>
		/// gets called every frame use this for calucaltions that need to happen every frame
		/// like input or timers
		/// </summary>
		public override void UpdateScene()
		{
			base.UpdateScene();

			if (Raylib.IsKeyPressed(KeyboardKey.Zero))
			{
				SceneService.LoadSceneByIndex(0);
			}
			
			

        }

		/// <summary>
		/// use this to draw textures in the world
		/// </summary>
		public override void DrawScene()
		{
			Raylib.DrawTexture(background, 0, 0, Color.LightGray);
			base.DrawScene();
		}

		/// <summary>
		/// use this to draw textures on the screan
		/// </summary>
		public override void DrawUIScene()
		{
			base.DrawUIScene();

			Raylib.DrawText(Name, 50, 10, 20, Color.White);
			Raylib.DrawText("WASD : MOVE", 50, 40, 10, Color.LightGray);
			Raylib.DrawText("Space : Jump", 50, 50, 10, Color.LightGray);
		}
	}
}
