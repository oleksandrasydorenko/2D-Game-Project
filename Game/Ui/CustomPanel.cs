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
    public class CustomPanel : UiElement
    {
        public GameButton returnButton;
        private GameButton firingRangeEditorButton;
        private GameButton tutorialButton; 
        private GameButton level1Button;
        private GameButton level2Button;
        private GameButton level3Button;
		private GameButton customEditorButton;
        private UiText text;

        public CustomPanel(float x = 0, float y = 0, string name = "CustomPanel", AnchoringPosition anchor = default) :base(x, y, AnchoringPosition.Center, name)
        {
			//Create character selection buttons
			returnButton = InstanceService.Instantiate(new GameButton());
            firingRangeEditorButton = InstanceService.Instantiate(new GameButton(new Sprite("Game/Assets/Textures/customButton.png"),text: "FiringRange Editor"));
            level3Button = InstanceService.Instantiate(new GameButton(new Sprite("Game/Assets/Textures/customButton.png"),text:"Level 3 Editor"));
			level1Button = InstanceService.Instantiate(new GameButton(new Sprite("Game/Assets/Textures/customButton.png"), text: "Level 1 Editor"));
			tutorialButton = InstanceService.Instantiate(new GameButton(new Sprite("Game/Assets/Textures/customButton.png"), text: "Tutorial Editor"));
			level2Button = InstanceService.Instantiate(new GameButton(new Sprite("Game/Assets/Textures/customButton.png"), text: "Level 2 Editor"));
			customEditorButton = InstanceService.Instantiate(new GameButton(new Sprite("Game/Assets/Textures/customButton.png"), text: "Custom Editor"));

            //Set button color
			returnButton.HoveredColor = new Color(196, 0, 0);
            firingRangeEditorButton.HoveredColor = new Color(64, 224, 208);
            level3Button.HoveredColor = new Color(64, 224, 208);
            level1Button.HoveredColor = new Color(64, 224, 208);
            tutorialButton.HoveredColor = new Color(64, 224, 208);
            level2Button.HoveredColor = new Color(64, 224, 208);
			customEditorButton.HoveredColor = new Color(64, 224, 208);

			returnButton.PressedColor = new Color(150, 0, 0);
            firingRangeEditorButton.PressedColor = new Color(0, 120, 120);
            level3Button.PressedColor = new Color(0, 120, 120);
            level1Button.PressedColor = new Color(0, 120, 120);
            tutorialButton.PressedColor = new Color(0, 120, 120);
            level2Button.PressedColor = new Color(0, 120, 120);
			customEditorButton.HoveredColor = new Color(0, 120, 120);

            returnButton.Text = "Return";
            returnButton.CurrentAnchor = AnchoringPosition.LeftBottom;

            //Set button position
            customEditorButton.SetPositionY(175);
            firingRangeEditorButton.SetPositionY(105);
            level1Button.SetPositionY(-105);
            tutorialButton.SetPositionY(-175);  
            level2Button.SetPositionY(-35);
            level3Button.SetPositionY(35);
            returnButton.SetPosition(125, -60);

            //Register pressed events
            returnButton.OnClicked += OnExitButtonClicked;
            firingRangeEditorButton.OnClicked += OnFiringRangeEditorButtonClicked;
            level3Button.OnClicked += OnLevel3ButtonClicked;
			level1Button.OnClicked += OnLevel1ButtonClicked;
			tutorialButton.OnClicked += OnTutorialButtonClicked;
			level2Button.OnClicked += OnLevel2ButtonClicked;
			customEditorButton.OnClicked += OnEditorButtonClicked;

            text = InstanceService.Instantiate(new UiText(Color.White, "Warning: Only Custom Editor will be saved!", 20, -105, -30,AnchoringPosition.Bottom));
        }

        //Event handlers
        public void OnExitButtonClicked()
        {
            InstanceService.Instantiate(new MainMenuPanel());
            InstanceService.Destroy(this);
        }
        public void OnTutorialButtonClicked()
        {
			SceneService.LoadSceneByName("TutorialEditor");
		}
		public void OnLevel1ButtonClicked()
        {
            SceneService.LoadSceneByName("Level1Editor");
		}
        public void OnLevel2ButtonClicked()
        {
            SceneService.LoadSceneByName("Level2Editor");
		}

        public void OnLevel3ButtonClicked()
        {
            SceneService.LoadSceneByName("Level3Editor");
		}

		public void OnEditorButtonClicked()
		{
            SceneService.LoadSceneByName("CustomLevelEditor");
		}

        public void OnFiringRangeEditorButtonClicked()
        {
            SceneService.LoadSceneByName("FiringRangeEditor");
		}

        /// <summary>
        /// Destroys all buttons
        /// </summary>
        public override void Destroy()
        {
            base.Destroy();

            InstanceService.Destroy(level3Button);
            level3Button = null;
            InstanceService.Destroy(tutorialButton);
            tutorialButton = null;
            InstanceService.Destroy(level1Button);
            level1Button = null;
            InstanceService.Destroy(returnButton);
            returnButton = null;
            InstanceService.Destroy(level2Button);
            level2Button = null;
			InstanceService.Destroy(firingRangeEditorButton);
			firingRangeEditorButton = null;
			InstanceService.Destroy(customEditorButton);
			customEditorButton = null;
			InstanceService.Destroy(text);
			text = null;
		}
    }
}
