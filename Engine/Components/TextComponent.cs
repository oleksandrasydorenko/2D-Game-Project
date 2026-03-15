using Raylib_cs;
using RocketEngine.Ui;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Security;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;


namespace RocketEngine
{
    public class TextComponent : InstantiableComponent, IUiDrawable, IDrawable
    {
        public Color color;
        public float offsetX;
        public float offsetY;
        public int size;
        public string text;

        public bool visible = true;

        public Font? CustomFont { get; set; } = null;

        private SortingLayers sortingLayer = SortingLayers.Default;
		public SortingLayers SortingLayer
		{
			get { return sortingLayer; }
			set { Utils.SortingLayerHelper.ChangeSortingLayer(this, ref sortingLayer, value, parentIsUiElement); }
		}

		private int zIndex = 0;
        private bool parentIsUiElement;

        public int ZIndex
		{
			get { return zIndex; }
			set { Utils.SortingLayerHelper.ChangeZIndex(this, ref zIndex, value, parentIsUiElement); }
		}

        public TextComponent(ComponentBase parent) :base(parent, "TextComponent")
		{
            if (parent is Ui.UiElement) // if the parent is not a ui element then we draw the sprite in the world and not on the screen
            {
                parentIsUiElement = true;
				SortingLayer = sortingLayer;
				ZIndex = zIndex;
			}
            this.color = Color.White;
			this.offsetX = 0;
			this.offsetY = 0;
			this.size = 20;
			this.text = "Text";
		}

		public TextComponent(ComponentBase parent, Color color, string text = "Text", int fontSize = 20, float offsetX = 0, float offsetY = 0, string name = "TextComponent") : base(parent, name)
        {
            if (parent is Ui.UiElement) // if the parent is not a ui element then we draw the sprite in the world and not on the screen
            {
                parentIsUiElement = true;
            }
            this.color = color;
            this.offsetX = offsetX;
            this.offsetY = offsetY;
            this.size = fontSize;
            this.text = text;
        }

        public void DrawUi()
        {
            if (!parentIsUiElement)
            {
                return;
            }
            Render();
        }

        public void Draw()
        {
            if (parentIsUiElement)
            {
                return;
            }
            Render();
        }

        public void Render()
        {
            if (!visible) return;
            int finalX = (int)(Parent.GetPositionX() + offsetX);
            int finalY = (int)(Parent.GetPositionY() + offsetY);
            if (CustomFont.HasValue)
            {
                Raylib.DrawTextEx(CustomFont.Value, text, new Vector2(finalX, finalY), size, 0, color);
            }
            else
            {
                Raylib.DrawText(text, finalX, finalY, size, color);
            }
        }
    }
}