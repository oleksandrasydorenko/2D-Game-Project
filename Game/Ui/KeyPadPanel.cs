using JailBreaker.Data;
using JailBreaker.Enemy;
using Raylib_cs;
using RocketEngine;
using RocketEngine.Scenemanagement;
using RocketEngine.Ui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static RocketEngine.InstanceService;

namespace JailBreaker.Ui
{
    /// <summary>
    /// KeyPadPanel with buttons. Right combination allows to (for example) open the door
    /// </summary>
    public class KeyPadPanel : UiElement
    {
        private float timer = 0f;
        private float delay = 3f;
        private AudioComponent correctSound;
        private AudioComponent wrongSound;

        public Action onCorrectInputEntered;
        private bool wrongPassword = false;
        private KeyPadButton oneButton;
        private KeyPadButton twoButton;
        private KeyPadButton threeButton;
        private KeyPadButton fourButton;
        private KeyPadButton fiveButton;
        private KeyPadButton sixButton;
        private KeyPadButton sevenButton;
        private KeyPadButton eightButton;
        private KeyPadButton nineButton;
        private KeyPadButton zeroButton;
        private KeyPadButton closeButton;
        private KeyPadButton okButton;
        private int counter;
        private string input;
        public string Input

        {
            get { return input; }
            set
            {
                input = value;
            }
        }
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
        private UiText passwordinput;
        public UiText PasswordInput

        {
            get { return passwordinput; }
            set
            {
                passwordinput = value;
            }
        }

        private string password;
        public string Password

        {
            get { return password; }
            set
            {
                password = value;
            }
        }

        public KeyPadPanel(Sprite sprite = null, float x = 0, float y = 0, AnchoringPosition anchor = AnchoringPosition.Center, string name = "KeyPad") : base(x, y, AnchoringPosition.Center, name)
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

            spriteComponent = new SpriteComponent(this, new Sprite("Game/Assets/Textures/keypadpanel.png"), new Color(0, 255, 255));
            spriteComponent.SpriteScale = 1.8f;

            //Create character selection buttons
            oneButton = InstanceService.Instantiate(new KeyPadButton(new Sprite("Game/Assets/Textures/keypadButton.png")));
            twoButton = InstanceService.Instantiate(new KeyPadButton(new Sprite("Game/Assets/Textures/keypadButton.png")));
            threeButton = InstanceService.Instantiate(new KeyPadButton(new Sprite("Game/Assets/Textures/keypadButton.png")));
            fourButton = InstanceService.Instantiate(new KeyPadButton(new Sprite("Game/Assets/Textures/keypadButton.png")));
            fiveButton = InstanceService.Instantiate(new KeyPadButton(new Sprite("Game/Assets/Textures/keypadButton.png")));
            sixButton = InstanceService.Instantiate(new KeyPadButton(new Sprite("Game/Assets/Textures/keypadButton.png")));
            sevenButton = InstanceService.Instantiate(new KeyPadButton(new Sprite("Game/Assets/Textures/keypadButton.png")));
            eightButton = InstanceService.Instantiate(new KeyPadButton(new Sprite("Game/Assets/Textures/keypadButton.png")));
            nineButton = InstanceService.Instantiate(new KeyPadButton(new Sprite("Game/Assets/Textures/keypadButton.png")));
            zeroButton = InstanceService.Instantiate(new KeyPadButton(new Sprite("Game/Assets/Textures/keypadButton.png")));
            okButton = InstanceService.Instantiate(new KeyPadButton(new Sprite("Game/Assets/Textures/OKKeyPadButton.png")));
            closeButton = InstanceService.Instantiate(new KeyPadButton(new Sprite("Game/Assets/Textures/CloseKeyPadButton.png")));

            passwordinput = InstanceService.Instantiate(new UiText(new Color(0, 255, 255), "Enter the password:", 30, -90, -135));

            correctSound = new AudioComponent(this, "Game/Assets/Audio/keyPadCorrect.mp3", false);
            wrongSound = new AudioComponent(this, "Game/Assets/Audio/keyPadWrong.mp3", false);


            oneButton.SetText("1");
            twoButton.SetText("2");
            threeButton.SetText("3");
            fourButton.SetText("4");
            fiveButton.SetText("5");
            sixButton.SetText("6");
            sevenButton.SetText("7");
            eightButton.SetText("8");
            nineButton.SetText("9");
            zeroButton.SetText("0");

            oneButton.SetPosition(-120, -130);
            twoButton.SetPosition(0, -130);
            threeButton.SetPosition(120, -130);
            fourButton.SetPosition(-120, -10);
            fiveButton.SetPosition(0, -10);
            sixButton.SetPosition(120, -10);
            sevenButton.SetPosition(-120, 110);
            eightButton.SetPosition(0, 110);
            nineButton.SetPosition(120, 110);
            zeroButton.SetPosition(0, 230);
            okButton.SetPosition(-120, 230);
            closeButton.SetPosition(120, 230);


