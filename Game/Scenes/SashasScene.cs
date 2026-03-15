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
	public class SashasScene : Scene
	{
		public SashasScene(string name = "Sasha") : base(name){ }
        examplePlayer player;
        Texture2D background;
        Box box;
        Box box2;
        private PauseMenuPanel pauseMenu;
        private CreditsMenuPanel creditMenu;
        private KeyPad keypad;
        private GameButton menuButton;


        /// <summary>
        /// gets called once at the beginning when the scene is created
        /// </summary>
        public override void CreateScene()
        {
            base.CreateScene();
            player = InstanceService.Instantiate(new examplePlayer());
            keypad = InstanceService.Instantiate(new KeyPad());
            background = TextureService.LoadTexture("Game/Assets/Textures/Error.png");

            Font bangersFont = Raylib.LoadFont("Game/Assets/Fonts/Bangers-Regular.ttf");

            GameText text = InstanceService.Instantiate(new GameText(Color.Yellow, "TEST GAME TEXT", 40, 100, 100));
            UiText name = InstanceService.Instantiate(new UiText(Color.Pink, Name, 30, 20, 20, UiElement.AnchoringPosition.LeftTop));
            name.TextComponent.CustomFont = bangersFont;
            UiText movement = InstanceService.Instantiate(new UiText(Color.Beige, "WASD : MOVE", 20, 20, 40, UiElement.AnchoringPosition.LeftTop));
            UiText jump = InstanceService.Instantiate(new UiText(Color.Beige, "Space : Jump", 20, 20, 50, UiElement.AnchoringPosition.LeftTop));

            menuButton = InstanceService.Instantiate(new GameButton(new Sprite("Game/Assets/Textures/Button.png", 3), string.Empty, -50, 20, UiElement.AnchoringPosition.RightTop));
            //menuButton.SetPosition(new Vector2(200, -60));
            menuButton.OnClicked += OnMenuButtonClicked;
            creditMenu = InstanceService.Instantiate(new CreditsMenuPanel());
            background = TextureService.LoadTexture("Game/Assets/Textures/BGTest.png");

            box = InstanceService.Instantiate(new Box());
            box.SetPosition(300, 280);
            box.chestLoot = Box.ChestLootOptions.Nothing;


            box2 = InstanceService.Instantiate(new Box());
            box2.SetPosition(250, 280);
            box2.chestLoot = Box.ChestLootOptions.HealthPack;



            /*KeyPad keyPad = InstanceService.Instantiate(new KeyPad());
            keyPad.SetPosition(new Vector2(300, 300));
            // door
            RemoteDoor door = InstanceService.Instantiate(new RemoteDoor());
            door.SetPosition(new Vector2(200, 300));
            door.onDoorTriggerEntered += () => SceneService.LoadSceneByIndex(0);

            // connecting door and leaver
            door.sender = keyPad;
            keyPad.reactor = door;*/

            Leaver doorLever = InstanceService.Instantiate(new Leaver());
            doorLever.SetPosition(new Vector2(16 * 93, 16 * 6f));
            // door
            RemoteDoor door = InstanceService.Instantiate(new RemoteDoor());
            door.SetPosition(new Vector2(16 * 50, 16 * 11));
            door.onDoorTriggerEntered += () => SceneService.LoadSceneByIndex(0);

            // connecting door and leaver
            door.sender = doorLever;
            doorLever.reactor = door;

			mainCamera.Target = player;
		}

        public void OnMenuButtonClicked()
        {
            Console.WriteLine("switched to Main Menu");
            SceneService.LoadSceneByName("MainMenu");
        }

        /// <summary>
        /// gets called every frame use this for calucaltions that need to happen every frame
        /// like input or timers
        /// </summary>
        public override void UpdateScene()
		{
			base.UpdateScene();
            Raylib.ShowCursor();
            if (Raylib.IsKeyPressed(KeyboardKey.P))
            {
                if (pauseMenu == null)
                {
                    pauseMenu = InstanceService.Instantiate(new PauseMenuPanel());
                    pauseMenu.OnDestroyed += () => pauseMenu = null;
                    GameManager.GamePaused = true;
                }
                else
                {
                    GameManager.GamePaused = false;
                } 
            }


            if (Name == "Player2") return;

            if (GameManager.GamePaused) return;

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
		}
	}
}
