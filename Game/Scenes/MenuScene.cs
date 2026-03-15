using JailBreaker.Player;
using JailBreaker.Ui;
using Raylib_cs;
using RocketEngine;
using RocketEngine.Scenemanagement;
using RocketEngine.Ui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static RocketEngine.Ui.UiElement;


namespace JailBreaker.Scenes
{
    public class MainMenuScene :Scene
    {   
        public MainMenuScene(string name = "MainMenu") : base(name) { }

        /// <summary>
        /// gets called once at the beginning when the scene is created
        /// </summary>
        public override void CreateScene()
        {
            base.CreateScene();

			GameObject audioholder = InstanceService.Instantiate(new GameObject());
			AudioComponent menuMusic = new AudioComponent(audioholder, "Game/Assets/Audio/Music/TutorialCreditsSoundTrack.mp3", true);
			menuMusic.Volume = .3f;
			menuMusic.Play();

			MainMenuBackgroundPanel background = InstanceService.Instantiate(new MainMenuBackgroundPanel());
            MainMenuPanel mainmenu = InstanceService.Instantiate(new MainMenuPanel());
            GameManager.GamePaused = false;
			GameManager.ShowCursor = true;
		}
    }
}
