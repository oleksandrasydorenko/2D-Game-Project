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
using RocketEngine.DataStore;
using JailBreaker.Data;
using JailBreaker.Interactibles;
using RocketEngine.Scenemanagement;
using System.Security.Cryptography;

namespace JailBreaker.Scenes
{
	public class LaniasScene : Scene
	{
		public LaniasScene(string name = "Lania") : base(name){ }

		LaniasPlayer player;

		Texture2D background;

		HealthPack playerHealth = new HealthPack();

        DataPreSet data = new DataPreSet();

		bool changeScene = false;
        void HandleDoorOpened()
        {
			changeScene = true;
        }

		AudioComponent sc;
        AudioComponent sc2;

        AudioComponent levelMusic;
        AudioComponent menuMusic;
        AudioComponent pianoMusic;

        /// <summary>
        /// gets called once at the beginning when the scene is created
        /// </summary>
        public override void CreateScene()
		{
			base.CreateScene();

			// player and player attributes
			player = InstanceService.Instantiate(new LaniasPlayer());
			player.Name = "Player";
			player.PhysicsComponent.Groundy0 = true;
            player.SetPosition(new Vector2(0, 0));

            playerHealth = InstanceService.Instantiate(new HealthPack());
			playerHealth.SetPosition(new Vector2(100, 0));

            // background
            background = TextureService.LoadTexture("Game/Assets/Textures/BGTest.png");

			// leaver, key, doors
			Leaver leaver1 = InstanceService.Instantiate(new Leaver());
			leaver1.SetPosition(new Vector2(10, 0));

			Leaver leaver2 = InstanceService.Instantiate(new Leaver());
            leaver2.SetPosition(new Vector2(350, 0));

            Key key = InstanceService.Instantiate(new Key());
			key.SetPosition(new Vector2(40, 0));

			Door door1 = InstanceService.Instantiate(new Door());
            door1.SetPosition(new Vector2(200, 0));
			Door door2 = InstanceService.Instantiate(new Door());
            door2.SetPosition(new Vector2(400, 0));

            door1.onDoorOpened += HandleDoorOpened;

			leaver2.reactor = door2;

			// sound 
			GameObject soundTestObj = InstanceService.Instantiate(new GameObject());
			sc = new AudioComponent(soundTestObj, "Game/Assets/Audio/winningCoinnn.wav", true, 0.5f, 1, true, true);
			sc.onSoundFinished += () => Console.WriteLine("Sound ist fertig");

            sc2 = new AudioComponent(soundTestObj, "Game/Assets/Audio/piano2.wav", false, 0.5f, 1f, false, false);
            sc.UseDistanceBasedAudio = true;
            sc2.onSoundFinished += () => Console.WriteLine("Sound ist fertig");

			// music 
			GameObject musicTestObj = InstanceService.Instantiate(new GameObject());
            levelMusic = new AudioComponent(musicTestObj, "Game/Assets/Audio/JailbreakerBeat.mp3", true);
			menuMusic = new AudioComponent(musicTestObj, "Game/Assets/Audio/MainMenuMusic.mp3", true);
            pianoMusic = new AudioComponent(musicTestObj, "Game/Assets/Audio/piano2.wav", true);
			mainCamera.Target = player;
		}


		/// <summary>
		/// gets called every frame use this for calculations that need to happen every frame
		/// like input or timers
		/// </summary>


        public override void UpdateScene()
		{
            base.UpdateScene();

            if (Raylib.IsKeyPressed(KeyboardKey.Zero))
			{
                SceneService.LoadSceneByName("MainMenu");
            }

            if (Raylib.IsKeyPressed(KeyboardKey.S))
            {
				//SaveSystem.SaveData("Level", 5);
				SaveSystem.SaveData("PlayerPosition", player.GetPosition());
            }
            if (Raylib.IsKeyPressed(KeyboardKey.L))
            {
				player.SetPosition(SaveSystem.LoadData().PlayerPosition);
            }
            if (Raylib.IsKeyPressed(KeyboardKey.N))
            {
				//sc2.Play();
				sc.Play();
            }
            if (Raylib.IsKeyPressed(KeyboardKey.M))
            {
				//sc2.Pause();
				sc.Pause();
            }
            if (Raylib.IsKeyPressed(KeyboardKey.B))
            {
				sc2.Play();
            }
            if (changeScene)
			{
                levelMusic.Play(); // not working
                SceneService.LoadSceneByIndex(0);
				//wenn hier steht: mc.Play(); warum wird keine Musik beim szenenwechel abgespielt?
            }
			if(Raylib.IsKeyPressed(KeyboardKey.K))
			{
                pianoMusic.Play();
            }
            if (Raylib.IsKeyPressed(KeyboardKey.H))
            {
                pianoMusic.Pause();
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
