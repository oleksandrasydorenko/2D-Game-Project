using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketEngine
{
	public class SpriteAnimation:Animation
	{
		protected override int CurrentFrameIndex
		{
			get 
			{
				return base.CurrentFrameIndex; 
			}
			set
			{
				base.CurrentFrameIndex = value;

				if(sprite != null)
				sprite.CurrentFrame = base.CurrentFrameIndex; // sprite wird erst im constructor gestezt 
			}
		}

		public Sprite sprite;

		public SpriteAnimation(Sprite sprite, float speed = 1, bool loop = true, bool reverse = false, AnimationSignal[] animationSignals = null) :base(sprite.TileAmount, speed,loop ,reverse, animationSignals) 
		{
			this.sprite = sprite;
			sprite.CurrentFrame = base.CurrentFrameIndex;
		}

	}
}
