using Raylib_cs;
using System.Numerics;
using static Raylib_cs.Raylib;
using static RocketEngine.DrawService;
using static RocketEngine.UpdateService;
using static System.Net.Mime.MediaTypeNames;

namespace RocketEngine.Ui
{
    /// <summary>
    /// Base for all UI elements, implements anchoring system that allows
    /// to place UI elements on relative position to screen edges
    /// </summary>
    public class UiElement : ComponentBase
    {

        protected bool visible;
        public virtual bool Visible { get { return visible; } set { visible = value; } }

        public enum AnchoringPosition { Center = 0, Right = 1, Left = 2, RightBottom = 3, RightTop = 4, LeftBottom = 5, LeftTop = 6, Top = 7, Bottom = 8}
        private float anchorOffsetX;
        private float anchorOffsetY;
        private AnchoringPosition currentAnchor;
        public AnchoringPosition CurrentAnchor
        {
            get => currentAnchor;
            set
            {
				
				currentAnchor = value;

				float x = GetPositionX() - anchorOffsetX;
				float y = GetPositionY() - anchorOffsetY;

				SetAnchoringPosition();

				SetPosition(new Vector2(x,y));
            }
        }
		public UiElement() : base() 
        {
            Name = "UiElement";
            CurrentAnchor = AnchoringPosition.Center;
        }

		public UiElement(float x = 0, float y = 0, AnchoringPosition anchor = AnchoringPosition.Center, string name = "UiElement") : base(x, y, name) 
        { 
            CurrentAnchor = anchor;
        }
        /// <summary>
        /// Calculates anchor offset values based on current anchor 
        /// </summary>
        private void SetAnchoringPosition()
        {
            int screenWidth = GetScreenWidth();
            int screenHeight = GetScreenHeight();

            switch (currentAnchor)
            {
                case AnchoringPosition.Center:
                    anchorOffsetX = screenWidth / 2;
                    anchorOffsetY = screenHeight / 2;
                    break;
                case AnchoringPosition.Right:
					anchorOffsetX = screenWidth;
                    anchorOffsetY = screenHeight / 2;
                    break;
                case AnchoringPosition.Left:
                    anchorOffsetX = 0;
                    anchorOffsetY = screenHeight / 2;
                    break;
                case AnchoringPosition.RightTop:
                    anchorOffsetX = screenWidth;
                    anchorOffsetY = 0;
                    break;
                case AnchoringPosition.LeftTop:
                    anchorOffsetX = 0;
                    anchorOffsetY = 0;
                    break;
                case AnchoringPosition.RightBottom:
                    anchorOffsetX = screenWidth;
                    anchorOffsetY = screenHeight;
                    break;
                case AnchoringPosition.LeftBottom:
                    anchorOffsetX = 0;
                    anchorOffsetY = screenHeight;
                    break;
                case AnchoringPosition.Top:
                    anchorOffsetX = screenWidth/2;
                    anchorOffsetY = 0;
                    break;
                case AnchoringPosition.Bottom:
                    anchorOffsetX = screenWidth / 2;
                    anchorOffsetY = screenHeight;
                    break;
                default:
                    anchorOffsetX = 0;
                    anchorOffsetY = 0;
                    break;
            }
        }
        public override void SetPosition(Vector2 newPosition)
        {
            base.SetPosition(newPosition.X + anchorOffsetX, newPosition.Y + anchorOffsetY);
        }

        public override void SetPosition(float newX, float newY)
        {
            base.SetPosition(newX + anchorOffsetX, newY + anchorOffsetY);
        }
        public override void SetPositionX(float newX)
        {
            base.SetPositionX(newX + anchorOffsetX);
        }
        public override void SetPositionY( float newY)
        {
            base.SetPositionY(newY + anchorOffsetY);
        }
    }
}
