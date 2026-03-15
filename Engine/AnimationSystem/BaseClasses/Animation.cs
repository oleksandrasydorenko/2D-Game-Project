using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Security;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RocketEngine
{

	/// <summary>
	/// base class for animations, takes care of the frames and what frame to go to next
	/// handles animation signals that can be subscribed to 
	/// </summary>

	public abstract class Animation
	{
		protected int maxFrameIndex = 0;

		public float speed;
		public bool reverse;
		public bool loop = true;

		public Action onAnimationStarted;
		public Action onAnimationFinished;

		private bool animationFinished = false;

		protected Dictionary<int, AnimationSignal> animationSignals = new Dictionary<int, AnimationSignal>();

		private int lastFrame = -1;
		private int currentFrameIndex = 0;
		protected virtual int CurrentFrameIndex
		{
			get
			{	
				return currentFrameIndex;
			}
			set
			{
				if (value < 0) value = 0;
			
				int amount = maxFrameIndex == 0 ? 0 :  value % ( maxFrameIndex + 1);

				if(value == maxFrameIndex)
				{
					amount = maxFrameIndex;
				}
				currentFrameIndex = amount;

			}
		}

		public Animation(int frameAmount = 1, float speed = 1, bool loop = true, bool reverse = false, AnimationSignal[] animationSignals = null)
		{
			this.speed = speed;
			this.loop = loop;
			this.reverse = reverse;
			this.maxFrameIndex = frameAmount - 1;

			if(!reverse)
			{
				CurrentFrameIndex = 0;
			}
			else
			{
				CurrentFrameIndex = maxFrameIndex;
			}
			

			if(animationSignals != null)
			{
				for (int i = 0; i < animationSignals.Length; i++)
				{
					int insertIndex = animationSignals[i].Frame - 1;

					// makes sure we dont go out of range 
					if (insertIndex < 0) continue;
					else if (insertIndex > maxFrameIndex) continue;

					this.animationSignals.Add(insertIndex , animationSignals[i]);
				}
			}
		}

		public void SetFrame(int frame)
		{
			if (frame <= 1) frame = 1; 
			if(frame - 1 >=  maxFrameIndex) frame = maxFrameIndex + 1;

			CurrentFrameIndex = frame -1;	
		}

		public virtual void UpdateFrames(int amount)
		{
			if (!loop && animationFinished) return;

			if (amount < 0)
			{
				amount = 0;
				return; 
			}

			
			if(maxFrameIndex == 0) // save performance
			{
				if (loop) AnimationFinished(); 

				return;
			}
						
			lastFrame = CurrentFrameIndex;

			int adjustedAmount = amount % (maxFrameIndex + 1);

			if(amount == maxFrameIndex + 1)
			{
				adjustedAmount = maxFrameIndex + 1;
				amount = maxFrameIndex + 1;
			}

			if (!reverse)
			{
				CurrentFrameIndex = lastFrame + adjustedAmount;

                
				// last frame + amount falls wir bei frame 5 sind und frame 6 waere der letzte und iwr amount 3 haben zb dann würde das event nicht feuern
				if (!animationFinished && (lastFrame + amount) >= maxFrameIndex)
				{
                    AnimationFinished();
				}
			}
			else
			{
				int temp = lastFrame - adjustedAmount;

				if (temp < 0)
				{
					CurrentFrameIndex = (maxFrameIndex + 1) + temp;
				}
				else
				{
					CurrentFrameIndex = temp;
				}

				if (!animationFinished && (lastFrame - amount) <= 0)
				{
					AnimationFinished();
				}
			}


			#region animation Signals
			if (animationSignals != null)
			{
				
				int lastIndex = (amount > maxFrameIndex + 1) ? lastFrame + maxFrameIndex + 1 : lastFrame + amount; 

				// max steps to check to not fire events more then once each frame
				int steps = Math.Abs(lastFrame - lastIndex);

				if (!reverse)
				{
					// we start at the last frame +1 because the amount is always at least one

					for (int i = lastFrame + 1; i <= lastFrame + steps; i++)
					{
						int frameIndex = i;

						if(frameIndex > maxFrameIndex)
						{
							int divisor = maxFrameIndex == 0 ? frameIndex : frameIndex / maxFrameIndex;
							frameIndex = frameIndex - (divisor * maxFrameIndex +1);
							
						}

						// looking in the signals array for the animation at that index if yes we trigger it
						if (animationSignals.ContainsKey(frameIndex))
						{
							animationSignals[frameIndex].onAnimationSignalTriggered?.Invoke();
						}
					}
				}
				else
				{
					// checking for animation signals but in reverse
					for (int i = lastFrame - 1; i >= lastFrame - steps; i--) 
					{
						int frameIndex = i;

						if (frameIndex < 0)
						{
							frameIndex = maxFrameIndex + 1 + frameIndex;
						}

						if (animationSignals.ContainsKey(frameIndex))
						{
							animationSignals[frameIndex].onAnimationSignalTriggered?.Invoke();
						}
					}
				}
				#endregion

			}

		}

		public virtual void StartAnimation()
		{
			AnimationStarted();
		}

		protected virtual void AnimationStarted()
		{
			animationFinished = false;

            Console.WriteLine("started animation ++++++++++++++++++++");

			if (!reverse)
			{
				CurrentFrameIndex = 0;
			}
			else
			{
				CurrentFrameIndex = maxFrameIndex;
			}

			onAnimationStarted?.Invoke();

		}

		protected virtual void AnimationFinished()
		{
			if (!loop)
			{
				// when looping the animation is not finished so that would be missleading to put it outside
				animationFinished = true; 
				
				if (!reverse)
				{
					CurrentFrameIndex = maxFrameIndex;
				}
				else
				{
					CurrentFrameIndex = 0;
				}

				onAnimationFinished?.Invoke();
			}
		}
	}
}
