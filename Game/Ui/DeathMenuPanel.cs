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

namespace JailBreaker.Ui
{
    /// <summary>
    /// DeathMenuPanel with buttons, each button loads different scene
    /// </summary>
    public class DeathMenuPanel : UiElement
    {
        private GameButton menuButton;
        private GameButton restartButton;
        private UiText menuHeader;
        public SpriteComponent spriteComponent;
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

        public DeathMenuPanel(Sprite sprite = null, float x = 0, float y = 0, AnchoringPosition anchor = AnchoringPosition.Center, string name = "DeathMenuPanel") : base(x, y, AnchoringPosition.Center, name)
        {
            this.backgroundSprite = sprite;
            if (backgroundSprite == null)
            {
                this.backgroundSprite = new Sprite("Game/Assets/Textures/BGTest.png");
            }
        }

        public override void Construct()
        {
            base.Construct();

            spriteComponent = new SpriteComponent(this, backgroundSprite, new Color(0, 0, 0, 255), offsetX: 450,offsetY: 450);
            spriteComponent.SpriteScale = 10;

            //Create character selection buttons
            menuButton = InstanceService.Instantiate(new GameButton());
            restartButton = InstanceService.Instantiate(new GameButton());

            menuButton.HoveredColor = Color.Red;
            restartButton.HoveredColor = Color.Red;

            menuButton.PressedColor = new Color(120, 0, 0);
            restartButton.PressedColor = new Color(120, 0, 0);

            menuButton.SetText("Main menu");
            restartButton.SetText("Restart");

            menuButton.CurrentAnchor = AnchoringPosition.Center;
            restartButton.CurrentAnchor = AnchoringPosition.Center;

            menuButton.SetPosition(0, 35);
            restartButton.SetPosition(0, -35);


            //Register pressed events
            menuButton.OnClicked += OnMainMenuButtonClicked;
            restartButton.OnClicked += OnRestartButtonClicked;

            menuHeader = InstanceService.Instantiate(new UiText(Color.Red, "GAME OVER", 110, -155, -140, UiElement.AnchoringPosition.Center));
        }

        //Event handlers
        public void OnMainMenuButtonClicked()
        {
            Console.WriteLine("switched to Main Menu");
            SceneService.LoadSceneByName("MainMenu");
        }
        public void OnRestartButtonClicked()
        {
            Console.WriteLine("switched to ... scene");
            SceneService.LoadScene(SceneService.ActiveScene);
        }
    }
}

