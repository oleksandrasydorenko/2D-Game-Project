using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketEngine.Settings
{
	public static class GameSettings
	{
		public const float GRAVITY = 9.76f;

		public const Raylib_cs.KeyboardKey ExitKey = KeyboardKey.Null;

		public static Scene[] scenes =
		{
			new JailBreaker.Scenes.MainMenuScene(),

			new JailBreaker.Scenes.TutorialEditor(),
			new JailBreaker.Scenes.TutorialLevel(),

			new JailBreaker.Scenes.Level1Editor(),
			new JailBreaker.Scenes.Level1(),

			new JailBreaker.Scenes.Level2Editor(),
			new JailBreaker.Scenes.Level2NewScene(),

			new JailBreaker.Scenes.Level3SceneEditor(),
			new JailBreaker.Scenes.Level3Scene(),

			new JailBreaker.Scenes.FiringRangeEditor(),
			new JailBreaker.Scenes.FiringRangeLevel(),

			new JailBreaker.Scenes.CustomLevelEditor(),
			new JailBreaker.Scenes.CustomLevel(),

			new JailBreaker.Scenes.CreditsScene(),
        };
	}
}
