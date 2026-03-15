using Raylib_cs;
using RocketEngine;
using RocketEngine.Ui;
using RocketEngine.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace JailBreaker.Ui
{
	/// <summary>
	/// TO DO:
	/// events when finished
	/// only subscribe to update loop when starting
	/// </summary>
	public class ScreenFade : UiElement
	{
		public float Time { get; set; } = 1;

		private float alpha;
		public float Alpha { get { return alpha; } set { alpha = value; alpha = Math.Clamp(alpha, 0, 1); duration = Time * alpha; CalcuatateColor(); } }

		private Raylib_cs.Color color;
		public Raylib_cs.Color Color { get { return color; } set { color = new Raylib_cs.Color(value.R, value.G, value.B, (byte)(alpha * 255)); }}

		public Action onFadeInFinished;
		public Action onFadeOutFinished;

		float duration = 0;
		byte value = 0;

		private bool fadeIn = false;
		public byte fadeInTarget = 255;
		public byte fadeOutTarget = 0;

		SpriteComponent spriteComponent;

		public override void Construct()
		{
			base.Construct();

			Sprite backgroundSprite = new Sprite("Game/Assets/Textures/BGPause.png");
			spriteComponent = new SpriteComponent(this, backgroundSprite, new Raylib_cs.Color(Color.R, Color.G, Color.B, (byte)0));
			spriteComponent.SpriteScale = 80;

			int screenWidth = Raylib.GetScreenWidth();
			float factor = (float)screenWidth / backgroundSprite.FrameWidth;
			spriteComponent.SpriteScale = factor;
		}

		public void FadeIn()
		{
			fadeIn = true;
			duration = Time * alpha;
			value = (byte)(255 * alpha);
		}

		public void FadeOut()
		{
			fadeIn = false;
			duration = Time * alpha;
			value = (byte)(255 * alpha);
		}

		public override void Update()
		{
			base.Update();

			if (fadeIn)
			{
				if (value == fadeInTarget) return;
				duration += RocketEngine.Time.DeltaTime;
				duration = Math.Clamp(duration, 0f, Time);
			}
			else
			{
				if(value == fadeOutTarget) return;
				duration -= RocketEngine.Time.DeltaTime;
				duration = Math.Clamp(duration, 0f, Time);
			}

			alpha = duration / Time;
			Alpha = Math.Clamp(alpha, 0, 1);
		}

		void CalcuatateColor()
		{
			value = (byte)(255 * Alpha);
			value = Math.Clamp(value, fadeOutTarget, fadeInTarget);

			spriteComponent.colorTint = new Raylib_cs.Color(Color.R, Color.G, Color.B, value);

			if(fadeIn)
			{
				if (value == fadeInTarget)
				{
					onFadeInFinished?.Invoke();
				}
			}
			else
			{
				if (value == fadeOutTarget)
				{
					onFadeOutFinished?.Invoke();
				}
			}
		}
	}
}
