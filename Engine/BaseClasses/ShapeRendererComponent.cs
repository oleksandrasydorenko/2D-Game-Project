using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Security;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Raylib_cs;

namespace RocketEngine
{
	public abstract class ShapeRendererComponent : InstantiableComponent, IDrawable, IUiDrawable
	{
		public bool visible = true;
		public Color color;
		public Vector2 textureOffset;


		private SortingLayers sortingLayer = SortingLayers.Default;
		public SortingLayers SortingLayer
		{
			get { return sortingLayer; }
			set { Utils.SortingLayerHelper.ChangeSortingLayer(this, ref sortingLayer, value, parentIsUiElement); }
		}

		private int zIndex = 0;
		public int ZIndex
		{
			get { return zIndex; }
			set { Utils.SortingLayerHelper.ChangeZIndex(this, ref zIndex, value, parentIsUiElement); }
		}


		private bool parentIsUiElement = false;


		public ShapeRendererComponent(ComponentBase parent, Color color, float offsetX = 0, float offsetY = 0, string name = "ShapeRenderer") :base(parent, name)
		{
			if (parent is Ui.UiElement) // if the parent is not a ui element then we draw the sprite in the world and not on the screen
			{
				parentIsUiElement = true;
				SortingLayer = sortingLayer;
				ZIndex = zIndex;
			}

			this.color = color;
			textureOffset = new Vector2(offsetX, offsetY);

			Parent.onTransformChanged += TransformChanged;
		}

		protected virtual void TransformChanged()
		{

		}

		public virtual void DrawUi()
		{
			if (!parentIsUiElement) return;

			Render();


		}

		public virtual void Draw()
		{
			if (parentIsUiElement) return;

			Render();
		}

		protected virtual void Render()
		{
			if (!visible) return;
		}
	}
}
