using JailBreaker.Player;
using Raylib_cs;
using RocketEngine;
using RocketEngine.Scenemanagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using RocketEngine.Utils;
using JailBreaker.Destructibles;
using JailBreaker.Interactibles;

namespace JailBreaker.Scenes
{
	public class SebastiansScene : Scene
	{
		public SebastiansScene(string name = "Sebastian") : base(name){ }

		LaniasPlayer player;

		Tilemap ground;
		Tilemap bg;

		Door door;
		public override void CreateScene()
		{
			base.CreateScene();
			 door = InstanceService.Instantiate(new Door());
		}

		/// <summary>
		/// gets called every frame use this for calucaltions that need to happen every frame
		/// like input or timers
		/// </summary>
		/// 
		private float t;

		public override void UpdateScene()
		{
			base.UpdateScene();

		

			if (Raylib.IsKeyPressed(KeyboardKey.Zero))
			{
				SceneService.LoadSceneByIndex(0);
			}

			if (Raylib.IsKeyPressed(KeyboardKey.Nine))
			{
				SceneService.LoadSceneByIndex(6);
			}

			if (Raylib.IsKeyPressed(KeyboardKey.E))
			{
				LaniasPlayer p = new LaniasPlayer();
				p.Name = "Player";
				door.Interact(p);


				PrefabService.GetPrefabFromIndex(0);
			}

		

		}

	}
}
