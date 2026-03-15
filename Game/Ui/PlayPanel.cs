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
using RocketEngine.Scenemanagement;
using RocketEngine.Ui;
using JailBreaker.Data;

namespace JailBreaker.Ui
{
    /// <summary>
    /// DeathMenuPanel with buttons, each button loads different scene
    /// </summary>
    public class PlayPanel : UiElement
    {
        private GameButton newGameButton;
        private GameButton continueButton;
		private GameButton sceneSelectButton;
		private GameButton returnButton;
        private UiText menuHeader;
        public SpriteComponent spriteComponent;
        public SpriteComponent spriteComponent2;
        private Sprite backgroundSprite;
        public Sprite BackgroundSprite

        {
            get { return backgroundSprite; }
            set
            {
                backgroundSprite = value;
                if (backgroundSprite != null)
                {
                    spriteComponent.sprite = backgroundSprite;
                }
            }
        }

        public PlayPanel(Sprite sprite = null, float x = 0, float y = 0, AnchoringPosition anchor = AnchoringPosition.Center, string name = "DeathMenuPanel") : base(x, y, AnchoringPosition.Center, name)
        {
            this.backgroundSprite = sprite;
            if (backgroundSprite == null)
            {
                this.backgroundSprite = new Sprite("Game/Assets/Textures/BGPause.png");
            }
        }

        public override void Construct()
        {
            base.Construct();

            spriteComponent = new SpriteComponent(this, backgroundSprite, new Color(0, 0, 0, 170));
            spriteComponent.SpriteScale = 80;
            spriteComponent.visible = false;

            spriteComponent = new SpriteComponent(this, backgroundSprite, Color.LightGray);
            spriteComponent.SpriteScale = 3;
			spriteComponent.visible = false;

			//Create character selection buttons
			newGameButton = InstanceService.Instantiate(new GameButton());
            continueButton = InstanceService.Instantiate(new GameButton());
			sceneSelectButton = InstanceService.Instantiate(new GameButton());
			returnButton = InstanceService.Instantiate(new GameButton());

			newGameButton.HoveredColor = new Color(64, 224, 208);
            continueButton.HoveredColor = new Color(64, 224, 208);
			sceneSelectButton.HoveredColor = new Color(64, 224, 208);
			returnButton.HoveredColor = new Color(196, 0, 0); 


            newGameButton.PressedColor = new Color(0, 120, 120);
            continueButton.PressedColor = new Color(0, 120, 120);
			sceneSelectButton.PressedColor = new Color(0, 120, 120);
			returnButton.PressedColor = new Color(150, 0, 0);

            newGameButton.SetText("New Game");
            continueButton.SetText("Continue");
			sceneSelectButton.SetText("Scenes");
			returnButton.SetText("Return");

            newGameButton.CurrentAnchor = AnchoringPosition.Center;
            continueButton.CurrentAnchor = AnchoringPosition.Center;
			sceneSelectButton.CurrentAnchor = AnchoringPosition.Center;
			returnButton.CurrentAnchor = AnchoringPosition.LeftBottom;

            newGameButton.SetPosition(0, -35);
            continueButton.SetPosition(0, -105);
			sceneSelectButton.SetPosition(0, 35);
			returnButton.SetPosition(125, -60);


            //Register pressed events
            newGameButton.OnClicked += OnNewGameButtonClicked;
            continueButton.OnClicked += OnContinueButtonClicked;
			sceneSelectButton.OnClicked += OnSceneSelectButtonClicked;
			returnButton.OnClicked += OnReturnButtonClicked;

			menuHeader = InstanceService.Instantiate(new UiText(new Color(64, 224, 208), "JAIL BREAKER", 90, -170, 50, AnchoringPosition.Top));

		}

		//Event handlers
		public void OnNewGameButtonClicked()
        {
            Console.WriteLine("switched to Level 1");

            SaveSystem.SaveData("Level", 1);
            SceneService.LoadSceneByName("AlphaLevel1");
        }
        public void OnContinueButtonClicked()
        {
            Console.WriteLine("switched to ... scene");

            int levelIndex = SaveSystem.LoadData().Level;
            SceneService.LoadSceneByName($"AlphaLevel{levelIndex}");
        }

        public void OnSceneSelectButtonClicked()
        {
            InstanceService.Instantiate(new SceneSelectPanel());
            InstanceService.Destroy(this);
		}
        public void OnReturnButtonClicked()
        {
            Console.WriteLine("switched to MainMenu scene");

            SceneService.LoadSceneByName("MainMenu");
        }

		public override void Destroy()
		{
			base.Destroy();

            if(continueButton != null)
            {
                InstanceService.Destroy(continueButton);
                continueButton = null;
            }

			if (newGameButton != null)
			{
				InstanceService.Destroy(newGameButton);
				newGameButton = null;
			}

			if (sceneSelectButton != null)
			{
				InstanceService.Destroy(sceneSelectButton);
				sceneSelectButton = null;
			}

			if (returnButton != null)
			{
				InstanceService.Destroy(returnButton);
				returnButton = null;
			}

            if(menuHeader != null)
            {
                InstanceService.Destroy(menuHeader);
                menuHeader = null;
            }

		}
	}
}
