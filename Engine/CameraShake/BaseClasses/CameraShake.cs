using RocketEngine.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RocketEngine
{
	public class CameraShake
	{
		protected float runTime = 0;
		protected float strenght = 0;
		protected float speed = 0;
		protected float duration = 0;
		protected float blendInTime = 0;
		protected float blendOutTime = 0;
		public bool distanceBase = false;
		public Vector2 distance = Vector2.Zero;
		public Vector2 origin = Vector2.Zero;

		protected float whenToBlendOut = 0f;
		protected bool blendingOut = false;

		public CameraShake(CameraShakeArguments arg)
		{
			this.runTime = 0;
			this.speed = arg.speed;
			this.strenght = arg.strenght;
			this.duration = arg.duration;
			this.blendInTime = arg.blendInTime;
			this.blendOutTime = arg.blendOutTime;
			this.distanceBase = arg.distanceBased;
			this.distance = new Vector2(arg.distance.X, arg.distance.Y);
			this.origin = new Vector2(arg.origin.X, arg.origin.Y);

			whenToBlendOut = duration - blendOutTime;
		}

		public Vector2 GetShakeOffset(float DeltaTime, out Vector2 origin)
		{
			float alpha = 0;
			Vector2 offset = Vector2.Zero;

			runTime += DeltaTime;

			// duration based camera shake logic
			if (duration > 0 && !blendingOut)
			{
				if (runTime >= duration)
				{
					origin = this.origin;
					return Vector2.Zero; // we are already over the limit
				}
					

				if(runTime <= blendInTime)
				{
					alpha = runTime / blendInTime;
				}
				else if(runTime >= whenToBlendOut)
				{
					alpha = (duration - runTime) / blendOutTime;
				}
				else
				{
					alpha = 1;
				}

				alpha = Math.Clamp(alpha, 0, 1);

				offset = new Vector2(
					MathUtils.Noise(runTime * speed),
					MathUtils.Noise(runTime * speed)
					) * strenght;

				// for the most extreem rare case that its just randomly 0,0 
				if (offset == Vector2.Zero) { offset = new Vector2(0.1f, 0.1f); }

				offset *= alpha;

				origin = this.origin;
				return offset; // we are already over the limit
			}
			else // endless camera shake logic
			{
				if(runTime <= blendInTime) //  blending in
				{
					alpha = runTime / blendInTime;
				}
				else if(blendingOut) // blending out, if we say stop camera shake blending out will be set true
				{
					alpha = (duration - runTime) / blendOutTime;
				}
				else
				{
					alpha = 1;
				}

				alpha = Math.Clamp(alpha, 0, 1);

				if (blendingOut && alpha <= 0)
				{
					origin = this.origin;
					return Vector2.Zero; // we are already over the limit
				}

				offset = new Vector2( // random noise
					MathUtils.Noise(runTime * speed),
					MathUtils.Noise(runTime * speed)
					) * strenght;


				// for the most extreem rare case that its just randomly 0,0 
				if(offset == Vector2.Zero) { offset = new Vector2(0.1f, 0.1f); }

				offset *= alpha;

				origin = this.origin;
				return offset; // we are already over the limit
			}
		}
	}
}
