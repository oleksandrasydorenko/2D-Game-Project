using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static RocketEngine.InstanceService;
using RocketEngine;
using System.Linq.Expressions;
using RocketEngine.Scenemanagement;
using RocketEngine.Ui;
using System.Security.Cryptography.X509Certificates;

namespace JailBreaker.Ui
{
    /// <summary>
    /// MainMenuPanel with character selection buttons, each button loads different scene
    /// </summary>
    public class MainMenuPanel : UiElement
    {
        private GameButton exitButton;
        public GameButton playButton;
		public GameButton editorButton;
		private GameButton creditsButton;
        private UiText menuHeader;

        public MainMenuPanel(float x = 0, float y = 0, string name = "MainMenuPannel") :base(x, y, AnchoringPosition.Center, name)
        {
			//Create character selection buttons
			exitButton = InstanceService.Instantiate(new GameButton(text:"EXIT"));
            playButton = InstanceService.Instantiate(new GameButton(text: "Play"));
			editorButton = InstanceService.Instantiate(new GameButton(text: "Editor"));
			creditsButton = InstanceService.Instantiate(new GameButton(text: "Credits"));

            //Set button color
			exitButton.HoveredColor = new Color(196, 0, 0);
            playButton.HoveredColor = new Color(64, 224, 208);
			editorButton.HoveredColor = new Color(64, 224, 208);
			creditsButton.HoveredColor = new Color(64, 224, 208);

			exitButton.PressedColor = new Color(150, 0, 0);
            playButton.PressedColor = new Color(0, 120, 120);
			editorButton.PressedColor = new Color(0, 120, 120);
			creditsButton.PressedColor = new Color(0, 120, 120);

            //Set button position
            playButton.SetPositionY(-105);
            editorButton.SetPositionY(-35);
            exitButton.SetPositionY(105);
            creditsButton.SetPositionY(35);

			//Register pressed events
			exitButton.OnClicked += OnExitButtonClicked;
            playButton.OnClicked += OnPlayButtonClicked;
			editorButton.OnClicked += OnEditorButtonClicked;
			creditsButton.OnClicked += OnCreditsButtonClicked;

		    menuHeader = InstanceService.Instantiate(new UiText(new Color(64, 224, 208), "JAIL BREAKER", 90, -170, 50,AnchoringPosition.Top));

            GameManager.ShowCursor = true;
        }

        //Event handlers
        public void OnExitButtonClicked()
        {
            Console.WriteLine("Window closed");
            EngineSerivce.CloseGame();

        }

        public void OnPlayButtonClicked()
        {
            InstanceService.Instantiate(new PlayPanel());
            InstanceService.Destroy(this);
        }

		public void OnEditorButtonClicked()
		{
			InstanceService.Instantiate(new CustomPanel());
			InstanceService.Destroy(this);
		}

        public void OnCreditsButtonClicked()
		{
            SceneService.LoadSceneByName("Credits");
		}
         /// <summary>
         /// Destroys all buttons
         /// </summary>
		public override void Destroy()
        {
            base.Destroy();
            
            InstanceService.Destroy(exitButton);
            exitButton = null;
            InstanceService.Destroy(playButton);
            playButton = null;
			InstanceService.Destroy(editorButton);
			editorButton = null;
			InstanceService.Destroy(creditsButton);
			creditsButton = null;
            InstanceService.Destroy(menuHeader);
            menuHeader = null;
		}
    }
}
