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

namespace JailBreaker.Ui
{
    public class GameButton : UIButton
    {
        public AudioComponent clickSound;
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
        public Color DefaultColor = Color.Gray;
        public Color HoveredColor = Color.White;
        public Color PressedColor = Color.DarkGray;


		#region changing the buttons sorting layer, changes the renders sorting layer and the texts at the same time 
		public SortingLayers SortingLayer
		{
			get { return sortingLayer; }
			set
			{
				base.SortingLayer = value;

                if(buttonText != null)
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


		public GameButton(Sprite sprite = null,string text = "Button", float x = 0, float y = 0, AnchoringPosition anchor = AnchoringPosition.Center, string name = "GameButton") : base(sprite,x,y,anchor,name)
        {
            spriteComponent.colorTint = DefaultColor;
			buttonText = new TextComponent(this, Color.Black,fontSize: 30);
			Text = text;
            ButtonTransformChanged();
            clickSound = new AudioComponent(this, "Game/Assets/Audio/click.wav", false);
        }


        protected override void ButtonTransformChanged()
        {
            base.ButtonTransformChanged();

            int textWidth = Raylib.MeasureText(Text, buttonText.size);
            buttonText.offsetX = -textWidth / 2;
            buttonText.offsetY = -buttonText.size / 2;
        }
        public override void ButtonPressed()
        {
            Console.WriteLine("Pressed");
            spriteComponent.colorTint = PressedColor;
        }
        public void SetText(string newText)
        {
            text = newText;
            buttonText.text = newText;
            ButtonTransformChanged();

		}

        public override void ButtonHovered()
        {
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
            Console.WriteLine("Released");
            clickSound.PlayOneShot();
        }
    }
}
