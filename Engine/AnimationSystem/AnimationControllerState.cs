using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketEngine
{
	/// <summary>
	/// Der Animation Controller State kümmert sich um das updaten der animation und hat ein namen welcher genutzt werden kann
	/// um im Animation controller in den State zu wechseln
	/// </summary>
	public class AnimationControllerState
	{
		public string name;
		public Animation animation;

		public Action onStateEntered;
		public Action onStateExited;

		float runTime = 0;
		float frameTime = 0; // default is 24fps // can be changed in engine settings

		public AnimationControllerState(string name, Animation animation)
		{
			this.name = name;
			this.animation = animation;

			frameTime = (1 / RocketEngine.Settings.AnimationSettings.ANIMATION_FRAMES_PER_SECOND);

			onStateEntered += StateEntered;
			onStateExited += StateExited;
		}

		public void StateEntered()
		{
			runTime = 0;
			animation.StartAnimation();
		}

		public void StateExited()
		{
			//runTime = 0;
			//animation.AniamtionCanceled();
		}

		public void UpdateState(float deltaTime)
		{
			runTime += deltaTime * animation.speed;
	
			if (runTime >= frameTime)
			{
				// Ich muss diese intensive operation machen um längere frames zu accounten
				int amount = (int)(runTime / frameTime);

				runTime -= amount * frameTime;
				animation.UpdateFrames(amount);
			}
		}
	}
}
