using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketEngine.Scenemanagement
{
	public class DefaultScene : Scene
	{
		public DefaultScene(string name = "DefaultScene") : base(name){}

		public override void DrawScene()
		{
			base.DrawScene();	
		}

		public override void DrawUIScene()
		{
			base.DrawUIScene();
			Raylib.DrawText("This is the default Scene, please create a new scene \nand add it to the game settings in the engine", 50, RocketEngine.Settings.DisplaySettings.WINDOW_HEIGHT / 2, 20, Color.White);
		}

		public override void CreateScene()
		{
			base.CreateScene();
		}

		public override void UpdateScene()
		{
			base.UpdateScene();

			if (Raylib.IsKeyPressed(KeyboardKey.One))
			{
				SceneService.LoadSceneByName("Sebastian");
			}

            if (Raylib.IsKeyPressed(KeyboardKey.Two))
            {
                SceneService.LoadSceneByIndex(2);
            }

            if (Raylib.IsKeyPressed(KeyboardKey.Three))
            {
                SceneService.LoadSceneByIndex(3);
            }
            if (Raylib.IsKeyPressed(KeyboardKey.Four))
            {
                SceneService.LoadSceneByName("Nicole");
            }
            if (Raylib.IsKeyPressed(KeyboardKey.Five))
            {
                SceneService.LoadSceneByName("Lania");
            }
            if (Raylib.IsKeyPressed(KeyboardKey.Six))
            {
                SceneService.LoadSceneByName("MainMenu");
            }
        }
	}
}
