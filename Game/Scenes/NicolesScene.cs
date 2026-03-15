using JailBreaker.Enemy;
using JailBreaker.Game.Classes.Weapons.Projectiles;
using JailBreaker.Player;
using Raylib_cs;
using RocketEngine;
using RocketEngine.Scenemanagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace JailBreaker.Scenes
{
	public class NicolesScene : Scene
	{
		public NicolesScene(string name = "Nicole") : base(name){ }

		LaniasPlayer player;
		//test
		DogRoboter Enemy1;
		Drone drone;

		/// <summary>
		/// gets called once at the beginning when the scene is created
		/// </summary>
		/// 

		Tilemap ground;
		public override void CreateScene()
		{
			base.CreateScene();

			ground = InstanceService.Instantiate(new Tilemap(new JailBreaker.DefaultTileSheet(), "Game/Assets/Tilemaps/TestTlemap3.json", columns: 50, rows: 30, hasCollison: true));
			ground.ZIndex = -1;


			player = InstanceService.Instantiate(new LaniasPlayer());
			player.SetPosition(50, 0);

			player.Name = "Player1";

			//Enemy1 = InstanceService.Instantiate(new Rat());
			//Enemy1.Name = "Enemy1";
			//Enemy1.SetPosition(100, 0);

			//Box box = InstanceService.Instantiate(new Box());
			//box.SetPosition(new Vector2(16 * 8,16 *6.5f));
			//Rat rat = InstanceService.Instantiate(new Rat());
			//rat.SetPosition(100, 0);

		    //Enemy1 = InstanceService.Instantiate(new DogRoboter());
			//Enemy1.SetPosition(200,0);

			//drone = InstanceService.Instantiate(new Drone());
			//drone.SetPosition(200,10);

			//PoliceRoboter police = InstanceService.Instantiate(new PoliceRoboter());
			//police.SetPosition(200, 0);
			Bomb mine = InstanceService.Instantiate(new Bomb());
			mine.SetPosition(200, 0);

			mainCamera.Target = player;
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
