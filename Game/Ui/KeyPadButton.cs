using JailBreaker.Game.Ui;
using Raylib_cs;
using RocketEngine;
using RocketEngine.Scenemanagement;
using RocketEngine.Ui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static RocketEngine.InstanceService;

namespace JailBreaker.Ui
{
    /// <summary>
    /// implements specific button that is used by keypad
    /// </summary>
    public class KeyPadButton : UIButton
    {
        private AudioComponent clickSound;
        private TextComponent buttonText;
        private string text;
        public string Text
        {
            get { return text; }
            set
            {
                SetText(value);
            }
        }
        public Color DefaultColor = new Color (0, 164, 167);
        public Color HoveredColor = new Color(0, 255, 255);
        public Color PressedColor = new Color(0, 89, 91);


        #region changing the buttons sorting layer, changes the renders sorting layer and the texts at the same time 
        public SortingLayers SortingLayer
        {
            get { return sortingLayer; }
            set
            {
                base.SortingLayer = value;

                if (buttonText != null)
                {
                    buttonText.SortingLayer = value;
                }

            }
        }

        public int ZIndex
        {
            get { return zIndex; }
            set
            {
                base.ZIndex = value;

                if (buttonText != null)
                {
                    buttonText.ZIndex = value;
                }

            }
        }
        #endregion


        public KeyPadButton(Sprite sprite,string text = "", float x = 0, float y = 0, AnchoringPosition anchor = AnchoringPosition.Center, string name = "GameButton") : base(sprite, x, y, anchor, name)
        {
            spriteComponent.colorTint = DefaultColor;
            buttonText = new TextComponent(this, Color.Black, fontSize: 30);
            Text = text;
            ButtonTransformChanged();
            Font bangersFont = Raylib.LoadFont("Game/Assets/Fonts/ZCOOL.ttf");
            buttonText.CustomFont = bangersFont;
            buttonText.size = 50;
            buttonText.color = Color.White;
            clickSound = new AudioComponent(this, "Game/Assets/Audio/keyPadTyping.mp3", false);
        }

        public override void Construct()
        {
            base.Construct();
        }

        protected override void ButtonTransformChanged()
        {
            base.ButtonTransformChanged();

            int textWidth = Raylib.MeasureText(Text, buttonText.size);
            buttonText.offsetX = -textWidth / 2;
            buttonText.offsetY = -buttonText.size / 2;
        }
        public void SetText(string newText)
        {
            text = newText;
            buttonText.text = newText;
            ButtonTransformChanged();
        }

        //these methods change button colors based on button state
        public override void ButtonPressed()
        {
            if (GameManager.GamePaused) return;

            Console.WriteLine("Pressed");
            spriteComponent.colorTint = PressedColor;
            clickSound.PlayOneShot();
        }

        public override void ButtonHovered()
        {
			if (GameManager.GamePaused) return;

			Console.WriteLine("Hovered");
            spriteComponent.colorTint = HoveredColor;
        }

        public override void ButtonUnhovered()
        {
            Console.WriteLine("Unhovered");
            spriteComponent.colorTint = DefaultColor;
        }
        public override void ButtonReleased()
        {
			if (GameManager.GamePaused) return;

			Console.WriteLine("Released");
        }
    }
}
