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
    /// PauseMenuPanel, allows to continue the game or switch to main menu
    /// </summary>
    public class PauseMenuPanel : UiElement
    {
        public Action OnDestroyed;
        private GameButton exitButton;
        private GameButton resumeButton;
		private GameButton restartButton;
		private UiText pauseMenuHeader;
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
        public PauseMenuPanel(Sprite sprite = null,float x = 0, float y = 0, AnchoringPosition anchor = AnchoringPosition.Center, string name = "PauseMenuPanel") : base(x, y, AnchoringPosition.Center, name)
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

            int screenWidth = Raylib.GetScreenWidth();
            float factor = (float)screenWidth / backgroundSprite.FrameWidth;
            spriteComponent.SpriteScale = factor;
            pauseMenuHeader = InstanceService.Instantiate(new UiText(new Color(64, 224, 208), "PAUSE", 90));
            pauseMenuHeader.CurrentAnchor = AnchoringPosition.Top;
            //Create character selection buttons
            exitButton = InstanceService.Instantiate(new GameButton());
            resumeButton = InstanceService.Instantiate(new GameButton());
			restartButton = InstanceService.Instantiate(new GameButton());

			exitButton.HoveredColor = new Color(196, 0, 0);
            resumeButton.HoveredColor = new Color(64, 224, 208);
			restartButton.HoveredColor = new Color(64, 224, 208);

			exitButton.PressedColor = new Color(150, 0, 0);
            resumeButton.PressedColor = new Color(0, 120, 120);
			restartButton.PressedColor = new Color(0, 120, 120);

			exitButton.SetText("Main Menu");
            resumeButton.SetText("Continue");
			restartButton.SetText("Restart");

			exitButton.SetPosition(0, 105);
            resumeButton.SetPosition(0, -35);
			restartButton.SetPosition(0, 35);
			pauseMenuHeader.SetPosition(-165, 70);

            //Register pressed events
            exitButton.OnClicked += OnExitButtonClicked;
            resumeButton.OnClicked += OnResumeButtonClicked;
			restartButton.OnClicked += OnRestartButtonClicked;
			//Register GameManager events
			GameManager.onGameUnpaused += OnGameUnpaused;
        }

        //Event handlers
        public void OnExitButtonClicked()
        {
            Console.WriteLine("switched to main menu");
            SceneService.LoadSceneByIndex(0);
            GameManager.GamePaused = false;
        }
        public void OnResumeButtonClicked()
        {
            Console.WriteLine("Game continued");
            GameManager.GamePaused = false;
        }
        /// <summary>
        /// implements pause logic
        /// </summary>
        public void OnGameUnpaused()
        {
            Console.WriteLine("Game unpaused");
            GameManager.onGameUnpaused -= OnGameUnpaused;
            if (exitButton != null)
            {
                exitButton.OnClicked -= OnExitButtonClicked;
                InstanceService.Destroy(exitButton);
                exitButton = null;
            }

            if (resumeButton != null)
            {
                resumeButton.OnClicked -= OnResumeButtonClicked;
                InstanceService.Destroy(resumeButton);
                resumeButton = null;
            }

			if (restartButton != null)
			{
				restartButton.OnClicked -= OnResumeButtonClicked;
				InstanceService.Destroy(restartButton);
				restartButton = null;
			}

			if (pauseMenuHeader != null)
            {
                InstanceService.Destroy(pauseMenuHeader);
                pauseMenuHeader = null;
            }
            InstanceService.Destroy(this);
            OnDestroyed?.Invoke();
            GameManager.ShowCursor = false;
        }
		public void OnRestartButtonClicked()
		{
			OnResumeButtonClicked();
			SceneService.LoadScene(SceneService.ActiveScene);
            
		}
	}


}
