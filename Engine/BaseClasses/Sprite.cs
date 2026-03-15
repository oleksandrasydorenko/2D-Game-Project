using Raylib_cs;
using RocketEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketEngine
{

	public class Sprite
	{

		private string texturePath;

		private Texture2D texture;
		public Texture2D Texture
		{
			get { return texture; }
			set {texture = value;}
		}

		private int tileAmount = 1;
		public int TileAmount
		{
			get { return tileAmount; }
			set
			{

				if (value <= 0) value = 1;

				tileAmount = value;
				FrameWidth = texture.Width / value;
				FrameHeight = texture.Height;
			}
		}

		private int currentFrame;
		public int CurrentFrame 
		{
			get { return currentFrame; }
			set
			{
				if (value < 0) value = 0;
				currentFrame = value % tileAmount;
			}
		}

		public int FrameWidth { get; private set; }
		public int FrameHeight { get; private set; }

		public Sprite(string texturePath, int tileAmount = 1, int startFrame = 0)
		{
			this.texturePath = texturePath;
			texture = SetSprite(texturePath);
			TileAmount = tileAmount;
			CurrentFrame = startFrame;

		}

		private Texture2D SetSprite(string texturePath)
		{
			return TextureService.LoadTexture(texturePath);
		}


	}
}