            //Register pressed events
            oneButton.OnClicked += OnOneButtonClicked;
            twoButton.OnClicked += OnTwoButtonClicked;
            threeButton.OnClicked += OnThreeButtonClicked;
            fourButton.OnClicked += OnFourButtonClicked;
            fiveButton.OnClicked += OnFiveButtonClicked;
            sixButton.OnClicked += OnSixButtonClicked;
            sevenButton.OnClicked += OnSevenButtonClicked;
            eightButton.OnClicked += OnEightButtonClicked;
            nineButton.OnClicked += OnNineButtonClicked;
            zeroButton.OnClicked += OnZeroButtonClicked;
            okButton.OnClicked += OnOkButtonClicked;
            closeButton.OnClicked += OnCloseButtonClicked;

			GameManager.ShowCursor = true;

            GameManager.onGameStateChanged += GameUnpausedCheck;
		}

        private void GameUnpausedCheck(GameState newState)
        {
            if(newState == GameState.Play)
            {
                GameManager.ShowCursor = true;
            }
        }


        public override void Update()
        {
            base.Update();

            if (wrongPassword)
            {
                Console.WriteLine("falxseeeeeeeeeeeeeeeeee");
                timer += Time.DeltaTime;
                if (timer >= delay)
                {
                    counter = 0;
                    input = "";
                    InstanceService.Destroy(passwordinput);
                    wrongPassword = false;
                    timer = 0;
                }
            }
        }

        //Event handlers
        public void OnOneButtonClicked()
        {
			if (GameManager.GamePaused) return;

			counter++;
            if (counter <= 4)
            {
                if (passwordinput != null)
                {
                    InstanceService.Destroy(passwordinput);
                }
                input = input + "1";
                passwordinput = InstanceService.Instantiate(new UiText(new Color(0, 255, 255), input, 60, -90, -145));
            }
        }
        public void OnTwoButtonClicked()
        {
			if (GameManager.GamePaused) return;

			counter++;
            if (counter <= 4)
            {
                if (passwordinput != null)
                {
                    InstanceService.Destroy(passwordinput);
                }
                input = input + "2";
                passwordinput = InstanceService.Instantiate(new UiText(new Color(0, 255, 255), input, 60, -90, -145));
            }
        }
        public void OnThreeButtonClicked()
        {
			if (GameManager.GamePaused) return;

			counter++;
            if (counter <= 4)
            {
                if (passwordinput != null)
                {
                    InstanceService.Destroy(passwordinput);
                }
                input = input + "3";
                passwordinput = InstanceService.Instantiate(new UiText(new Color(0, 255, 255), input, 60, -90, -145));
            }
        }
        public void OnFourButtonClicked()
        {
			if (GameManager.GamePaused) return;

			counter++;
            if (counter <= 4)
            {
                if (passwordinput != null)
                {
                    InstanceService.Destroy(passwordinput);
                }
                input = input + "4";
                passwordinput = InstanceService.Instantiate(new UiText(new Color(0, 255, 255), input, 60, -90, -145));
            }
        }
        public void OnFiveButtonClicked()
        {
			if (GameManager.GamePaused) return;

			counter++;
            if (counter <= 4)
            {
                if (passwordinput != null)
                {
                    InstanceService.Destroy(passwordinput);
                }
                input = input + "5";
                passwordinput = InstanceService.Instantiate(new UiText(new Color(0, 255, 255), input, 60, -90, -145));
            }
        }
        public void OnSixButtonClicked()
        {
			if (GameManager.GamePaused) return;

			counter++;
            if (counter <= 4)
            {
                if (passwordinput != null)
                {
                    InstanceService.Destroy(passwordinput);
                }
                input = input + "6";
                passwordinput = InstanceService.Instantiate(new UiText(new Color(0, 255, 255), input, 60, -90, -145));
            }
        }
        public void OnSevenButtonClicked()
        {
			if (GameManager.GamePaused) return;

			counter++;
            if (counter <= 4)
            {
                if (passwordinput != null)
                {
                    InstanceService.Destroy(passwordinput);
                }
                input = input + "7";
                passwordinput = InstanceService.Instantiate(new UiText(new Color(0, 255, 255), input, 60, -90, -145));
            }
        }
        public void OnEightButtonClicked()
        {
			if (GameManager.GamePaused) return;

			counter++;
            if (counter <= 4)
            {
                if (passwordinput != null)
                {
                    InstanceService.Destroy(passwordinput);
                }
                input = input + "8";
                passwordinput = InstanceService.Instantiate(new UiText(new Color(0, 255, 255), input, 60, -90, -145));
            }
        }
        public void OnNineButtonClicked() 
        {
			if (GameManager.GamePaused) return;

			counter++;
            if ( counter <= 4)
            {
                if (passwordinput != null)
                {
                    InstanceService.Destroy(passwordinput);
                }
                input = input + "9";
                passwordinput = InstanceService.Instantiate(new UiText(new Color(0, 255, 255), input, 60, -90, -145));
            }
        }
        public void OnZeroButtonClicked()
        {
			if (GameManager.GamePaused) return;

			counter++;
            if (counter <= 4)
            {
                if (passwordinput != null)
                {
                    InstanceService.Destroy(passwordinput);
                }
                input = input + "0";
                passwordinput = InstanceService.Instantiate(new UiText(new Color(0, 255, 255), input, 60, -90, -145));
            }
        }
        public void OnOkButtonClicked()
        {
			if (GameManager.GamePaused) return;

			if (counter >= 4 && input == Password) //password is correct
            {
                if (passwordinput != null)
                {
                    InstanceService.Destroy(passwordinput);
                }
                passwordinput = InstanceService.Instantiate(new UiText(new Color(0, 255, 0), "Door opened! :)", 30, -90, -135));
                correctSound.PlayOneShot();
                onCorrectInputEntered?.Invoke();
                InstanceService.Destroy(this);
            }
            else if (counter < 4) //password is too short
            {
                passwordinput.Color = Color.Red;
                wrongSound.PlayOneShot();
            }
            else if (counter >= 4 && input != Password) //password is false
            {
                if (passwordinput != null)
                {
                    InstanceService.Destroy(passwordinput);
                }
                passwordinput = InstanceService.Instantiate(new UiText(Color.Red, "Wrong! New enemies incoming", 25, -90, -135));
                wrongSound.PlayOneShot();
                wrongPassword = true;
                InstanceService.InstantiateWithPosition(new Drone(), new Vector2(16 * 46, 16 * 21));
                InstanceService.InstantiateWithPosition(new DogRoboter(), new Vector2(16 * 53, 16 * 26));
            }
        }
        public void OnCloseButtonClicked() //deletes password input
        {
			if (GameManager.GamePaused) return;

			if (passwordinput != null)
            {
                InstanceService.Destroy(passwordinput);
            }
            counter = 0;
            input = "";
        }

