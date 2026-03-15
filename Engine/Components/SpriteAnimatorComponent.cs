using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketEngine
{
	/// optimisation suggestion: make the list of animation in the animation controller init and then check here on time if they are all sprite animations and dont cast always


	/// <summary>
	/// Der SpriteAnimatorComponent ist ein spezieller Component welcher ein Animatior Conroller nutzt zm zwischen sprite animationen zu blenden
	/// </summary>
	public class SpriteAnimatorComponent : InstantiableComponent, IUpdatable
	{
		private SpriteComponent spriteComponent;
		private AnimationController controller;

		public TimeManagement.UpdateMode updateMode;

		private bool animatorPaused = false;
		public SpriteAnimatorComponent(ComponentBase parent, SpriteComponent spriteComponent, AnimationController animatonController,string name = "SpriteAnimatorComponent") : base(parent, name)
		{
			this.spriteComponent = spriteComponent;
			this.controller = animatonController;

			spriteComponent.onDestroyed += (InstantiableComponent component) => Destroy();
		}

		public void Update()
		{
			if (animatorPaused) return;

			controller.UpdateController(updateMode == TimeManagement.UpdateMode.RealTime ? Time.UnscaledDeltaTime : Time.DeltaTime);

			SpriteAnimation currentAnimation = (controller.ActiveState.animation) as SpriteAnimation;

			// Savety check das es sich auch wirklich um sprite animationen handelt "as" gibt null zurück falls es keine sprite animation ist
			if (currentAnimation == null) throw new ArgumentException("sprite animation required in" + Name);

			spriteComponent.sprite = currentAnimation.sprite;
		}

		public void SetState(string name)
		{
			controller.SetState(name);	
		}

		public string GetState()
		{
			return controller.ActiveState.name;
        }

		public void PauseAnimator(bool pause = true)
		{
			animatorPaused = pause;
		}

		public void SetActiveAnimtionFrame(int frame)
		{
			controller.ActiveState.animation.SetFrame(frame);
		}
	}
}
