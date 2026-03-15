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


namespace RocketEngine.Ui
{
    /// <summary>
    /// UIButton with different states and ability to handle mouse interactions
    /// </summary>
    public class UIButton : UiElement, ISortingLayer
	{

		public override bool Visible { get => base.Visible; set { base.Visible = value; spriteComponent.visible = visible; } }

		public Action OnPressed { get; set; }
        public Action OnHovered { get; set; }
        public Action OnUnhovered { get; set; }
        public Action OnReleased { get; set; }

		public Action OnClicked { get; set; }
		public enum ButtonState { Unhovered = 0, Hovered = 1, Pressed =2, Released = 3}
        protected ButtonState currentButtonState = ButtonState.Unhovered;
        protected ButtonState previousButtonState = ButtonState.Unhovered;

		protected Rectangle buttonBounds;

        protected Sprite buttonSprite; 
      
        public Sprite ButtonSprite // if we set the button sprite we also need to update the sprite component
        {
            get { return buttonSprite; }
            set 
            { 
                buttonSprite = value; 
                spriteComponent.sprite = buttonSprite;
            }
        }
        public SpriteComponent spriteComponent;

        public ButtonState CurrentButtonState
        {
            get
                { return currentButtonState; }
            protected set {  currentButtonState = value; }
        }
        public ButtonState PreviousButtonState
        {
            get
            { return previousButtonState; }
            protected set { previousButtonState = value; }
        }


		#region changing the buttons sorting layer, changes the renders sorting layer at the same time 
		protected SortingLayers sortingLayer = SortingLayers.Default;
		public SortingLayers SortingLayer
		{
			get { return sortingLayer; }
			set 
            {
				Utils.SortingLayerHelper.ChangeSortingLayer(this, ref sortingLayer, value, false);

				if (spriteComponent != null)
                {
                    spriteComponent.SortingLayer = value;
                }

                if(boundsOutline != null)
                {
                    boundsOutline.SortingLayer = value;
                }
            
            }
		}

		protected int zIndex = 0;
		public int ZIndex
		{
			get { return zIndex; }
			set 
            {

				Utils.SortingLayerHelper.ChangeZIndex(this, ref zIndex, value, false);

				if (spriteComponent != null)
                {
                    spriteComponent.ZIndex = value; 
                }

				if (boundsOutline != null)
				{
					boundsOutline.ZIndex = value + 1 ; // so the outline is above the renderer
				}
			}
		}
		#endregion 

		#region Debugging

		private RectangleLineRendererComponent boundsOutline;
		private bool showBounds = false;

		public bool ShowBounds
		{
			get
			{
				return showBounds;
			}
			set
			{
				showBounds = value;

				if (showBounds && boundsOutline == null) // if we want to show the bounds we 
				{
					boundsOutline = new RectangleLineRendererComponent(this, Color.Red);

					if (buttonSprite != null)
					{
						CalcuateButtonBounds();
					}
				}
				else if (!showBounds && boundsOutline != null)
				{
					boundsOutline.Destroy();
				}
			}
		}

		#endregion

		public UIButton(Sprite sprite = null, float x = 0, float y = 0, AnchoringPosition anchor = AnchoringPosition.Center, string name = "UiButton") : base(x, y, anchor, name)
		{
			this.buttonSprite = sprite;
			if (buttonSprite == null)
			{
				this.buttonSprite = new Sprite("Game/Assets/Textures/defaultButton.png");
			}

			onTransformChanged += ButtonTransformChanged;// we only calucate the bounds if the buttons position got changed
			spriteComponent = new SpriteComponent(this, buttonSprite, Color.White);
			spriteComponent.SpriteScale = 3f;

            CalcuateButtonBounds();

			#region Debugging
			if (showBounds)
            {
                ShowBounds = showBounds;
            }
			#endregion

		}

		protected virtual void ButtonTransformChanged()
        {
            CalcuateButtonBounds();
        }
        /// <summary>
        /// Calculate button collision bounds (based on sprite size)
        /// </summary>
        protected void CalcuateButtonBounds()
        {
			buttonBounds = new Rectangle(GetPositionX() - (buttonSprite.FrameWidth * spriteComponent.SpriteScale / 2), GetPositionY() - (buttonSprite.FrameHeight * spriteComponent.SpriteScale / 2), buttonSprite.FrameWidth * spriteComponent.SpriteScale, buttonSprite.FrameHeight * spriteComponent.SpriteScale);

			#region Debugging
			if (ShowBounds && boundsOutline != null)
            {
                boundsOutline.rec.Size = buttonBounds.Size; // we dont need to do calcuations again for the debug outline if we can just reuse the new bounds
			}
			#endregion
		}
		public override void Update()
        {
            base.Update();
  
            Vector2 mousePos = Raylib.GetMousePosition();
            bool mouseIsOver = Raylib.CheckCollisionPointRec(mousePos, buttonBounds);

            if (mouseIsOver)
            {
                if (Raylib.IsMouseButtonDown(MouseButton.Left))
                {
                    if (CurrentButtonState != ButtonState.Pressed && CurrentButtonState != ButtonState.Unhovered)
                    {
                        PreviousButtonState = currentButtonState;
                        CurrentButtonState = ButtonState.Pressed;
                        ButtonPressed();
                    }
                }
                else
                {
                    if (CurrentButtonState != ButtonState.Hovered)
                    {
                        PreviousButtonState = currentButtonState;
                        CurrentButtonState = ButtonState.Hovered;
                        OnHovered?.Invoke();
                        ButtonHovered();

                    }
                    if (Raylib.IsMouseButtonReleased(MouseButton.Left) && PreviousButtonState == ButtonState.Pressed)
                    {
                        if (CurrentButtonState != ButtonState.Released)
                        {
                            PreviousButtonState = currentButtonState;
                            CurrentButtonState = ButtonState.Released;

                            OnReleased?.Invoke();
                            ButtonReleased();

                            if(mouseIsOver)
                            {
                                OnClicked?.Invoke();
                                ButtonClicked();
                            }

                            return;
                        }
                    }
                }
            }
            else
            {
                //Keep pressed state if mouse was in pressed state and was moved outside
                if (Raylib.IsMouseButtonDown(MouseButton.Left) && CurrentButtonState == ButtonState.Pressed)
                {
                    return;
                }
                else if (CurrentButtonState == ButtonState.Pressed && PreviousButtonState != ButtonState.Pressed)
                {
                    OnPressed?.Invoke();
                }

                if (CurrentButtonState != ButtonState.Unhovered)
                {
                    PreviousButtonState = currentButtonState;
                    CurrentButtonState = ButtonState.Unhovered;
                    OnUnhovered?.Invoke();
                    ButtonUnhovered();

                }
            }
        }



		public virtual void ButtonPressed()
        {
            Console.WriteLine("Pressed");
        }

        public virtual void ButtonHovered()
        {
            Console.WriteLine("Hovered");
        }

        public virtual void ButtonUnhovered()
        {
            Console.WriteLine("Unhovered");
        }
        public virtual void ButtonReleased()
        {
            Console.WriteLine("Released");
        }
        
        public virtual void ButtonClicked()
        {
            Console.WriteLine("Clicked");
        }
    }
}