        /// <summary>
        /// destroys all buttons, KeyPadPanel and UITexts
        /// </summary>
        public override void Destroy()
        {
            base.Destroy ();
            input = "";
            counter = 0;
            if (oneButton != null)
            {
                oneButton.OnClicked -= OnOneButtonClicked;
                InstanceService.Destroy(oneButton);
                oneButton = null;
            }

            if (twoButton != null)
            {
                twoButton.OnClicked -= OnTwoButtonClicked;
                InstanceService.Destroy(twoButton);
                twoButton = null;
            }

            if (threeButton != null)
            {
                threeButton.OnClicked -= OnThreeButtonClicked;
                InstanceService.Destroy(threeButton);
                threeButton = null;
            }
            if (fourButton != null)
            {
                fourButton.OnClicked -= OnFourButtonClicked;
                InstanceService.Destroy(fourButton);
                fourButton = null;
            }

            if (fiveButton != null)
            {
                fiveButton.OnClicked -= OnFiveButtonClicked;
                InstanceService.Destroy(fiveButton);
                fiveButton = null;
            }

            if (sixButton != null)
            {
                sixButton.OnClicked -= OnSixButtonClicked;
                InstanceService.Destroy(sixButton);
                sixButton = null;
            }
            if (sevenButton != null)
            {
                sevenButton.OnClicked -= OnSevenButtonClicked;
                InstanceService.Destroy(sevenButton);
                sevenButton = null;
            }

            if (eightButton != null)
            {
                eightButton.OnClicked -= OnEightButtonClicked;
                InstanceService.Destroy(eightButton);
                eightButton = null;
            }

            if (nineButton != null)
            {
                nineButton.OnClicked -= OnNineButtonClicked;
                InstanceService.Destroy(nineButton);
                nineButton = null;
            }
            if (zeroButton != null)
            {
                zeroButton.OnClicked -= OnZeroButtonClicked;
                InstanceService.Destroy(zeroButton);
                zeroButton = null;
            }
            if (okButton != null)
            {
                okButton.OnClicked -= OnOkButtonClicked;
                InstanceService.Destroy(okButton);
                okButton = null;
            }
            if (closeButton != null)
            {
                closeButton.OnClicked -= OnCloseButtonClicked;
                InstanceService.Destroy(closeButton);
                closeButton = null;
            }
            if (passwordinput != null)
            {
                InstanceService.Destroy(passwordinput);
                passwordinput = null;
            }

            GameManager.ShowCursor = false;
        }
    }
}