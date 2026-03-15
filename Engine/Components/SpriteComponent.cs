using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Security;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Raylib_cs;
using RocketEngine.Scenemanagement;

namespace RocketEngine
{
	/// <summary>
	/// Das Sprite Component kümmert sich darum sprites in der szene darzustellen
	/// </summary>
	public class SpriteComponent : InstantiableComponent, IDrawable, IUiDrawable
	{
		public bool visible = true;

		private SortingLayers sortingLayer = SortingLayers.Default;
		public SortingLayers SortingLayer
		{
			get { return sortingLayer; }
			set {  Utils.SortingLayerHelper.ChangeSortingLayer(this, ref sortingLayer ,value, parentIsUiElement); }
		}

		private int zIndex = 0;
		public int ZIndex
		{
			get { return zIndex; }
			set { Utils.SortingLayerHelper.ChangeZIndex(this, ref zIndex, value, parentIsUiElement); }
		}

		private float finalRotation;
		private float localRotation = 0;
		public float LocalRotation
		{
			get
			{
				return localRotation;
			}
			set
			{
				value = value % 360;
				localRotation = value;	
				TransformChanged();
			}
		}

		public Sprite sprite;

		public Color colorTint;

		private Vector2 textureOffset;
		public Vector2 TextureOffset
		{
			get { return textureOffset; }
			set { textureOffset = value; TransformChanged(); }
		}

		private float spriteScale = 1f;
		public float SpriteScale
		{
			get { return spriteScale; }
			set {  spriteScale = value; TransformChanged(); }
		}

		private bool flipSpriteVerticaly;
		private int verticalFlipFactor = 1;

		private bool parentIsUiElement = false;

		public bool FlipSpriteVerticaly
		{
			get { return flipSpriteVerticaly; }
			set
			{
				flipSpriteVerticaly = value;

				if (flipSpriteVerticaly)
				{
					verticalFlipFactor = -1;
				}
				else
				{
					verticalFlipFactor = 1;
				}
			}
		}

		private bool flipSpriteHorizontaly = false;
		private int horizontalFlipFactor = 1;
		public bool FlipSpriteHorizontaly
		{
			get { return flipSpriteHorizontaly; }
			set {
				flipSpriteHorizontaly = value;

				if (flipSpriteHorizontaly)
				{
					horizontalFlipFactor = -1;
				}
				else
				{
					horizontalFlipFactor = 1;
				}
			}
		}

		private Rectangle source = new Rectangle();
		private Rectangle destination = new Rectangle();
		private Vector2 origin = new Vector2();
		public SpriteComponent(ComponentBase parent, Sprite sprite, Color color, string name = "SpriteComponent", float offsetX = 0, float offsetY = 0) : base(parent,name)
		{
			if (parent is Ui.UiElement) // if the parent is not a ui element then we draw the sprite in the world and not on the screen
			{
				parentIsUiElement = true;
				SortingLayer = sortingLayer;
				ZIndex = zIndex;
			}

			this.sprite = sprite;

			this.colorTint = color;
			this.TextureOffset = new Vector2(offsetX, offsetY);

			source.Y = 0;

			parent.onTransformChanged += TransformChanged;

			TransformChanged();
		}


		private void TransformChanged()
		{
			//

			float texturePosX = (Parent.GetPosition().X + TextureOffset.X);
			float texturePosY = (Parent.GetPosition().Y + TextureOffset.Y);


			source.X = sprite.FrameWidth * sprite.CurrentFrame;
			source.Width = sprite.FrameWidth * verticalFlipFactor;
			source.Height = sprite.FrameHeight * horizontalFlipFactor;

			destination.X = texturePosX;
			destination.Y = texturePosY;
			destination.Width = sprite.FrameWidth * SpriteScale;
			destination.Height = sprite.FrameHeight * SpriteScale;


			origin.X = destination.Width / 2;
			origin.Y = destination.Height / 2;

			finalRotation = (Parent.GetRotationInDeg() + LocalRotation) % 360;
		}

		public void DrawUi()
		{

			if(!parentIsUiElement) return;

			Render();

           
		}

		public void Draw()
		{
			if(parentIsUiElement) return;

			// check if this really makes it more performant
			//if (((SceneService.ActiveScene.mainCamera.Target) - Parent.GetPosition()).Length() > 50) return;

			Render();
		}
		private void Render()
		{
            if (!visible) return;

			source.X = sprite.FrameWidth * sprite.CurrentFrame;
			source.Width = sprite.FrameWidth * verticalFlipFactor;
			source.Height = sprite.FrameHeight * horizontalFlipFactor;

			Raylib.DrawTexturePro(sprite.Texture, source, destination, origin, finalRotation, colorTint);
        }
	}
}
