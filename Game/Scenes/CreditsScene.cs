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
    public class CreditsScene : Scene
    {
        public CreditsScene(string name = "Credits") : base(name) { }
        private CreditsMenuPanel creditMenu;
        private GameButton menuButton;
        private float timer = 0f;
        private float delay = 45f;
        private bool credits = true;

        public override void CreateScene()
        {
            base.CreateScene();

			GameObject audioholder = InstanceService.Instantiate(new GameObject());
			AudioComponent menuMusic = new AudioComponent(audioholder, "Game/Assets/Audio/Music/TutorialCreditsSoundTrack.mp3", true);
			menuMusic.Volume = .3f;
			menuMusic.Play();

			creditMenu = InstanceService.Instantiate(new CreditsMenuPanel());
            menuButton = InstanceService.Instantiate(new GameButton());
            menuButton.Text = "Menu";
            menuButton.CurrentAnchor = UiElement.AnchoringPosition.LeftBottom;
            menuButton.SetPosition(125, -60);
            menuButton.HoveredColor = new Color(196,0,0);
            menuButton.PressedColor = new Color(150,0,0);
            menuButton.OnClicked += LoadMenu;

		}


        private void LoadMenu()
        {
			SceneService.LoadSceneByName("MainMenu");
			credits = false;
			timer = 0f;
		}

        public override void UpdateScene()
        {
            base.UpdateScene();
            //shows credits only 45 seconds, then switches to main menu
            if (credits)
            {
                timer += Time.DeltaTime;
                if (timer >= delay)
                {
                    LoadMenu();
                }
            }
        }
    }
}

